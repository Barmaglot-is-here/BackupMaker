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

            var keyInfo = Console.ReadKey(true);

            if (keyInfo.Key == CONFIRM_KEY)
                return true;
            else if (keyInfo.Key == DECLINE_KEY)
                return false;

            Console.WriteLine("Wrong key");
        }
    }
}