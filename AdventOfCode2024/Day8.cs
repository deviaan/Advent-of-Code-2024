namespace AdventOfCode2024;

class Day8 : Day
{
    public override string InputFile { 
        get => "day8.txt"; 
        set { }
    }

    private readonly Dictionary<char,  List<(int x, int y)>> _antennaPositions = [];
    private int _maxY = 0;
    private int _maxX = 0;

    protected override void FirstSolution()
    {
        ProcessInputFile();
        var antinodes = new HashSet<(int x, int y)>();

        foreach (var antennaPair in AntennaIterator())
        {
            var distance = SubtractAntenna(antennaPair.Item1, antennaPair.Item2);
            var antinode0 = AddAntenna(antennaPair.Item1, distance);
            var antinode1 = SubtractAntenna(antennaPair.Item2, distance);

            if (CoordinatesInRange(antinode0))
                antinodes.Add(antinode0);

            if (CoordinatesInRange(antinode1))
                    antinodes.Add(antinode1);
        }

        Console.WriteLine($"Unique Antinodes: {antinodes.Count}");
    }

    protected override void SecondSolution()
    {
        var antinodes = new HashSet<(int x, int y)>();
        bool inRange = true;

        foreach (var antinodePair in AntennaIterator())
        {
            var distance = SubtractAntenna(antinodePair.Item1, antinodePair.Item2);
            var antinode = antinodePair.Item1;
            antinodes.Add(antinode);

            do {
                antinode = AddAntenna(antinode, distance);
                if (CoordinatesInRange(antinode))
                    antinodes.Add(antinode);
                else
                    inRange = false;

            } while(inRange);

            antinode = antinodePair.Item2;
            inRange = true;
            antinodes.Add(antinode);

            do {
                antinode = SubtractAntenna(antinode, distance);
                if (CoordinatesInRange(antinode))
                    antinodes.Add(antinode);
                else
                    inRange = false;

            } while(inRange);
        }

        Console.WriteLine($"Unique Antinodes in line: {antinodes.Count}");
    }

    private void ProcessInputFile()
    {
        var inputFile = ReadInputFile();
        var lines = inputFile.Split('\n');
        _maxY = lines.Length;

        for (int y = 0; y < lines.Length; y++)
        {
            char[] currentLine = lines[y].Trim().ToCharArray();
            if (_maxX == 0)
                _maxX = currentLine.Length;

            for (int x = 0; x < currentLine.Length; x++)
            {
                if (currentLine[x] == '.')
                    continue;

                if (!_antennaPositions.ContainsKey(currentLine[x]))
                    _antennaPositions.Add(currentLine[x], []);

                _antennaPositions[currentLine[x]].Add((x, y));
            }
        }
    }

    private static (int x, int y) AddAntenna((int x, int y) first, (int x, int y) second)
    {
        return (first.x + second.x, first.y + second.y);
    }

    private static (int x, int y) SubtractAntenna((int x, int y) first, (int x, int y) second)
    {
        return (first.x - second.x, first.y - second.y);
    }

    private static IEnumerable<((int x, int y), (int x, int y))> AntennaPermutations(List<(int x, int y)> antennas)
    {
        foreach (var antenna0 in antennas)
            foreach (var antenna1 in antennas)
                if (antenna0 != antenna1)
                    yield return (antenna0, antenna1);
    }

    private IEnumerable<((int x, int y), (int x, int y))> AntennaIterator()
    {
        foreach (var antennaType in _antennaPositions)
            foreach (var antennaPair in AntennaPermutations(antennaType.Value))
                yield return antennaPair;
    }

    private bool CoordinatesInRange((int x, int y) coords)
    {
        return coords is { x: >= 0, y: >= 0} &&
            coords.x < _maxX &&
            coords.y < _maxY;
    }

    private void PrintMap(HashSet<(int x, int y)> antinodes)
    {
        char[,] map = new char[_maxY, _maxX];

        for (int i = 0; i < _maxY; i++)
            for (int j = 0; j < _maxX; j++)
                map[i, j] = '.';

        foreach (var antinode in antinodes)
            map[antinode.y, antinode.x] = '#';

        foreach (var antenna in _antennaPositions)
        {
            var antennaType = antenna.Key;
            foreach (var antennaCoords in antenna.Value)
                map[antennaCoords.y, antennaCoords.x] = antennaType;
        }

        for (int i = 0; i < map.GetLength(0); i++)
        {
            string line = "";
            for (int j = 0; j < map.GetLength(1); j++)
                line += map[i, j];
            Console.WriteLine(line);
        }
    }
}
