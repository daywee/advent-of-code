namespace AdventOfCode.Year2023.Day05;
internal class Solver
{
    private const StringSplitOptions _removeAndTrim = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;

    public Solver()
    {
        Debug.Assert(Solve("""
seeds: 79 14 55 13

seed-to-soil map:
50 98 2
52 50 48

soil-to-fertilizer map:
0 15 37
37 52 2
39 0 15

fertilizer-to-water map:
49 53 8
0 11 42
42 0 7
57 7 4

water-to-light map:
88 18 7
18 25 70

light-to-temperature map:
45 77 23
81 45 19
68 64 13

temperature-to-humidity map:
0 69 1
1 0 69

humidity-to-location map:
60 56 37
56 93 4
""") == 46);
    }

    public long Solve(string input)
    {
        var rows = input.Split(Environment.NewLine);

        var (seedRanges, maps) = ParseInput(rows);
        var seedCount = GetSeedCount(seedRanges);

        var sw = Stopwatch.StartNew();

        // this is naive brute force solution
        var result = GetSeeds(seedRanges)
            .Select((e, i) =>
            {
                if (i % 1_000_000 == 0)
                    Console.WriteLine($"{i}/{seedCount} ({(double)i / seedCount * 100}%) - elapsed {sw.Elapsed.TotalMinutes} minutes");
                return MapAll(e);
            })
            .Min();

        return result;

        long MapAll(long seed) => maps!.Aggregate(seed, (acc, map) => map.MapToDestination(acc));
    }

    private IEnumerable<long> GetSeeds(long[] seedRanges)
    {
        for (int i = 0; i < seedRanges.Length / 2; i++)
        {
            var from = seedRanges[i * 2];
            var range = seedRanges[i * 2 + 1];

            Console.WriteLine($"doing from:{from} range:{range}");
            for (long j = 0; j < range; j++)
            {
                yield return from + j;
            }
        }
    }

    private long GetSeedCount(long[] seedRanges)
    {
        long sum = 0;
        for (int i = 0; i < seedRanges.Length / 2; i++)
        {
            var range = seedRanges[i * 2 + 1];
            sum += range;
        }

        return sum;
    }

    private (long[] SeedRanges, List<Map> Maps) ParseInput(string[] rows)
    {
        var seedRanges = rows[0].Split(' ', _removeAndTrim).Skip(1).Select(long.Parse).ToArray();

        var maps = new List<Map>
        {
            new Map(MappingType.Seed, MappingType.Soil),
            new Map(MappingType.Soil, MappingType.Fertilizer),
            new Map(MappingType.Fertilizer, MappingType.Water),
            new Map(MappingType.Water, MappingType.Light),
            new Map(MappingType.Light, MappingType.Temperature),
            new Map(MappingType.Temperature, MappingType.Humidity),
            new Map(MappingType.Humidity, MappingType.Location),
        };


        Map currentMap = null;
        foreach (var row in rows.Skip(1))
        {
            if (row.Contains("map"))
            {
                var mappingParts = row.Split(new[] { '-', ' ' }, _removeAndTrim);
                var sourceType = Enum.Parse<MappingType>(mappingParts[0], true);
                var destinationType = Enum.Parse<MappingType>(mappingParts[2], true);

                currentMap = maps.Single(e => e.SourceType == sourceType && e.DestinationType == destinationType);

                continue;
            }

            if (string.IsNullOrEmpty(row))
                continue;

            var mappingIds = row.Split(' ', _removeAndTrim).Select(long.Parse).ToArray();
            currentMap!.AddMappingRule(mappingIds[0], mappingIds[1], mappingIds[2]);
        }

        return (seedRanges, maps);
    }

    class Map
    {
        private readonly List<MappingRule> _rules = [];

        public Map(MappingType sourceType, MappingType destinationType)
        {
            SourceType = sourceType;
            DestinationType = destinationType;
        }

        public MappingType SourceType { get; }
        public MappingType DestinationType { get; }

        public void AddMappingRule(long destinationRangeStart, long sourceRangeStart, long rangeLength)
        {
            _rules.Add(new(sourceRangeStart, sourceRangeStart + rangeLength - 1, destinationRangeStart - sourceRangeStart));
        }

        public long MapToDestination(long source)
        {
            var rule = _rules.FirstOrDefault(e => e.SourceStart <= source && e.SourceEnd >= source);

            if (rule is null)
                return source;

            return source + rule.Diff;
        }
    }

    record MappingRule(long SourceStart, long SourceEnd, long Diff);

    enum MappingType
    {
        Seed,
        Soil,
        Fertilizer,
        Water,
        Light,
        Temperature,
        Humidity,
        Location,
    }
}
