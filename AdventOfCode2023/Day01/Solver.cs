namespace AdventOfCode.Year2023.Day01;

internal class Solver
{
    private static readonly Dictionary<string, int> _wordValues = new()
    {
        { "one", 1 },
        { "two", 2 },
        { "three", 3 },
        { "four", 4 },
        { "five", 5 },
        { "six", 6 },
        { "seven", 7 },
        { "eight", 8 },
        { "nine", 9 },
        { "0", 0 },
        { "1", 1 },
        { "2", 2 },
        { "3", 3 },
        { "4", 4 },
        { "5", 5 },
        { "6", 6 },
        { "7", 7 },
        { "8", 8 },
        { "9", 9 },
    };

    public Solver()
    {
        Debug.Assert(Solve("""
        two1nine
        eightwothree
        abcone2threexyz
        xtwone3four
        4nineeightseven2
        zoneight234
        7pqrstsixteen
        """) == 281);
    }

    public int Solve(string input)
    {
        var rows = input.Split(Environment.NewLine);

        var result = 0;

        for (int i = 0; i < rows.Length; i++)
        {
            var row = rows[i];

            var a = FindFirst(row);
            var b = FindLast(row);

            var num = int.Parse($"{a}{b}");
            result += num;
        }

        return result;
    }

    private static int FindFirst(string text)
    {
        return _wordValues
            .Select(e => (text.IndexOf(e.Key), e.Value))
            .Where(e => e.Item1 >= 0)
            .MinBy(e => e.Item1)
            .Value;
    }

    private static int FindLast(string text)
    {
        return _wordValues
            .Select(e => (text.LastIndexOf(e.Key), e.Value))
            .Where(e => e.Item1 >= 0)
            .MaxBy(e => e.Item1)
            .Value;
    }
}
