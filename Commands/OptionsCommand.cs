namespace BackupMaker;

internal partial class Command
{
    private const int FIRST_NUMBER_KEY_INDEX = 48;
    private const int LAST_NUMBER_KEY_INDEX = 57;

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