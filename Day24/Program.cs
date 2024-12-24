string[] input = File.ReadAllText("puzzle.txt").Split("\n\n");
Dictionary<string, string> registers = [];

foreach (string line in input[0].Split('\n')) {
    string[] parts = line.Split(": ");
    registers[parts[0]] = parts[1];
}
foreach (string line in input[1].Split('\n')) {
    string[] parts = line.Split(" -> ");
    registers[parts[1]] = parts[0];
}

long answer1 = Expand();
Console.WriteLine($"Part 1 answer: {answer1}");
return;

long Expand() =>
    registers.Keys.Where(s => s.StartsWith('z')).OrderByDescending(s => s)
        .Aggregate<string, long>(0, (current, name) => current * 2 + Evaluate(name));

int Evaluate(string name) {
    string value = registers[name];
    if (int.TryParse(value, out int result)) {
        return result;
    }
    string[] parts = value.Split(' ');
    int op1 = Evaluate(parts[0]);
    int op2 = Evaluate(parts[2]);
    return parts[1] switch {
        "XOR" => op1 ^ op2,
        "AND" => op1 & op2,
        _ => op1 | op2
    };
}