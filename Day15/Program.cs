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
        Point pt = robot;
        char tr = map[pt.Y + dy][pt.X + dx];
        if (tr == WALL) {
            continue;
        }
        if (tr == BOX) {
            do {
                pt.Offset(dx, dy);
            } while (map[pt.Y][pt.X] == BOX);
            if (map[pt.Y][pt.X] == WALL) {
                continue;
            }
            while (pt != robot) {
                map[pt.Y][pt.X] = map[pt.Y - dy][pt.X - dx];
                pt.Offset(-dx, -dy);
            }
            tr = SPACE;
        }
        if (tr == SPACE) {
            pt.Offset(dx, dy);
        }
        map[robot.Y][robot.X] = SPACE;
        map[pt.Y][pt.X] = ROBOT;
        robot = pt;
    }
    int sum = 0;
    for (int r = 0; r < map.Length; r++) {
        for (int c = 0; c < map[r].Length; c++) {
            if (map[r][c] == BOX) {
                sum += 100 * r + c;
            }
        }
    }
    return sum;
}