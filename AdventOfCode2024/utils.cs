namespace AdventOfCode2024;

public readonly struct Coords(int x, int y) : IEquatable<Coords>
{
    public int X { get; } = x;
    public int Y { get; } = y;

    public override string ToString() => $"({X}, {Y})";
    public override int GetHashCode() => (X, Y).GetHashCode();
    
    // Operation with other coords
    public static Coords operator +(Coords left, Coords right) => new(left.X + right.X, left.Y + right.Y);
    public static Coords operator -(Coords left, Coords right) => new(left.X - right.X, left.Y - right.Y);
    public static Coords operator % (Coords left, Coords right) => new (left.X % right.X, left.Y % right.Y);
    
    public static bool operator ==(Coords left, Coords right) => left.Equals(right);
    public static bool operator !=(Coords left, Coords right) => !(left == right);
    public override bool Equals(object? obj) => obj is Coords other && this.Equals(other);
    public bool Equals(Coords other) => X == other.X && Y == other.Y;
    
    // Operations with numbers
    public static Coords operator *(Coords left, int x) => new(left.X * x, left.Y * x);
    
    // Helpers
    public Coords WrapAround(Coords other) => 
        new Coords(this.X < 0 ? other.X + this.X : this.X, this.Y < 0 ? other.Y + this.Y : this.Y);

    public void Deconstruct(out int x, out int y)
    {
        x = this.X;
        y = this.Y;
    }
}

public static class Directions
{
    public static readonly Coords Up        = new (0, -1);
    public static readonly Coords Down      = new (0, 1);
    public static readonly Coords Left      = new (-1, 0);
    public static readonly Coords Right     = new (1, 0);

    public static IEnumerable<Coords> OrthogonalDirections()
    {
        yield return Up;
        yield return Down;
        yield return Left;
        yield return Right;
    }

    public static IEnumerable<Coords[]> DiagonalDirections()
    {
        yield return [Up, Left];
        yield return [Up, Right];
        yield return [Down, Left];
        yield return [Down, Right];
    }
}

public static class Helpers
{
    public static IEnumerable<Coords> WalkMatrix(char[,] matrix)
    {
        for (var i = 0; i < matrix.GetLength(0); i++)
        for (var j = 0; j < matrix.GetLength(1); j++)
            yield return new Coords(j, i);    
    }

    public static bool CoordsInBounds(char[,] matrix, Coords coords) => 
        coords is {X: >= 0, Y: >= 0} &&
        coords.Y < matrix.GetLength(0) &&
        coords.X < matrix.GetLength(1);
}