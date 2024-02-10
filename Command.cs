namespace BackupMaker
{
    internal class Command
    {
        public static string GetPath(string msg, bool canCreateIfDoesntExists)
        {
            while (true)
            {
                Console.WriteLine(msg);

                string path = Console.ReadLine() ?? string.Empty;

                if (!Directory.Exists(path))
                    if (canCreateIfDoesntExists)
                    {
                        if (Confirm("directory doesn't exists. Create?"))
                            Directory.CreateDirectory(path);
                        else
                            continue;
                    }
                    else
                    {
                        Console.WriteLine("directory doesn't exists");

                        continue;
                    }

                Console.WriteLine("Ok");

                return path;
            }
        }

        public static bool Confirm(string msg)
        {
            while (true)
            {
                Console.WriteLine($"{msg} [Y/N]");

                var keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.Y)
                    return true;
                else if (keyInfo.Key == ConsoleKey.N)
                    return false;

                Console.WriteLine("Wrong key");
            }
        }
    }
}