using OptimineLoader.Models;
using OptimineLoader.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace OptimineLoader.Services
{
    class ModulesInstaller
    {
        ObservableCollection<Module> MissingModules;
        ProgressBar PBar;
        Module CurrentModule;

        public ModulesInstaller(ProgressBar progressBar, List<Module> modules)
        {
            PBar = progressBar;
            MissingModules = new ObservableCollection<Module>(modules);
            MissingModules.CollectionChanged += MissingModules_onClear;
        }

        private void MissingModules_onClear(object sender, NotifyCollectionChangedEventArgs e)
        {
            PBar.Details = "Запускаем лаунчер";
            OptimineLauncher.Start();
        }

        public async void DownloadModulesAsync()
        {
            foreach (var missingModule in MissingModules)
            {
                CurrentModule = missingModule;
                PBar.Details = "Загружаем " + CurrentModule;

                Uri moduleUri = CurrentModule switch
                {
                    Module.Launcher => new Uri(Configuration.WebDir + "Optimine.jar"),
                    Module.Java => new Uri(Configuration.WebDir + Configuration.JavaVersion + ".zip")
                };
                string downloadPath = CurrentModule switch
                {
                    Module.Launcher => Configuration.UpdatesFolderPath + "\\" + "Optimine.jar",
                    Module.Java => Configuration.UpdatesFolderPath + "\\" + Configuration.JavaVersion + ".zip"
                };

                using (WebClient web = new WebClient())
                {
                    web.DownloadProgressChanged += Module_onDownloading;
                    await web.DownloadFileTaskAsync(moduleUri, downloadPath);
                    await Task.Run(Module_onDownloaded);
                }
            }
            MissingModules.Clear();
        }

        private void Module_onDownloading(object sender, DownloadProgressChangedEventArgs e)
        {
            int percent = e.ProgressPercentage;

            if (CurrentModule == Module.Launcher)
                PBar.LauncherDownloadingValue = percent * (MissingModules.Count == 1 ? 1 : 0.15);
            else
                PBar.JavaInstallingValue = percent * (MissingModules.Count == 1 ? 1 : 0.85);
        }

        private void Module_onDownloaded()
        {
            if (CurrentModule == Module.Java)
                InstallJava();
        }

        private void InstallJava()
        {
            PBar.Details = "Развёртывание Java";
            var javaPack = Configuration.UpdatesFolderPath + "\\" + Configuration.JavaVersion + ".zip";
            ZipFile.ExtractToDirectory(javaPack, Configuration.UpdatesFolderPath);
            File.Delete(javaPack);
        }
    }
}
