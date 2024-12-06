using System.Text.RegularExpressions;

// Regex feels like cheating

public class Day3 : Day
{
    public override string InputFile 
    {
        get { return "day3.txt"; }
        set { }
    }

    private string? inputFile = null;

    public override void FirstSolution()
    {
        if (inputFile == null)
            inputFile = ReadInputFile();

        const string pattern = @"mul\(\d{1,3}\,\d{1,3}\)";
        int result = 0;

        foreach (Match match in Regex.Matches(inputFile, pattern))
        {
            string[] values = match.Value.Remove(match.Value.Length -1, 1).Remove(0, 4).Split(',');
            result += int.Parse(values[0]) * int.Parse(values[1]);
        }

        Console.WriteLine($"Final Result: {result}");
    }

    public override void SecondSolution()
    {
        if (inputFile == null)
            inputFile = ReadInputFile();

        const string pattern = @"mul\(\d{1,3}\,\d{1,3}\)|do\(\)|don't\(\)";
        bool enabled = true;
        int result = 0;

        foreach (Match match in Regex.Matches(inputFile, pattern))
        {
            if (match.Value == "do()")
                enabled = true;
            else if (match.Value == "don't()")
                enabled = false;
            else if (enabled)
            {
                string[] values = match.Value.Remove(match.Value.Length -1, 1).Remove(0, 4).Split(',');
                result += int.Parse(values[0]) * int.Parse(values[1]);
            }
        }

        Console.WriteLine($"Final Result with conditionals: {result}");
    }
}
