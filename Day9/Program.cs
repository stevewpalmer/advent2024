List<char> input = File.ReadAllText("puzzle.txt").ToList();

const int FREEBLOCK = -1;

Console.WriteLine($"Part 1 answer: {Compact(1)}");
Console.WriteLine($"Part 2 answer: {Compact(2)}");
return;

long Compact(int part) {
    int fileId = 0;
    bool isFile = true;
    List<(int, int)> disk = [];
    foreach (char ch in input) {
        disk.Add((isFile ? fileId++ : FREEBLOCK, ch - '0'));
        isFile = !isFile;
    }

    int index = disk.Count - 1;
    while (index > 0) {
        (int type, int count) = disk[index];
        if (type != FREEBLOCK) {
            int c = 0;
            while (c < index) {
                if (disk[c].Item1 == FREEBLOCK && (part == 1 || (part == 2 && disk[c].Item2 >= count))) {
                    break;
                }
                c++;
            }
            if (c < index) {
                int remainder = disk[c].Item2 - count;
                if (remainder < 0) {

                    // Only for part 1, fit as much of the file into disk[c] as we can
                    // and shorten the original file to what is left. Then we try and fit
                    // the remainder on the next pass through the loop.
                    disk[c] = (type, count + remainder);
                    disk[index++] = (type, -remainder);
                }
                else {

                    // All the file fits into the free block at disk[c], so remove it
                    // from its original location. If there's any free space left over,
                    // create another node for the remainder.
                    disk[c] = (type, count);
                    if (remainder > 0) {
                        disk.Insert(c + 1, (FREEBLOCK, remainder));
                        index++;
                    }
                    disk[index] = (FREEBLOCK, count);
                }
            }
        }
        index--;
    }

    long checksum = 0;
    int p = 0;
    foreach ((int, int) d in disk) {
        if (d.Item1 != FREEBLOCK) {
            checksum += Enumerable.Range(p, d.Item2).Sum(m => (long)d.Item1 * m);
        }
        p += d.Item2;
    }
    return checksum;
}