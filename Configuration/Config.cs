namespace BackupMaker.Configuration;

internal struct Config
{
    private const string CONFIG_NAME = "config.bin";

    public bool EnableOutput { get; set; }
    public bool CreateTopFolderToo { get; set; }

    public static Config Default
    {
        get
        {
            Config config               = new();
            config.EnableOutput         = true;
            config.CreateTopFolderToo   = true;

            return config;
        }
    }

    public static bool TryLoad(string directory, out Config config)
    {
        string configPath   = directory + CONFIG_NAME;
        bool exists         = File.Exists(configPath);

        config = exists ? Load(configPath) : default;

        return exists;
    }

    public static Config Load(string path)
    {
        using BinaryReader reader = new(File.Open(path, FileMode.Open));

        Config config               = new();
        config.EnableOutput         = reader.ReadBoolean();
        config.CreateTopFolderToo   = reader.ReadBoolean();

        return config;
    }

    public void Save(string directory)
    {
        string configPath = directory + CONFIG_NAME;

        using BinaryWriter writer = new(File.Open(configPath, FileMode.OpenOrCreate));

        writer.Write(EnableOutput);
        writer.Write(CreateTopFolderToo);
    }

    public void Show()
    {
        Console.WriteLine("[Configuration]");
        Console.WriteLine($"Enable console output:      {EnableOutput}");
        Console.WriteLine($"Create top level folder:    {CreateTopFolderToo}");
    }
}