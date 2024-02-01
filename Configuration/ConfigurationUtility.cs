namespace BackupMaker.Configuration
{
    internal static class ConfigurationUtility
    {
        public static Config GetConfig()
        {
            Config config;
            string configDirectory = Directory.GetCurrentDirectory();

            Config? configOrNull = Config.Load(configDirectory);

            if (configOrNull != null)
                config = (Config)configOrNull;
            else
            {
                config = Congfigure();

                config.Save(configDirectory);
            }

            return config;
        }

        private static Config Congfigure()
        {
            bool enableOutput       = Command.Confirm("Enable console output?");
            bool enableLogging      = Command.Confirm("Enable logging?");
            bool createTopFolderToo = Command.Confirm("Create top level folder?");

            return new Config(enableOutput, enableLogging, createTopFolderToo);
        }
    }
}