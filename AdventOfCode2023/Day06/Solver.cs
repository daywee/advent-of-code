namespace AdventOfCode.Year2023.Day06;

internal class Solver
{
    private const StringSplitOptions _removeAndTrim = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;

    public Solver()
    {
        Debug.Assert(Solve("""
Time:      7  15   30
Distance:  9  40  200
""") == 288);
    }

    public long Solve(string input)
    {
        var rows = input.Split(Environment.NewLine);
        var raceDuration = long.Parse(string.Join("", rows[0].Split(' ', _removeAndTrim).Skip(1)));
        var raceDistance = long.Parse(string.Join("", rows[1].Split(' ', _removeAndTrim).Skip(1)));

        var lower = GetLowerBound(raceDuration, raceDistance);
        var upper = GetUpperBound(raceDuration, raceDistance);

        var numberOfWaysToBeatRecord = upper - lower + 1;

        return numberOfWaysToBeatRecord;
    }

    private long GetLowerBound(long duration, long distance)
    {
        for (long speed = 0; speed < duration; speed++)
        {
            var travelingTime = duration - speed;
            var traveledDistance = travelingTime * speed;

            if (traveledDistance > distance)
            {
                return speed;
            }
        }

        throw new InvalidOperationException();
    }

    private long GetUpperBound(long duration, long distance)
    {
        for (long speed = duration - 1; speed >= 0; speed--)
        {
            var travelingTime = duration - speed;
            var traveledDistance = travelingTime * speed;

            if (traveledDistance > distance)
            {
                return speed;
            }
        }

        throw new InvalidOperationException();
    }
}
