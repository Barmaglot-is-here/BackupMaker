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

                    ExtendedConsole.WriteLine();
                    _isOverwritingAllowed = Confirm("Directory contains duplicate files. Allow owerwriting?");
                    ExtendedConsole.WriteLine();
                }

                return _isOverwritingAllowed;
            }
        }

        static void Main(string[] args)
        {
            EncodingSetter.Set(1251);

            while (true)
            {
                Configuration config = Congfigure();

                if (!Confirm("Configuration is complete. Begin?"))
                    continue;

                string destinationFolder = config.CreateTopFolderToo
                    ? CreateTopLevelFolder(config.StartFolder, config.DestinationFolder)
                    : config.DestinationFolder;

                try
                {
                    MakeBackup(config.StartFolder, destinationFolder);
                }
                catch (Exception ex)
                {
                    if (!ExtendedConsole.IsLoggingEnabled)
                        ExtendedConsole.EnableLogging();

                    ExtendedConsole.WriteErrorLine(ex.ToString());

                    break;
                }

                Reset();

                if (!Confirm("Backup something else?"))
                    break;
            }

            ExtendedConsole.OnExit();
        }

        private static Configuration Congfigure()
        {
            string startFolder = GetPath("Enter start folder:", false);
            string destinationFolder = GetPath("Enter destination folder:", true);
            bool createTopFolderToo = Confirm("Create top level folder?");

            if (Confirm("Enable console output?"))
                ExtendedConsole.EnableOutput();

            if (Confirm("Enable logging?"))
                ExtendedConsole.EnableLogging();

            return new Configuration(startFolder, destinationFolder, createTopFolderToo);
        }

        private static string GetPath(string msg, bool createIfDoesntExists)
        {
            while (true)
            {
                ExtendedConsole.WriteLine(msg);

                string path = ExtendedConsole.ReadLine() ?? string.Empty;

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
                        ExtendedConsole.WriteLine("directory doesn't exists");

                        continue;
                    }

                ExtendedConsole.WriteLine("Ok");

                return path;
            }
        }

        private static bool Confirm(string msg)
        {
            while (true)
            {
                ExtendedConsole.WriteLine($"{msg} [Y/N]");

                var keyInfo = ExtendedConsole.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.Y)
                    return true;
                else if (keyInfo.Key == ConsoleKey.N)
                    return false;

                ExtendedConsole.WriteLine("Wrong key");
            }
        }

        private static string CreateTopLevelFolder(string startFolder, string destinationFolder)
        {
            string topLevelFolder = Path.GetDirectoryName(startFolder)!;
            destinationFolder += topLevelFolder;

            Directory.CreateDirectory(destinationFolder);

            return destinationFolder;
        }

        private static void MakeBackup(string startFolder, string destinationFolder)
        {
            ExtendedConsole.WriteLine("\n-----------------------Start-----------------------");

            DeepCopy(startFolder, destinationFolder, Gitignore.Empty);

            ExtendedConsole.WriteLine("-----------------------Done-----------------------\n");
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
                    ExtendedConsole.WriteLine(file + " --ignored");

                    continue;
                }

                string finalPath = GetFinalPath(file, copyTo);

                if (File.Exists(finalPath))
                {
                    if (IsOverwritingAllowed)
                    {
                        File.Copy(file, finalPath, true);

                        ExtendedConsole.WriteLine(file + " --owerwrited");
                    }
                    else
                        ExtendedConsole.WriteLine(file + " --skiped");
                }
                else
                {
                    File.Copy(file, finalPath);

                    ExtendedConsole.WriteLine(file);
                }
            }
        }

        private static void CopySubdirectories(string directory, string copyTo, Gitignore ignore)
        {
            foreach (var subdirectory in Directory.EnumerateDirectories(directory))
            {
                if (ignore.IsIgnored(subdirectory))
                {
                    ExtendedConsole.WriteLine(subdirectory + " --ignored");

                    continue;
                }

                string finalPath = GetFinalPath(subdirectory, copyTo);

                Directory.CreateDirectory(finalPath);

                ExtendedConsole.WriteLine(subdirectory);

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