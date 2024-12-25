string[] sets = File.ReadAllText("puzzle.txt").Split("\n\n");

List<long> locks = [];
List<long> keys = [];

foreach (string set in sets) {
    long value = Convert.ToInt64(set.Split("\n").Aggregate("", (current, line) => current + line.Replace('.', '0').Replace('#', '1')), 2);
    if ((value & 31) != 0) {
        locks.Add(value);
    } else {
        keys.Add(value);
    }
}

int overlaps = 0;
int total = 0;
foreach (long alock in locks) {
    foreach (long akey in keys) {
        total++;
        if ((alock & akey) != 0) {
            ++overlaps;
        }
    }
}
Console.WriteLine($"Part 1 answer: {total - overlaps}");