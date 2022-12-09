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
""") == 157);
    }

    public int Solve(string input)
    {
        var sum = 0;
        var lines = input.Split(Environment.NewLine);
        foreach (var line in lines)
        {
            var half = line.Length / 2;
            var firstPart = line[..half].ToHashSet();
            var secondPart = line[half..].ToHashSet();

            var commonItem = firstPart.Intersect(secondPart).Single();
            var priority = GetPriority(commonItem);

            sum += priority;
        }

        return sum;
    }

    private int GetPriority(char c)
    {
        var alphabetLetterCount = 'z' - 'a' + 1;

        int priority = char.ToLower(c) - 'a';

        if (char.IsUpper(c))
            priority += alphabetLetterCount;

        return priority + 1;
    }
}
