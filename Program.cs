using BackupMaker.Utils;

namespace BackupMaker
{
    internal class Program
    {
        private static bool _owerwritingConfirmed;
        private static bool _isOverwritingAllowed;

        private static bool IsOverwritingAllowed 
        {  
            get
            {
                if (!_owerwritingConfirmed)
                {
                    _owerwritingConfirmed = true;

                    Logger.WriteLine();
                    _isOverwritingAllowed = Confirm("Directory contains duplicate files. Allow owerwriting?");
                    Logger.WriteLine();
                }

                return _isOverwritingAllowed;
            }
        }

        static void Main(string[] args)
        {
            ParseArgs(args);
            EncodingSetter.Set(1251);

            while (true)
            {
                string startFolder = GetPath("Enter start folder:", false);
                string destinationFolder = GetPath("Enter destination folder:", true);

                if (!Confirm("Configuration is complete. Begin?"))
                    break;

                try
                {
                    MakeBackup(startFolder, destinationFolder);
                }
                catch (Exception ex)
                {
                    if (!Logger.IsLoggingEnabled)
                        Logger.EnableLogging();

                    Logger.WriteErrorLine(ex.ToString());

                    break;
                }

                Reset();

                if (!Confirm("Backup something else?"))
                    break;
            }

            Logger.OnExit();
        }

        private static void ParseArgs(string[] args)
        {
            if (args.Contains("-log"))
                Logger.EnableLogging();
        }

        private static string GetPath(string msg, bool createIfDoesntExists)
        {
            while (true)
            {
                Logger.WriteLine(msg);

                string path = Logger.ReadLine() ?? string.Empty;

                if (!Directory.Exists(path))
                    if (createIfDoesntExists)
                    {
                        if (Confirm("directory doesn't exists. Create?"))
                            Directory.CreateDirectory(path);
                        else
                            continue;
                    }
                    else
                    {
                        Logger.WriteLine("directory doesn't exists");

                        continue;
                    }

                Logger.WriteLine("Ok");

                return path;
            }
        }

        private static bool Confirm(string msg)
        {
            while (true)
            {
                Logger.WriteLine($"{msg} [Y/N]");

                var key = Logger.ReadKey(true);

                if (key.KeyChar == 'Y' || key.KeyChar == 'y')
                    return true;
                else if (key.KeyChar == 'N' || key.KeyChar == 'n')
                    return false;

                Logger.WriteLine("Wrong key");
            }
        }

        private static void MakeBackup(string startFolder, string destinationFolder)
        {
            Logger.WriteLine("\n-----------------------Start-----------------------");

            DeepCopy(startFolder, destinationFolder, Gitignore.Empty);

            Logger.WriteLine("-----------------------Done-----------------------\n");
        }

        private static void DeepCopy(string directory, string copyTo, Gitignore ignore)
        {
            ignore = GetIgnore(directory) ?? ignore;

            CopyFiles(directory, copyTo, ignore);
            CopySubdirectories(directory, copyTo, ignore);
        }

        private static Gitignore? GetIgnore(string directory)
        {
            string path = Path.Combine(directory, ".gitignore");

            return File.Exists(path) ? Gitignore.Parse(path) : null;
        }

        private static void CopyFiles(string directory, string copyTo, Gitignore ignore)
        {
            foreach (var file in Directory.EnumerateFiles(directory))
            {
                if (ignore.IsIgnored(file))
                {
                    Logger.WriteLine(file + " --ignored");

                    continue;
                }

                string finalPath = GetFinalPath(file, copyTo);

                if (File.Exists(finalPath))
                {
                    if (IsOverwritingAllowed)
                    {
                        File.Copy(file, finalPath, true);

                        Logger.WriteLine(file + " --owerwrited");
                    }
                    else
                        Logger.WriteLine(file + " --skiped");
                }
                else
                {
                    File.Copy(file, finalPath);

                    Logger.WriteLine(file);
                }
            }
        }

        private static void CopySubdirectories(string directory, string copyTo, Gitignore ignore)
        {
            foreach (var subdirectory in Directory.EnumerateDirectories(directory))
            {
                if (ignore.IsIgnored(subdirectory))
                {
                    Logger.WriteLine(subdirectory + " --ignored");

                    continue;
                }

                string finalPath = GetFinalPath(subdirectory, copyTo);

                Directory.CreateDirectory(finalPath);

                Logger.WriteLine(subdirectory);

                DeepCopy(subdirectory, finalPath, ignore);
            }
        }

        private static string GetFinalPath(string currentPath, string destinationFolder)
        {
            currentPath = Path.GetFileName(currentPath);

            return Path.Combine(destinationFolder, currentPath);
        }

        private static void Reset()
        {
            _owerwritingConfirmed = false;
        }
    }
}