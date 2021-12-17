using OptimineLoader.Models;
using System;
using System.Diagnostics;

namespace OptimineLoader.Services
{
    class OptimineLauncher
    {
        public static void Start()
        {
            string javaPath = $@"{Configuration.UpdatesFolderPath}\{Configuration.JavaVersion}\bin\javaw.exe";
            string launcherPath = Configuration.UpdatesFolderPath + "\\" + "Optimine.jar";
            Process.Start(javaPath, "-jar " + launcherPath);
            Environment.Exit(0);
        }
    }
}
