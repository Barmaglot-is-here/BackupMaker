namespace BackupMaker.Configuration
{
    internal static class ConfigurationUtility
    {
        public static Config Congfigure()
        {
            bool enableOutput       = Command.Confirm("Enable console output?");
            bool enableLogging      = Command.Confirm("Enable logging?");
            bool createTopFolderToo = Command.Confirm("Create top level folder?");

            return new Config(enableOutput, enableLogging, createTopFolderToo);
        }
    }
}