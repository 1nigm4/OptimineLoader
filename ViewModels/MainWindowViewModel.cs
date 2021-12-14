using OptimineLoader.Models;
using OptimineLoader.ViewModels.Base;
using System.Collections.Generic;
using System.IO;

namespace OptimineLoader.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        public ProgressBar ProgressBar { get; }
        Configuration Configuration { get; }
        VersionChecker VersionChecker { get; }
        List<Module> MissingModules { get; }

        public MainWindowViewModel()
        {
            Configuration = new Configuration();
            ProgressBar = new ProgressBar() { Details = "Загрузка..." };
            VersionChecker = new VersionChecker();
            MissingModules = new List<Module>();

            bool isHashesDownloaded = VersionChecker.DownloadHashes();
            if (isHashesDownloaded)
                CheckModulesExist();
            else
                ProgressBar.Details = "Ошибка соединения с сервером!";
        }

        public void CheckModulesExist()
        {
            if (!Directory.Exists(Configuration.UpdatesFolderPath))
            {
                Directory.CreateDirectory(Config.ProjectDir);
                MissingModules.Add(Module.Java);
                MissingModules.Add(Module.Launcher);
            }

            if (MissingModules.Count == 0)
                Launcher.Start();
            else
            {
                DownloadModule(MissingModules[0]);
            }
        }
    }
}