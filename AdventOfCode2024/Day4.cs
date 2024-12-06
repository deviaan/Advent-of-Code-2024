public class Day4 : Day {
    public override string InputFile { 
        get { return "day4.txt"; }
        set { }
    }

    private const string XMAS = "XMAS";
    private const string MAS = "MAS";
    private int VERTICAL = 0;
    private int HORIZONTAL = 0;
    // Forward, Backward, Down, Up, Up Right, Up Left, Down Right, Down Left
    private readonly int[,] SEARCH_DIRECTIONS = { {0, 1}, {0, -1}, {1, 0}, {-1, 0}, {1, 1}, {1, -1}, {-1, 1}, {-1, -1} };

    public override void FirstSolution()
    {
        char[,] charMatrix = ProcessInputFile();
        int wordsFound = 0;

        for (int i = 0; i < VERTICAL; i++)
        {
            for (int j = 0; j < HORIZONTAL; j++)
            {
                for (int k = 0; k < SEARCH_DIRECTIONS.GetLength(0); k++)
                {
                    if (
                        charMatrix[i, j] == 'X' &&
                        Search(charMatrix, i, j, SEARCH_DIRECTIONS[k, 0], SEARCH_DIRECTIONS[k, 1])
                    )
                        wordsFound++;
                }
            }
        }

        Console.WriteLine($"XMAS found: {wordsFound}");
    }

    public override void SecondSolution()
    {
        char[,] charMatrix = ProcessInputFile();
        int wordsFound = 0;

        for (int i = 0; i < VERTICAL; i++)
        {
            for (int j = 0; j < HORIZONTAL; j++)
            {
                if (
                    charMatrix[i, j] == 'A' &&
                    XSearch(charMatrix, i, j)
                )
                    wordsFound++;
            }
        }

        Console.WriteLine($"X-MAS Found: {wordsFound}");
    }

    private char[,] ProcessInputFile()
    {

        string inputFile = ReadInputFile();
        string[] lines = inputFile.Split('\n');
    
        VERTICAL = lines.Length;
        HORIZONTAL = lines[0].Trim().Length;

        char[,] charMatrix = new char[VERTICAL, HORIZONTAL];

        for (int i = 0; i < VERTICAL; i++)
        {
            char[] lineChars = lines[i].Trim().ToCharArray();
            for (int j = 0; j < HORIZONTAL; j++)
            {
                charMatrix[i, j] = lineChars[j];
            }
        }

        return charMatrix;
    }

    private bool Search(char[,] charMatrix, int i, int j, int iOffset, int jOffset)
    {
        for (int k = 0; k < XMAS.Length; k++)
        {
            int ii = i + (iOffset * k);
            int jj = j + (jOffset * k);

            if (ii < 0 || ii >= VERTICAL)
            {
                return false;
            }

            if (jj < 0 || jj >= HORIZONTAL)
            {
                return false;
            }

            if (charMatrix[ii, jj] != XMAS[k])
            {
                return false;
            }
        }

        return true;
    }

    private bool XSearch(char[,] charMatrix, int i, int j)
    {
        // top left & bottom right, top right & bottom left
        int[][] coords = [ [i-1, j-1, i+1, j+1], [i-1, j+1, i+1, j-1] ];

        foreach (int[] coord in coords)
        {
            for (int k = 0; k < coord.Length; k++)
            {
                if (coord[k] < 0 || coord[k] >= (k % 2 == 0 ? VERTICAL: HORIZONTAL))
                    return false;
            }

            char topChar = charMatrix[coord[0], coord[1]];
            char botChar = charMatrix[coord[2], coord[3]];

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