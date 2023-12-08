namespace AdventOfCode.Year2023.Day08;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
RL

AAA = (BBB, CCC)
BBB = (DDD, EEE)
CCC = (ZZZ, GGG)
DDD = (DDD, DDD)
EEE = (EEE, EEE)
GGG = (GGG, GGG)
ZZZ = (ZZZ, ZZZ)
""") == 2);
    }

    public int Solve(string input)
    {
        var rows = input.Split(Environment.NewLine);

        var (instructions, nodes) = ParseInput(rows);

        var startNode = "AAA";
        var endNode = "ZZZ";

        var currentNode = nodes[startNode];

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
        } while (currentNode.Value != endNode);

        return steps;
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
