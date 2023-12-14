using System.Text;

namespace AdventOfCode.Year2023.Day14;

internal class Solver
{
    public const int Iterations = 1_000_000_000;

    public Solver()
    {
        Debug.Assert(Solve("""
O....#....
O.OO#....#
.....##...
OO.#O....O
.O.....O#.
O.#..O.#.#
..O..#O..O
.......O..
#....###..
#OO..#....
""") == 64);
    }

    public int Solve(string input)
    {
        var rows = input.Split(Environment.NewLine);
        var matrix = new Matrix(rows);

        var directions = new List<FacingDirection> { FacingDirection.Up, FacingDirection.Left, FacingDirection.Down, FacingDirection.Right };

        var loadPerCycle = new List<int>();

        for (int i = 0; i < Iterations; i++)
        {
            foreach (var direction in directions)
            {
                matrix.Direction = direction;
                matrix.TiltUp();
            }

            loadPerCycle.Add(matrix.GetTotalLoad());

            var cycle = DetectCycle();
            if (cycle.HasValue)
                return cycle.Value;
        }

        throw new InvalidOperationException("Cycle not detected.");

        int? DetectCycle()
        {
            const int maxCycleLength = 100;
            const int minCycleLength = 10;

            if (loadPerCycle.Count < maxCycleLength * 2)
                return null;

            for (int cycleLength = minCycleLength; cycleLength < maxCycleLength; cycleLength++)
            {
                var from = loadPerCycle.Count - cycleLength;

                var detected = Enumerable.Range(from, cycleLength).All(i => loadPerCycle[i] == loadPerCycle[i - cycleLength]);
                if (detected)
                {
                    var x = (Iterations - from) % cycleLength;
                    var result = loadPerCycle[from + x - 1];

                    return result;
                }
            }

            return null;
        }
    }

    private class Matrix
    {
        private readonly char[,] _matrix;

        public Matrix(string[] rows)
        {
            _matrix = CreateMatrix(rows);
            Direction = FacingDirection.Up;
        }

        public FacingDirection Direction { get; set; }

        private int GetDimension(int dimension)
        {
            return Direction switch
            {
                FacingDirection.Up or FacingDirection.Down => _matrix.GetLength(dimension),
                FacingDirection.Left or FacingDirection.Right => _matrix.GetLength(dimension == 0 ? 1 : 0),
                _ => throw new InvalidOperationException(),
            };
        }

        private (int X, int Y) RotatePosition(int x, int y)
        {
            return Direction switch
            {
                FacingDirection.Up => (x, y),
                FacingDirection.Left => (y, _matrix.GetLength(1) - x - 1),
                FacingDirection.Down => (_matrix.GetLength(1) - x - 1, _matrix.GetLength(0) - y - 1),
                FacingDirection.Right => (_matrix.GetLength(0) - y - 1, x),
                _ => throw new InvalidOperationException(),
            };
        }

        private char GetSymbol(int x, int y)
        {
            var (rx, ry) = RotatePosition(x, y);

            return _matrix[rx, ry];
        }

        private void SetSymbol(int x, int y, char value)
        {
            var (rx, ry) = RotatePosition(x, y);

            _matrix[rx, ry] = value;
        }

        public void TiltUp()
        {
            for (int i = 0; i < GetDimension(0); i++)
            {
                var firstEmpty = 0;
                var rocks = 0;

                for (int j = 0; j < GetDimension(1); j++)
                {
                    var symbol = GetSymbol(i, j);
                    if (symbol == '#')
                    {
                        MoveRocks();

                        firstEmpty = j + 1;
                        rocks = 0;
                        continue;
                    }

                    if (symbol == '.')
                    {
                        continue;
                    }

                    if (symbol == 'O')
                    {
                        rocks++;
                        SetSymbol(i, j, '.');
                        continue;
                    }
                }

                MoveRocks();

                void MoveRocks()
                {
                    for (int r = 0; r < rocks; r++)
                    {
                        SetSymbol(i, firstEmpty + r, 'O');
                    }
                }
            }
        }

        public int GetTotalLoad()
        {
            var sum = 0;

            for (int j = 0; j < _matrix.GetLength(1); j++)
            {
                var rockWeight = _matrix.GetLength(1) - j;
                for (int i = 0; i < _matrix.GetLength(0); i++)
                {
                    if (_matrix[i, j] == 'O')
                    {
                        sum += rockWeight;
                    }
                }
            }

            return sum;
        }

        public void Print()
        {
            var sb = new StringBuilder();

            for (int j = 0; j < _matrix.GetLength(1); j++)
            {
                for (int i = 0; i < _matrix.GetLength(0); i++)
                {
                    sb.Append(_matrix[i, j]);
                }
                sb.AppendLine();
            }

            Console.WriteLine(sb.ToString());
        }

        private static char[,] CreateMatrix(string[] rows)
        {
            var matrix = new char[rows[0].Length, rows.Length];
            for (int j = 0; j < rows.Length; j++)
            {
                for (int i = 0; i < rows[0].Length; i++)
                {
                    matrix[i, j] = rows[j][i];
                }
            }

            return matrix;
        }
    }

    private enum FacingDirection
    {
        Up, Left, Down, Right
    }
}
