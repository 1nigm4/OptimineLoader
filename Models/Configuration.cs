using System;

namespace OptimineLoader.Models
{
    internal class Configuration
    {
        public int OsBit { get; }
        public string AppDataFolderPath { get; }
        public string UpdatesFolderPath { get; }
        public Configuration()
        {
            OsBit = Environment.Is64BitOperatingSystem ? 64 : 86;
            AppDataFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            UpdatesFolderPath = $@"{AppDataFolderPath}\Optimine\updates";
        }
    }
}
