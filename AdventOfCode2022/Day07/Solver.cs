namespace AdventOfCode2022.Day07;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
        $ cd /
        $ ls
        dir a
        14848514 b.txt
        8504156 c.dat
        dir d
        $ cd a
        $ ls
        dir e
        29116 f
        2557 g
        62596 h.lst
        $ cd e
        $ ls
        584 i
        $ cd ..
        $ cd ..
        $ cd d
        $ ls
        4060174 j
        8033020 d.log
        5626152 d.ext
        7214296 k
        """) == 95437);
    }

    public int Solve(string input)
    {
        var root = ParseDirectoryTree(input);

        var directories = FindDirectories(root, 100_000);

        return directories.Sum(e => e.Size);
    }

    private static List<Directory> FindDirectories(Directory root, int maxSize)
    {
        var result = new List<Directory>();

        var toExplore = new Queue<Directory>();
        toExplore.Enqueue(root);

        while (toExplore.Count > 0)
        {
            var current = toExplore.Dequeue();
            current.Directories.ForEach(e => toExplore.Enqueue(e));

            if (current.Size <= maxSize)
            {
                result.Add(current);
            }
        }

        return result;
    }

    private static Directory ParseDirectoryTree(string input)
    {
        var lines = input.Split(Environment.NewLine);

        var rootDirectory = new Directory("/");
        var currentDirectory = rootDirectory;

        foreach (var line in lines)
        {
            currentDirectory = line switch
            {
                ['$', ' ', .. var command] => command switch
                {
                    ['c', 'd', ' ', .. var directoryName] => ChangeDirectory(directoryName),
                    ['l', 's'] => currentDirectory,
                    _ => throw new Exception(),
                },
                ['d', 'i', 'r', ' ', .. var directoryName] => AddDirectory(directoryName),
                var sizeWithName => AddFile(sizeWithName)
            };
        }

        return rootDirectory;

        Directory ChangeDirectory(string name)
        {
            return name switch
            {
                ['/'] => rootDirectory,
                ['.', '.'] => currentDirectory!.Parent ?? throw new Exception("Parent is null"),
                _ => currentDirectory!.Directories.Single(e => e.Name == name),
            };
        }

        Directory AddDirectory(string name)
        {
            if (!currentDirectory!.Directories.Any(e => e.Name == name))
                currentDirectory.Directories.Add(new Directory(name, currentDirectory));

            return currentDirectory;
        }

        Directory AddFile(string sizeWithName)
        {
            var splitted = sizeWithName.Split(' ');
            var newFile = new File(splitted[1], int.Parse(splitted[0]));
            if (!currentDirectory!.Files.Any(e => e.Name == newFile.Name))
                currentDirectory.Files.Add(newFile);

            return currentDirectory;
        }
    }

    [DebuggerDisplay("{Name}")]
    private class Directory
    {
        public string Name { get; set; }
        public Directory? Parent { get; set; }
        public List<Directory> Directories { get; set; } = new();
        public List<File> Files { get; set; } = new();
        public int Size => Files.Sum(e => e.Size) + Directories.Sum(e => e.Size);

        public Directory(string name, Directory? parent = null)
        {
            Name = name;
            Parent = parent;
        }
    }

    [DebuggerDisplay("{Name} ({Size})")]
    private class File
    {
        public string Name { get; set; }
        public int Size { get; set; }

        public File(string name, int size)
        {
            Name = name;
            Size = size;
        }
    }
}
