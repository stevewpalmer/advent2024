using System.Drawing;

char[][] maze = File.ReadAllLines("puzzle.txt").Select(l => l.ToCharArray()).ToArray();
(int dx, int dy)[] directions = [(0, -1), (1, 0), (0, 1), (-1, 0)];
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

(int answer1, int answer2) = Walk();
Console.WriteLine($"Part 1 answer: {answer1}");
Console.WriteLine($"Part 2 answer: {answer2}");
return;

(int, int) Walk() {
    PriorityQueue<(Point, int, List<Point>), int> queue = new();
    Dictionary<(Point, int), int> visited = [];
    HashSet<Point> paths = [start];
    queue.Enqueue((start, 1, []), 0);

    int best = int.MaxValue;
    while (queue.TryDequeue(out var element, out int cost)) {
        (Point pt, int dt, List<Point> path) = element;
        if (visited.TryGetValue((pt, dt), out int score) && score < cost) {
            continue;
        }
        visited[(pt, dt)] = cost;
        if (pt == end && cost <= best) {
            best = cost;
            paths.UnionWith(path);
        }
        foreach ((int d, int c) in ((int, int)[]) [(1, 1001), (-1, 1001), (0, 1)]) {
            int nd = (dt + d % 4 + 4) % 4;
            Point step = new(pt.X + directions[nd].dx, pt.Y + directions[nd].dy);
            if (maze[step.Y][step.X] != '#') {
                queue.Enqueue((step, nd, [.. path, step]), cost + c);
            }
        }
    }
    return (best, paths.Count);
}