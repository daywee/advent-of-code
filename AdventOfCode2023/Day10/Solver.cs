using System.Text;

namespace AdventOfCode.Year2023.Day10;

internal class Solver
{
    private const int DistanceInitialValue = -100;

    public Solver()
    {
        Debug.Assert(Solve("""
        FF7FSF7F7F7F7F7F---7
        L|LJ||||||||||||F--J
        FL-7LJLJ||||||LJL-77
        F--JF--7||LJLJ7F7FJ-
        L---JF-JLJ.||-FJLJJ7
        |F|F-JF---7F7-L7L|7|
        |FFJF7L7F-JF7|JL---7
        7-L-JL7||F7|L7F-7F7|
        L.L7LFJ|||||FJL7||LJ
        L7JLJL-JLJLJL--JLJ.L
        """) == 10);

        Debug.Assert(Solve("""
        .F----7F7F7F7F-7....
        .|F--7||||||||FJ....
        .||.FJ||||||||L7....
        FJL7L7LJLJ||LJ.L-7..
        L--J.L7...LJS7F-7L7.
        ....F-J..F7FJ|L7L7L7
        ....L7.F7||L7|.L7L7|
        .....|FJLJ|FJ|F7|.LJ
        ....FJL-7.||.||||...
        ....L---J.LJ.LJLJ...
        """) == 8);

        Debug.Assert(Solve("""
        ...........
        .S-------7.
        .|F-----7|.
        .||.....||.
        .||.....||.
        .|L-7.F-J|.
        .|..|.|..|.
        .L--J.L--J.
        ...........
        """) == 4);

        Debug.Assert(Solve("""
        ..........
        .S------7.
        .|F----7|.
        .||OOOO||.
        .||OOOO||.
        .|L-7F-J|.
        .|II||II|.
        .L--JL--J.
        ..........
        """) == 4);
    }

    public int Solve(string input)
    {
        var rows = input.Split(Environment.NewLine);
        var matrix = new Matrix(rows);

        _ = matrix.Solve();

        var result = matrix.Solve2();

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

        public int Solve2()
        {
            ReplaceStartPoint();

            var insideCount = 0;
            var insideMatrix = new char[_distanceMatrix.GetLength(0), _distanceMatrix.GetLength(1)];
            for (int j = 0; j < _distanceMatrix.GetLength(1); j++)
                for (int i = 0; i < _distanceMatrix.GetLength(0); i++)
                    insideMatrix[i, j] = '.';

            for (int j = 0; j < _distanceMatrix.GetLength(1); j++)
            {
                var isInside = false;
                char? horizontalWallStartSymbol = null;

                for (int i = 0; i < _distanceMatrix.GetLength(0); i++)
                {
                    var distance = _distanceMatrix[i, j];
                    var symbol = _matrix[i, j];

                    // this is wall
                    if (distance >= 0)
                    {
                        if (symbol is '-')
                            continue;

                        var isVerticalWall = symbol is '|';
                        if (isVerticalWall)
                        {
                            isInside = !isInside;
                            continue;
                        }

                        var isHorizontalWallStart = symbol is 'F' or 'L';
                        if (isHorizontalWallStart)
                        {
                            horizontalWallStartSymbol = symbol;
                            continue;
                        }

                        var isHorizontalWallEnd = symbol is '7' or 'J';
                        if (isHorizontalWallEnd)
                        {
                            if ((horizontalWallStartSymbol is 'F' && symbol is 'J') || (horizontalWallStartSymbol is 'L' && symbol is '7'))
                            {
                                isInside = !isInside;
                            }

                            horizontalWallStartSymbol = null;
                            continue;
                        }

                        throw new InvalidOperationException();
                    }

                    if (isInside)
                    {
                        insideCount++;
                        insideMatrix[i, j] = 'X';
                    }
                }
            }

            //var printed = Print(insideMatrix);
            //Console.WriteLine(printed);

            return insideCount;
        }

        private void ReplaceStartPoint()
        {
            var left = GetValue(StartPoint.X - 1, StartPoint.Y);
            var right = GetValue(StartPoint.X + 1, StartPoint.Y);
            var up = GetValue(StartPoint.X, StartPoint.Y - 1);
            var down = GetValue(StartPoint.X, StartPoint.Y + 1);

            var newSymbol = (left, right, up, down) switch
            {
                (1, 1, _, _) => '-',
                (1, _, 1, _) => 'J',
                (1, _, _, 1) => '7',
                (_, 1, 1, _) => 'L',
                (_, _, 1, 1) => '|',
                (_, 1, _, 1) => 'F',
                _ => throw new InvalidOperationException()
            };

            _matrix[StartPoint.X, StartPoint.Y] = newSymbol;

            int GetValue(int x, int y)
            {
                var p = new Point(x, y, 0);
                return IsValid(p) ? _distanceMatrix[p.X, p.Y] : -1;
            }
        }

        public string Print(char[,] m)
        {
            var sb = new StringBuilder();

            for (int j = 0; j < m.GetLength(1); j++)
            {
                for (int i = 0; i < m.GetLength(0); i++)
                {
                    sb.Append(m[i, j]);
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

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

            if (_distanceMatrix[to.X, to.Y] != DistanceInitialValue)
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
            var d = new int[matrix.GetLength(0), matrix.GetLength(1)];
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    d[i, j] = DistanceInitialValue;
                }
            }
            return d;
        }
    }

    private record Point(int X, int Y, int Distance);
}
