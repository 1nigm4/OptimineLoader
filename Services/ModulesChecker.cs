using OptimineLoader.Models;
using System.Collections.Generic;
using System.IO;

namespace OptimineLoader.Services
{
    class ModulesChecker
    {
        public static List<Module> CheckModulesExist()
        {
            List<Module> MissingModules = new List<Module>();

            Directory.CreateDirectory(Configuration.UpdatesFolderPath);

            if (!File.Exists(Configuration.LauncherPath))
                MissingModules.Add(Module.Launcher);
            if (!Directory.Exists(Configuration.JavaFolderPath))
                MissingModules.Add(Module.Java);

            return MissingModules;
        }
    }
}