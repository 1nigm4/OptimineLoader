using System;

namespace OptimineLoader.Models
{
    internal class Configuration
    {
        public static int OsBit { get; }
        public static string AppDataFolderPath { get; }
        public static string UpdatesFolderPath { get; }
        public static string JavaFolderPath { get; }
        public static string JavaVersion { get; }
        public static string WebDir { get; }
        static Configuration()
        {
            OsBit = Environment.Is64BitOperatingSystem ? 64 : 86;
            AppDataFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            UpdatesFolderPath = $@"{AppDataFolderPath}\Optimine\updates";
            JavaVersion = "jre-win" + OsBit;
            JavaFolderPath = $@"{UpdatesFolderPath}\{JavaVersion}";
            WebDir = "https://launch.optimine.su/";
        }
    }
}
