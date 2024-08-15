using BackupMaker.Configuration;

namespace BackupMaker;

internal static class Backup
{
    private static bool _enableConsoleOutput;

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
        destinationFolder = config.CreateTopFolderToo
            ? CreateTopLevelFolder(startFolder, destinationFolder)
            : destinationFolder;

        _enableConsoleOutput = config.EnableOutput;

        Console.WriteLine("Start");

        DeepCopy(startFolder, destinationFolder, Ignore.Empty);

        Console.WriteLine("Done\n");
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
        string path = Path.Combine(directory, Ignore.Extension);

        return File.Exists(path) ? Ignore.Parse(path) : null;
    }

    private static void CopyFiles(string directory, string copyTo, Ignore ignore)
    {
        foreach (var file in Directory.EnumerateFiles(directory))
        {
            if (ignore.IsIgnored(file))
            {
                WriteLine(file + " --ignored");

                continue;
            }

            string finalPath = GetFinalPath(file, copyTo);

            if (File.Exists(finalPath))
            {
                if (IsOverwritingAllowed)
                {
                    File.Copy(file, finalPath, true);

                    WriteLine(file + " --overwrited");
                }
                else
                    WriteLine(file + " --skiped");
            }
            else
            {
                File.Copy(file, finalPath);

                WriteLine(file);
            }
        }
    }

    private static void CopySubdirectories(string directory, string copyTo, Ignore ignore)
    {
        foreach (var subdirectory in Directory.EnumerateDirectories(directory))
        {
            if (ignore.IsIgnored(subdirectory))
            {
                WriteLine(subdirectory + " --ignored");

                continue;
            }

            string finalPath = GetFinalPath(subdirectory, copyTo);

            Directory.CreateDirectory(finalPath);

            WriteLine(subdirectory);

            DeepCopy(subdirectory, finalPath, ignore);
        }
    }

    private static void WriteLine(string msg)
    {
        if (_enableConsoleOutput)
            Console.WriteLine(msg);
    }

    private static string GetFinalPath(string currentPath, string destinationFolder)
    {
        currentPath = Path.GetFileName(currentPath);

        return Path.Combine(destinationFolder, currentPath);
    }
}