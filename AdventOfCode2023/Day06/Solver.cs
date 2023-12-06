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

    public int Solve(string input)
    {
        var result = 1;

        var rows = input.Split(Environment.NewLine);
        var raceDurations = rows[0].Split(' ', _removeAndTrim).Skip(1).Select(int.Parse).ToArray();
        var raceDistances = rows[1].Split(' ', _removeAndTrim).Skip(1).Select(int.Parse).ToArray();

        for (int i = 0; i < raceDurations.Length; i++)
        {
            var duration = raceDurations[i];
            var distance = raceDistances[i];

            var lower = GetLowerBound(duration, distance);
            var upper = GetUpperBound(duration, distance);

            var numberOfWaysToBeatRecord = upper - lower + 1;

            result *= numberOfWaysToBeatRecord;
        }


        return result;
    }

    private int GetLowerBound(int duration, int distance)
    {
        for (int speed = 0; speed < duration; speed++)
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

    private int GetUpperBound(int duration, int distance)
    {
        for (int speed = duration - 1; speed >= 0; speed--)
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
