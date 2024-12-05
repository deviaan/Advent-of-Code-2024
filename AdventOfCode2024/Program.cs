int day;
bool validInput;
Day[] days = [new Day1()];


do
{
    Console.WriteLine($"Run which day (max {days.Length})?");
    validInput = int.TryParse(Console.ReadLine(), out day);
    if (!validInput || day > days.Length)
    {
        validInput = false;
        Console.WriteLine("Invalid day received.");
    }
    else
     days[day -1].PrintSolution();

} while (!validInput);

