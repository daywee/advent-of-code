namespace AdventOfCode.Year2023.Day09;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
0 3 6 9 12 15
1 3 6 10 15 21
10 13 16 21 30 45
""") == 114);
    }

    public int Solve(string input)
    {
        var result = 0;

        var rows = input.Split(Environment.NewLine);
        foreach (var row in rows)
        {
            var history = row.Split(' ', Constants.RemoveAndTrim).Select(int.Parse).ToList();

            var differences = CreateDifferences(history);
            var predicted = PredictNextValue(differences);

            result += predicted;
        }

        return result;
    }

    private int PredictNextValue(List<List<int>> differences)
    {
        for (int i = differences.Count - 1; i > 0; i--)
        {
            var last = differences[i];
            var secondLast = differences[i - 1];

            var extrapolated = secondLast.Last() + last.Last();

            secondLast.Add(extrapolated);
        }

        return differences[0].Last();
    }

    private List<List<int>> CreateDifferences(List<int> history)
    {
        var differences = new List<List<int>> { history };

        while (!differences.Last().All(e => e == 0))
        {
            var current = differences.Last();

            var newDifference = new List<int>(current.Count - 1);
            for (int i = 0; i < current.Count - 1; i++)
            {
                newDifference.Add(current[i + 1] - current[i]);
            }

            differences.Add(newDifference);
        }

        return differences;
    }
}
