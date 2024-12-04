using System.Drawing;

char[][] grid = File.ReadLines("day4.txt").Select(line => line.ToCharArray()).ToArray();
Rectangle gridBounds = new Rectangle(0, 0, grid[0].Length, grid.Length);

int countOfMatches1 = 0;
int countOfMatches2 = 0;

char [] xmas = "XMAS".ToCharArray();
char [] x_mas = "MAS".ToCharArray();
char [] x_sam = "SAM".ToCharArray();

for (int y = 0; y < grid.Length; y++) {
    for (int x = 0; x < grid[0].Length; x++) {
        if (grid[y][x] == xmas[0]) {
            countOfMatches1 += Match(xmas, x, y, 1, 0) +
                               Match(xmas, x, y, -1, 0) +
                               Match(xmas, x, y, 0, 1) +
                               Match(xmas, x, y, 0, -1) +
                               Match(xmas, x, y, 1, 1) +
                               Match(xmas, x, y, -1, -1) +
                               Match(xmas, x, y, 1, -1) +
                               Match(xmas, x, y, -1, 1);
        }
        if (grid[y][x] == x_mas[0] || grid[y][x] == x_sam[0]) {
            countOfMatches2 += Match(x_mas, x, y, 1, 1) * Match(x_mas, x + 2, y, -1, 1) +
                               Match(x_mas, x, y, 1, 1) * Match(x_sam, x + 2, y, -1, 1) +
                               Match(x_sam, x, y, 1, 1) * Match(x_mas, x + 2, y, -1, 1) +
                               Match(x_sam, x, y, 1, 1) * Match(x_sam, x + 2, y, -1, 1);
        }
    }
}

Console.WriteLine($"Puzzle 1 answer : {countOfMatches1}");
Console.WriteLine($"Puzzle 2 answer : {countOfMatches2}");
return;

int Match(char[] matchString, int x, int y, int dx, int dy) {
    int index = 0;
    while (index < matchString.Length) {
        if (!gridBounds.Contains(x, y)) {
            return 0;
        }
        if (grid[y][x] != matchString[index++]) {
            return 0;
        }
        x += dx;
        y += dy;
    }
    return 1;
}
