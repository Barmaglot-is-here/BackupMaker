using BackupMaker.Configuration;
using BackupMaker.Utils;

namespace BackupMaker
{
    internal class Backup
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

                    Console.WriteLine();
                    _isOverwritingAllowed = Command.Confirm("Directory contains duplicate files. Allow owerwriting?");
                    Console.WriteLine();
                }

                return _isOverwritingAllowed;
            }
        }

        public static void Make(Config config, string startFolder, string destinationFolder)
        {
            Reset();

            destinationFolder = config.CreateTopFolderToo
                ? CreateTopLevelFolder(startFolder, destinationFolder)
                : destinationFolder;

            if (config.EnableOutput)
                ExtendedConsole.EnableOutput();

            ExtendedConsole.WriteLine("\nStart");

            if (config.EnableLogging)
                ExtendedConsole.EnableLogging();

            DeepCopy(startFolder, destinationFolder, Ignore.Empty);

            ExtendedConsole.SaveLog();

            if (config.EnableLogging)
                ExtendedConsole.DisableLogging();

            ExtendedConsole.WriteLine("----------------------------------------------\n");

            ExtendedConsole.DisableOutput();
        }

        private static string CreateTopLevelFolder(string startFolder, string destinationFolder)
        {
            string topLevelFolder   = Path.GetFileName(startFolder)!;
            destinationFolder       = Path.Combine(destinationFolder, topLevelFolder);

            Directory.CreateDirectory(destinationFolder);

            return destinationFolder;
        }

        private static void DeepCopy(string directory, string copyTo, Ignore ignore)
        {
            ignore = GetIgnore(directory) ?? ignore;

            CopyFiles(directory, copyTo, ignore);
            CopySubdirectories(directory, copyTo, ignore);
        }

        private static Ignore? GetIgnore(string directory)
        {
            string path = Path.Combine(directory, ".gitignore");

            return File.Exists(path) ? Ignore.Parse(path) : null;
        }

        private static void CopyFiles(string directory, string copyTo, Ignore ignore)
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

        private static void CopySubdirectories(string directory, string copyTo, Ignore ignore)
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
            _isOverwritingAllowed = _owerwritingConfirmed = false;
        }
    }
}