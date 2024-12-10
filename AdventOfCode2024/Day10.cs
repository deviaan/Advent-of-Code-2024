using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Runtime.Intrinsics.X86;

namespace AdventOfCode2024;

class Day10: Day
{
    public override string InputFile { 
        get => "day10.txt"; 
        set { }
    }

    private readonly struct Coords(int x, int y) : IEquatable<Coords>
    {
        public int X { get; } = x;
        public int Y { get; } = y;

        public override string ToString() => $"({X}, {Y})";

        public override int GetHashCode() => (X, Y).GetHashCode();
        public override bool Equals(object? obj) => obj is Coords other && this.Equals(other);
        public bool Equals(Coords other) => X == other.X && Y == other.Y;
        
        public static Coords operator +(Coords left, Coords right) => new(left.X + right.X, left.Y + right.Y);
        public static Coords operator -(Coords left, Coords right) => new(left.X - right.X, left.Y - right.Y);
    }

    private static readonly Coords Up = new(0, -1);
    private static readonly Coords Down = new(0, 1);
    private static readonly Coords Right = new(1, 0);
    private static readonly Coords Left = new(-1, 0);

    private int[,] _trailMap = new int[0,0]; 

    protected override void FirstSolution()
    {
        ProcessInput();
        var validTrails = 0;

        foreach (var coord in TrailMapCoords())
        {
            if (!CheckPositionForValue(coord, 0)) continue;
            var uniqueTrailsEnd = new HashSet<Coords>(FindTrailEndPointFrom(coord));
            validTrails += uniqueTrailsEnd.Count;
        }

        Console.WriteLine($"Trail Score {validTrails}");
    }

    protected override void SecondSolution()
    {
        var validTrails = 0;

        foreach (var coord in TrailMapCoords())
            if (CheckPositionForValue(coord, 0))
                validTrails += FindTrailEndPointFrom(coord).Count;

        Console.WriteLine($"Trail Rating {validTrails}");
    }

    private void ProcessInput()
    {
        var inputFile = ReadInputFile();
        var lines = inputFile.Split('\n');
        _trailMap = new int[lines.Length, lines[0].Trim().ToCharArray().Length];

        for (var i = 0; i < lines.Length; i++)
        {
            var currentLine = lines[i].Trim().ToCharArray();
            for (var j = 0; j < currentLine.Length; j++)
                _trailMap[i, j] = int.Parse($"{currentLine[j]}");
        }
    }

    private IEnumerable<Coords> TrailMapCoords()
    {
        for (var i = 0; i < _trailMap.GetLength(0); i++)
        for (var j = 0; j < _trailMap.GetLength(1); j++)
            yield return new Coords(j, i);
    }

    private bool CoordsInBounds(Coords coords) => coords is { X: >= 0, Y: >= 0 } && 
                                                  coords.X < _trailMap.GetLength(1) &&
                                                  coords.Y < _trailMap.GetLength(0);
    

    private bool CheckPositionForValue(Coords coord, int value, Coords? direction = null) 
    {
        if (direction != null)
            coord += direction.Value;
        
        return CoordsInBounds(coord) && _trailMap[coord.Y, coord.X] == value;
    }

    private List<Coords> FindTrailEndPointFrom(Coords coord)
    {
        var trails = new List<Coords>();
        var nextValue = _trailMap[coord.Y, coord.X] + 1;

        foreach (var direction in (Coords[]) [Up, Down, Left, Right])
        {
            if (!CheckPositionForValue(coord, nextValue, direction))
                continue;

            if (nextValue == 9) trails.Add(coord + direction); 
            else trails.AddRange(FindTrailEndPointFrom(coord + direction));
        }
            
        return trails;
    }
}