List<int> list1 = [];
List<int> list2 = [];

foreach (string line in File.ReadAllLines("puzzle.txt")) {
    string[] parts = line.Split("  ");
    list1.Add(int.Parse(parts[0]));
    list2.Add(int.Parse(parts[1]));
}
list1.Sort();
list2.Sort();

int sumDistance = 0;
int similarityScore = 0;
for (int i = 0; i < list1.Count; i++) {
    sumDistance += Math.Abs(list1[i] - list2[i]);
    similarityScore += list1[i] * list2.Count(c => c == list1[i]);
}

Console.WriteLine($"Part 1 answer : {sumDistance}");
Console.WriteLine($"Part 2 answer : {similarityScore}");