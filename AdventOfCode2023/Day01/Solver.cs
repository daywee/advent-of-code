namespace AdventOfCode.Year2023.Day01;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
1abc2
pqr3stu8vwx
a1b2c3d4e5f
treb7uchet
""") == 142);
    }

    public int Solve(string input)
    {
        var rows = input.Split(Environment.NewLine);

        var digits = new List<char>[rows.Length];

        for (int i = 0; i < rows.Length; i++)
        {
            digits[i] = [];

            var row = rows[i];

            foreach (var c in row)
            {
                if (char.IsDigit(c))
                {
                    //var value = (int)char.GetNumericValue(c);
                    digits[i].Add(c);
                }
            }
        }

        var result = digits
            .Select(e => $"{e.First()}{e.Last()}")
            .Select(e => int.Parse(e))
            .Sum();


        return result;
    }
}
