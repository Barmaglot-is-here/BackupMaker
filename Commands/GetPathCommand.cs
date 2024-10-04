namespace BackupMaker;

internal partial class Command
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
}