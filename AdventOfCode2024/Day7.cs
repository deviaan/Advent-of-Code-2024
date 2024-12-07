namespace AdventOfCode2024;

public class Day7 : Day
{
    public override string InputFile
    {
        get => "day7.txt";
        set { }
    }

    private long[][]? _equations;
    private enum Operators {
        Add,
        Multiply,
        Concat,
    }

    protected override void FirstSolution()
    {
        ProcessInputFile();
        long total = 0;

        foreach (var equation in _equations ?? [])
        {
            total += AttemptOperations(
                equation[0], equation[1..], [Operators.Add, Operators.Multiply]
            ) ?? 0;
        }

        Console.WriteLine($"Valid Operation Sum: {total}");
    }

    protected override void SecondSolution()
    {
        long total = 0;

        foreach (var equation in _equations ?? [])
        {
            total += AttemptOperations(
                equation[0], equation[1..], [Operators.Add, Operators.Multiply, Operators.Concat]
            ) ?? 0;
        }

        Console.WriteLine($"Valid Operation Sum: {total}");
    }

    private void ProcessInputFile()
    {
        var inputFile = ReadInputFile();
        var lines = inputFile.Split('\n');

        _equations = new long[lines.Length][];
        for (var i = 0; i < lines.Length; i++)
        {
            var resultAndOperators = lines[i].Trim().Split(':');
            var operandStrings = resultAndOperators[1].Trim().Split(' ');
            var values = new long[operandStrings.Length + 1];

            values[0] = long.Parse(resultAndOperators[0]);

            for (var j = 1; j < values.Length; j++)
            {
                values[j] = long.Parse(operandStrings[j-1]);
            }

            _equations[i] = values;
        }
    }

    private static long? AttemptOperations(long result, long[] operands, Operators[] operators)
    {
        var operatorCombinations = OperatorCombinations(operators, operands.Length - 1);

        foreach (var operatorCombination in operatorCombinations)
        {
            if (EvaluatePrefixOperation(operands, operatorCombination) == result)
                return result;
        }

        return null;
    }

    private static IEnumerable<Operators[]> OperatorCombinations(Operators[] operators, int size)
    {
        foreach (var oper in operators)
        {
            var operatorCombinations = new Operators[size];
            operatorCombinations[0] = oper;
            
            if (size <= 1)
                yield return operatorCombinations;
            else
                foreach (var nested in OperatorCombinations(operators, size - 1))
                {
                    for (var i = 0; i < nested.Length; i++)
                    {
                        operatorCombinations[i + 1] = nested[i];
                    }
                    yield return operatorCombinations;
                }
        }
    }

    private static long EvaluatePrefixOperation(long[] operands, Operators[] operators)
    {
        if (operands.Length != operators.Length + 1)
            throw new ArgumentException("Invalid operator and operand combination.");

        var operandStack = new Stack<long>(operands.Reverse());

        foreach (var oper in operators)
        {
            var operand0 = operandStack.Pop();
            var operand1 = operandStack.Pop();

            switch (oper)
            {
                case Operators.Add:
                    operandStack.Push(operand0 + operand1);
                    break;

                case Operators.Multiply:
                    operandStack.Push(operand0 * operand1);
                    break;

                case Operators.Concat:
                    operandStack.Push(long.Parse($"{operand0}{operand1}"));
                    break;
            }
        }


        return operandStack.Pop();
    }
}