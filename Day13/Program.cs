using System.Text.RegularExpressions;

string[] lines = File.ReadAllLines("puzzle.txt");

long answer1 = 0;
long answer2 = 0;
foreach (string[] item in lines.Chunk(4)) {
    int[] buttonA = Regex.Matches(item[0], @"\d+")
        .Select(m => int.Parse(m.Value))
        .ToArray();
    int[] buttonB = Regex.Matches(item[1], @"\d+")
        .Select(m => int.Parse(m.Value))
        .ToArray();
    long[] prize = Regex.Matches(item[2], @"\d+")
        .Select(m => long.Parse(m.Value))
        .ToArray();

    (int ax, int ay) = (buttonA[0], buttonA[1]);
    (int bx, int by) = (buttonB[0], buttonB[1]);
    (long px, long py) = (prize[0], prize[1]);

    answer1 += Solve(ax, ay, bx, by, px, py);
    answer2 += Solve(ax, ay, bx, by, px + 10000000000000, py + 10000000000000);
}

Console.WriteLine($"Part 1 answer: {answer1}");
Console.WriteLine($"Part 2 answer: {answer2}");
return;

long Solve(int ax, int ay, int bx, int by, long px, long py) {
    long b = Math.DivRem(ay * px - ax * py, ay * bx - ax * by, out long brem);
    long a = Math.DivRem(px - b * bx, ax, out long arem);
    return brem + arem == 0 ? a * 3 + b : 0;
}