long answer1 = 0;
long answer2 = 0;
foreach (string line in File.ReadAllLines("puzzle.txt")) {
    List<long> values = line.Replace(":", "").Split(' ').Select(long.Parse).ToList();
    if (TrySolve(values[0], values[1..], false)) {
        answer1 += values[0];
    }
    if (TrySolve(values[0], values[1..], true)) {
        answer2 += values[0];
    }
}
Console.WriteLine($"Part 1 answer: {answer1}");
Console.WriteLine($"Part 2 answer: {answer2}");
return;

bool TrySolve(long total, List<long> values, bool concat) {
    if (values.Count == 1) {
        return total == values[0];
    }
    if (values[0] > total) {
        return false;
    }
    List<long> newValues = [0];
    if (values.Count > 2) {
        newValues.AddRange(values[2..]);
    }
    newValues[0] = values[0] * values[1];
    if (TrySolve(total, newValues, concat)) {
        return true;
    }
    newValues[0] = values[0] + values[1];
    if (TrySolve(total, newValues, concat)) {
        return true;
    }
    if (concat) {
        newValues[0] = long.Parse($"{values[0]}{values[1]}");
        if (TrySolve(total, newValues, true)) {
            return true;
        }
    }
    return false;
}
