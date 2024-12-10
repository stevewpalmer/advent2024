using System.Drawing;

char [][] map = File.ReadAllLines("puzzle.txt").Select(l => l.ToCharArray()).ToArray();
int w = map[0].Length;
int h = map.Length;

int answer1 = 0;
int answer2 = 0;
for (int y = 0; y < h; y++) {
    for (int x = 0; x < w; x++) {
        if (map[y][x] == '0') {
            answer1 += Walk(x, y, 1);
            answer2 += Walk(x, y, 2);
        }
    }
}

Console.WriteLine($"Part 1: {answer1}");
Console.WriteLine($"Part 2: {answer2}");
return;

int Walk(int x, int y, int part) {
    HashSet<Point> found = [];
    int distinct = 0;
    TryWalk(-1, x, y, ref found, ref distinct);
    return part == 1 ? found.Count : distinct;
}

void TryWalk(int elevation, int x, int y, ref HashSet<Point> count, ref int distinct) {
    if (x < 0 || y < 0 || x == w || y == h) {
        return;
    }
    int step = map[y][x] - '0';
    if (step != elevation + 1) {
        return;
    }
    if (step == 9) {
        distinct++;
        count.Add(new Point(x, y));
        return;
    }
    TryWalk(step, x - 1, y, ref count, ref distinct);
    TryWalk(step, x, y - 1, ref count, ref distinct);
    TryWalk(step, x + 1, y, ref count, ref distinct);
    TryWalk(step, x, y + 1, ref count, ref distinct);
}