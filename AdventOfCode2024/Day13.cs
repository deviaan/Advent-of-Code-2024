using System.Text.RegularExpressions;

namespace AdventOfCode2024;

public class Day13 : Day
{
    public override string InputFile { get => "day13.txt"; set { } }

    private readonly struct ClawMachine((string, string) buttonA, (string, string) buttonB, (string, string) prize)
    {
        private readonly (int x, int y) _buttonA = (int.Parse(buttonA.Item1), int.Parse(buttonA.Item2));
        private readonly (int x, int y) _buttonB = (int.Parse(buttonB.Item1), int.Parse(buttonB.Item2));
        private readonly (int x, int y) _prize = (int.Parse(prize.Item1), int.Parse(prize.Item2));

        private const int ButtonACost = 3;
        private const int ButtonBCost = 1;
        private const long PrizeAdjustment = 10000000000000;

        private long? BPresses(bool adjustPrize = false)
        {
            var (prizeX, prizeY) = (
                _prize.x + (adjustPrize ? ClawMachine.PrizeAdjustment : 0),
                _prize.y + (adjustPrize ? ClawMachine.PrizeAdjustment : 0)
            );
            
            var top = (this._buttonA.x * prizeY) - (this._buttonA.y * prizeX);
            var bot = (this._buttonA.x * this._buttonB.y) - (this._buttonA.y * this._buttonB.x);

            return (top % bot == 0) ? top / bot : null;
        }

        private long? APresses(bool adjustPrize = false)
        {
            var bPresses = this.BPresses(adjustPrize);
            if (bPresses == null) return null;
            
            var prizeX = this._prize.x + (adjustPrize ? ClawMachine.PrizeAdjustment : 0);

            var top = prizeX - (bPresses * this._buttonB.x);
            var bot = this._buttonA.x;
            
            return (top % bot == 0) ? top / bot : null;
        }

        public long? MachineCost(bool adjustPrize = false)
        {
            var bPresses = this.BPresses(adjustPrize);
            var aPresses = this.APresses(adjustPrize);
            
            return (aPresses == null || bPresses == null) ? 
                null : 
                aPresses * ClawMachine.ButtonACost +  bPresses * ClawMachine.ButtonBCost;
        }
    }
    
    private readonly List<ClawMachine> _clawMachines = [];
    private const int ButtonLimit = 100;
    
    protected override void FirstSolution()
    {
        ProcessInputFile();
        long total = 0;

        foreach (var clawMachine in _clawMachines)
            total += clawMachine.MachineCost() ?? 0;
        
        Console.WriteLine($"To win all prizes: {total}");
    }

    protected override void SecondSolution()
    {
        long total = 0;

        foreach (var clawMachine in _clawMachines)
            total += clawMachine.MachineCost(true) ?? 0;
        
        Console.WriteLine($"To win all prizes: {total}");
    }

    private void ProcessInputFile()
    {
        const string buttonPattern = @"Button (?<button>A|B): X(?<xval>(\+|-)\d+), Y(?<yval>(\+|-)\d+)$";
        const string prizePattern = @"Prize: X=(?<xprize>\d+), Y=(?<yprize>\d+)$";

        var input = ReadInputFile();
        var lines = input.Split('\n');

        for (var i = 0; i < lines.Length; i += 4)
        {
            var buttonA = Regex.Match(lines[i].Trim(), buttonPattern);
            var buttonB = Regex.Match(lines[i + 1].Trim(), buttonPattern);
            var prize = Regex.Match(lines[i + 2].Trim(), prizePattern);
            
            _clawMachines.Add(new ClawMachine(
                (buttonA.Groups["xval"].Value, buttonA.Groups["yval"].Value),
                (buttonB.Groups["xval"].Value, buttonB.Groups["yval"].Value),
                (prize.Groups["xprize"].Value, prize.Groups["yprize"].Value)
            ));
        }
    }
}