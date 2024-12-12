using AdventOfCode2024;

bool validInput;
Day[] days = [
    new Day1(), new Day2(), new Day3(), new Day4(), new Day5(), 
    new Day6(), new Day7(), new Day8(), new Day9(), new Day10(),
    new Day11(), new Day12(), 
];


do
{
    Console.WriteLine($"Run which day (max {days.Length})?");
    validInput = int.TryParse(Console.ReadLine(), out var day);
    if (!validInput || day > days.Length)
    {
        validInput = false;
        Console.WriteLine("Invalid day received.");
    }
    else
     days[day -1].PrintSolution();

} while (!validInput);

