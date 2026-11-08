namespace D2STemperingCalculator.Models;

public sealed class TemperingItem
{
    public string Name { get; set; } = string.Empty;

    public List<TemperingAffix> Affixes { get; set; } =
    [
        new() { Value = 1 },
        new() { Value = 0 }
    ];
}

public sealed class TemperingAffix
{
    public string Name { get; set; } = string.Empty;

    public double Value { get; set; }
}