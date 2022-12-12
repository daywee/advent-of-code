namespace AdventOfCode2022.Day12;

internal class Solver
{
    private const char StartPoint = 'S';
    private const char EndPoint = 'E';

    public Solver()
    {
        Debug.Assert(Solve("""
        Sabqponm
        abcryxxl
        accszExk
        acctuvwj
        abdefghi
        """) == 31);
    }

    public int Solve(string input)
    {
        var lines = input.Split(Environment.NewLine);

        var sizeX = lines[0].Length;
        var sizeY = lines.Length;

        var start = FindPoint(StartPoint);
        var end = FindPoint(EndPoint);

        var visitedMap = new int[sizeX, sizeY];
        for (int i = 0; i < sizeX; i++)
            for (int j = 0; j < sizeY; j++)
                visitedMap[i, j] = int.MaxValue;

        Traverse(start.X, start.Y);

        var stepsNeeded = visitedMap[end.X, end.Y];

        return stepsNeeded;

        void Traverse(int startX, int startY)
        {
            var queue = new Queue<(int X, int Y, int Steps)>();
            queue.Enqueue((startX, startY, 0));

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                if (current.Steps < visitedMap[current.X, current.Y])
                {
                    visitedMap[current.X, current.Y] = current.Steps;
                }
                else
                {
                    continue;
                }

                var nextPoints = new (int X, int Y)[]
                {
                    (current.X + 1, current.Y),
                    (current.X - 1, current.Y),
                    (current.X, current.Y + 1),
                    (current.X, current.Y - 1),
                };

                foreach (var nextPoint in nextPoints)
                {
                    if (IsPointInside(nextPoint.X, nextPoint.Y) && GetHeight(nextPoint.X, nextPoint.Y) - GetHeight(current.X, current.Y) <= 1)
                    {
                        queue.Enqueue((nextPoint.X, nextPoint.Y, current.Steps + 1));
                    }
                }
            }
        }

        bool IsPointInside(int x, int y)
        {
            return x >= 0 && x < sizeX && y >= 0 && y < sizeY;
        }

        int GetHeight(int x, int y)
        {
            var point = lines[y][x];

            if (point == StartPoint)
                point = 'a';

            if (point == EndPoint)
                point = 'z';

            return point - 'a';
        }

        (int X, int Y) FindPoint(char point)
        {
            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    if (lines[j][i] == point)
                        return (i, j);
                }
            }

            throw new InvalidOperationException("Point not found.");
        }
    }
}
