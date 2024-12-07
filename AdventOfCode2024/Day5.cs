namespace AdventOfCode2024;

public class Day5 : Day
{
    public override string InputFile { 
        get => "day5.txt";
        set { }
    }

    private readonly Dictionary<int, List<int>> _pageOrderRules = [];
    private int[][]? _updates = null;

    protected override void FirstSolution()
    {
        ProcessInputFile();
        if (_updates == null)
            return;

        var total = 0;

        foreach (var update in _updates)
            if (UpdateFollowsRules(update))
                total += update[update.Length/2];

        Console.WriteLine($"Middle Page Sum {total}");
    }

    protected override void SecondSolution()
    {
        ProcessInputFile();
        if (_updates == null)
            return;

        var total = 0;

        foreach (var update in _updates)
        {
            if (UpdateFollowsRules(update))
                continue;

            var sortedPage = PageOrderRuleSort(update);
            total += sortedPage[sortedPage.Length/2];
        }

        Console.WriteLine($"Corrected Middle Page Sum {total}");
    }

    private void ProcessInputFile()
    {
        if (_updates != null)
            return;

        var inputFile = ReadInputFile();
        var lines = inputFile.Split('\n');
        var fileLength = lines.Length;
        var updatesIndex = 0;

        for (var i = 0; i < fileLength; i++)
        {
            var line = lines[i].Trim();
            if (line == "")
            {
                updatesIndex = i + 1;
                break;
            }

            var pages = line.Split('|');

            var pageBefore = int.Parse(pages[0]);
            var pageAfter = int.Parse(pages[1]);

            if (!_pageOrderRules.ContainsKey(pageBefore))
                _pageOrderRules.Add(pageBefore, []);

            _pageOrderRules[pageBefore].Add(pageAfter);
        }

        if (updatesIndex == 0)
        {
            Console.WriteLine("No updates in input");
            return;
        }

        _updates = new int[fileLength - updatesIndex][];

        for (var i = updatesIndex; i < fileLength; i++)
        {
            var line = lines[i].Trim().Split(',');
            var update = new int[line.Length];

            for (var j = 0; j < line.Length; j++)
            {
                update[j] = int.Parse(line[j]);
            }

            _updates[i - updatesIndex] = update;
        }
    }

    private static bool PageInRules(int page, List<int> pageRules)
    {
        foreach (var pageMustComeBefore in pageRules)
            if (page == pageMustComeBefore)
                return true;
        
        return false;
    }

    private static int[] Swap(int[] update, int i, int j)
    {
        var prevI = update[i];
        var prevJ = update[j];

        update[i] = prevJ;
        update[j] = prevI;

        return update;
    }

    private bool UpdateFollowsRules(int[] update)
    {
        var pagesSeen = new int?[update.Length];
        var pageSeenIndex = 0;

        foreach (var page in update)
        {
            if (_pageOrderRules.TryGetValue(page, out List<int>? pageRules))
            {
                foreach (var pageSeen in pagesSeen)
                {
                    if (pageSeen == null)
                        continue;

                    if (PageInRules((int) pageSeen, pageRules))
                        return false;
                }
            }

            pagesSeen[pageSeenIndex] = page;
            pageSeenIndex++;
        }

        return true;
    }

    private int[] PageOrderRuleSort(int[] update)
    {
        for (var i = 1; i < update.Length; i++)
        {
            if (!_pageOrderRules.TryGetValue(update[i], out List<int>? pageRules))
                continue;

            for (var j = i-1; j >= 0; j--)
            {
                if (!PageInRules(update[j], pageRules))
                    continue;
                
                update = Swap(update, i, j);
                i = 0;
                break;
            }
        }

        return update;
    }
}