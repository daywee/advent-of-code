using System.Reflection;

// Heavily inspired (read as copy-pasted with minor adjustmens) by https://github.com/JanVargovsky/advent-of-code.

var day = DateTime.UtcNow.Day;
var dayFolder = $"Day{day:D2}";

var solverType = Assembly.GetExecutingAssembly().GetType($"AdventOfCode2022.{dayFolder}.Solver") ?? throw new NotImplementedException("Solver not implemented.");
dynamic solver = Activator.CreateInstance(solverType)!;

var input = await GetOrDownloadInputAsync();
var result = Convert.ToString(solver.Solve(input));
Console.WriteLine(result);

async Task<string> GetOrDownloadInputAsync()
{
    var path = Path.Combine(dayFolder, "input.txt");
    if (!File.Exists(path))
    {
        const string AOCSESSION = "AOCSESSION";
        var session = Environment.GetEnvironmentVariable(AOCSESSION, EnvironmentVariableTarget.User)
            ?? throw new ArgumentException($"Env. variable '{AOCSESSION}' is missing.");
        using HttpClient httpClient = new();
        httpClient.DefaultRequestHeaders.Add("Cookie", $"session={session}");
        httpClient.DefaultRequestHeaders.UserAgent.Add(new("github.com/daywee/advent-of-code by d4v1d.nemec@gmail.com"));
        var input = await httpClient.GetStringAsync($"https://adventofcode.com/2022/day/{day}/input");
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        // downloaded input is using Unix line ending, so we convert according to current OS
        await File.WriteAllTextAsync(path, string.Join(Environment.NewLine, input[..^1].Split("\n")));
    }

    return await File.ReadAllTextAsync(path);
}
