char[][] input = File.ReadAllLines("puzzle.txt").Select(l => l.ToCharArray()).ToArray();
int w = input[0].Length;
int h = input.Length;
List<(int area, int perimeter, int corners)> regions = [];
List<(int x, int y)> visited = [];

for (int y = 0; y < h; y++) {
    for (int x = 0; x < w; x++) {
        char ch = input[y][x];
        List<(int x, int y)> plot = [];
        (int area, int perimeter) result = Walk(ch, x, y, plot);
        if (result.area > 0) {
            regions.Add((result.area, result.perimeter, CountCorners(plot)));
        }
    }
}
int answer1 = 0;
int answer2 = 0;

foreach ((int area, int perimeter, int corners) plot in regions) {
    answer1 += plot.area * plot.perimeter;
    answer2 += plot.area * plot.corners;
}

Console.WriteLine($"Part 1 answer: {answer1}");
Console.WriteLine($"Part 2 answer: {answer2}");
return;

int CountCorners(List<(int x, int y)> region) {
    int corners = 0;
    foreach ((int x, int y) in region) {
        (int, int) left = (x - 1, y);
        (int, int) right = (x + 1, y);
        (int, int) top = (x, y - 1);
        (int, int) bottom = (x, y + 1);
        (int, int) top_left = (x - 1, y - 1);
        (int, int) top_right = (x + 1, y - 1);
        (int, int) bottom_left = (x - 1, y + 1);
        (int, int) bottom_right = (x + 1, y + 1);
        if (!region.Contains(left) && !region.Contains(top)) {
            ++corners;
        }
        if (!region.Contains(right) && !region.Contains(top)) {
            ++corners;
        }
        if (!region.Contains(right) && !region.Contains(bottom)) {
            ++corners;
        }
        if (!region.Contains(left) && !region.Contains(bottom)) {
            ++corners;
        }
        if (region.Contains(right) && !region.Contains(bottom_right) && region.Contains(bottom)) {
            ++corners;
        }
        if (region.Contains(right) && !region.Contains(top_right) && region.Contains(top)) {
            ++corners;
        }
        if (region.Contains(left) && !region.Contains(top_left) && region.Contains(top)) {
            ++corners;
        }
        if (region.Contains(left) && !region.Contains(bottom_left) && region.Contains(bottom)) {
            ++corners;
        }
    }
    return corners;
}

(int, int) Walk(char ch, int x, int y, List<(int x, int y)> plot) {
    if (visited.Contains((x, y))) {
        return (0, 0);
    }
    plot.Add((x, y));
    visited.Add((x, y));
    (int area, int perimeter) = (1, 4);
    if (x < w - 1 && input[y][x + 1] == ch) {
        (int area, int perimeter) result = Walk(ch, x + 1, y, plot);
        area += result.area;
        perimeter += result.perimeter - 1;
    }
    if (x > 0 && input[y][x - 1] == ch) {
        (int area, int perimeter) result = Walk(ch, x - 1, y, plot);
        area += result.area;
        perimeter += result.perimeter - 1;
    }
    if (y < h - 1 && input[y + 1][x] == ch) {
        (int area, int perimeter) result = Walk(ch, x, y + 1, plot);
        area += result.area;
        perimeter += result.perimeter - 1;
    }
    if (y > 0 && input[y - 1][x] == ch) {
        (int area, int perimeter) result = Walk(ch, x, y - 1, plot);
        area += result.area;
        perimeter += result.perimeter - 1;
    }
    return (area, perimeter);
}