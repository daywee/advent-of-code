namespace AdventOfCode.Year2023.Day08;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
LR

11A = (11B, XXX)
11B = (XXX, 11Z)
11Z = (11B, XXX)
22A = (22B, XXX)
22B = (22C, 22C)
22C = (22Z, 22Z)
22Z = (22B, 22B)
XXX = (XXX, XXX)
""") == 6);
    }

    public int Solve(string input)
    {
        var rows = input.Split(Environment.NewLine);

        var (instructions, nodes) = ParseInput(rows);

        var startNodes = nodes.Where(e => e.Key.EndsWith('A')).Select(e => e.Value).ToArray();

        var numberOfStepsForEachNode = startNodes.Select(FindNumberOfSteps).ToArray();

        // The result is LCM (Least Common Multiple)
        // https://www.calculatorsoup.com/calculators/math/lcm.php?input=16579+18827+19951+12083+22199+17141&data=none&action=solve
        // Result: 16,342,438,708,751

        return 0;

        int FindNumberOfSteps(Node startNode)
        {
            var currentNode = startNode;
            var steps = 0;
            do
            {
                var instruction = instructions[steps % instructions.Length];

                steps++;
                currentNode = instruction switch
                {
                    'L' => nodes[currentNode.Left],
                    'R' => nodes[currentNode.Right],
                    _ => throw new InvalidOperationException(),
                };
            } while (!currentNode.Value.EndsWith('Z'));

            return steps;
        }
    }

    private (string Instructions, Dictionary<string, Node> Nodes) ParseInput(string[] rows)
    {
        var instructions = rows[0];
        var nodes = new Dictionary<string, Node>();

        foreach (var row in rows.Skip(2))
        {
            var rowParts = row.Split(new[] { '=', ',', '(', ')' }, Constants.RemoveAndTrim);

            var currentNode = rowParts[0];
            var leftNode = rowParts[1];
            var rightNode = rowParts[2];

            nodes[currentNode] = new Node(currentNode, leftNode, rightNode);
        }

        return (instructions, nodes);
    }

    private record Node(string Value, string Left, string Right);
}
