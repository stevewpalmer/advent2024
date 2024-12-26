//#define DRAW_THE_TREE

using System.Drawing;
using System.Text.RegularExpressions;

List<(int seconds, int factor, List<Point> robots)> factors = [];

List<(Point p, Point v)> robots = File.ReadAllLines("puzzle.txt")
    .Select(line => Regex.Matches(line, @"p=(\d+),(\d+) v=(\-*\d+),(\-*\d+)")
        .SelectMany(m => m.Groups.Cast<Group>()
            .SelectMany(g => g.Captures
                .Select(c => c.Value)))
        .Skip(1)
        .Select(int.Parse)
        .ToArray())
    .Select(values => (new Point(values[0], values[1]), new Point(values[2], values[3])))
    .ToList();

int seconds = 0;
const int width = 101;
const int height = 103;

while (seconds < 10402) {
    for (int i = 0; i < robots.Count; i++) {
        (Point p, Point v) robot = robots[i];
        robot.p.X = (robot.p.X + robot.v.X) % width;
        robot.p.Y = (robot.p.Y + robot.v.Y) % height;
        if (robot.p.Y < 0) {
            robot.p.Y += height;
        }
        if (robot.p.X < 0) {
            robot.p.X += width;
        }
        robots[i] = robot;
    }

    factors.Add((++seconds,
        robots.Count(r => r.p is { X: < width / 2, Y: < height / 2 }) *
        robots.Count(r => r.p is { X: > width / 2, Y: < height / 2 }) *
        robots.Count(r => r.p is { X: < width / 2, Y: > height / 2 }) *
        robots.Count(r => r.p is { X: > width / 2, Y: > height / 2 }),
        robots.Select(r => r.p).ToList()));
}

int answer1 = factors.First(r => r.seconds == 100).factor;
int answer2 = factors.OrderBy(f => f.factor).First().seconds;

#if DRAW_THE_TREE
List<Point> tree = factors.OrderBy(f => f.factor).First().robots;
for (int r = 0; r < height; r++) {
    for (int c = 0; c < width; c++) {
        int count = tree.Count(t => t.Y == r && t.X == c);
        Console.Write(count == 0 ? "." : count.ToString());
    }
    Console.WriteLine();
}
#endif

Console.WriteLine($"Part 1 answer: {answer1}");
Console.WriteLine($"Part 2 answer: {answer2}");