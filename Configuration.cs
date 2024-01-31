namespace BackupMaker
{
    internal struct Configuration
    {
        public string StartFolder { get; }
        public string DestinationFolder { get; }
        public bool CreateTopFolderToo { get; }

        public Configuration(string startFolder, string destinationFolder, bool createTopFolderToo)
        {
            StartFolder = startFolder; 
            DestinationFolder = destinationFolder; 
            CreateTopFolderToo = createTopFolderToo;
        }
    }
}