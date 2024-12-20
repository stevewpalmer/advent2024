using System.Drawing;

char[][] maze = File.ReadAllLines("puzzle.txt").Select(l => l.ToCharArray()).ToArray();
Point start = Point.Empty;
Point end = Point.Empty;

for (int r = 0; r < maze.Length; r++) {
    for (int c = 0; c < maze[r].Length; c++) {
        if (maze[r][c] == 'S') {
            start = new Point(c, r);
        }
        if (maze[r][c] == 'E') {
            end = new Point(c, r);
        }
    }
}

Dictionary<Point, int> path = Walk();

Console.WriteLine($"Part 1 answer: {CalculateCheats(2)}");
Console.WriteLine($"Part 2 answer: {CalculateCheats(20)}");
return;

int Distance(Point p1, Point p2) => Math.Abs(p2.X - p1.X) + Math.Abs(p2.Y - p1.Y);

int CalculateCheats(int picoseconds) =>
    path.Keys.Select(pt => path.Keys.Where(p => Distance(pt, p) <= picoseconds)
            .Select(p => (pt, p, Distance(pt, p))))
        .Select(cheats => cheats
            .Count(cheat => path[cheat.Item1] + cheat.Item3 <= path[cheat.Item2] - 100))
        .Sum();

Dictionary<Point, int> Walk() {
    Point pt = start;
    Dictionary<Point, int> visited = new() { { start, 0 } };

    while (pt != end) {
        Point step = Point.Empty;
        foreach ((int dx, int dy) in ((int, int)[]) [(0, -1), (1, 0), (0, 1), (-1, 0)]) {
            step = new Point(pt.X + dx, pt.Y + dy);
            if (maze[step.Y][step.X] != '#' && !visited.ContainsKey(step)) {
                break;
            }
        }
        visited.TryAdd(step, visited[pt] + 1);
        pt = step;
    }
    return visited;
}