using System.Diagnostics;

namespace AdventOfCode2022.Day06;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("mjqjpqmgbljsphdztnvjfqwrcgsmlb") == 19);
        Debug.Assert(Solve("bvwbjplbgvbhsrlpgdmjqwftvncz") == 23);
        Debug.Assert(Solve("nppdvjthqldpwncqszvftbrmjlhg") == 23);
        Debug.Assert(Solve("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg") == 29);
        Debug.Assert(Solve("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw") == 26);
    }

    public int Solve(string input)
    {
        return DetectSequence(input, 14, 0);
    }

    private int DetectSequence(string input, int requiredLength, int after)
    {
        var span = input.AsSpan();
        for (int i = after + requiredLength; i < input.Length; i++)
        {
            var x = span.Slice(i - requiredLength, requiredLength);
            if (x.ToArray().Distinct().Count() == requiredLength)
                return i;
        }

        throw new Exception("Sequence not found.");
    }
}
