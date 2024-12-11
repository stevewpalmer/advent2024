List<long> stones = File.ReadAllText("puzzle.txt").Split(' ').Select(long.Parse).ToList();
Dictionary<(int, long), long> cache = [];

long answer1 = stones.Sum(s => CountSplits(25, s));
long answer2 = stones.Sum(s => CountSplits(75, s));

Console.WriteLine($"Part 1 answer: {answer1}");
Console.WriteLine($"Part 2 answer: {answer2}");
return;

long CountSplits(int blink, long stone) {
    if (cache.TryGetValue((blink, stone), out long result)) {
        return result;
    }
    int c = (int)Math.Floor(Math.Log10(stone)) + 1;
    if (blink == 0) {
        result = 1;
    }
    else if (stone == 0) {
        result = CountSplits(blink - 1, 1);
    }
    else if (c % 2 == 0) {
        long div = (long)Math.Pow(10, c / 2.0);
        long left = stone / div;
        long right = stone % div;
        result = CountSplits(blink - 1, left) + CountSplits(blink - 1, right);
    }
    else {
        result = CountSplits(blink - 1, stone * 2024);
    }
    cache[(blink, stone)] = result;
    return result;
}