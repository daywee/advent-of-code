using System.Text;

namespace AdventOfCode.Year2023.Day14;

internal class Solver
{
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
""") == 136);
    }

    public int Solve(string input)
    {
        var rows = input.Split(Environment.NewLine);
        var matrix = new Matrix(rows);

        matrix.TiltUp();
        matrix.Print();

        var result = matrix.GetTotalLoad();

        return result;
    }

    private class Matrix
    {
        private readonly char[,] _matrix;

        public Matrix(string[] rows)
        {
            _matrix = CreateMatrix(rows);
        }

        public void TiltUp()
        {
            for (int i = 0; i < _matrix.GetLength(0); i++)
            {
                var firstEmpty = 0;
                var rocks = 0;

                for (int j = 0; j < _matrix.GetLength(1); j++)
                {
                    var symbol = _matrix[i, j];
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
                        _matrix[i, j] = '.';
                        continue;
                    }
                }

                MoveRocks();

                void MoveRocks()
                {
                    for (int r = 0; r < rocks; r++)
                    {
                        _matrix[i, firstEmpty + r] = 'O';
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
}
