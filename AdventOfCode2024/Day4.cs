namespace AdventOfCode2024;

public class Day4 : Day {
    public override string InputFile { 
        get => "day4.txt";
        set { }
    }

    private const string XMAS = "XMAS";
    private int _vertical = 0;
    private int _horizontal = 0;
    // Forward, Backward, Down, Up, Up Right, Up Left, Down Right, Down Left
    private readonly int[,] _searchDirections = { {0, 1}, {0, -1}, {1, 0}, {-1, 0}, {1, 1}, {1, -1}, {-1, 1}, {-1, -1} };
    private char[,]? _charMatrix = null;

    protected override void FirstSolution()
    {
        ProcessInputFile();
        if (_charMatrix == null)
        {
            Console.WriteLine("Could not read input");
            return;
        }

        var wordsFound = 0;

        for (var i = 0; i < _vertical; i++)
        {
            for (var j = 0; j < _horizontal; j++)
            {
                for (var k = 0; k < _searchDirections.GetLength(0); k++)
                {
                    if (
                        _charMatrix[i, j] == 'X' &&
                        Search(_charMatrix, i, j, _searchDirections[k, 0], _searchDirections[k, 1])
                    )
                        wordsFound++;
                }
            }
        }

        Console.WriteLine($"XMAS found: {wordsFound}");
    }

    protected override void SecondSolution()
    {
        ProcessInputFile();
        if (_charMatrix == null)
        {
            Console.WriteLine("Could not read input");
            return;
        }

        int wordsFound = 0;

        for (int i = 0; i < _vertical; i++)
        {
            for (int j = 0; j < _horizontal; j++)
            {
                if (
                    _charMatrix[i, j] == 'A' &&
                    XSearch(_charMatrix, i, j)
                )
                    wordsFound++;
            }
        }

        Console.WriteLine($"X-MAS Found: {wordsFound}");
    }

    private void ProcessInputFile()
    {
        if (_charMatrix != null)
            return;

        var inputFile = ReadInputFile();
        var lines = inputFile.Split('\n');
    
        _vertical = lines.Length;
        _horizontal = lines[0].Trim().Length;

        _charMatrix = new char[_vertical, _horizontal];

        for (var i = 0; i < _vertical; i++)
        {
            var lineChars = lines[i].Trim().ToCharArray();
            for (var j = 0; j < _horizontal; j++)
            {
                _charMatrix[i, j] = lineChars[j];
            }
        }
    }

    private bool Search(char[,] charMatrix, int i, int j, int iOffset, int jOffset)
    {
        for (var k = 0; k < XMAS.Length; k++)
        {
            var ii = i + (iOffset * k);
            var jj = j + (jOffset * k);

            if (ii < 0 || ii >= _vertical)
                return false;

            if (jj < 0 || jj >= _horizontal)
                return false;

            if (charMatrix[ii, jj] != XMAS[k])
                return false;
        }

        return true;
    }

    private bool XSearch(char[,] charMatrix, int i, int j)
    {
        // top left & bottom right, top right & bottom left
        int[][] coords = [ [i-1, j-1, i+1, j+1], [i-1, j+1, i+1, j-1] ];

        foreach (var coord in coords)
        {
            for (var k = 0; k < coord.Length; k++)
            {
                if (coord[k] < 0 || coord[k] >= (k % 2 == 0 ? _vertical: _horizontal))
                    return false;
            }

            var topChar = charMatrix[coord[0], coord[1]];
            var botChar = charMatrix[coord[2], coord[3]];

            if (
                !(topChar == 'M' || topChar == 'S') ||
                !(botChar == 'M' || botChar == 'S') ||
                topChar == botChar
            )
                return false;
        }

        return true;
    }
}