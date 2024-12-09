namespace AdventOfCode2024;

class Day9: Day
{
    private readonly struct Block(int index, int size, int? value)
    {
        public int Index { get; } = index;
        public int Size { get; } = size;
        public int? Value { get; } = value;
    }

    public override string InputFile { 
        get => "day9.txt"; 
        set { }
        }

    private enum BlockType
    {
        Free,
        File,
    }

    protected override void FirstSolution()
    {
        var initialDisk = ReadInputFile();
        var disk = Decompress(initialDisk);
        disk = Fragment(disk);
        Console.WriteLine($"Checksum is {Checksum(disk)}");
    }

    protected override void SecondSolution()
    {
        var initialDisk= ReadInputFile();
        var disk = Decompress(initialDisk);
        disk = Defragment(disk);

        Console.WriteLine($"Checksum is {Checksum(disk)}");
    }

    private static int?[] Decompress(string disk)
    {
        var output = new List<int?>();
        var fileFlag = true;
        var fileId = 0;

        foreach (var c in disk.ToArray())
        {
            var blockSize = int.Parse($"{c}");
            for (var i = 0; i < blockSize; i++)
                output.Add(fileFlag ? fileId : null);

            if (fileFlag)
                fileId++;

            fileFlag = !fileFlag;
        }

        var outputArray = new int?[output.Count];
        for (var i = 0; i < output.Count; i++)
            outputArray[i] = output[i];

        return outputArray;
    }

    private static int?[] Fragment(int?[] disk)
    {
        var leftIndex = 0;
        var rightIndex = disk.Length - 1;

        do 
        {
            if (disk[leftIndex] == null)
            {
                // Get the next non-null point
                while(disk[rightIndex] == null && rightIndex > leftIndex)
                    rightIndex--;

                // We've gone back on our already clean data
                if (rightIndex < leftIndex)
                    break;

                disk[leftIndex] = disk[rightIndex];
                disk[rightIndex] = null;

                rightIndex--;
            }

            leftIndex++;

        } while (leftIndex < rightIndex);

        return disk;
    }

    private static int?[] Defragment(int?[] disk)
    {
        var lastFreeBlockIndex = 0;
        foreach (var fileBlock in GetContiguousBlockOfType(disk, BlockType.File, stopIndex: lastFreeBlockIndex))
            foreach (var freeBlock in GetContiguousBlockOfType(disk, BlockType.Free, stopIndex: fileBlock.Index))
            if (freeBlock.Size >= fileBlock.Size)
            {
                SwapBlocks(disk, fileBlock, freeBlock);
                lastFreeBlockIndex = freeBlock.Index + freeBlock.Size;
                break;
            }

        return disk;
    }

    private static IEnumerable<Block> GetContiguousBlockOfType(int?[] disk, BlockType blockType, int? stopIndex = null)
    {
        var (index, offset) = blockType == BlockType.File ? (disk.Length - 1, -1) : (0, 1);
        do
        {
            if ((blockType == BlockType.File && disk[index] == null) ||
                (blockType == BlockType.Free && disk[index] != null))
            {
                index += offset;
                continue;
            }

            var block = GetContiguousBlock(disk, index, reverse: blockType == BlockType.File);
            yield return block;
            index += offset * block.Size;

        } while (blockType == BlockType.File ? 
                     index < disk.Length && (stopIndex == null || index > stopIndex) :  
                     index >= 0 && (stopIndex == null || index < stopIndex)
         );
    }

    private static Block GetContiguousBlock(int?[] disk, int initialIndex, bool reverse = false)
    {
        var currentValue = disk[initialIndex];
        var offset = reverse ? -1 : 1;
        var size = 1;
        var index = initialIndex + (1 * offset);

        while (reverse ? index >= 0 : index < disk.Length)
        {
            if (currentValue == disk[index])
            {
                size++;
                index += offset;
            }
            else
                break;
        }

        var finalIndex = reverse ? index + 1 : initialIndex;
        return new Block(finalIndex, size, currentValue);
    }

    private static void SwapBlocks(int?[] disk,  Block block0, Block block1)
    {
        for (var i = 0; i < block0.Size; i++)
        {
            disk[block0.Index + i] = block1.Value;
            disk[block1.Index + i] = block0.Value;
        }
    }

    private static long Checksum(int?[] disk)
    {
        long total = 0;

        for (var i = 0; i < disk.Length; i++)
        {
            if (disk[i] == null)
                continue;

            total += i * int.Parse($"{disk[i]}");
        }

        return total;
    }

    private static void PrintDisk(int?[] disk)
    {
        Console.WriteLine($"{string.Join('-', disk)}");
    }
}