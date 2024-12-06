public class Day5 : Day
{
    public override string InputFile { 
        get { return "day5.txt"; }
        set { }
    }

    private readonly Dictionary<int, List<int>> PageOrderRules = [];
    private int[][]? Updates = null;

    public override void FirstSolution()
    {
        ProcessInputFile();
        if (Updates == null)
            return;

        int total = 0;

        foreach (int[] update in Updates)
            if (UpdateFollowsRules(update))
                total += update[update.Length/2];

        Console.WriteLine($"Middle Page Sum {total}");
    }

    public override void SecondSolution()
    {
        ProcessInputFile();
        if (Updates == null)
            return;

        int total = 0;

        foreach (int[] update in Updates)
        {
            if (UpdateFollowsRules(update))
                continue;

            int[] sortedPage = PageOrderRuleSort(update);
            total += sortedPage[sortedPage.Length/2];
        }

        Console.WriteLine($"Corrected Middle Page Sum {total}");
    }

    public void ProcessInputFile()
    {
        if (Updates != null)
            return;

        string inputFile = ReadInputFile();
        string[] lines = inputFile.Split('\n');
        int fileLength = lines.Length;
        int updatesIndex = 0;

        for (int i = 0; i < fileLength; i++)
        {
            string line = lines[i].Trim();
            if (line == "")
            {
                updatesIndex = i + 1;
                break;
            }

            string[] pages = line.Split('|');

            int pageBefore = int.Parse(pages[0]);
            int pageAfter = int.Parse(pages[1]);

            if (!PageOrderRules.ContainsKey(pageBefore))
                PageOrderRules.Add(pageBefore, []);

            PageOrderRules[pageBefore].Add(pageAfter);
        }

        if (updatesIndex == 0)
        {
            Console.WriteLine("No updates in input");
            return;
        }

        Updates = new int[fileLength - updatesIndex][];

        for (int i = updatesIndex; i < fileLength; i++)
        {
            string[] line = lines[i].Trim().Split(',');
            int[] update = new int[line.Length];

            for (int j = 0; j < line.Length; j++)
            {
                update[j] = int.Parse(line[j]);
            }

            Updates[i - updatesIndex] = update;
        }
    }

    private static bool PageInRules(int page, List<int> pageRules)
    {
        foreach (int pageMustComeBefore in pageRules)
            if (page == pageMustComeBefore)
                return true;
        
        return false;
    }

    private static int[] Swap(int[] update, int i, int j)
    {
        int prevI = update[i];
        int prevJ = update[j];

        update[i] = prevJ;
        update[j] = prevI;

        return update;
    }

    private bool UpdateFollowsRules(int[] update)
    {
        int?[] pagesSeen = new int?[update.Length];
        int pageSeenIndex = 0;

        foreach (int page in update)
        {
            if (PageOrderRules.TryGetValue(page, out List<int>? pageRules))
            {
                foreach (int? pageSeen in pagesSeen)
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
        for (int i = 1; i < update.Length; i++)
        {
            if (!PageOrderRules.TryGetValue(update[i], out List<int>? pageRules))
                continue;

            for (int j = i-1; j >= 0; j--)
            {
                if (PageInRules(update[j], pageRules))
                {
                    update = Swap(update, i, j);
                    i = 0;
                    break;
                }
            }
        }

        return update;
    }
}