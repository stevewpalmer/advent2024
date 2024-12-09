using System.Drawing;

Dictionary<Point, char> map = new();
char[][] grid = File.ReadAllLines("puzzle.txt").Select(l => l.ToCharArray()).ToArray();
for (int y = 0; y < grid.Length; y++) {
    for (int x = 0; x < grid[y].Length; x++) {
        if (grid[y][x] != '.') {
            map[new Point(x, y)] = grid[y][x];
        }
    }
}
Rectangle bounds = new(0, 0, grid[0].Length, grid.Length);
HashSet<Point> antinodesPart1 = [];
HashSet<Point> antinodesPart2 = [];

foreach ((Point mp, char freq) in map) {
    Point[] antennas = map.Where(p => p.Value == freq && p.Key != mp).Select(p => p.Key).ToArray();
    foreach (Point antenna in antennas) {
        antinodesPart2.Add(antenna);
        antinodesPart2.Add(mp);

        Point offset1 = new(antenna.X - mp.X, antenna.Y - mp.Y);
        Point offset2 = new(mp.X - antenna.X, mp.Y - antenna.Y);

        foreach ((Point p, Point o) in ((Point, Point)[])[(antenna, offset1), (mp, offset2)]) {
            p.Offset(o);
            if (bounds.Contains(p)) {
                antinodesPart1.Add(p);
                do {
                    antinodesPart2.Add(p);
                    p.Offset(o);
                } while (bounds.Contains(p));
            }
        }
    }
}
Console.WriteLine($"Part 1 answer : {antinodesPart1.Count}");
Console.WriteLine($"Part 2 answer : {antinodesPart2.Count}");