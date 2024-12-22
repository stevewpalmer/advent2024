string[] input = File.ReadAllLines("puzzle.txt");

long answer1 = input.Sum(l => Secret(long.Parse(l)));

Console.WriteLine($"Part 1 answer: {answer1}");
return;

long Secret(long number) {
    for (int c = 0; c < 2000; c++) {
        number = (number * 64) ^ number;
        number %= 16777216;
        number = (long)Math.Floor((double)number / 32) ^ number;
        number %= 16777216;
        number = (number * 2048) ^ number;
        number %= 16777216;
    }
    return number;
}