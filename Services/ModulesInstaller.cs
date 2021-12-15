using OptimineLoader.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading.Tasks;

namespace OptimineLoader.Services
{
    internal class ModulesInstaller
    {
        List<Module> MissingModules;
        ProgressBar PBar;

        Module CurrentModule;
        public ModulesInstaller(ProgressBar progressBar, List<Module> modules)
        {
            PBar = progressBar;
            MissingModules = modules;
        }

        public async void DownloadModules()
        {
            foreach (var missingModule in MissingModules)
            {
                CurrentModule = missingModule;
                PBar.Details = "Проверка и загрузка " + CurrentModule;

                Uri moduleUri = CurrentModule switch
                {
                    Module.Java => new Uri(Configuration.WebDir + Configuration.JavaVersion + ".zip"),
                    Module.Launcher => new Uri(Configuration.WebDir + "Optimine.jar")
                };
                string downloadPath = CurrentModule switch
                {
                    Module.Java => Configuration.UpdatesFolderPath + "\\" + Configuration.JavaVersion + ".zip",
                    Module.Launcher => Configuration.UpdatesFolderPath + "\\" + "Optimine.jar"
                };

                using (WebClient web = new WebClient())
                {
                    web.DownloadFileCompleted += DownloadedFile;
                    web.DownloadProgressChanged += ProgressBarDownloading;
                    await Task.Run(() => web.DownloadFile(moduleUri, downloadPath));
                }
            }
        }

        private void ProgressBarDownloading(object sender, DownloadProgressChangedEventArgs e)
        {
            int percent = e.ProgressPercentage;

            if (CurrentModule == Module.Java)
                PBar.JavaInstallingValue = percent * (MissingModules.Count == 1 ? 1 : 0.85);
            else
                PBar.LauncherDownloadingValue = percent * (PBar.JavaInstallingValue == 0 ? 1 : 0.15);
        }

        private async void DownloadedFile(object sender, AsyncCompletedEventArgs e)
        {
            if (CurrentModule == Module.Java)
                await Task.Run(() => InstallJava());
        }
        private void InstallJava()
        {
            var javaPack = Configuration.UpdatesFolderPath + "\\" + Configuration.JavaVersion + ".zip";
            ZipFile.ExtractToDirectory(javaPack, Configuration.UpdatesFolderPath);
            File.Delete(javaPack);
        }
    }
}
