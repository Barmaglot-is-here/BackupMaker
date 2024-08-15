using BackupMaker.Configuration;
using BackupMaker.Utils;

namespace BackupMaker;

internal class Program
{
    private const int RU_ENCODING = 1251;

    static void Main()
    {
        EncodingSetter.Set(RU_ENCODING);

        bool loop = true;

        while (loop)
        {
            string startFolder          = Command.GetPath("Enter start folder:", false);
            string destinationFolder    = Command.GetPath("Enter destination folder:", true);
            string curentDirectory      = Directory.GetCurrentDirectory();

            Config config = ConfigurationUtility.GetConfig(curentDirectory);

            OptionsSelect:
            int option = Command.Options("Begin", "Configuration", "Back", 
                                        "Exit");

            switch (option)
            {
                case 1:
                    Backup.Make(config, startFolder, destinationFolder);

                    loop = Command.Confirm("Backup something else?");

                    break;
                case 2:
                    config.Show();

                    option = Command.Options("Change", "Set Defaults", "Back");

                    if (option == 1)
                        config = ConfigurationUtility.ConfigureAndSave(curentDirectory);
                    else if (option == 2)
                        config = ConfigurationUtility.SetDefaultsAndSave(curentDirectory);

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
    }
}