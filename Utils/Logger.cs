namespace BackupMaker.Utils
{
    internal static class Logger
    {
        private const string LOG_DATE_FORMAT = "yyyy_MM_dd-hh_mm_ss";
        private const string LOG_EXTENSION = ".log";

        private static Queue<string>? _log;

        public static bool IsLoggingEnabled { get; private set; }

        public static void EnableLogging()
        {
            _log = new Queue<string>();

            IsLoggingEnabled = true;
        }

        public static void WriteLine(string str)
        {
            Console.WriteLine(str);

            _log?.Enqueue(str);
        }

        public static void WriteLine()
        {
            Console.WriteLine();

            _log?.Enqueue("");
        }

        public static void WriteErrorLine(string err)
        {
            Console.Error.WriteLine(err);

            _log?.Enqueue(err);
        }

        public static string? ReadLine()
        {
            string? line = Console.ReadLine();

            if (line != null)
                _log?.Enqueue(line);
            else
                _log?.Enqueue("null");

            return line;
        }

        public static ConsoleKeyInfo ReadKey(bool intercept)
        {
            var key = Console.ReadKey(intercept);

            _log?.Enqueue(key.KeyChar.ToString());

            return key;
        }

        public static void OnExit()
        {
            if (_log == null)
                return;

            Console.WriteLine("Log saving...");

            SaveLog(_log, Directory.GetCurrentDirectory());

            Console.WriteLine($"Done. Saved at: {"tets"}\n");
        }

        private static void SaveLog(Queue<string> log, string destinationFolder)
        {
            string currentTime = DateTime.Now.ToString(LOG_DATE_FORMAT);
            string logName = Path.Combine(destinationFolder, currentTime + LOG_EXTENSION);

            StreamWriter writer = new StreamWriter(logName);

            foreach (string line in log)
                writer.WriteLine(line);

            writer.Dispose();
        }
    }
}