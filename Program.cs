using BackupMaker.Configuration;
using BackupMaker.Utils;

namespace BackupMaker;

internal class Program
{
    private static readonly string _curentDirectory;

    static Program()
    {
        _curentDirectory = Directory.GetCurrentDirectory();

        EncodingSetter.Set(EncodingSetter.RU_ENCODING);
    }

    static void Main()
    {
        bool loop = true;

        while (loop)
        {
            string startFolder          = Command.GetPath("Enter start folder:", false);
            string destinationFolder    = Command.GetPath("Enter destination folder:", true);

            Config config = ConfigurationUtility.GetConfig(_curentDirectory);

        OptionsSelect:
            int option = Command.Options("Begin", "Configuration", "Back", "Exit");

            switch (option)
            {
                case 1:
                    MakeBackup(ref config, startFolder, destinationFolder);

                    loop = Command.Confirm("Backup something else?");

                    break;
                case 2:
                    Configure(ref config);

                    goto OptionsSelect;
                case 3:
                    break;
                case 4:
                    loop = false;

                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        SaveLog(_curentDirectory);
    }

    private static void MakeBackup(ref Config config, string startFolder, 
                                                      string destinationFolder)
    {
        try
        {
            Backup.Make(config, startFolder, destinationFolder);
        }
        catch (Exception ex)
        {
            string message = "\n" +
                             "---ERROR--- \n" +
                             ex.ToString() +
                             "'\n----------- \n";

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;

            if (config.EnableLoging)
            {
                Loger.Add(message);

                Loger.Save(_curentDirectory);
            }

            Console.ReadKey();

            Environment.Exit(0);
        }
    }

    private static void Configure(ref Config config)
    {
        config.Show();

        int option = Command.Options("Change", "Set Defaults", "Back");

        if (option == 1)
            config = ConfigurationUtility.ConfigureAndSave(_curentDirectory);
        else if (option == 2)
            config = ConfigurationUtility.SetDefaultsAndSave(_curentDirectory);
    }

    private static void SaveLog(string path)
    {
        if (!Loger.IsEmpty)
            Loger.Save(path);
    }
}