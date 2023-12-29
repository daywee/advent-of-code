namespace AdventOfCode.Year2023.Day07;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
32T3K 765
T55J5 684
KK677 28
KTJJT 220
QQQJA 483
""") == 5905);
    }

    public int Solve(string input)
    {
        var rows = input.Split(Environment.NewLine);
        var result = rows
            .Select(e => e.Split(' '))
            .Select(e => (Hand: new Hand(e[0]), Bet: int.Parse(e[1])))
            .OrderBy(e => e.Hand.ToNumericValue())
            .Select((e, i) => e.Bet * (i + 1))
            .Sum();

        return result;
    }

    private class Hand
    {
        private static List<char> _cardStrength = ['A', 'K', 'Q', 'T', '9', '8', '7', '6', '5', '4', '3', '2', 'J'];

        private readonly string _cards;

        public Hand(string cards)
        {
            _cards = cards;
        }

        public int ToNumericValue()
        {
            var typeValue = GetBestTypeNumericValue() * (int)Math.Pow(_cardStrength.Count, _cards.Length);
            var cardValues = _cards.Select((e, i) => GetCardNumericValue(e) * (int)Math.Pow(_cardStrength.Count, _cards.Length - i - 1));
            var cardsValue = cardValues.Sum();

            return typeValue + cardsValue;
        }

        private int GetCardNumericValue(char card)
        {
            var index = _cardStrength.IndexOf(card);
            if (index < 0)
                throw new InvalidOperationException();

            return _cardStrength.Count - index;
        }

        private int GetBestTypeNumericValue()
        {
            if (!_cards.Contains('J'))
                return GetTypeNumericValue(_cards);

            var allCardTypes = new HashSet<char>(_cards);
            allCardTypes.ExceptWith(['J']);

            var bestValue = GetTypeNumericValue(_cards);

            foreach (var type in allCardTypes)
            {
                var betterCards = _cards.Replace('J', type);
                var value = GetTypeNumericValue(betterCards);

                bestValue = Math.Max(bestValue, value);
            }

            return bestValue;
        }

        private int GetTypeNumericValue(string cards)
        {
            var grouped = cards.GroupBy(e => e).Select(e => (e.Key, Count: e.Count())).ToList();

            // five of a kind
            if (grouped.Count == 1)
                return 7;

            // four of a kind
            if (grouped.Any(e => e.Count == 4))
                return 6;

            // full house
            if (grouped.Count == 2 && grouped.Any(e => e.Count == 3) && grouped.Any(e => e.Count == 2))
                return 5;

            // three of a kind
            if (grouped.Count == 3 && grouped.Any(e => e.Count == 3))
                return 4;

            // two pair
            if (grouped.Count == 3 && grouped.Where(e => e.Count == 2).Count() == 2)
                return 3;

            // one pair
            if (grouped.Count == 4 && grouped.Where(e => e.Count == 2).Count() == 1)
                return 2;

            if (grouped.Count == 5)
                return 1;

            return 0;
        }

        public override string ToString() => $"{_cards} ({ToNumericValue()})";
    }
}
