namespace AdventOfCode2024;

public class Day11 : Day
{
    private class DictionaryCounter: Dictionary<string, long>
    {
        public void Add(string key)
        {
            if (!ContainsKey(key))
                this.Add(key, 0);
            this[key]++;
        }

        public new void Add(string key, long value)
        {
            if (ContainsKey(key))
                value += this[key];
            this[key] = value;
        }
    }
    
    public override string InputFile { get => "day11.txt"; set { } }

    protected override void FirstSolution()
    {
        var stones = ReadInputFile().Split(' ').ToList();
        const int blinks = 25;

        for (var i = 0; i < blinks; i++)
            stones = Blink(stones);

        Console.WriteLine($"Stone Count: {stones.Count}");
    }

    protected override void SecondSolution()
    {
        var stoneArray = ReadInputFile().Split(' ');
        var stones = new  DictionaryCounter();
        const int blinks = 75;

        foreach (var stone in stoneArray)
            stones.Add(stone);

        for (var i = 0; i < blinks; i++)
            stones = BetterBlink(stones);

        long total = 0;
        foreach (var stone in stones)
            total += stone.Value;
        
        Console.WriteLine($"Stone Count (better blink): {total}");
    }

    private static List<string> Blink(List<string> stones)
    {
        var newStones = new List<string>();

        foreach (var stone in stones)
        {
            if (stone == "0")
                newStones.Add("1");
            else if (stone.Length % 2 == 0)
            {
                var i = stone.Length / 2;
                newStones.Add(stone[..i]);
                newStones.Add($"{long.Parse(stone[i..])}");
            }
            else
                newStones.Add($"{long.Parse(stone) * 2024}");
        }

        return newStones;
    }
    
    private static DictionaryCounter BetterBlink(DictionaryCounter stones)
    {
        var newStones = new DictionaryCounter();

        foreach (var stone in stones)
        {
            if (stone.Key == "0")
                newStones.Add("1", stone.Value);
            else if (stone.Key.Length % 2 == 0)
            {
                var i = stone.Key.Length / 2;
                newStones.Add(stone.Key[..i], stone.Value);
                newStones.Add($"{long.Parse(stone.Key[i..])}", stone.Value);
            }
            else
                newStones.Add($"{long.Parse(stone.Key) * 2024}", stone.Value);
        }

        return newStones;
    }
}