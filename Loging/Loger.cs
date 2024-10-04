namespace BackupMaker;
internal static class Loger
{
    private const string LOG_EXTENSION = ".txt";

    private readonly static Queue<string> _log;

    public static bool IsEmpty => _log.Count == 0;

    static Loger()
    {
        _log = new();
    }

    public static void Add(string? str)
    {
        if (str != null)
            _log.Enqueue(str);
    }

    public static void Save(string directory)
    {
        string logName  = DateTime.Now.ToString("yyyy_MM_dd_HH") + LOG_EXTENSION;
        string savePath = Path.Combine(directory, logName);
    
        File.WriteAllLines(savePath, _log!);
    }
}