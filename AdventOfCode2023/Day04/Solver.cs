namespace AdventOfCode.Year2023.Day04;

internal class Solver
{
    private const StringSplitOptions _removeAndTrim = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;

    public Solver()
    {
        Debug.Assert(Solve("""
Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19
Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1
Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83
Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36
Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11
""") == 13);
    }

    public int Solve(string input)
    {
        var result = 0;

        var rows = input.Split(Environment.NewLine);
        foreach (var row in rows)
        {
            if (!(row.Split(new[] { ':', '|' }, _removeAndTrim) is [var cardPart, var winningPart, var guessedPart]))
                throw new InvalidOperationException();

            var winning = ParseNumbers(winningPart);
            var guessed = ParseNumbers(guessedPart).ToHashSet();

            var guessedCorrectly = winning.Where(w => guessed.Contains(w)).Count();

            var cardResult = (int)Math.Pow(2, guessedCorrectly - 1);

            result += cardResult;
        }

        return result;
    }

    private static int[] ParseNumbers(string input)
    {
        return input.Split(' ', _removeAndTrim).Select(e => int.Parse(e)).ToArray();
    }
}
