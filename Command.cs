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
                        {
                            Console.WriteLine();

                            continue;
                        }
                    }
                    else
                    {
                        Console.WriteLine("directory doesn't exists\n");

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

        public static int Options(params string[] options)
        {
            if (options.Length > 9)
                throw new ArgumentException("Too mutch options");

            while (true)
            {
                Console.WriteLine();

                for (int i = 0; i < options.Length; i++)
                    Console.WriteLine($"{i + 1}) {options[i]}");

                Console.WriteLine();

                var keyInfo = Console.ReadKey(true);
                int keyIndex = KeyToInt(keyInfo.Key);                

                if (keyIndex == -1 || keyIndex > options.Length)
                    Console.WriteLine("Wrong key");
                else
                    return keyIndex;
            }
        }

        private static int KeyToInt(ConsoleKey consoleKey)
        {
            int keyIndex = (int)consoleKey;

            if (keyIndex < 48 || keyIndex > 57)
                return -1;

            return keyIndex - 48;
        }
    }
}