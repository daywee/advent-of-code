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
""") == "6");
        
        Debug.Assert(Solve("""
                           L268
                           """) == "3");
        
        Debug.Assert(Solve("""
                           R1000
                           """) == "10");
        Debug.Assert(Solve("""
                           R50
                           """) == "1");
        
        Debug.Assert(Solve("""
                           L50
                           """) == "1");
    }

    public string Solve(string input)
    {
        const int dialCount = 100;
        var rows = input.Split(Environment.NewLine);

        var movesOverZero = 0;

        var position = 50;
        foreach (var row in rows)
        {
            var sign = row[..1] is "L" ? -1 : +1;
            var steps = int.Parse(row[1..]);
            
            // normalize to the interval 0..99
            movesOverZero += steps / dialCount;
            steps %= dialCount;
            
            var move = sign * steps;

            if (sign == -1 && position == 0)
                movesOverZero--;
            
            position += move;

            if (position < 0)
            {
                position += dialCount;
                movesOverZero++;
            }
            
            if (position > dialCount - 1)
            {
                position -= dialCount;
                movesOverZero++;
            }
            
            if (sign == -1 && position == 0)
                movesOverZero++;
        }
        
        var result = movesOverZero.ToString();
        Console.WriteLine(result);
        return result;
    }
}
