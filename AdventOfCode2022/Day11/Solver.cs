using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace AdventOfCode2022.Day11;

public class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
        Monkey 0:
          Starting items: 79, 98
          Operation: new = old * 19
          Test: divisible by 23
            If true: throw to monkey 2
            If false: throw to monkey 3

        Monkey 1:
          Starting items: 54, 65, 75, 74
          Operation: new = old + 6
          Test: divisible by 19
            If true: throw to monkey 2
            If false: throw to monkey 0

        Monkey 2:
          Starting items: 79, 60, 97
          Operation: new = old * old
          Test: divisible by 13
            If true: throw to monkey 1
            If false: throw to monkey 3

        Monkey 3:
          Starting items: 74
          Operation: new = old + 3
          Test: divisible by 17
            If true: throw to monkey 0
            If false: throw to monkey 1
        """) == 10605);
    }

    public int Solve(string input)
    {
        var rounds = 20;
        var monkeys = ParseInput(input);

        for (int round = 0; round < rounds; round++)
        {
            foreach (var monkey in monkeys)
            {
                foreach (var itemWorryLevel in monkey.ItemWorryLevels)
                {
                    var newWorryLevel = monkey.GetNewWorryLevel(itemWorryLevel);
                    newWorryLevel /= 3;

                    var throwToMonkey = newWorryLevel % monkey.DivisibleTest == 0
                        ? monkey.MonkeyIdToThrowWhenDivisibleTestIsTrue
                        : monkey.MonkeyIdToThrowWhenDivisibleTestIsFalse;

                    monkeys[throwToMonkey].ItemWorryLevels.Add(newWorryLevel);
                }

                monkey.ItemsInspected += monkey.ItemWorryLevels.Count;
                monkey.ItemWorryLevels.Clear();
            }
        }

        var mostActiveMonkeys = monkeys.OrderByDescending(e => e.ItemsInspected).Take(2);
        var monkeyBusiness = mostActiveMonkeys.Aggregate(1, (e, acc) => e * acc.ItemsInspected);

        return monkeyBusiness;
    }

    private List<Monkey> ParseInput(string input)
    {
        var monkeys = new List<Monkey>();

        var monkeyInputs = input.Split(Environment.NewLine + Environment.NewLine);
        foreach (var monkeyInput in monkeyInputs)
        {
            var monkeyLines = monkeyInput.Split(Environment.NewLine);
            var monkey = new Monkey { Id = monkeys.Count };
            foreach (var monkeyLine in monkeyLines)
            {
                var monkeyAttribute = monkeyLine.Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                switch (monkeyAttribute)
                {
                    case ["Starting items", var items]:
                        monkey.ItemWorryLevels = new List<int>(items.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(int.Parse));
                        break;
                    case ["Operation", var operation]:
                        monkey.Operation = operation;
                        break;
                    case ["Test", var test]:
                        monkey.DivisibleTest = int.Parse(test.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[^1]);
                        break;
                    case ["If true", var throwToMonkey]:
                        monkey.MonkeyIdToThrowWhenDivisibleTestIsTrue = int.Parse(throwToMonkey.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[^1]);
                        break;
                    case ["If false", var throwToMonkey]:
                        monkey.MonkeyIdToThrowWhenDivisibleTestIsFalse = int.Parse(throwToMonkey.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[^1]);
                        break;
                    default:
                        break;
                }
            }

            monkey.Initialize();
            monkeys.Add(monkey);
        }

        return monkeys;
    }

    private class Monkey
    {
        private Script<int> _script;

        public int Id { get; set; }
        public List<int> ItemWorryLevels { get; set; }
        public string Operation { get; set; }
        public int DivisibleTest { get; set; }
        public int MonkeyIdToThrowWhenDivisibleTestIsTrue { get; set; }
        public int MonkeyIdToThrowWhenDivisibleTestIsFalse { get; set; }

        public int ItemsInspected { get; set; }

        public int GetNewWorryLevel(int old)
        {
            var result = _script.RunAsync(new ScriptGlobals { old = old }).Result;

            return result.ReturnValue;
        }

        public void Initialize()
        {
            var script = $"""
                var @{Operation};
                return @new;
                """;

            _script = CSharpScript.Create<int>(script, globalsType: typeof(ScriptGlobals));
            _script.Compile();
        }
    }

    public class ScriptGlobals
    {
        public int old;
    }
}
