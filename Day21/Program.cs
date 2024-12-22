using System.Drawing;
using System.Text.RegularExpressions;

string[] codes = File.ReadAllLines("puzzle.txt");

Dictionary<char, Point> keyPad = new() {
    { '7', new Point(0, 0) },
    { '8', new Point(1, 0) },
    { '9', new Point(2, 0) },
    { '4', new Point(0, 1) },
    { '5', new Point(1, 1) },
    { '6', new Point(2, 1) },
    { '1', new Point(0, 2) },
    { '2', new Point(1, 2) },
    { '3', new Point(2, 2) },
    { '0', new Point(1, 3) },
    { 'A', new Point(2, 3) }
};
Dictionary<char, Point> controlPad = new() {
    { '^', new Point(1, 0) },
    { 'A', new Point(2, 0) },
    { '<', new Point(0, 1) },
    { 'v', new Point(1, 1) },
    { '>', new Point(2, 1) }
};
Dictionary<char, Point> directions = new() {
    { '^', new Point(0, -1) },
    { '<', new Point(-1, 0) },
    { 'v', new Point(0, 1) },
    { '>', new Point(1, 0) }
};

long answer1 = 0;
long answer2 = 0;

Dictionary<(string, Dictionary<char, Point>, Point, int), long> cache = new();

foreach (string code in codes) {
    int value = Convert.ToInt32(Regex.Replace(code, "[^0-9]", ""));
    answer1 += Calculate(code, keyPad, keyPad['A'], 2) * value;
    answer2 += Calculate(code, keyPad, keyPad['A'], 25) * value;
}

Console.WriteLine($"Part 1 answer: {answer1}");
Console.WriteLine($"Part 2 answer: {answer2}");
return;

long Calculate(string code, Dictionary<char, Point> pad, Point current, int depth) {
    if (cache.TryGetValue((code, pad, current, depth), out long result)) {
        return result;
    }
    if (code.Length == 0) {
        return 0;
    }
    Point target = pad[code[0]];

    string pathPoints = "";
    int dx = target.X - current.X;
    int dy = target.Y - current.Y;
    for (int d = 0; d < dx; d++) {
        pathPoints += '>';
    }
    for (int d = dx; d < 0; d++) {
        pathPoints += '<';
    }
    for (int d = 0; d < dy; d++) {
        pathPoints += 'v';
    }
    for (int d = dy; d < 0; d++) {
        pathPoints += '^';
    }
    long best = pathPoints.Length + 1;
    if (depth > 0) {
        List<long> tries = [];
        if (pathPoints.Length == 0) {
            tries.Add(Calculate("A", controlPad, controlPad['A'], depth - 1));
        }
        else {
            HashSet<string> allPaths = Permutations(pathPoints).ToHashSet();
            foreach (string path in allPaths) {
                Point step = current;
                bool valid = true;
                foreach (char dch in path) {
                    step.Offset(directions[dch]);
                    if (!pad.ContainsValue(step)) {
                        valid = false;
                        break;
                    }
                }
                if (valid) {
                    tries.Add(Calculate(path + "A", controlPad, controlPad['A'], depth - 1));
                }
            }
        }
        best = tries.Min();
    }
    result = best + Calculate(code[1..], pad, target, depth);
    cache.TryAdd((code, pad, current, depth), result);
    return result;
}

List<string> Permutations(string s) {
    List<string> list = [];
    DoPermute(s, 0, s.Length - 1, list);
    return list;
}

void DoPermute(string s, int start, int end, List<string> list) {
    if (start == end) {
        list.Add(s);
    }
    else {
        char[] chStr = s.ToCharArray();
        for (int i = start; i <= end; i++) {
            (chStr[start], chStr[i]) = (chStr[i], chStr[start]);
            DoPermute(new string(chStr), start + 1, end, list);
            (chStr[start], chStr[i]) = (chStr[i], chStr[start]);
        }
    }
}