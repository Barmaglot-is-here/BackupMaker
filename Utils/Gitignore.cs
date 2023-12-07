namespace BackupMaker.Utils
{
    public class Gitignore
    {
        public static Gitignore Empty { get; } = new Gitignore(Array.Empty<string>());

        private IEnumerable<string> _excludeList;

        internal Gitignore(IEnumerable<string> excludeList)
        {
            _excludeList = excludeList;
        }

        public static Gitignore Parse(string path)
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

            return new Gitignore(excludeList);
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