namespace AdventOfCode2024;

public class Day12 : Day
{
    public override string InputFile { get => "day12.txt"; set { }}

    private class Region(char value)
    {
        public char Value { get; } = value;
        private readonly HashSet<Coords> _plots = [];

        public void AddPlot(Coords plot) => _plots.Add(plot);
        public bool Contains(Coords plot) => _plots.Contains(plot);
        public int Area() => _plots.Count;

        public int Perimeter()
        {
            var perimeter = 0;
            
            foreach (var plot in _plots)
            foreach (var direction in Directions.OrthogonalDirections())
                if (!Contains(plot + direction))
                    perimeter++;

            return perimeter;
        }

        public int Corners()
        {
            var corners = 0;
            
            foreach (var plot in _plots)
            foreach (var cornerDirection in Directions.DiagonalDirections())
                if (PlotIsOnCorner(plot, cornerDirection))
                    corners++;

            return corners;
        }

        private bool PlotIsOnCorner(Coords plot, Coords[] corner)
        {
            var containsDiagonal = Contains(plot + corner[0] + corner[1]);
            var containsOrthogonal0 = Contains(plot + corner[0]);
            var containsOrthogonal1 = Contains(plot + corner[1]);
            
            return
                (containsDiagonal && !containsOrthogonal0 && !containsOrthogonal1) ||
                (!containsDiagonal && containsOrthogonal0 == containsOrthogonal1);
        }
}

    private static char[,] _garden = new char[0, 0];
    private readonly List<Region> _regions = [];
    
    protected override void FirstSolution()
    {
        ProcessInput();
        foreach (var plot in Helpers.WalkMatrix(_garden))
            if (!PlotInRegion(plot))
                _regions.Add(BuildRegion(plot));

        var price = 0;
        foreach (var region in _regions)
            price += region.Area() * region.Perimeter();
        
        Console.WriteLine($"Total Price: {price}");
    }

    protected override void SecondSolution()
    {
        var price = 0;
        foreach (var region in _regions)
            price += region.Area() * region.Corners();

        Console.WriteLine($"Total Bulk Price: {price}");
    }

    private void ProcessInput()
    {
        var input = ReadInputFile();
        var lines = input.Split('\n');
        _garden = new char[lines.Length, lines[0].Trim().Length];
        
        for (var y = 0; y < _garden.GetLength(0); y++)
        for (var x = 0; x < _garden.GetLength(1); x++)
            _garden[y, x] = lines[y][x];
    }

    private static Region BuildRegion(Coords coords, Region? region = null)
    {
        var value = _garden[coords.Y, coords.X];
        region ??= new Region(value);
        region.AddPlot(coords);

        foreach (var direction in Directions.OrthogonalDirections())
        {
            var lookAt = coords + direction;
            
            if (
                !Helpers.CoordsInBounds(_garden, lookAt) || 
                _garden[lookAt.Y, lookAt.X] != value ||
                region.Contains(lookAt)
            )
                continue;
            
            BuildRegion(lookAt, region);
        }

        return region;
    }

    private bool PlotInRegion(Coords plot)
    {
        foreach (var region in _regions)
            if (region.Contains(plot))
                return true;

        return false;
    }
}