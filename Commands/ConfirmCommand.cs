namespace BackupMaker;

internal partial class Command
{
    private const ConsoleKey CONFIRM_KEY = ConsoleKey.Y;
    private const ConsoleKey DECLINE_KEY = ConsoleKey.N;

    public static bool Confirm(string msg)
    {
        while (true)
        {
            Console.WriteLine($"{msg} [{CONFIRM_KEY}/{DECLINE_KEY}]");

            var succes = Console.IsInputRedirected 
                ? TryConfirmFromFile(out bool result) 
                : TryConfirmFromConsole(out result);

            if (!succes)
                Console.WriteLine("Wrong key");
            else
                return result;
        }
    }

    private static bool TryConfirmFromConsole(out bool result)
    {
        var keyInfo = Console.ReadKey(true);

        if (keyInfo.Key == CONFIRM_KEY)
            result = true;
        else if (keyInfo.Key == DECLINE_KEY)
            result = false;
        else
        {
            result = false;

            return false;
        }

        return true;
    }
    
    private static bool TryConfirmFromFile(out bool result)
    {
        var input   = Console.ReadLine();

        if (input == "n" || input == "N")
            result = false;
        else if (input == "y" || input == "Y")
            result = true;
        else
        {
            result = false;

            return false;
        }

        return true;
    }
}