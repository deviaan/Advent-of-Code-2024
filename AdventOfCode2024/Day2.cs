public class Day2: Day
{
    public override string InputFile
    {
        get { return "day2.txt"; }
        set { }
    }

    public override void FirstSolution()
    {
        int[][] reports = ProcessInputFile();
        int safeCounter = 0;

        foreach (int[] report in reports)
        {
            if (ReportIsSafe(report))
                safeCounter++;
        }

        Console.WriteLine($"Safe Reports: {safeCounter}");
    }

    public override void SecondSolution()
    {
        int[][] reports = ProcessInputFile();
        int safeCounter = 0;

        foreach (int[] report in reports)
        {
            if (ReportIsSafe(report) || ReportCanBeMadeSafe(report))
                safeCounter++;
        }

        Console.WriteLine($"Safe Reports: {safeCounter}");
    }

    private int[][] ProcessInputFile()
    {
        string input = ReadInputFile();
        string[] reportsInput = input.Split('\n');
        int[][] reports = new int[reportsInput.Length][];

        for (int i = 0; i < reportsInput.Length; i++)
        {
            string[] currentLevelsInput = reportsInput[i].Split(' ');
            int[] currentLevels = new int[currentLevelsInput.Length];
            for (int j = 0; j < currentLevelsInput.Length; j++)
            {
                currentLevels[j] = int.Parse(currentLevelsInput[j]);
            }
            reports[i] = currentLevels;
        }

        return reports;
    }

    private bool ReportIsSafe(int[] report)
    {
        bool? reportIsIncreasing = null;

        for (int i = 1; i < report.Length; i++)
        {
            int difference = report[i-1] - report[i];
            if (
                Math.Abs(difference) < 1 
                || Math.Abs(difference) > 3
                || (reportIsIncreasing == true && difference < 0)
                || (reportIsIncreasing == false && difference > 0)
            )
                return false;
            else if (reportIsIncreasing == null)
                reportIsIncreasing = difference > 0;
        }

        return true;
    }

    private bool ReportCanBeMadeSafe(int[] report)
    {
        int[] smallReport = new int[report.Length - 1];
        for (int i = 0; i < report.Length; i++)
        {
            bool addOne = false;
    
            for (int j = 0; j < smallReport.Length; j++)
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