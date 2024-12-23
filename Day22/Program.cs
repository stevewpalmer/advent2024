string[] input = File.ReadAllLines("puzzle.txt");

Dictionary<int, int> totals = [];

long answer1 = input.Sum(l => Secret(long.Parse(l)));
int answer2 = totals.Max(b => b.Value);

Console.WriteLine($"Part 1 answer: {answer1}");
Console.WriteLine($"Part 2 answer: {answer2}");
return;

long Secret(long number) {
    HashSet<int> sequences = [];
    int lastPrice = (int)(number % 10);
    int a1 = 0, a2 = 0, a3 = 0, a4;
    for (int index = 1; index <= 2000; index++) {
        number = ((number * 64) ^ number) & 0xFFFFFF;
        number = ((number >> 5) ^ number) & 0xFFFFFF;
        number = ((number * 2048) ^ number) & 0xFFFFFF;

        int price = (int)(number % 10);
        a4 = a3;
        a3 = a2;
        a2 = a1;
        a1 = price - lastPrice;
        if (index >= 3) {
            int wispa = ((a4 < 0 ? -a4 | 0x80 : a4) << 24) |
                        ((a3 < 0 ? -a3 | 0x80 : a3) << 16) |
                        ((a2 < 0 ? -a2 | 0x80 : a2) << 8) |
                        (a1 < 0 ? -a1 | 0x80 : a1);
            if (!sequences.Contains(wispa)) {
                totals.TryAdd(wispa, 0);
                totals[wispa] += price;
            }
            sequences.Add(wispa);
        }
        lastPrice = price;
    }
    return number;
}