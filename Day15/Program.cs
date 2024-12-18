using System.Drawing;

const char WALL = '#';
const char BOX = 'O';
const char SPACE = '.';
const char ROBOT = '@';
const char LEFT = '[';
const char RIGHT = ']';

Point robot = new(-1, -1);

string[] input = File.ReadAllLines("puzzle.txt");

char[] route = string.Join("", input.SkipWhile(l => !string.IsNullOrEmpty(l))
        .Select(line => line))
    .ToCharArray();

Console.WriteLine($"Part 1 answer: {Solve(GetMap())}");
Console.WriteLine($"Part 2 answer: {Solve(Resize(GetMap()))}");
return;

char[][] GetMap() =>
    input.TakeWhile(l => !string.IsNullOrEmpty(l))
        .Select(line => line.ToCharArray())
        .ToArray();

char[][] Resize(char[][] map) {
    List<char[]> newMap = [];
    foreach (char[] row in map) {
        List<char> newRow = [];
        foreach (char tile in row) {
            newRow.AddRange(tile switch {
                WALL => [WALL, WALL],
                BOX => [LEFT, RIGHT],
                SPACE => [SPACE, SPACE],
                ROBOT => [ROBOT, SPACE],
                _ => []
            });
        }
        newMap.Add(newRow.ToArray());
    }
    return newMap.ToArray();
}

bool TryMove(char[][] map, Point pt, int dx, int dy) {
    pt.Offset(dx, dy);
    char tile = map[pt.Y][pt.X];
    switch (tile) {
        case SPACE:
            return true;
        case BOX: {
            do {
                pt.Offset(dx, dy);
                tile = map[pt.Y][pt.X];
            } while (tile == BOX);
            return tile != WALL;
        }
        case WALL:
            return false;
        case RIGHT when dx == 0:
            return TryMove(map, new Point(pt.X, pt.Y), dx, dy) &&
                   TryMove(map, new Point(pt.X - 1, pt.Y), dx, dy);
        case LEFT when dx == 0:
            return TryMove(map, new Point(pt.X, pt.Y), dx, dy) &&
                   TryMove(map, new Point(pt.X + 1, pt.Y), dx, dy);
        case RIGHT:
        case LEFT:
            return TryMove(map, new Point(pt.X + dx, pt.Y), dx, dy);
        default:
            return false;
    }
}

void Move(char[][] map, Point cp, int dx, int dy) {
    Point pt = cp;
    pt.Offset(dx, dy);
    char tile = map[pt.Y][pt.X];
    switch (tile) {
        case SPACE:
            (map[pt.Y][pt.X], map[cp.Y][cp.X]) = (map[cp.Y][cp.X], map[pt.Y][pt.X]);
            break;
        case BOX:
            Move(map, pt, dx, dy);
            (map[pt.Y][pt.X], map[cp.Y][cp.X]) = (map[cp.Y][cp.X], map[pt.Y][pt.X]);
            break;
        case RIGHT when dx == 0:
            Move(map, pt, dx, dy);
            Move(map, new Point(pt.X - 1, pt.Y), dx, dy);
            (map[pt.Y][pt.X], map[cp.Y][cp.X]) = (map[cp.Y][cp.X], map[pt.Y][pt.X]);
            break;
        case LEFT when dx == 0:
            Move(map, pt, dx, dy);
            Move(map, new Point(pt.X + 1, pt.Y), dx, dy);
            (map[pt.Y][pt.X], map[cp.Y][cp.X]) = (map[cp.Y][cp.X], map[pt.Y][pt.X]);
            break;
        case RIGHT or LEFT:
            Move(map, new Point(pt.X + dx, pt.Y), dx, dy);
            (map[pt.Y][pt.X + dx], map[pt.Y][pt.X], map[cp.Y][cp.X]) = (map[pt.Y][pt.X], map[cp.Y][cp.X], map[pt.Y][pt.X + dx]);
            break;
    }
}

int Solve(char[][] map) {
    robot.Y = map.Select((item, index) => (item, index)).First(l => l.item.Contains(ROBOT)).index;
    robot.X = Array.FindIndex(map[robot.Y], c => c == ROBOT);

    foreach (char ch in route) {
        int dx = 0, dy = 0;
        switch (ch) {
            case '<': dx = -1; break;
            case '>': dx = 1; break;
            case '^': dy = -1; break;
            case 'v': dy = 1; break;
        }
        if (TryMove(map, robot, dx, dy)) {
            Move(map, robot, dx, dy);
            robot.Offset(dx, dy);
        }
    }
    int sum = 0;
    for (int r = 0; r < map.Length; r++) {
        for (int c = 0; c < map[r].Length; c++) {
            if (map[r][c] == BOX || map[r][c] == LEFT) {
                sum += 100 * r + c;
            }
        }
    }
    return sum;
}