#### Grundlagen
#### Gegenstände
Charaktere beziehen einen großen Teil ihre Kampfkraft aus ihrer Ausrüstung. Ein Ausrüstungsgegenstand besteht dabei aus einem Basistyp (z.B. ein Langschwert), der über eine Anzahl magischer Affixe modifiziert und verstärkt wird. Die hier betrachteten Gegenstände verfügen immer über mindestens zwei Affixe. Nach oben ist die Anzahl theoretisch unbegrenzt, praktisch hat aber kein Gegenstand mehr als 20 Affixe. Jeder Gegenstand verfügt außerdem über einen eindeutigen Namen.
#### Tempering
Tempering ist eine Spielmechanik, die es erlaubt, einzelne Affixe eines Gegenstands zu verbessern. Jeder Versuch kostet eine Einheit der Ressource _Essence_. Die Erfolgswahrscheinlichkeit hängt von der Anzahl der vorherigen erfolgreichen Temperingversuche auf dem Gegenstand ab, die im Folgenden als _Temperinglevel_ bezeichnet wird:
- Temperinglevel 0: 100% Erfolgswahrscheinlichkeit
- 1: 80%
- 2: 67%
- 3: 57%
- 4: 50%
- 5: 44%
- 6: 40%
- 7: 36%
- 8: 33%
- 9: 31%
Das Temperinglevel kann durch Tempering nicht über 10 erhöht werden.
Ist das Tempering erfolgreich, steigt wie erwähnt das Temperinglevel und ein zufällig gewähltes Affix wird verstärkt. Bei wiederholtem Tempering kann das gleiche Affix auch mehrmals verbessert werden.
##### Hell Forge
Ein Gegenstand mit Temperinglevel 10 kann zum Preis von einer Essenz einmalig einem Verbesserungsversuch in der _Hell Forge_ unterzogen werden. Im Fall eines Erfolgs, der eine feste Wahrscheinlichkeit von 40% hat, wird für jedes einzelne Affix die Anzahl der Temperingverstärkungen verdoppelt. Das Temperinglevel des Items steigt damit auf 20 und kann nicht mehr weiter erhöht werden.
Schlägt die Behandlung in der Hell Forge fehl, verliert der Gegenstand alle Verbesserungen durch das vorherige Tempering und wird auf Temperinglevel 0 zurückgesetzt. Er kann zwar anschließend wieder getempert werden, aber auch nach dem erneuten Erreichen von Temperinglevel 10, kann kein weiterer Versuch in der Hell Forge unternommen werden.
##### Cleaning
Zum Preis einer Essenz können alle Verbesserungen durch Tempering an einem Gegenstand entfernt und sein Temperinglevel wieder auf 0 gesetzt werden. Dies ermöglicht allerdings keinen erneuten Versuch in der Hell Forge, falls für diesen Gegenstand bereits einer unternommen wurde.

### Motivation
Vor Einführung von Tempering waren Gegenstände mit einer höheren Anzahl magische Affixe üblicherweise stärker als solche mit weniger Verstärkungen. Da die verschiedenen Affixe allerdings in ihrem Wert stark variieren, wirken zusätzliche, schwächere Verstärkungen nicht länger als nettes Beiwerk, sondern hemmen das Potenzial eines Items, durch Tempering stark an Macht zu gewinnen.
Das zu entwickelnde Tool soll dabei helfen, gute Zielgegenstände und ihr Potenzial zu bestimmen. Da die erwarteten Kosten pro Verbesserung durch die sinkenden Erfolgswahrscheinlichkeiten auf höheren Temperinglevels steigen, soll es außerdem helfen, zu erkennen, ob sich weitere Versuche mit einem Gegenstand lohnen oder stattdessen lieber auf Cleaning oder die Beschaffung einer neuen Basis gesetzt werden sollte.
### Berechnung des Tempering Score
Um zu bestimmen, wie erfolgreich das Tempering war, wird der Tempering Score bestimmt. Dieser bildet sich aus der Summe der Werte der einzelnen Verbesserungen. Der Wert einer Verbesserung richtet sich danach, wie erstrebenswert eine Verbesserung des Affixes relativ zum Pool aller Affixe ist. Dieser Wert wird als Fließkommazahl zwischen 0 und 1 dargestellt, wobei 1 den besten Affixen (z.B. _+X to all Skills_) vorbehalten ist, während die große Mehrheit der Affixe einen Wert von 0 hat, da ihre Verbesserung ungleich viel weniger effektiv ist.

### Benutzeroberfläche
Die gesamte Benutzeroberfläche ist auf Englisch und enthält die folgenden Bereiche:
#### Items verwalten
In späteren Versionen sollen die Items von der offiziellen Website importiert werden, doch vorerst werden sie vom Benutzer selbst definiert. Die so definierten Gegenstände können im lokalen Speicher für zukünftige Sessions abgespeichert werden.
Außerhalb des Bereiches, in dem das Item bearbeitet wird, befinden sich die Buttons _New Item_, _Save Item_ und _Load Item_ mit folgenden Funktionen:
- _New Item_: verwerfe Änderungen am aktuellen Item und ersetze es durch ein neues. Neue Items haben einen leeren Namen und zwei Affixe, wobei das erste den Wert 1 und das zweite den Wert 0 hat.
- _Save Item_: Speichere das aktuelle Item im lokalen Speicher. Der Name wird dabei als Schlüssel verwendet und gleichnamige Items ggf. überschrieben.
- _Load Item_: Zeige eine Liste aller gespeicherten Items. Durch Klicken auf ein Item in dieser Liste, werden Änderungen am aktuellen Item verworfen und dieses durch das zu ladende Item ersetzt. Die Auswahl kann abgebrochen werden; in dem Fall bleibt das aktuelle Item aktiv.
#### Items bearbeiten
Das Element für das Bearbeiten eines Items hat die folgenden Subelemente:
- Ein Eingabefeld für den Namen des Items, das auch als Überschrift dient
	- Damit ein Item gespeichert werden kann, darf dieses Feld nicht leer sein
- Eine Liste von Affixen, die jeweils die folgenden Bestandteile haben
	- Eingabefeld für den Namen (kann leer gelassen werden)
	- Eingabefeld für den Wert, den jede Verbesserung dieses Affixes zum Tempering Score beiträgt
	- Ein kleiner Button zum Entfernen dieses Affixes
- Ein Button zum Hinzufügen eines neuen Affixes am Ende der Liste
#### Analyse
Der Analysebereich liefert Informationen über die beste Tempering-Strategie, um einen bestimmten Zielwert für den Tempering Score mit dem Einsatz möglichst weniger Essenzen zu erreichen.
Da die durchschnittlichen Kosten für jeden erfolgreichen Temperingversuch durch die niedrigere Erfolgswahrscheinlichkeit mit dem Temperinglevel ansteigen, lohnt es sich oft nicht mehr, auf der Jagd nach einem bestimmten Score, gute Essenzen schlechten hinterher zu werfen. Stattdessen ist es effizienter, eine Essenz auszugeben, um das Item zu bereinigen und neu zu starten. Im Analysebereich wird die mathematisch effizienteste Strategie bestimmt, indem für jedes Temperinglevel berechnet wird, welcher Score mindestens erreicht sein muss, damit sich weitere Versuche anstelle eines Neustarts lohnen.
(Es ist noch nicht sicher, ob sich die Erwartungswerte und Wahrscheinlichkeiten effizient berechnen lassen; andernfalls müssten diese Näherungsweise z.B. per Monte Carlo Simulation bestimmt werden)

Der Analysebereich enthält die folgenden Elemente:
- Anzeige des Erwartungswert für den Tempering Score auf Temperinglevel 10
- Eingabefeld für den Ziel-Score
- Anzeige für die Wahrscheinlichkeit, bei Temperinglevel 10 mindestens den Ziel-Score zu erreichen
- Button zum Starten der Strategieberechnung
	- Bestimmt für jedes Temperinglevel von 1 bis 9 den Mindestscore, bei dem sich weitere Versuche lohnen.
- Falls die Strategie berechnet ist, eine Liste der erforderlichen Scores für die jeweiligen Temperinglevel.
- Anzeige für die erwarteten Gesamtkosten, um einmal den Ziel-Score zu erreichen.