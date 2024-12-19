string[] lines = File.ReadAllLines("puzzle.txt");
string[] patterns = lines[0].Split(',', StringSplitOptions.TrimEntries).OrderByDescending(l => l.Length).ToArray();
string[] designs = lines.Skip(2).Select(l => l).ToArray();
Dictionary<string, long> cache = [];

int answer1 = designs.Count(d => TryMatch(d) > 0);
long answer2 = designs.Sum(TryMatch);

Console.WriteLine($"Part 1 answer: {answer1}");
Console.WriteLine($"Part 2 answer: {answer2}");
return;

long TryMatch(string design) {
    if (cache.TryGetValue(design, out long count)) {
        return count;
    }
    if (design.Length == 0) {
        return 1;
    }
    count = patterns.Where(design.StartsWith).Sum(s => TryMatch(design[s.Length..]));
    cache.Add(design, count);
    return count;
}