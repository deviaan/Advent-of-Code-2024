using System.Text.RegularExpressions;

namespace AdventOfCode2024;

public partial class Day14 : Day
{
    public override string InputFile { get => "day14.txt"; set { }}

    private List<(Coords position, Coords velocity)> _robots = [];
    private const int Height = 103;
    private const int Width = 101;
    private static readonly Coords MaxCoords = new Coords(Width, Height);

    protected override void FirstSolution()
    {
        ProcessInput();
        
        var quadrants = new int[4];
        const int iterations = 100;
        
        foreach (var robot in _robots)
        {
            var endPosition = ((robot.position + robot.velocity * iterations) % MaxCoords).WrapAround(MaxCoords);
            AddToQuadrant(endPosition, quadrants);
        }
        
        Console.WriteLine($"Safety Score: {quadrants[0] * quadrants[1] * quadrants[2] * quadrants[3]}");
    }

    protected override void SecondSolution()
    {
        var robotCoords = new Coords[_robots.Count];
        HashSet<Coords> uniquePos;
        var seconds = 0;


        do
        {
            for (var i = 0; i < _robots.Count; i++)
            {
                var robot = _robots[i];
                robotCoords[i] = ((robot.position + robot.velocity * seconds) % MaxCoords).WrapAround(MaxCoords);
            }

            uniquePos = new HashSet<Coords>(robotCoords);


            seconds++;

        } while (seconds < Height * Width && uniquePos.Count != robotCoords.Length);

        DrawPositions(robotCoords);
        Console.WriteLine($"Seconds: {seconds - 1}");

    }

    [GeneratedRegex(@"^p=(?<px>-?\d+),(?<py>-?\d+) v=(?<vx>-?\d+),(?<vy>-?\d+)$")]
    private static partial Regex RobotInput();
    
    private void ProcessInput()
    {
        var input = ReadInputFile();
        foreach (var line in input.Split('\n'))
        {
            var match = RobotInput().Match(line.Trim());
            var position = new Coords(int.Parse(match.Groups["px"].Value), int.Parse(match.Groups["py"].Value));
            var velocity = new Coords(int.Parse(match.Groups["vx"].Value), int.Parse(match.Groups["vy"].Value));
            _robots.Add((position, velocity));
        }
    }

    private int? AddToQuadrant(Coords position, int[] quadrants)
    {
        const int midWidth = Width / 2;
        const int midHeight = Height / 2;

        return position switch
        {
            { X: < midWidth, Y: < midHeight } => quadrants[0]++,
            { X: > midWidth, Y: < midHeight } => quadrants[1]++,
            { X: < midWidth, Y: > midHeight } => quadrants[2]++,
            { X: > midWidth, Y: > midHeight } => quadrants[3]++,
            _ => null
        };
    }

    private void DrawPositions(Coords[] robots)
    {
        var drawing = new char?[Height, Width];
        foreach (var robot in robots)
            drawing[robot.Y, robot.X] = '@';

        Console.WriteLine();
        for (var i = 0; i < drawing.GetLength(0); i++)
        {
            for (var j = 0; j < drawing.GetLength(1); j++)
            {
                var glyph = drawing[i, j] == null ? '.': drawing[i, j];
                Console.Write($"{glyph}");
            }

            Console.WriteLine();
        }
    }
}