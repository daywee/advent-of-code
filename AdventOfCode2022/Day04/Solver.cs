namespace AdventOfCode2022.Day04;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
        2-4,6-8
        2-3,4-5
        5-7,7-9
        2-8,3-7
        6-6,4-6
        2-6,4-8
        """) == 4);
    }

    public int Solve(string input)
    {
        var sum = 0;
        var lines = input.Split(Environment.NewLine);
        foreach (var line in lines)
        {
            var (r1, r2) = ParseLine(line);
            if (r1.PartiallyOverlapsWith(r2))
                sum++;
        }

        return sum;

        static (Range A, Range B) ParseLine(string line)
        {
            if (line.Split(new[] { ',', '-' }) is [var a1, var a2, var b1, var b2])
            {
                return (new Range(int.Parse(a1), int.Parse(a2)), new Range(int.Parse(b1), int.Parse(b2)));
            }
            else
            {
                throw new Exception();
            }
        }
    }
}

file record Range(int From, int To)
{
    public bool PartiallyOverlapsWith(Range other)
    {
        if (From <= other.To && To >= other.From)
            return true;

        if (From >= other.To && To <= other.From)
            return true;

        return false;
    }
}
