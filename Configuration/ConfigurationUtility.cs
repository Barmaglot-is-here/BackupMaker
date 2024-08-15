namespace BackupMaker.Configuration;

internal static class ConfigurationUtility
{
    public static Config Congfigure()
    {
        Config config               = new();
        config.EnableOutput         = Command.Confirm("Enable console output?");
        config.CreateTopFolderToo   = Command.Confirm("Create top level folder?");

        return config;
    }

    public static Config ConfigureAndSave(string saveDirrectory)
    {
        Config config = Congfigure();

        config.Save(saveDirrectory);

        return config;
    }

    public static Config SetDefaultsAndSave(string saveDirrectory)
    {
        var config = Config.Default;

        config.Save(saveDirrectory);

        return config;
    }

    public static Config GetConfig(string directory)
    {
        if (Config.TryLoad(directory, out Config config))
            return config;
        else
        {
            Console.WriteLine("Configuration doesn't exist");

            int option = Command.Options("Configure", "Set Defaults");

            return option == 1  ? ConfigureAndSave(directory)
                                : SetDefaultsAndSave(directory);
        }
    }
}