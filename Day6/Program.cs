using System.Drawing;

Point[] turns = [new(0, -1), new(1, 0), new(0, 1), new(-1, 0)];
Point guardStart = new(-1, -1);
HashSet<Point> obstacles = [];

string[] rows = File.ReadAllLines("puzzle.txt");
for (int y = 0; y < rows.Length; y++) {
    char[] columns = rows[y].ToCharArray();
    for (int x = 0; x < columns.Length; x++) {
        if (columns[x] == '#') {
            obstacles.Add(new Point(x, y));
        }
        if (columns[x] == '^') {
            guardStart = new Point(x, y);
        }
    }
}
Rectangle floor = new(0, 0, rows[0].Length, rows.Length);

int answer1 = 0;
int answer2 = 0;

HashSet<Point> newObstacles = [];
foreach (Point point in Walk1(guardStart, obstacles)) {
    HashSet<Point> newGrid = obstacles.ToHashSet();
    newGrid.Add(point);
    if (Walk2(guardStart, newGrid) && newObstacles.Add(point)) {
        answer2++;
    }
    answer1++;
}

Console.WriteLine($"Puzzle 1 answer : {answer1}");
Console.WriteLine($"Puzzle 2 answer : {answer2}");
return;

HashSet<Point> Walk1(Point guard, HashSet<Point> grid) {
    HashSet<Point> visited = [];
    int direction = 0;
    while (true) {
        Point next = guard;
        visited.Add(next);
        do {
            next.X = guard.X + turns[direction].X;
            next.Y = guard.Y + turns[direction].Y;
            if (!grid.Contains(next)) {
                break;
            }
            direction = ++direction % turns.Length;
        } while (true);
        if (!floor.Contains(next)) {
            break;
        }
        guard = next;
    }
    return visited;
}

bool Walk2(Point guard, HashSet<Point> grid) {
    HashSet<(Point, int)> visited = [];
    int direction = 0;
    while (true) {
        Point next = guard;
        do {
            next.X = guard.X + turns[direction].X;
            next.Y = guard.Y + turns[direction].Y;
            if (!grid.Contains(next)) {
                break;
            }
            direction = ++direction % turns.Length;
        } while (true);
        if (!floor.Contains(next)) {
            break;
        }
        if (!visited.Add((next, direction))) {
            return true;
        }
        guard = next;
    }
    return false;
}