namespace AdventOfCode2022.Day03;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
    vJrwpWtwJgWrhcsFMMfFFhFp
    jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
    PmmdzqPrVvPwwTWBwg
    wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
    ttgJtRGJQctTZtZT
    CrZsJsPPZsGzwwsLwLmpwMDw
    """) == 70);
    }

    public int Solve(string input)
    {
        var sum = 0;
        var lines = input.Split(Environment.NewLine);
        foreach (var linesOfthree in lines.Chunk(3))
        {
            var commonItem = linesOfthree[0].Intersect(linesOfthree[1]).Intersect(linesOfthree[2]).Single();
            var priority = GetPriority(commonItem);

            sum += priority;
        }

        return sum;
    }

    private static int GetPriority(char c)
    {
        var alphabetLetterCount = 'z' - 'a' + 1;

        int priority = char.ToLower(c) - 'a';

        if (char.IsUpper(c))
            priority += alphabetLetterCount;

        return priority + 1;
    }
}
