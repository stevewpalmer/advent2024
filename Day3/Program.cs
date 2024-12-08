using System.Text.RegularExpressions;

string input = File.ReadAllText("puzzle.txt");
MatchCollection matches = Regex.Matches(input, @"mul\((\d+),(\d+)\)|do\(\)|don't\(\)");

int answer1Total = 0;
int answer2Total = 0;
bool enabled = true;
foreach (Match match in matches) {
    GroupCollection groups = match.Groups;
    if (groups[0].Value == "do()") {
        enabled = true;
        continue;
    }
    if (groups[0].Value == "don't()") {
        enabled = false;
        continue;
    }
    int result = int.Parse(groups[1].Value) * int.Parse(groups[2].Value);
    if (enabled) {
        answer2Total += result;
    }
    answer1Total += result;
}

Console.WriteLine($"Part 1 answer : {answer1Total}");
Console.WriteLine($"Part 2 answer : {answer2Total}");