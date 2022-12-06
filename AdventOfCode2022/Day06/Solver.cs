using System.Diagnostics;

namespace AdventOfCode2022.Day06;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("mjqjpqmgbljsphdztnvjfqwrcgsmlb") == 7);
        Debug.Assert(Solve("bvwbjplbgvbhsrlpgdmjqwftvncz") == 5);
        Debug.Assert(Solve("nppdvjthqldpwncqszvftbrmjlhg") == 6);
        Debug.Assert(Solve("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg") == 10);
        Debug.Assert(Solve("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw") == 11);
    }

    public int Solve(string input)
    {
        return DetectSequence(input, 4, 0);
    }

    private int DetectSequence(string input, int requiredLenght, int after)
    {
        var span = input.AsSpan();
        for (int i = after + requiredLenght; i < input.Length; i++)
        {
            var x = span.Slice(i - requiredLenght, requiredLenght);
            if (x.ToArray().Distinct().Count() == requiredLenght)
                return i;
        }

        throw new Exception("Sequence not found.");
    }
}
