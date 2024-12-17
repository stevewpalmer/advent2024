using System.Drawing;

const char WALL = '#';
const char BOX = 'O';
const char SPACE = '.';
const char ROBOT = '@';
const char LEFT = '[';
const char RIGHT = ']';

Point robot = new(-1, -1);

string[] input = File.ReadAllLines("puzzle.txt");

char[][] map = input.TakeWhile(l => !string.IsNullOrEmpty(l))
    .Select(line => line.ToCharArray())
    .ToArray();

char[] route = string.Join("", input.SkipWhile(l => !string.IsNullOrEmpty(l))
        .Select(line => line))
    .ToCharArray();

Console.WriteLine($"Part 1 answer: {Solve(map)}");
Console.WriteLine($"Part 2 answer: {Solve(Resize(map))}");
return;

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
    char tr = map[pt.Y][pt.X];
    if (tr == SPACE) {
        return true;
    }
    if (tr == BOX) {
        do {
            pt.Offset(dx, dy);
            tr = map[pt.Y][pt.X];
        } while (tr == BOX);
        return tr != WALL;
    }
    if (tr == WALL) {
        return false;
    }
    if (dx == 0) {
        if (tr == RIGHT) {
            return TryMove(map, new Point(pt.X, pt.Y), dx, dy) && TryMove(map, new Point(pt.X - 1, pt.Y), dx, dy);
        }
        if (tr == LEFT) {
            return TryMove(map, new Point(pt.X, pt.Y), dx, dy) && TryMove(map, new Point(pt.X + 1, pt.Y), dx, dy);
        }
    }
    else if (dx == -1) {
        if (tr == RIGHT) {
            return TryMove(map, new Point(pt.X - 1, pt.Y), dx, dy);
        }
    }
    else if (dx == 1) {
        if (tr == LEFT) {
            return TryMove(map, new Point(pt.X + 1, pt.Y), dx, dy);
        }
    }
    return false;
}

void Move(char[][] map, Point pt, int dx, int dy) {
    Point st = pt;
    st.Offset(dx, dy);
    char tr = map[st.Y][st.X];
    if (tr == SPACE) {
        (map[st.Y][st.X], map[pt.Y][pt.X]) = (map[pt.Y][pt.X], map[st.Y][st.X]);
    }
    else if (tr == BOX) {
        Move(map, st, dx, dy);
        (map[st.Y][st.X], map[pt.Y][pt.X]) = (map[pt.Y][pt.X], map[st.Y][st.X]);
    }
    else if (dx == 0) {
        if (tr == RIGHT) {
            Move(map, new Point(st.X, st.Y), dx, dy);
            Move(map, new Point(st.X - 1, st.Y), dx, dy);
            (map[st.Y][st.X], map[pt.Y][pt.X]) = (map[pt.Y][pt.X], map[st.Y][st.X]);
        }
        if (tr == LEFT) {
            Move(map, new Point(st.X, st.Y), dx, dy);
            Move(map, new Point(st.X + 1, st.Y), dx, dy);
            (map[st.Y][st.X], map[pt.Y][pt.X]) = (map[pt.Y][pt.X], map[st.Y][st.X]);
        }
    }
    else if (dx == -1) {
        if (tr == RIGHT) {
            Move(map, new Point(st.X - 1, st.Y), dx, dy);
            (map[st.Y][st.X - 1], map[st.Y][st.X], map[pt.Y][pt.X]) = (map[st.Y][st.X], map[pt.Y][pt.X], map[st.Y][st.X - 1]);
        }
    }
    else if (dx == 1) {
        if (tr == LEFT) {
            Move(map, new Point(st.X + 1, st.Y), dx, dy);
            (map[st.Y][st.X + 1], map[st.Y][st.X], map[pt.Y][pt.X]) = (map[st.Y][st.X], map[pt.Y][pt.X], map[st.Y][st.X + 1]);
        }
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
            Console.Write(map[r][c]);
            if (map[r][c] == BOX || map[r][c] == LEFT) {
                sum += 100 * r + c;
            }
        }
        Console.WriteLine();
    }
    return sum;
}