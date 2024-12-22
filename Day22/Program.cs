string[] input = File.ReadAllLines("puzzle.txt");

HashSet<(long, string)> buyerSequences = [];
Dictionary<string, int> totals = [];

long answer1 = input.Sum(l => Secret(long.Parse(l)));
int answer2 = totals.Max(b => b.Value);

Console.WriteLine($"Part 1 answer: {answer1}");
Console.WriteLine($"Part 2 answer: {answer2}");
return;

long Secret(long number) {
    long buyerId = number;
    long lastPrice = number % 10;
    int a1 = 0, a2 = 0, a3 = 0, a4;
    for (int index = 1; index <= 2000; index++) {
        number = (number * 64) ^ number;
        number %= 16777216;
        number = (long)Math.Floor((double)number / 32) ^ number;
        number %= 16777216;
        number = (number * 2048) ^ number;
        number %= 16777216;

        int price = (int)(number % 10);
        a4 = a3;
        a3 = a2;
        a2 = a1;
        a1 = (int)(price - lastPrice);
        if (index >= 3) {
            string sequenceString = string.Join("", a1, a2, a3, a4);
            if (!buyerSequences.Contains((buyerId, sequenceString))) {
                totals.TryAdd(sequenceString, 0);
                totals[sequenceString] += price;
            }
            buyerSequences.Add((buyerId, sequenceString));
        }
        lastPrice = price;
    }
    return number;
}