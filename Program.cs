using System.IO;
using BackupMaker.Configuration;
using BackupMaker.Utils;

namespace BackupMaker
{
    //Поковырять Ignore
    //И сам Backup
    //и с логированием что-нибудь сделать
    internal class Program
    {
        static void Main(string[] args)
        {
            EncodingSetter.Set(1251);

            bool loop = true;

            while (loop)
            {
                string startFolder          = Command.GetPath("Enter start folder:", false);
                string destinationFolder    = Command.GetPath("Enter destination folder:", true);
                string curentDirectory      = Directory.GetCurrentDirectory();

                Config config = GetConfig(curentDirectory);

                int option = Command.Options("Begin", "Configuration", "Change Paths", 
                                            "Exit");

                switch (option)
                {
                    case 1:
                        Backup.Make(config, startFolder, destinationFolder);

                        if (!Command.Confirm("Backup something else?"))
                            loop = false;

                        break;
                    case 2:
                        Console.WriteLine("[Configuration]");
                        config.Show();

                        int option2 = Command.Options("Change", "Set Defaults", "Back");

                        if (option2 == 1)
                            config = ConfigurationUtility.ConfigureAndSave(curentDirectory);
                        else if (option2 == 2)
                            config = ConfigurationUtility.ResetAndSave(curentDirectory);
                        else
                            continue;

                        break;
                    case 3:
                        break;
                    case 4:
                        loop = false;

                        break;
                }
            }
        }

        private static Config GetConfig(string directory)
        {
            Config? configOrNull = Config.Load(directory);

            if (configOrNull == null)
            {
                Console.WriteLine("Configuration doesn't exist");

                int option = Command.Options("Configure", "Set Defaults");

                return option == 1  ? ConfigurationUtility.ConfigureAndSave(directory) 
                                    : ConfigurationUtility.ResetAndSave(directory);
            }
            else
                return (Config)configOrNull;
        }

    }
}