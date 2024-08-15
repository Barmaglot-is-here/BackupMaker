namespace BackupMaker;

internal class Command
{
    private const ConsoleKey CONFIRM_KEY = ConsoleKey.Y;
    private const ConsoleKey DECLINE_KEY = ConsoleKey.N;

    private const int FIRST_NUMBER_KEY_INDEX = 48;
    private const int LAST_NUMBER_KEY_INDEX = 57;

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
            Console.WriteLine($"{msg} [{CONFIRM_KEY}/{DECLINE_KEY}]");

            var keyInfo = Console.ReadKey(true);

            if (keyInfo.Key == CONFIRM_KEY)
                return true;
            else if (keyInfo.Key == DECLINE_KEY)
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

        if (keyIndex < FIRST_NUMBER_KEY_INDEX || keyIndex > LAST_NUMBER_KEY_INDEX)
            return -1;

        return keyIndex - FIRST_NUMBER_KEY_INDEX;
    }
}