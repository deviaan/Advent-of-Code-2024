using System.Text.RegularExpressions;

// Regex feels like cheating

namespace AdventOfCode2024;

public class Day3 : Day
{
    public override string InputFile 
    {
        get => "day3.txt";
        set { }
    }

    private string? _inputFile = null;

    protected override void FirstSolution()
    {
        _inputFile ??= ReadInputFile();

        const string pattern = @"mul\(\d{1,3}\,\d{1,3}\)";
        var result = 0;

        foreach (Match match in Regex.Matches(_inputFile, pattern))
        {
            var values = match.Value.Remove(match.Value.Length -1, 1).Remove(0, 4).Split(',');
            result += int.Parse(values[0]) * int.Parse(values[1]);
        }

        Console.WriteLine($"Final Result: {result}");
    }

    protected override void SecondSolution()
    {
        _inputFile ??= ReadInputFile();

        const string pattern = @"mul\(\d{1,3}\,\d{1,3}\)|do\(\)|don't\(\)";
        var enabled = true;
        var result = 0;

        foreach (Match match in Regex.Matches(_inputFile, pattern))
        {
            switch (match.Value)
            {
                case "do()":
                    enabled = true;
                    break;
                case "don't()":
                    enabled = false;
                    break;
                default:
                {
                    if (enabled)
                    {
                        var values = match.Value.Remove(match.Value.Length -1, 1).Remove(0, 4).Split(',');
                        result += int.Parse(values[0]) * int.Parse(values[1]);
                    }

                    break;
                }
            }
        }

        Console.WriteLine($"Final Result with conditionals: {result}");
    }
}