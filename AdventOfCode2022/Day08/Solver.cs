namespace AdventOfCode2022.Day08;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
        30373
        25512
        65332
        33549
        35390
        """) == 8);
    }

    public int Solve(string input)
    {
        var lines = input.Split(Environment.NewLine);

        var sizeX = lines[0].Length;
        var sizeY = lines.Length;

        var scenicScoreMap = new int[sizeX, sizeY];

        for (int i = 0; i < sizeX; i++)
        {
            var previousHeights = new List<int>();
            foreach (var j in Enumerable.Range(0, sizeY))
            {
                UpdateVisibilityMap(i, j, previousHeights);
            }
            previousHeights = new List<int>();
            foreach (var j in Enumerable.Range(0, sizeY).Reverse())
            {
                UpdateVisibilityMap(i, j, previousHeights);
            }
        }

        for (int j = 0; j < sizeY; j++)
        {
            var previousHeights = new List<int>();
            foreach (var i in Enumerable.Range(0, sizeX))
            {
                UpdateVisibilityMap(i, j, previousHeights);
            }
            previousHeights = new List<int>();
            foreach (var i in Enumerable.Range(0, sizeX).Reverse())
            {
                UpdateVisibilityMap(i, j, previousHeights);
            }
        }

        var maxScore = 0;
        foreach (var score in scenicScoreMap)
        {
            maxScore = Math.Max(score, maxScore);
        }

        return maxScore;

        int GetTreeHeight(int x, int y)
        {
            return int.Parse(lines[y][x].ToString());
        }

        void UpdateVisibilityMap(int i, int j, List<int> previousHeights)
        {
            var currentTreeHeight = GetTreeHeight(i, j);
            // Edge tree - at least one direction is always 0 => scenic score must be 0 too.
            if (i == 0 || j == 0 || i == sizeX - 1 || j == sizeY - 1)
            {
                previousHeights.Add(currentTreeHeight);
                return;
            }

            var score = 0;
            foreach (var prev in (previousHeights as IEnumerable<int>).Reverse())
            {
                score++;
                if (prev >= currentTreeHeight)
                    break;
            }

            if (scenicScoreMap[i, j] == 0)
                scenicScoreMap[i, j] = 1;

            scenicScoreMap[i, j] *= score;

            previousHeights.Add(currentTreeHeight);
        }
    }
}
