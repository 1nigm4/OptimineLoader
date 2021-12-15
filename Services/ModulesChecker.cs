using OptimineLoader.Models;
using System.Collections.Generic;
using System.IO;

namespace OptimineLoader.Services
{
    internal class ModulesChecker
    {
        public static List<Module> CheckModulesExist()
        {
            List<Module> MissingModules = new List<Module>();

            Directory.CreateDirectory(Configuration.UpdatesFolderPath);
            if (!Directory.Exists(Configuration.JavaFolderPath))
                MissingModules.Add(Module.Java);

            string launcherPath = Configuration.UpdatesFolderPath + "\\" + "Optimine.jar";
            if (!File.Exists(launcherPath))
                MissingModules.Add(Module.Launcher);

            return MissingModules;
        }
    }
}
