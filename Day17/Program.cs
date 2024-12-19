int regA = 0;
int regB = 0;
int regC = 0;
int[] data = [];
List<int> outp = [];

foreach (string input in File.ReadLines("puzzle.txt")) {
    string[] parts = input.Split(":");
    switch (parts[0]) {
        case "Register A": regA = int.Parse(parts[1]); break;
        case "Register B": regB = int.Parse(parts[1]); break;
        case "Register C": regC = int.Parse(parts[1]); break;
        case "Program": data = parts[1].Split(",").Select(int.Parse).ToArray(); break;
    }
}

string answer1 = Run(regA);
Disassemble(data);

Console.WriteLine($"Part 1 answer: {answer1}");
return;

int combo(int operand) => ((int[]) [0, 1, 2, 3, regA, regB, regC])[operand];

void Disassemble(int[] program) {
    for (int pc = 0; pc < program.Length;) {
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

string Run(int input) {
    regA = input;
    for (int pc = 0; pc < data.Length;) {
        int opcode = data[pc++];
        int operand = data[pc++];
        switch (opcode) {
            case 0:
                regA /= 1 << combo(operand);
                break;
            case 1:
                regB ^= operand;
                break;
            case 2:
                regB = combo(operand) % 8;
                break;
            case 3:
                if (regA > 0) {
                    pc = operand;
                }
                break;
            case 4:
                regB ^= regC;
                break;
            case 5:
                outp.Add(combo(operand) % 8);
                break;
            case 6:
                regB = regA / (1 << combo(operand));
                break;
            case 7:
                regC = regA / (1 << combo(operand));
                break;
        }
    }
    return string.Join(",", outp);
}