using OptimineLoader.Models;
using System;
using System.Net;

namespace OptimineLoader.Services
{
    internal class ModuleInstaller
    {
        Module CurrentModule { get; }
        ProgressBar ProgressBar { get; set; }
        public ModuleInstaller(Module module)
        {
            CurrentModule = module;
        }

        public void DownloadModule()
        {
            ProgressBar.Details = "Проверка и загрузка" + CurrentModule;

            Uri componentUri = new Uri(CurrentModule == Module.Java ? Config.WebDir + Config.JavaName : Config.WebDir + Config.LauncherName);
            string componentPath = CurrentModule == Module.Java ? $"{Config.JavaPath}.zip" : Config.LauncherPath;
            using (WebClient web = new WebClient())
            {
                web.DownloadFileCompleted += DownloadedFile;
                web.DownloadProgressChanged += ProgressBarDownloading;
                web.DownloadFileAsync(componentUri, componentPath);
            }
        }

        private void ProgressBarDownloading(object sender, DownloadProgressChangedEventArgs e)
        {
            int percent = e.ProgressPercentage;

            if (_downloadingComponent == "java")
                _javaDownload = percent * (_missingComponents.Count == 1 ? 1 : 0.85);
            else
                _launcherDownload = percent * (_javaDownload == 0 ? 1 : 0.15);

            ProgressBar.CurrentValue = (int)(_javaDownload + _launcherDownload);
            Window.ProgressBarValue.Content = $"{Window.ProgressBarFull.Value:0}%";
        }

        private void DownloadedFile(object sender, AsyncCompletedEventArgs e)
        {
            _missingComponents.Remove(_downloadingComponent);

            if (_downloadingComponent == "java")
                InstallJava();

            if (_missingComponents.Count != 0)
                DownloadComponent();
            else if (!_javaInstalling)
                ComponentsExists();
            else
            {
                ProgressBar.Details = "Запуск лаунчера...";
            }
        }
        private async void InstallJava()
        {
            _javaInstalling = true;
            await Task.Run(() =>
            {
                if (Directory.Exists(Config.JavaPath))
                    Directory.Delete(Config.JavaPath, true);

                var javaPack = $"{Config.JavaPath}.zip";
                ZipFile.ExtractToDirectory(javaPack, Config.SystemPath);
                File.Delete(javaPack);

                _javaInstalling = false;
            });
            ComponentsExists();
        }
    }
}
