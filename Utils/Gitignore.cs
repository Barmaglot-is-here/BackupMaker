namespace BackupMaker.Utils
{
    public class Gitignore
    {
        public static Gitignore Empty { get; } = new Gitignore(Array.Empty<string>());

        public enum PathType
        {
            File,
            Directory,
        }

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

                excludeList.Enqueue(line);
            }

            reader.Dispose();

            return new Gitignore(excludeList);
        }

        public bool IsIgnored(string path, PathType type)
        {
            if (_excludeList.Count() == 0)
                return false;

            if (type == PathType.File)
                path = Path.GetFileName(path);


            foreach (string exclude in _excludeList)
                if (path.Contains(exclude))
                    return true;

            return false;
        }
    }
}