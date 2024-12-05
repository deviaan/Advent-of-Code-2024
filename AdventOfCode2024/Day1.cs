public class Day1: Day
{
    public override string InputFile
    {
        get { return "day1.txt"; }
        set { }
    }

    public override void FirstSolution() 
    {
        (int[] leftList, int[] rightList) = ProcessInputFile();
        int totalDistance = 0;

        for (int i = 0; i < leftList.Length; i++)
        {
            totalDistance += Math.Abs(leftList[i] - rightList[i]);
        }

        Console.WriteLine($"The total distance is: {totalDistance}");
    }

    public override void SecondSolution()
    {
        (int[] leftList, int[] rightList) = ProcessInputFile();
        int leftListIndex = 0;
        int rightListIndex = 0;
        int? currentNumber = null;
        int currentSimilarityCount = 0;
        int similarityScore = 0;


        while (leftListIndex < leftList.Length && rightListIndex < rightList.Length)
        {
            // on a new number, reset the similarity count
            if (currentNumber == null || leftList[leftListIndex] != currentNumber)
            {
                currentSimilarityCount = 0;
                currentNumber = leftList[leftListIndex];
            }

            // when numbers match, update our counter and check the next number on the right
            if (currentNumber == rightList[rightListIndex])
            { 
                currentSimilarityCount++;
                rightListIndex++;
            }
            // left list bigger, climb up right list
            else if (currentNumber > rightList[rightListIndex])
            {
                rightListIndex++;
            }
            // right list is bigger, add the current number & crawl up left list
            else
            {
                similarityScore += leftList[leftListIndex] * currentSimilarityCount;
                leftListIndex++;
            }
        }

        Console.WriteLine($"The similarity score is: {similarityScore}");
    }

    private (int[], int[]) ProcessInputFile(){
        string input = ReadInputFile();
        string[] lines = input.Split('\n');
        int[] leftList = new int[lines.Length];
        int[] rightList = new int[lines.Length];

        for (int i = 0; i < lines.Length; i++)
        {
            string[] currentLine = lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            leftList[i] = int.Parse(currentLine[0]);
            rightList[i] = int.Parse(currentLine[1]);
        }

        Array.Sort(leftList);
        Array.Sort(rightList); 

        return (leftList, rightList);
    }
}