namespace AdventOfCode.Year2023.Day03;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
467..114..
...*......
..35..633.
......#...
617*......
.....+.58.
..592.....
......755.
...$.*....
.664.598..
""") == 4361);
    }

    public int Solve(string input)
    {
        var rows = input.Split(Environment.NewLine);
        var matrix = CreateMatrix(rows);
        var parts = FindParts(matrix);
        var result = parts.Sum(e => e.Value);

        return result;
    }

    private List<PartNumber> FindParts(char[,] matrix)
    {
        var parts = new List<PartNumber>();
        for (int j = 0; j < matrix.GetLength(1); j++)
        {
            var partNumber = 0;
            Point? partLocation = null;
            var hasPartNearBy = false;

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                var value = matrix[i, j];
                if (char.IsDigit(value))
                {
                    partLocation ??= new Point(i, j);

                    partNumber *= 10;
                    partNumber += (int)char.GetNumericValue(value);
                    if (GetNearByPoints(i, j, matrix).Any(e => IsPartSymbol(matrix[e.X, e.Y])))
                        hasPartNearBy = true;
                }

                if ((!char.IsDigit(value) || i == matrix.GetLength(0) - 1) && partLocation is not null)
                {
                    if (hasPartNearBy)
                        parts.Add(new PartNumber(partNumber, partLocation));

                    partNumber = 0;
                    partLocation = null;
                    hasPartNearBy = false;
                }
            }
        }

        return parts;

        bool IsPartSymbol(char symbol) => !char.IsDigit(symbol) && symbol != '.';
    }

    private IEnumerable<Point> GetNearByPoints(int i, int j, char[,] matrix)
    {
        var possiblePoints = new (int, int)[]
        {
            (0,1),
            (1,1),
            (1,0),
            (0,-1),
            (-1,-1),
            (-1,0),
            (1,-1),
            (-1, 1),
        };

        foreach (var (x, y) in possiblePoints)
        {
            var a = i + x;
            var b = j + y;

            if (a >= 0 && a < matrix.GetLength(0) && b >= 0 && b < matrix.GetLength(1))
                yield return new Point(a, b);
        }
    }

    private char[,] CreateMatrix(string[] rows)
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

    private record Point(int X, int Y);
    private record PartNumber(int Value, Point Point);
}
