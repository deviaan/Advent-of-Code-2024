namespace AdventOfCode2024;

public class Day2: Day
{
    public override string InputFile
    {
        get => "day2.txt";
        set { }
    }

    private int[][]? _reports = null;

    protected override void FirstSolution()
    {
        ProcessInputFile();
        if (_reports == null)
        {
            Console.WriteLine("Could not read file");
            return;
        }

        var safeCounter = 0;

        foreach (var report in _reports)
        {
            if (ReportIsSafe(report))
                safeCounter++;
        }

        Console.WriteLine($"Safe Reports: {safeCounter}");
    }

    protected override void SecondSolution()
    {
        ProcessInputFile();
        if (_reports == null)
        {
            Console.WriteLine("Could not read file");
            return;
        }

        var safeCounter = 0;

        foreach (var report in _reports)
        {
            if (ReportIsSafe(report) || ReportCanBeMadeSafe(report))
                safeCounter++;
        }

        Console.WriteLine($"Safe Reports: {safeCounter}");
    }

    private void ProcessInputFile()
    {
        var input = ReadInputFile();
        var reportsInput = input.Split('\n');
        _reports = new int[reportsInput.Length][];

        for (var i = 0; i < reportsInput.Length; i++)
        {
            var currentLevelsInput = reportsInput[i].Split(' ');
            var currentLevels = new int[currentLevelsInput.Length];
            for (var j = 0; j < currentLevelsInput.Length; j++)
            {
                currentLevels[j] = int.Parse(currentLevelsInput[j]);
            }
            _reports[i] = currentLevels;
        }
    }

    private static bool ReportIsSafe(int[] report)
    {
        bool? reportIsIncreasing = null;

        for (var i = 1; i < report.Length; i++)
        {
            var difference = report[i-1] - report[i];
            if (
                Math.Abs(difference) < 1 
                || Math.Abs(difference) > 3
                || (reportIsIncreasing == true && difference < 0)
                || (reportIsIncreasing == false && difference > 0)
            )
                return false;
            reportIsIncreasing ??= difference > 0;
        }

        return true;
    }

    private bool ReportCanBeMadeSafe(int[] report)
    {
        var smallReport = new int[report.Length - 1];
        for (var i = 0; i < report.Length; i++)
        {
            var addOne = false;
    
            for (var j = 0; j < smallReport.Length; j++)
            {
                if (j == i)
                    addOne = true;

                smallReport[j] = addOne ? report[j + 1] : report[j];
            }

            if (ReportIsSafe(smallReport))
                return true;
        }

        return false;
    }
}