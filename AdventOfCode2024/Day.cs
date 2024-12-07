namespace AdventOfCode2024;

public abstract class Day
{
    public abstract string InputFile { get; set;}
    protected abstract void FirstSolution();
    protected abstract void SecondSolution();

    public void PrintSolution() {
        FirstSolution();
        SecondSolution();
    }

    protected string ReadInputFile() {
        if (InputFile == null)
            throw new Exception("Input file not specified for day.");

        using var reader = new StreamReader($"inputs/{InputFile}");
        return reader.ReadToEnd();
    }
}