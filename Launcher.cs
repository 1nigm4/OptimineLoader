using System;
using System.Diagnostics;

namespace OptimineLoader
{
    class Launcher
    {
        public static void Start()
        {
            Process.Start(Config.JavaPath + "\\bin\\javaw.exe", "-jar " + Config.LauncherPath);
            Environment.Exit(0);
        }
    }
}
