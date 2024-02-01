namespace BackupMaker.Utils
{
    internal static class ExtendedConsole
    {
        private const string LOG_DATE_FORMAT = "yyyy_MM_dd-hh_mm_ss";
        private const string LOG_EXTENSION = ".log";

        private static Queue<string>? _log;

        public static bool IsLoggingEnabled { get; private set; }
        public static bool IsOutputEnabled { get; private set; }

        public static void EnableOutput()
        {
            IsOutputEnabled = true;
        }
        
        public static void DisableOutput()
        {
            IsOutputEnabled = false;
        }

        public static void EnableLogging()
        {
            if (!IsLoggingEnabled)
                _log = new Queue<string>();

            IsLoggingEnabled = true;
        }
        
        public static void DisableLogging()
        {
            _log = null;

            IsLoggingEnabled = false;
        }

        public static void WriteLine(string str)
        {
            if (IsOutputEnabled)
                Console.WriteLine(str);

            _log?.Enqueue(str);
        }

        public static void WriteLine()
        {
            if (IsOutputEnabled)
                Console.WriteLine();

            _log?.Enqueue("");
        }

        public static void WriteErrorLine(string err)
        {
            if (IsOutputEnabled)
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

        public static void SaveLog()
        {
            if (_log == null)
                return;

            WriteLine("Log saving...");

            var saveFolder = Directory.GetCurrentDirectory();
            SaveLog(_log, saveFolder);

            WriteLine($"Done. Saved at: {saveFolder}\n");
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