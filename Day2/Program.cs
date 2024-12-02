int totalSafe1 = 0;
int totalSafe2 = 0;

foreach (string line in File.ReadLines("day2.txt")) {
    List<int> series = line.Split(' ').Select(int.Parse).ToList();
    if (IsSafe(series)) {
        totalSafe1++;
        totalSafe2++;
    }
    else {
        for (int c = 0; c < series.Count; c++) {
            if (IsSafe(series.Where((_, index) => index != c).ToList())) {
                ++totalSafe2;
                break;
            }
        }
    }
}

Console.WriteLine($"Puzzle 1 answer : {totalSafe1}");
Console.WriteLine($"Puzzle 2 answer : {totalSafe2}");
return;

bool IsSafe(List<int> series) {
    List<int> diffs  = series.Zip(series.Skip(1), Tuple.Create).Select(p => p.Item1 - p.Item2).ToList();
    return diffs.TrueForAll(d => d is >= 1 and <= 3) || diffs.TrueForAll(d => d is <= -1 and >= -3);
}