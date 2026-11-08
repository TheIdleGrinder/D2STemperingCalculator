using D2STemperingCalculator.Models;

namespace D2STemperingCalculator.Services;

public sealed record TemperingThreshold(int Level, double? MinimumScore);

public sealed record TemperingStrategy(
    double ExpectedEssences,
    IReadOnlyList<TemperingThreshold> Thresholds);

public sealed class TemperingAnalysis
{
    public const int Resolution = 1000;

    private static readonly double[] SuccessProbabilities =
    [
        1.00, 0.80, 0.67, 0.57, 0.50,
        0.44, 0.40, 0.36, 0.33, 0.31
    ];

    private readonly Outcome[] outcomes;
    private readonly double[][] distributions;
    private readonly int maximumIncrement;
    private readonly int target;

    public double ExpectedScore { get; }

    public double TargetProbability { get; }

    public bool IsReachable { get; }

    public TemperingAnalysis(TemperingItem item, double targetScore)
    {
        if (item.Affixes.Count < 2 ||
            item.Affixes.Any(affix =>
                !double.IsFinite(affix.Value) ||
                affix.Value < 0 ||
                affix.Value > 1))
        {
            throw new ArgumentException(
                "At least two affixes with values between 0 and 1 are required.");
        }

        if (!double.IsFinite(targetScore) ||
            targetScore < 0 ||
            targetScore > 10)
        {
            throw new ArgumentException("The target score must be between 0 and 10.");
        }

        ExpectedScore = 10 * item.Affixes.Average(affix => affix.Value);

        outcomes = item.Affixes
            .Select(affix => (int)Math.Round(
                affix.Value * Resolution,
                MidpointRounding.AwayFromZero))
            .GroupBy(value => value)
            .Select(group => new Outcome(
                group.Key,
                (double)group.Count() / item.Affixes.Count))
            .ToArray();

        maximumIncrement = outcomes.Max(outcome => outcome.Increment);

        // Scores lie on the discretized grid. The target is rounded upward
        // so a grid score below the requested target never counts as success.
        target = (int)Math.Ceiling(
            Math.Round(targetScore * Resolution, 9));

        distributions = BuildDistributions();
        IsReachable = target <= 10 * maximumIncrement;

        TargetProbability = target == 0
            ? 1
            : Math.Clamp(distributions[10].Skip(target).Sum(), 0, 1);
    }

    public async Task<TemperingStrategy> CalculateStrategyAsync()
    {
        if (target == 0)
        {
            return new TemperingStrategy(
                0,
                Enumerable.Range(1, 9)
                    .Select(level => new TemperingThreshold(level, 0))
                    .ToArray());
        }

        if (!IsReachable)
        {
            throw new InvalidOperationException(
                "The target cannot be reached at tempering level 10.");
        }

        double lower = 0;
        double upper = 1;

        // Find a bracket for C = expected cost from a fresh item.
        // Evaluate(C) assumes every cleaning operation costs 1 + C.
        while (Evaluate(upper, false).Cost > upper)
        {
            lower = upper;
            upper *= 2;

            if (upper > 1e12)
            {
                throw new InvalidOperationException(
                    "Expected costs exceed the numerical limit of 1 trillion essences. " +
                    "Please choose a lower target.");
            }

            await Task.Yield();
        }

        for (int iteration = 0; iteration < 60; iteration++)
        {
            double middle = lower + (upper - lower) / 2;

            if (Evaluate(middle, false).Cost > middle)
            {
                lower = middle;
            }
            else
            {
                upper = middle;
            }

            // Allow the WebAssembly UI to render between iterations.
            await Task.Yield();
        }

        double expectedCost = lower + (upper - lower) / 2;
        Evaluation result = Evaluate(expectedCost, true);

        return new TemperingStrategy(
            expectedCost,
            result.Thresholds);
    }

    private double[][] BuildDistributions()
    {
        var result = new double[11][];
        result[0] = [1];

        for (int level = 1; level <= 10; level++)
        {
            result[level] = new double[level * maximumIncrement + 1];

            for (int score = 0; score < result[level - 1].Length; score++)
            {
                double probability = result[level - 1][score];

                if (probability == 0)
                {
                    continue;
                }

                foreach (Outcome outcome in outcomes)
                {
                    result[level][score + outcome.Increment] +=
                        probability * outcome.Probability;
                }
            }
        }

        return result;
    }

    private Evaluation Evaluate(double restartCost, bool collectThresholds)
    {
        double resetCost = 1 + restartCost;

        // At level 10 an unsuccessful run must be cleaned.
        // States that have reached the target have zero remaining cost.
        var next = new double[target];
        Array.Fill(next, resetCost);

        var thresholds = new List<TemperingThreshold>();

        for (int level = 9; level >= 0; level--)
        {
            var current = new double[target];
            int highestScore = Math.Min(target - 1, level * maximumIncrement);
            int? minimumScore = null;

            for (int score = 0; score <= highestScore; score++)
            {
                double continueCost = 1 / SuccessProbabilities[level];

                foreach (Outcome outcome in outcomes)
                {
                    int nextScore = score + outcome.Increment;

                    if (nextScore < target)
                    {
                        continueCost += outcome.Probability * next[nextScore];
                    }
                }

                // Cleaning a fresh item cannot improve its state.
                current[score] = level == 0
                    ? continueCost
                    : Math.Min(continueCost, resetCost);

                if (collectThresholds &&
                    level > 0 &&
                    minimumScore is null &&
                    distributions[level][score] > 0 &&
                    continueCost <= resetCost)
                {
                    minimumScore = score;
                }
            }

            if (collectThresholds && level > 0)
            {
                // Reaching the target always means stopping successfully.
                if (minimumScore is null)
                {
                    for (int score = target;
                         score < distributions[level].Length;
                         score++)
                    {
                        if (distributions[level][score] > 0)
                        {
                            minimumScore = score;
                            break;
                        }
                    }
                }

                thresholds.Add(new TemperingThreshold(
                    level,
                    minimumScore.HasValue
                        ? minimumScore.Value / (double)Resolution
                        : null));
            }

            next = current;
        }

        thresholds.Reverse();
        return new Evaluation(next[0], thresholds);
    }

    private sealed record Outcome(int Increment, double Probability);

    private sealed record Evaluation(
        double Cost,
        IReadOnlyList<TemperingThreshold> Thresholds);
}