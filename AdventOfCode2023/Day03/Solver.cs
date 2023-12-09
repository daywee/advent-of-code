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
""") == 467835);
    }

    public int Solve(string input)
    {
        var rows = input.Split(Environment.NewLine);
        var matrix = CreateMatrix(rows);
        var parts = FindParts(matrix);

        var result = parts
            .SelectMany(e => e.GearsNearBy, (part, gear) => new PartNumberOneGear(part.Value, part.Point, gear))
            .GroupBy(e => e.GearNearBy)
            .Where(e => e.Count() == 2)
            .Select(e => e.First().Value * e.Last().Value)
            .Sum();

        return result;
    }

    private List<PartNumber> FindParts(char[,] matrix)
    {
        var parts = new List<PartNumber>();
        for (int j = 0; j < matrix.GetLength(1); j++)
        {
            var partNumber = 0;
            Point? partLocation = null;
            var gearPartsNearBy = new HashSet<Point>();

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                var value = matrix[i, j];
                if (char.IsDigit(value))
                {
                    partLocation ??= new Point(i, j);

                    partNumber *= 10;
                    partNumber += (int)char.GetNumericValue(value);

                    var gears = GetNearByPoints(i, j, matrix).Where(e => IsGearPartSymbol(matrix[e.X, e.Y]));
                    gearPartsNearBy.UnionWith(gears);
                }

                if ((!char.IsDigit(value) || i == matrix.GetLength(0) - 1) && partLocation is not null)
                {
                    if (gearPartsNearBy.Any())
                        parts.Add(new PartNumber(partNumber, partLocation, gearPartsNearBy));

                    partNumber = 0;
                    partLocation = null;
                    gearPartsNearBy = new HashSet<Point>();
                }
            }
        }

        return parts;

        bool IsGearPartSymbol(char symbol) => symbol == '*';
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
    private record PartNumber(int Value, Point Point, HashSet<Point> GearsNearBy);
    private record PartNumberOneGear(int Value, Point Point, Point GearNearBy);
}
