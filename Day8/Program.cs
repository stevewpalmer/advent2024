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
    Point[] antennas = map.Where(p => p.Value == freq).Select(p => p.Key).ToArray();
    foreach (Point antenna in antennas.Where(p => p != mp)) {
        antinodesPart2.Add(antenna);
        antinodesPart2.Add(mp);

        Point offset = new(antenna.X - mp.X, antenna.Y - mp.Y);
        Point antinode = new(antenna.X + offset.X, antenna.Y + offset.Y);

        if (bounds.Contains(antinode)) {
            antinodesPart1.Add(antinode);
            while (bounds.Contains(antinode)) {
                antinodesPart2.Add(antinode);
                antinode = new Point(antinode.X + offset.X, antinode.Y + offset.Y);
            }
        }

        offset = new Point(mp.X - antenna.X, mp.Y - antenna.Y);
        antinode = new Point(mp.X + offset.X, mp.Y + offset.Y);

        if (bounds.Contains(antinode)) {
            antinodesPart1.Add(antinode);
            while (bounds.Contains(antinode)) {
                antinodesPart2.Add(antinode);
                antinode = new Point(antinode.X + offset.X, antinode.Y + offset.Y);
            }
        }
    }
}
Console.WriteLine($"Part 1 answer : {antinodesPart1.Count}");
Console.WriteLine($"Part 2 answer : {antinodesPart2.Count}");