namespace AdventOfCode2024;

public class Day1: Day
{
    public override string InputFile
    {
        get => "day1.txt";
        set { }
    }

    private int[]? _leftList = null;
    private int[]? _rightList = null;

    protected override void FirstSolution() 
    {
        ProcessInputFile();
        if (_leftList == null || _rightList == null)
        {
            Console.WriteLine("Could not load files.");
            return;
        }

        var totalDistance = 0;

        for (var i = 0; i < _leftList.Length; i++)
        {
            totalDistance += Math.Abs(_leftList[i] - _rightList[i]);
        }

        Console.WriteLine($"The total distance is: {totalDistance}");
    }

    protected override void SecondSolution()
    {
        ProcessInputFile();
        if (_leftList == null || _rightList == null)
        {
            Console.WriteLine("Could not load files.");
            return;
        }

        var leftListIndex = 0;
        var rightListIndex = 0;
        int? currentNumber = null;
        var currentSimilarityCount = 0;
        var similarityScore = 0;


        while (leftListIndex < _leftList.Length && rightListIndex < _rightList.Length)
        {
            // on a new number, reset the similarity count
            if (currentNumber == null || _leftList[leftListIndex] != currentNumber)
            {
                currentSimilarityCount = 0;
                currentNumber = _leftList[leftListIndex];
            }

            // when numbers match, update our counter and check the next number on the right
            if (currentNumber == _rightList[rightListIndex])
            { 
                currentSimilarityCount++;
                rightListIndex++;
            }
            // left list bigger, climb up right list
            else if (currentNumber > _rightList[rightListIndex])
            {
                rightListIndex++;
            }
            // right list is bigger, add the current number & crawl up left list
            else
            {
                similarityScore += _leftList[leftListIndex] * currentSimilarityCount;
                leftListIndex++;
            }
        }

        Console.WriteLine($"The similarity score is: {similarityScore}");
    }

    private void ProcessInputFile(){
        if (_leftList != null && _rightList != null)
            return;

        var input = ReadInputFile();
        var lines = input.Split('\n');
        _leftList = new int[lines.Length];
        _rightList = new int[lines.Length];

        for (var i = 0; i < lines.Length; i++)
        {
            var currentLine = lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            _leftList[i] = int.Parse(currentLine[0]);
            _rightList[i] = int.Parse(currentLine[1]);
        }

        Array.Sort(_leftList);
        Array.Sort(_rightList); 
    }
}