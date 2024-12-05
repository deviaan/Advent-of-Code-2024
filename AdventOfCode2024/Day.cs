public abstract class Day
{
    abstract public string InputFile { get; set;}
    public abstract void FirstSolution();
    public abstract void SecondSolution();

    public void PrintSolution() {
        FirstSolution();
        SecondSolution();
    }

    public string ReadInputFile() {
        if (InputFile == null)
            throw new Exception("Input file not specified for day.");

        using StreamReader reader = new StreamReader($"inputs/{InputFile}");
        return reader.ReadToEnd();
    }
}