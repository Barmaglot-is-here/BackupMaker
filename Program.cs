using BackupMaker.Configuration;
using BackupMaker.Utils;

namespace BackupMaker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            EncodingSetter.Set(1251);

            while (true)
            {
                string startFolder          = Command.GetPath("Enter start folder:", false);
                string destinationFolder    = Command.GetPath("Enter destination folder:", true);
                
                Config config               = ConfigurationUtility.GetConfig();

                if (!Command.Confirm("Configuration is complete. Begin?"))
                    continue;

                Backup.Make(config, startFolder, destinationFolder);

                if (!Command.Confirm("Done. Backup something else?"))
                    break;
            }
        }
    }
}