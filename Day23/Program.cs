string[] lines = File.ReadAllLines("puzzle.txt");
Dictionary<string, HashSet<string>> connections = [];

foreach (string line in lines) {
    string[] p = line.Split("-");
    connections.TryAdd(p[0], []);
    connections.TryAdd(p[1], []);
    connections[p[0]].Add(p[1]);
    connections[p[1]].Add(p[0]);
}

HashSet<string> results = [];
foreach (string x in connections.Keys) {
    foreach (string y in connections[x]) {
        foreach (string z in connections[y]) {
            if (connections[x].Contains(z) && (x.StartsWith('t') || y.StartsWith('t') || z.StartsWith('t'))) {
                List<string> t = ((string[])[x, y, z]).OrderBy(s => s).ToList();
                results.Add(string.Join(",", t));
            }
        }
    }
}

HashSet<string> cliques = [];
foreach (string node in connections.Keys) {
    BronKerbosch([node], [..connections[node]], [], cliques);
}

int answer1 = results.Count;
string answer2 = cliques.MaxBy(c => c.Length) ?? "";

Console.WriteLine($"Part 1 answer: {answer1}");
Console.WriteLine($"Part 2 answer: {answer2}");
return;

void BronKerbosch(HashSet<string> R, HashSet<string> P, HashSet<string> X, HashSet<string> O) {
    if (P.Count == 0 && X.Count == 0) {
        O.Add(string.Join(",", R.OrderBy(s => s)));
        return;
    }
    HashSet<string> PC = [..P];
    foreach (string v in P) {
        HashSet<string> C = connections[v];
        BronKerbosch([..R.Union([v])], [..PC.Intersect(C)], [..X.Intersect(C)], O);
        PC.Remove(v);
        X.Add(v);
    }
}