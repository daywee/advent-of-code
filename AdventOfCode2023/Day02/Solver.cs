namespace AdventOfCode.Year2023.Day02;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green
""") == 8);
    }

    public int Solve(string input)
    {
        var rows = input.Split(Environment.NewLine);
        var result = 0;

        var bag = ParsePairs("12 red, 13 green, 14 blue");

        // TODO: try to use Span
        foreach (var row in rows)
        {
            if (!(row.Split([':', ';']) is [var gamePart, .. var revealParts]))
                throw new InvalidOperationException();

            if (!(gamePart.Split(' ') is [_, var number] && int.TryParse(number, out var gameNumber)))
                throw new InvalidOperationException();

            var reveals = revealParts.Select(ParsePairs).ToList();

            if (reveals.All(e => Test(bag, e)))
                result += gameNumber;
        }

        return result;
    }

    private static bool Test(Dictionary<string, int> bag, Dictionary<string, int> reveal)
    {
        foreach (var (color, neededCount) in reveal)
        {
            var availableCount = bag.GetValueOrDefault(color);
            if (neededCount > availableCount)
                return false;
        }

        return true;
    }

    private static Dictionary<string, int> ParsePairs(string revealPart)
    {
        var pairs = revealPart.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        var cubePairs = pairs
            .Select(e => e.Split(' '))
            .ToDictionary(e => e[1], e => int.Parse(e[0]));

        return cubePairs;
    }

    private record CubePair(string Name, int Count);
}
