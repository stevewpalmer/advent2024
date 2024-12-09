string[] data = File.ReadAllLines("puzzle.txt");
int line = 0;

List<(int, int)> rules = [];
while (line < data.Length && !string.IsNullOrEmpty(data[line])) {
    int[] parts = data[line++].Split("|").Select(int.Parse).ToArray();
    rules.Add((parts[0], parts[1]));
}
int totalAnswer1 = 0;
int totalAnswer2 = 0;
while (++line < data.Length) {
    int[] pages = data[line].Split(",").Select(int.Parse).ToArray();
    int[] sortedPages = (int[])pages.Clone();
    int middle = Math.Abs(pages.Length / 2);

    Array.Sort(sortedPages, (t1, t2) => rules.IndexOf((t2, t1)));
    if (pages.SequenceEqual(sortedPages)) {
        totalAnswer1 += pages[middle];
    }
    else {
        totalAnswer2 += sortedPages[middle];
    }
}
Console.WriteLine($"Part 1 answer : {totalAnswer1}");
Console.WriteLine($"Part 2 answer : {totalAnswer2}");