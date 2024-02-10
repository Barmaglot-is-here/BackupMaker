using BackupMaker.Configuration;
using BackupMaker.Utils;

namespace BackupMaker
{
    //Добавить возможность изменить конфигурацию
    //И откатиться на шаг незад
    //А потом поковырять Ignore
    //И сам Backup
    //и с логированием что-нибудь сделать
    internal class Program
    {
        static void Main(string[] args)
        {
            EncodingSetter.Set(1251);

            while (true)
            {
                string startFolder          = Command.GetPath("Enter start folder:", false);
                string destinationFolder    = Command.GetPath("Enter destination folder:", true);
                string curentDirectory      = Directory.GetCurrentDirectory();

                Config config = GetConfig(curentDirectory);

                if (!Command.Confirm("Done. Begin?"))
                    break;

                Backup.Make(config, startFolder, destinationFolder);

                if (!Command.Confirm("Done. Backup something else?"))
                    break;
            }
        }

        private static Config GetConfig(string directory)
        {
            Config? configOrNull = Config.Load(directory);
            Config config;

            if (configOrNull != null)
            {
                config = (Config)configOrNull;

                Console.WriteLine("\n[Configuration]");
                ShowConfigContent(config);
                Console.WriteLine();
            }
            else
            {
                config = ConfigurationUtility.Congfigure();

                config.Save(directory);
            }

            return config;
        }

        private static void ShowConfigContent(Config config)
        {
            Console.WriteLine($"Enable console output:      {config.EnableOutput}");
            Console.WriteLine($"Enable logging:             {config.EnableLogging}");
            Console.WriteLine($"Create top level folder:    {config.CreateTopFolderToo}");
        }
    }
}