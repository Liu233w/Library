using Abp.Dependency;

namespace Library.Configuration
{
    public class AppFoldersConfiguration : ISingletonDependency
    {
        public string AppDataFolder { get; set; }

        public string UploadFolder { get; set; }

        public string Attachments { get; set; }

        public string WebLogsFolder { get; set; }
    }
}