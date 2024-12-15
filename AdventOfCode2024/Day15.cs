namespace AdventOfCode2024;

public class Day15 : Day
{
    public override string InputFile
    {
        get => "day15.txt";
        set { }
    }

    private List<char[]> _warehouse = [];
    private List<char> _robotMoves = [];
    private Coords _robotCoords;
    private readonly Stack<(Coords newCoords, Coords oldCoords)> _moves = new Stack<(Coords oldCoords, Coords newCoords)>();

    private static readonly Dictionary<char, Coords> Directions = new()
    {
        { '^', AdventOfCode2024.Directions.Up },
        { 'v', AdventOfCode2024.Directions.Down },
        { '<', AdventOfCode2024.Directions.Left },
        { '>', AdventOfCode2024.Directions.Right },
    };

    protected override void FirstSolution()
    {
        ProcessInputFile();
        MoveRobot();
        PrintWarehouse();
        Console.WriteLine($"GPS Coord Sum is {Score()}");
    }

    protected override void SecondSolution()
    {
        ProcessInputFile();
        ExpandWarehouse();
        MoveRobot();
        Console.WriteLine($"GPS Wide Box Coord Sum is {Score()}");
    }

    private void ProcessInputFile()
    {
        var inputFile = ReadInputFile().Split('\n');
        var i = 0;
        var endOfMap = false;
        var warehouse = new List<char[]>();
        var instructions = new List<char>();

        do
        {
            var line = inputFile[i].Trim();

            if (line == "")
                endOfMap = true;
            else if (endOfMap)
                instructions.AddRange(line.ToCharArray());
            else
            {
                warehouse.Add(line.ToCharArray());
                for (var j = 0; j < line.Length; j++)
                    if (line[j] == '@')
                        _robotCoords = new Coords(j, i);
            }

            i++;
        } while (i < inputFile.Length);
        
        _warehouse = warehouse;
        _robotMoves = instructions;
    }

    private void PrintWarehouse()
    {
        foreach (var row in _warehouse)
            Console.WriteLine(string.Join("", row));
    }

    private void PrintRobotMoves(int from = 0)
    {
        Console.WriteLine(string.Join("", _robotMoves[from..]));
    }

    private bool ErrorInWarehouse()
    {
        foreach (var row in _warehouse)
        {
            for (var j = 0; j < row.Length; j++)
            {
                var current = row[j];
                if (j + 1 < row.Length)
                {
                    var next = row[j + 1];
                    if ((current == '[' && next != ']') || (next == ']' && current != '['))
                        return true;
                }
            }
        }

        return false;
    }

    private void MoveRobot()
    {
        var i = 0;
        var reported = false;
        foreach (var move in _robotMoves)
        {
            var newCoords = _robotCoords + Directions[move];
            var thing = _warehouse[newCoords.Y][newCoords.X];

            switch (thing)
            {
                case '#':
                    break;

                case '.':
                    _moves.Push((newCoords, _robotCoords));
                    break;

                case 'O':
                    _moves.Push((newCoords, _robotCoords));
                    if (!AttemptBoxMove(newCoords, Directions[move])) _moves.Pop();
                    break;
                
                case '[': case ']':
                    _moves.Push((newCoords, _robotCoords));
                    if (!AttemptWideBoxMove(newCoords, Directions[move])) _moves.Clear();
                    break;
            }
            MakeMoves();
            if (ErrorInWarehouse() && !reported)
            {
                Console.WriteLine($"Error in move: {i}");
                reported = true;
                PrintWarehouse();
            }
            i++;
        }
    }

    private void UpdateMap(Coords newCoords, Coords oldCoords)
    {
        var old = _warehouse[newCoords.Y][newCoords.X];
        var thing = _warehouse[oldCoords.Y][oldCoords.X];
        _warehouse[oldCoords.Y][oldCoords.X] = old;
        _warehouse[newCoords.Y][newCoords.X] = thing;
        if (thing == '@') _robotCoords = newCoords;
    }

    private void MakeMoves()
    {
        var alreadyMoved = new HashSet<(Coords, Coords)>();
        
        while (_moves.Count > 0)
        {
            var (newCoords, oldCoords) = _moves.Pop();
            if (alreadyMoved.Contains((oldCoords, _robotCoords))) continue;
            UpdateMap(newCoords, oldCoords);
            alreadyMoved.Add((oldCoords, _robotCoords));
        }
    }

    private bool AttemptBoxMove(Coords oldCoords, Coords direction)
    {
        var newCoords = oldCoords + direction;

        switch (_warehouse[newCoords.Y][newCoords.X])
        {
            case '#':
                return false;

            case '.':
                _moves.Push((newCoords, oldCoords));
                return true;

            case 'O':
                _moves.Push((newCoords, oldCoords));
                if (!AttemptBoxMove(newCoords, direction)) _moves.Pop();
                else return true;
                break;
        }

        return false;
    }

    private bool AttemptWideBoxMove(Coords oldCoords, Coords direction)
    {
        Coords oldLeftCoord;
        Coords oldRightCoord;

        if (_warehouse[oldCoords.Y][oldCoords.X] == '[')
        {
            oldLeftCoord = oldCoords;
            oldRightCoord = oldCoords + AdventOfCode2024.Directions.Right;
        }
        else
        {
            oldLeftCoord = oldCoords + AdventOfCode2024.Directions.Left;
            oldRightCoord = oldCoords;
        }
        
        var newLeftCoord = oldLeftCoord + direction;
        var newRightCoord = oldRightCoord + direction;
        var leftThing = _warehouse[newLeftCoord.Y][newLeftCoord.X];
        var rightThing = _warehouse[newRightCoord.Y][newRightCoord.X];

        switch (direction)
        {
            case (-1, 0):
                _moves.Push((newRightCoord, oldRightCoord));
                _moves.Push((newLeftCoord, oldLeftCoord));

                switch (leftThing)
                {
                    case '#':
                        break;
                    
                    case '.':
                        return true;
                    
                    case ']':
                        if (AttemptWideBoxMove(newLeftCoord, direction)) return true;
                        break;
                }

                break;
            
            case (1, 0):
                _moves.Push((newLeftCoord, oldLeftCoord));
                _moves.Push((newRightCoord, oldRightCoord));

                switch (rightThing)
                {
                    case '#':
                        break;
                    
                    case '.':
                        return true;
                    
                    case '[':
                        if (AttemptWideBoxMove(newRightCoord, direction)) return true;
                        break;
                }

                break;
            
            default:
                _moves.Push((newLeftCoord, oldLeftCoord));
                _moves.Push((newRightCoord, oldRightCoord));

                switch ((leftThing, rightThing))
                {
                    case ('#', _):
                    case (_, '#'):
                        break;
                    
                    case ('.', '.'):
                        return true;
                    
                    case ('.', _):
                        if (AttemptWideBoxMove(newRightCoord, direction)) return true;
                        break;
                    
                    case (_, '.'):
                        if (AttemptWideBoxMove(newLeftCoord, direction)) return true;
                        break;
                    
                    case (']', '['):
                        if (
                            AttemptWideBoxMove(newLeftCoord, direction) &&
                            AttemptWideBoxMove(newRightCoord, direction) 
                        ) return true;
                        break;
                    
                    case ('[', ']'):
                        if (
                            AttemptWideBoxMove(newLeftCoord, direction) ||
                            AttemptWideBoxMove(newRightCoord, direction)
                        ) return true;
                        break;
                }
                
                break;
        }
        
        _moves.Pop();
        _moves.Pop();
        
        return false;
    }

    private long Score()
    {
        long total = 0;
        for (var i = 0; i < _warehouse.Count; i++)
        for (var j = 0; j < _warehouse[i].Length; j++)
            if (_warehouse[i][j] == 'O' || _warehouse[i][j] == '[')
                total += 100 * i + j;

        return total;
    }

    private void ExpandWarehouse()
    {
        var warehouse = new List<char[]>();

        for(var i = 0; i < _warehouse.Count; i++)
        {
            var line = _warehouse[i];
            var row = new char[line.Length * 2];
            for (var j = 0; j < line.Length; j++)
            {
                var c = line[j];
                switch (c)
                {
                    case '#':
                        row[j * 2] = '#';
                        row[j * 2 + 1] = '#';
                        break;
                    
                    case 'O':
                        row[j * 2] = '[';
                        row[j * 2 + 1] = ']';
                        break;
                    
                    case '.':
                        row[j * 2] = '.';
                        row[j * 2 + 1] = '.';
                        break;
                    
                    case '@':
                        row[j * 2] = '@';
                        row[j * 2 + 1] = '.';
                        _robotCoords = new Coords(j * 2, i);
                        break;
                }
            }
            
            warehouse.Add(row); 
        }
        
        _warehouse = warehouse;
    }
}
