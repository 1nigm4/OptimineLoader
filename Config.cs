using System;

namespace OptimineLoader
{
    public class Config
    {
        private const string _PROJECTNAME = "Optimine";
        public static string LauncherName = $"{_PROJECTNAME}.jar";
        public static string JavaName = "jre-win";
        private const string _SystemDir = "updates";
        public const string WebDir = "https://launch.optimine.su/";

        private static readonly string _appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static readonly string ProjectDir = $"{_appData}\\{_PROJECTNAME}";
        public static readonly string SystemPath = $"{ProjectDir}\\{_SystemDir}";
        public static readonly string LauncherPath = $"{SystemPath}\\{LauncherName}";
        public static string JavaPath = $"{SystemPath}\\{JavaName}";
    }
}