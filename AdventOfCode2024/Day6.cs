namespace AdventOfCode2024;

public class Day6 : Day
{
    public override string InputFile
    {
        get => "day6.txt";
        set { }
    }

    private enum GuardDirections {
        Up = '^',
        Down = 'v',
        Left = '<',
        Right = '>',
    }
    private readonly GuardDirections[] _rotationOrder = [GuardDirections.Up, GuardDirections.Right, GuardDirections.Down, GuardDirections.Left];
    private readonly Dictionary<GuardDirections, (int x, int y)> _movement = new Dictionary<GuardDirections, (int x, int y)> {
        { GuardDirections.Up, (0, -1) },
        { GuardDirections.Down, (0, 1) },
        { GuardDirections.Right, (1, 0) },
        { GuardDirections.Left, (-1, 0) }
    };

    private char[,] _map = new char[0, 0];
    private (int x, int y) _currentPos;
    private (int x, int y) _startPos;
    private GuardDirections _currentDirection;
    private GuardDirections _startDirection;
    private readonly List<(int x, int y)> _path = [];


    protected override void FirstSolution()
    {
        ProcessInputFile();
        MoveGuard();
        Console.WriteLine($"Total Positions: {CountSpaces()}");
    }

    protected override void SecondSolution()
    {
        var createsLoop = 0;

        foreach (var (x, y) in _path)
        {
            ProcessInputFile();  // Re-process file to reset map
            
            var prevChar = _map[y, x];
            _map[y, x] = '#';

            if (!MoveGuard())
                createsLoop++;
            
            _map[y, x] = prevChar;
        }

        Console.WriteLine($"Blocks that create loop: {createsLoop}");
    }

    private void ProcessInputFile()
    {
        var inputFile = ReadInputFile();
        var lines = inputFile.Split('\n');
        _map = new char[lines.Length, lines[0].Trim().Length];

        for (var i = 0; i < lines.Length; i++)
        {
            var currentLine = lines[i].Trim().ToCharArray();
            for (var j = 0; j < currentLine.Length; j++)
            {
                _map[i, j] = currentLine[j];
                if (currentLine[j] == '.' || currentLine[j] == '#')
                    continue;
                
                _currentPos = (j, i);
                _startPos = (j, i);
                _currentDirection = (GuardDirections)currentLine[j];
                _startDirection = (GuardDirections)currentLine[j];
            }
        }
    }

    private bool PositionWithinBounds((int x, int y) pos)
    {
        return
            pos is { x: >= 0, y: >= 0 } &&
            pos.x < _map.GetLength(0) &&
            pos.y < _map.GetLength(1);
    }

    // Guard Movement Logic
    private bool MoveGuard()
    {
        var visitCount = 0;
        
        while (PositionWithinBounds(_currentPos) && visitCount < 2)
        {
            char? nextChar = null;
            do
            {
                nextChar = LookAhead();
                switch (nextChar)
                {
                    case '#':
                        ChangeDirection();
                        break;
                    
                    case 'X':
                        visitCount++;
                        break;
                    
                    case '.':
                        visitCount--;
                        break;
                }
            } while (nextChar == '#');

            _map[_currentPos.y, _currentPos.x] = 'X';
            UpdatePos();
        }

        return visitCount < 2;
    }

    private char? LookAhead()
    {
        var nextMove = _movement[_currentDirection];
        (int x, int y) nextPos = (_currentPos.x + nextMove.x, _currentPos.y + nextMove.y);

        return PositionWithinBounds(nextPos) ? _map[nextPos.y, nextPos.x] : null;
    }

    private void ChangeDirection()
    {
        var nextIndex = Array.IndexOf(_rotationOrder, _currentDirection) + 1;
        _currentDirection = _rotationOrder[nextIndex == _rotationOrder.Length ? 0 : nextIndex];
    }

    private void UpdatePos()
    {
        var nextMove = _movement[_currentDirection];
        _currentPos.x += nextMove.x;
        _currentPos.y += nextMove.y;
    }

    // Counting
    private int CountSpaces()
    {
        var total = 0;
    
        for (var i = 0; i < _map.GetLength(0); i++)
        for (var j = 0; j < _map.GetLength(1); j++)
            if (_map[i, j] == 'X')
            {
                total++;
                if (!(i == _startPos.y && j == _startPos.x))
                    _path.Add((j, i));
            }

        return total;
    }

    private void PrintMap()
    {
        for (var i = 0; i < _map.GetLength(0); i++)
        {
            var currentLine = "";
            for (var j = 0; j < _map.GetLength(1); j++)
                currentLine += _map[i, j];
            Console.WriteLine(currentLine);
        }
    }
}