namespace BackupMaker;

public class Ignore
{
    public readonly static Ignore Empty;
    public readonly static string Extension = ".ignore";

    private readonly IEnumerable<string> _excludeList;

    static Ignore()
    {
        Empty = new Ignore(Array.Empty<string>());
    }

    internal Ignore(IEnumerable<string> excludeList)
    {
        _excludeList = excludeList;
    }

    public static Ignore Parse(string path)
    {
        Queue<string> excludeList = new();
        using StreamReader reader = new(path);

        string? line;
        while ((line = reader.ReadLine()) != null)
            excludeList.Enqueue(line);

        return new Ignore(excludeList);
    }

    public bool IsIgnored(string path)
    {
        foreach (string exclude in _excludeList)
            if (path.EndsWith(exclude))
                return true;

        return false;
    }
}