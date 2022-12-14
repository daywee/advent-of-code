namespace AdventOfCode2022.Day13;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
        [1,1,3,1,1]
        [1,1,5,1,1]

        [[1],[2,3,4]]
        [[1],4]

        [9]
        [[8,7,6]]

        [[4,4],4,4]
        [[4,4],4,4,4]

        [7,7,7,7]
        [7,7,7]

        []
        [3]

        [[[]]]
        [[]]

        [1,[2,[3,[4,[5,6,7]]]],8,9]
        [1,[2,[3,[4,[5,6,0]]]],8,9]
        """) == 13);

        //Debug.Assert(Solve("""
        //[[[]]]
        //[[]]
        //""") == 0);
    }

    public int Solve(string input)
    {
        var correctOrder = new List<int>();

        var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        for (int i = 0; i < lines.Length / 2; i++)
        {
            var first = lines[i * 2];
            var second = lines[i * 2 + 1];

            var isInRightOrder = Solve(PrepareInput(first), PrepareInput(second));
            if (isInRightOrder)
                correctOrder.Add(i + 1);
        }

        return correctOrder.Sum();

        string PrepareInput(string text)
        {
            return text.Replace(",", string.Empty);
        }
    }

    private bool Solve(string first, string second)
    {
        for (int i = 0; i < first.Length; i++)
        {
            if (i == second.Length)
                return true;

            var t1 = first[i];
            var t2 = second[i];

            var isInt1 = char.IsDigit(t1);
            var isInt2 = char.IsDigit(t2);

            int? int1 = isInt1 ? int.Parse(t1.ToString()) : null;
            int? int2 = isInt2 ? int.Parse(t2.ToString()) : null;

            var resolution = Resolve(t1, t2, isInt1, isInt2, int1, int2, ref first, ref second, ref i);
            switch (resolution)
            {
                case Resolution.True:
                    return true;
                case Resolution.False:
                    return false;
                case Resolution.Continue:
                    continue;
            }
        }

        return true;
    }

    private Resolution Resolve(char t1, char t2, bool isInt1, bool isInt2, int? int1, int? int2, ref string first, ref string second, ref int current)
    {
        if (t1 == '[' && t2 == '[')
            return Resolution.Continue;

        // add other way?
        if (t1 == '[' && t2 == ']')
            return Resolution.False;

        //if (t1 == ']' && t2 == '[')
        //    return Resolution.True;

        if (t1 == ']' && isInt2)
            return Resolution.True;

        if (isInt1 && t2 == ']')
            return Resolution.False;

        if (isInt1 && isInt2)
        {
            if (int1 > int2)
            {
                return Resolution.False;
            }
            else if (int1 < int2)
            {
                return Resolution.True;
            }
        }

        if (isInt1 && t2 == '[')
        {
            first = first.Insert(current + 1, "]");
            first = first.Insert(current, "[");
            current--;
        }

        if (t1 == '[' && isInt2)
        {
            second = second.Insert(current + 1, "]");
            second = second.Insert(current, "[");
            current--;
        }

        return Resolution.Continue;
    }

    private enum Resolution
    {
        True,
        False,
        Continue,
    }
}
