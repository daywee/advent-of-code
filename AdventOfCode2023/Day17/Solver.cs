using System.Numerics;

namespace AdventOfCode.Year2023.Day17;

internal class Solver
{
    private const int DistanceInitialValue = -100;

    public Solver()
    {
        Debug.Assert(Solve("""
2413432311323
3215453535623
3255245654254
3446585845452
4546657867536
1438598798454
4457876987766
3637877979653
4654967986887
4564679986453
1224686865563
2546548887735
4322674655533
""") == 102);
    }

    public int Solve(string input)
    {
        var rows = input.Split(Environment.NewLine);
        var matrix = new Matrix(rows);

        var result = matrix.Solve();

        return result;
    }

    private class Matrix
    {
        private readonly int[,] _heatMatrix;

        public Matrix(string[] rows)
        {
            _heatMatrix = CreateMatrix(rows);
        }

        public int Solve()
        {
            var startPoint = new Point2(0, 0);
            var endPoint = new Point2(_heatMatrix.GetLength(0) - 1, _heatMatrix.GetLength(1) - 1);

            var distanceMapping = new Dictionary<Point2, int>(); // Distance aka Heat loss

            var queue = new Queue<(Point2 NextPoint, PreviousTurns PreviousTurns)>();
            queue.Enqueue((startPoint, new PreviousTurns(0, 'L', 0)));

            while (queue.Count > 0)
            {
                var (p, previousTurns) = queue.Dequeue();
                var currentDistance = _heatMatrix[p.X, p.Y] + previousTurns.Distance;

                if (distanceMapping.TryGetValue(p, out var existingDistance) && currentDistance >= existingDistance)
                {
                    continue;
                }

                distanceMapping[p] = currentDistance;

                EnqueueNext(Point2.ToRight, 'R');
                EnqueueNext(Point2.ToLeft, 'L');
                EnqueueNext(Point2.ToUp, 'U');
                EnqueueNext(Point2.ToDown, 'D');

                void EnqueueNext(Point2 direction, char directionSymbol)
                {
                    var next = p + direction;
                    if (IsValid(next) && CreateNewTurns(directionSymbol, currentDistance, previousTurns) is { } nextTurns)
                    {
                        queue.Enqueue((next, nextTurns));
                    }
                }
            }

            var distance = distanceMapping[endPoint] - _heatMatrix[startPoint.X, startPoint.Y] + _heatMatrix[endPoint.X, endPoint.Y];

            return distance;
        }

        private static PreviousTurns? CreateNewTurns(char wantedDirection, int currentDistance, PreviousTurns previousTurns)
        {
            if (previousTurns.Direction == wantedDirection)
            {
                if (previousTurns.Times > 3)
                    return null;

                return previousTurns with
                {
                    Distance = currentDistance,
                    Times = previousTurns.Times + 1,
                };
            }

            return new PreviousTurns(currentDistance, wantedDirection, 1);
        }

        private bool IsValid(Point2 point)
        {
            return point.X >= 0 && point.X < _heatMatrix.GetLength(0) && point.Y >= 0 && point.Y < _heatMatrix.GetLength(1);
        }

        private static int[,] CreateMatrix(string[] rows)
        {
            var matrix = new int[rows[0].Length, rows.Length];
            for (int j = 0; j < rows.Length; j++)
            {
                for (int i = 0; i < rows[0].Length; i++)
                {
                    matrix[i, j] = (int)char.GetNumericValue(rows[j][i]);
                }
            }

            return matrix;
        }
    }

    private record Point2(int X, int Y) : IAdditionOperators<Point2, Point2, Point2>
    {
        public static Point2 operator +(Point2 left, Point2 right)
        {
            return new Point2(left.X + right.X, left.Y + right.Y);
        }

        public static Point2 ToRight { get; } = new(1, 0);
        public static Point2 ToLeft { get; } = new(-1, 0);
        public static Point2 ToDown { get; } = new(0, 1);
        public static Point2 ToUp { get; } = new(0, -1);
    }

    private record PreviousTurns(int Distance, char Direction, int Times);
}
