long regA = 0;
long regB = 0;
long regC = 0;
List<int> data = [];

foreach (string input in File.ReadLines("puzzle.txt")) {
    string[] parts = input.Split(":");
    switch (parts[0]) {
        case "Register A": regA = int.Parse(parts[1]); break;
        case "Register B": regB = int.Parse(parts[1]); break;
        case "Register C": regC = int.Parse(parts[1]); break;
        case "Program": data = parts[1].Split(",").Select(int.Parse).ToList(); break;
    }
}

Disassemble(data);

string answer1 = string.Join(",", Run(regA, regB, regC));
long answer2 = Solve(0, 0);

Console.WriteLine($"Part 1 answer: {answer1}");
Console.WriteLine($"Part 2 answer: {answer2}");
return;

void Disassemble(List<int> program) {
    for (int pc = 0; pc < program.Count;) {
        int opcode = program[pc];
        int operand = program[pc + 1];
        string op2 = ((string[]) ["0", "1", "2", "3", "A", "B", "C"])[operand];
        string op1 = ((string[]) [
            $"A = A / (1 << {op2})",
            $"B = B ^ {op2}",
            $"B = {op2} % 8",
            $"IF A > 0 GOTO {op2}",
            "B = C ^ B",
            $"WRITE {op2} % 8",
            $"B = A / (1 << {op2})",
            $"C = A / (1 << {op2})"
        ])[opcode];
        Console.WriteLine($"{pc:D2} : {op1}");
        pc += 2;
    }
}

long Solve(long a, int depth) {
    if (depth == data.Count) {
        return a;
    }
    for (int n = 0; n < 8; n++) {
        List<int> output = Run(a * 8 + n, regB, regC);
        if (output.Count > 0 && output[0] == data[^(depth + 1)]) {
            long result = Solve(a * 8 + n, depth + 1);
            if (result > 0) {
                return result;
            }
        }
    }
    return 0;
}

List<int> Run(long a, long b, long c) {
    List<int> output = [];
    for (int pc = 0; pc < data.Count;) {
        long[] combo = [0, 1, 2, 3, a, b, c, 999999];
        int opcode = data[pc++];
        int operand = data[pc++];
        switch (opcode) {
            case 0:
                a /= (long)Math.Pow(2, combo[operand]);
                break;
            case 1:
                b ^= operand;
                break;
            case 2:
                b = combo[operand] % 8;
                break;
            case 3:
                if (a > 0) {
                    pc = operand;
                }
                break;
            case 4:
                b ^= c;
                break;
            case 5:
                output.Add((int)(combo[operand] % 8));
                break;
            case 6:
                b = a / (long)Math.Pow(2, combo[operand]);
                break;
            case 7:
                c = a / (long)Math.Pow(2, combo[operand]);
                break;
        }
    }
    return output;
}