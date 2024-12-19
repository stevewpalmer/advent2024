using System.Drawing;

char[][] maze = File.ReadAllLines("puzzle.txt").Select(l => l.ToCharArray()).ToArray();
(int dx, int dy)[] directions = [(0, -1), (1, 0), (0, 1), (-1, 0)];
Point start = Point.Empty;
Point end = Point.Empty;

int Mod(int a, int b) => (a % b + b) % b;

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

Console.WriteLine($"Part 1 answer: {Walk()}");
return;

int Walk() {
    PriorityQueue<(Point, int), int> queue = new();
    HashSet<(Point, int)> visited = [];
    queue.Enqueue((start, 1), 0);

    int best = int.MaxValue;
    while (queue.TryDequeue(out var element, out int cost)) {
        (Point pt, int dt) = element;
        if (!visited.Add((pt, dt))) {
            continue;
        }
        if (pt == end) {
            best = Math.Min(cost, best);
        }
        queue.Enqueue((pt, Mod(dt + 1, 4)), cost + 1000);
        queue.Enqueue((pt, Mod(dt - 1, 4)), cost + 1000);

        pt.Offset(directions[dt].dx, directions[dt].dy);
        if (maze[pt.Y][pt.X] != '#') {
            queue.Enqueue((pt, dt), cost + 1);
        }
    }
    return best;
}