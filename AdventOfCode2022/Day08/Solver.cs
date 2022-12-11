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
        """) == 21);
    }

    public int Solve(string input)
    {
        var lines = input.Split(Environment.NewLine);

        var sizeX = lines[0].Length;
        var sizeY = lines.Length;

        var visibilityMap = new bool[sizeX, sizeY];

        for (int i = 0; i < sizeX; i++)
        {
            var maxHeight = -1;
            foreach (var j in Enumerable.Range(0, sizeY))
            {
                UpdateVisibilityMap(i, j, ref maxHeight);
            }
            maxHeight = -1;
            foreach (var j in Enumerable.Range(0, sizeY).Reverse())
            {
                UpdateVisibilityMap(i, j, ref maxHeight);
            }
        }

        for (int j = 0; j < sizeY; j++)
        {
            var maxHeight = -1;
            foreach (var i in Enumerable.Range(0, sizeX))
            {
                UpdateVisibilityMap(i, j, ref maxHeight);
            }
            maxHeight = -1;
            foreach (var i in Enumerable.Range(0, sizeX).Reverse())
            {
                UpdateVisibilityMap(i, j, ref maxHeight);
            }
        }

        var visibleTrees = 0;
        foreach (var isTreeVisible in visibilityMap)
        {
            if (isTreeVisible)
                visibleTrees++;
        }

        return visibleTrees;

        int GetTreeHeight(int x, int y)
        {
            return int.Parse(lines[y][x].ToString());
        }

        void UpdateVisibilityMap(int i, int j, ref int maxHeight)
        {
            var currentTreeHeight = GetTreeHeight(i, j);
            if (i == 0 || j == 0 || i == sizeX - 1 || j == sizeY - 1)
            {
                visibilityMap[i, j] = true;
                maxHeight = Math.Max(maxHeight, currentTreeHeight);
                return;
            }

            if (currentTreeHeight > maxHeight)
            {
                visibilityMap[i, j] = true;
                maxHeight = currentTreeHeight;
            }
        }
    }
}

