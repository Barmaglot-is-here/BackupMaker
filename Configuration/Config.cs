namespace BackupMaker.Configuration
{
    internal struct Config
    {
        private const string CONFIG_NAME = "config.bin";

        public bool EnableOutput { get; }
        public bool EnableLogging { get; }
        public bool CreateTopFolderToo { get; }

        public Config(bool enableOutput, bool enableLogging, bool createTopFolderToo)
        {
            EnableOutput = enableOutput;
            EnableLogging = enableLogging;
            CreateTopFolderToo = createTopFolderToo;
        }

        public static Config? Load(string directory)
        {
            string configPath = directory + CONFIG_NAME;

            if (!File.Exists(configPath))
                return null;

            using BinaryReader reader = new(File.Open(configPath, FileMode.Open));

            bool enableOutput       = reader.ReadBoolean();
            bool enableLogging      = reader.ReadBoolean();
            bool createTopFolderToo = reader.ReadBoolean();

            return new Config(enableOutput, enableLogging, createTopFolderToo);
        }

        public void Save(string directory)
        {
            string configPath = directory + CONFIG_NAME;

            using BinaryWriter writer = new(File.Open(configPath, FileMode.CreateNew));

            writer.Write(EnableOutput);
            writer.Write(EnableLogging);
            writer.Write(CreateTopFolderToo);
        }
    }
}