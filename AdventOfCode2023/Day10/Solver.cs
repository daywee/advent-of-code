namespace AdventOfCode.Year2023.Day10;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
-L|F7
7S-7|
L|7||
-L-J|
L|-JF
""") == 4);
        Debug.Assert(Solve("""
7-F7-
.FJ|7
SJLL7
|F--J
LJ.LJ
""") == 8);
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
        private readonly char[,] _matrix;
        private readonly int[,] _distanceMatrix;

        public Matrix(string[] rows)
        {
            (_matrix, StartPoint) = CreateMatrix(rows);
            _distanceMatrix = CreateDistanceMatrix(_matrix);
        }

        public Point StartPoint { get; }

        public int Solve()
        {
            var maxDistance = 0;

            var queue = new Queue<Point>();
            queue.Enqueue(StartPoint);

            while (queue.Count > 0)
            {
                var p = queue.Dequeue();

                _distanceMatrix[p.X, p.Y] = p.Distance;
                maxDistance = Math.Max(maxDistance, p.Distance);

                if (MoveDown(p) is { } down)
                    queue.Enqueue(down);

                if (MoveUp(p) is { } up)
                    queue.Enqueue(up);

                if (MoveLeft(p) is { } left)
                    queue.Enqueue(left);

                if (MoveRight(p) is { } right)
                    queue.Enqueue(right);
            }

            return maxDistance;
        }

        private Point? Move(Point from, Point to, Func<char, bool> fromConstraint, Func<char, bool> toConstraint)
        {
            if (!fromConstraint(_matrix[from.X, from.Y]))
                return null;

            if (!IsValid(to))
                return null;

            if (!toConstraint(_matrix[to.X, to.Y]))
                return null;

            if (_distanceMatrix[to.X, to.Y] != 0)
                return null;

            return to;
        }

        public Point? MoveRight(Point from)
        {
            return Move(from, new Point(from.X + 1, from.Y, from.Distance + 1), e => e is 'S' or '-' or 'F' or 'L', e => e is '-' or '7' or 'J');
        }

        public Point? MoveLeft(Point from)
        {
            return Move(from, new Point(from.X - 1, from.Y, from.Distance + 1), e => e is 'S' or '-' or '7' or 'J', e => e is '-' or 'F' or 'L');
        }

        public Point? MoveUp(Point from)
        {
            return Move(from, new Point(from.X, from.Y - 1, from.Distance + 1), e => e is 'S' or '|' or 'L' or 'J', e => e is '|' or 'F' or '7');
        }

        public Point? MoveDown(Point from)
        {
            return Move(from, new Point(from.X, from.Y + 1, from.Distance + 1), e => e is 'S' or '|' or '7' or 'F', e => e is '|' or 'L' or 'J');
        }

        private bool IsValid(Point point)
        {
            return point.X >= 0 && point.X < _matrix.GetLength(0) && point.Y >= 0 && point.Y < _matrix.GetLength(1);
        }

        private static (char[,] matrix, Point) CreateMatrix(string[] rows)
        {
            Point? startPoint = null;
            var matrix = new char[rows[0].Length, rows.Length];
            for (int j = 0; j < rows.Length; j++)
            {
                for (int i = 0; i < rows[0].Length; i++)
                {
                    matrix[i, j] = rows[j][i];
                    if (matrix[i, j] == 'S')
                    {
                        startPoint = new Point(i, j, 0);
                    }
                }
            }

            return (matrix, startPoint ?? throw new InvalidOperationException());
        }

        private static int[,] CreateDistanceMatrix(char[,] matrix)
        {
            return new int[matrix.GetLength(0), matrix.GetLength(1)];
        }
    }

    private record Point(int X, int Y, int Distance);
}
