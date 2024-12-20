using System.Drawing;

const int width = 71;
const int height = 71;

string[] input = File.ReadAllLines("puzzle.txt");
char[,] maze = new char[height, width];
Point start = new(0, 0);
Point end = new(width - 1, height - 1);

int l = 0;
while (l < 1024) {
    int[] coords = input[l].Split(',').Select(int.Parse).ToArray();
    maze[coords[1], coords[0]] = '#';
    l++;
}
int answer1 = Walk();

while (l < input.Length) {
    int[] coords = input[l].Split(',').Select(int.Parse).ToArray();
    maze[coords[1], coords[0]] = '#';
    if (Walk() == int.MaxValue) {
        break;
    }
    l++;
}
string answer2 = input[l];

Console.WriteLine($"Part 1 answer: {answer1}");
Console.WriteLine($"Part 2 answer: {answer2}");
return;

int Walk() {
    (int dx, int dy)[] directions = [(0, -1), (1, 0), (0, 1), (-1, 0)];
    PriorityQueue<(Point, int), int> queue = new();
    HashSet<Point> visited = [];
    queue.Enqueue((start, 0), 0);

    int best = int.MaxValue;
    while (queue.TryDequeue(out var element, out int cost)) {
        (Point pt, int _) = element;
        if (pt == end) {
            best = cost;
            break;
        }
        if (!visited.Add(pt)) {
            continue;
        }
        foreach ((int dx, int dy) in directions) {
            Point step = new(pt.X + dx, pt.Y + dy);
            if (step is { Y: >= 0 and < height, X: >= 0 and < width } && maze[step.Y, step.X] != '#') {
                queue.Enqueue((step, cost + 1), cost + 1);
            }
        }
    }
    return best;
}