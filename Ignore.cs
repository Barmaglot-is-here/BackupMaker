namespace BackupMaker
{
    public class Ignore
    {
        public readonly static Ignore Empty;

        private IEnumerable<string> _excludeList;

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
            Queue<string> excludeList = new Queue<string>();
            excludeList.Enqueue("\\.git");

            StreamReader reader = new StreamReader(path);

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine()!;

                line = line.Replace("/", "\\");
                line = line.Replace("*", "");

                excludeList.Enqueue(line);
            }

            reader.Dispose();

            return new Ignore(excludeList);
        }

        public bool IsIgnored(string path)
        {
            if (_excludeList.Count() == 0)
                return false;

            foreach (string exclude in _excludeList)
                if (path.EndsWith(exclude))
                    return true;

            return false;
        }
    }
}