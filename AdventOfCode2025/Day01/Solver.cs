namespace AdventOfCode.Year2025.Day01;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
L68
L30
R48
L5
R60
L55
L1
L99
R14
L82
""") == "3");
    }

    public string Solve(string input)
    {
        const int dialCount = 100;
        var rows = input.Split(Environment.NewLine);

        var zeroPositionCount = 0;

        var position = 50;
        foreach (var row in rows)
        {
            var sign = row[..1] is "L" ? -1 : +1;
            var steps = int.Parse(row[1..]);
            var move = sign * steps;
            
            position = (position + move) % dialCount;

            while (position < 0)
            {
                position += dialCount;
            }
            
            if (position == 0)
            {
                zeroPositionCount++;
            }
        }
        
        var result = zeroPositionCount.ToString();
        return result;
    }
}
