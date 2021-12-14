using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading.Tasks;

namespace OptimineLoader
{
    class Loader
    {
        public MainWindow Window { get; set; }
        public static bool Connection { get; set; }
        private readonly List<string> _missingComponents = new List<string>();
        private string _downloadingComponent;
        private bool _javaInstalling;
        private double _javaDownload;
        private double _launcherDownload;

        public void ComponentsExists()
        {
            if (!Directory.Exists(Config.ProjectDir))
            {
                Directory.CreateDirectory(Config.ProjectDir);
                Directory.CreateDirectory(Config.SystemPath);
                _missingComponents.Add("launcher");
                _missingComponents.Add("java");
            }
                
            if (_missingComponents.Count == 0)
                Launcher.Start();
            else
            {
                Window.InitializeComponent();
                DownloadComponents();
            }
        }

        private void DownloadComponents()
        {
            _downloadingComponent = _missingComponents[0];
            Window.CurrentOperation.Text = _downloadingComponent == "java" ? "Проверка и загрузка Java" : "Проверка и загрузка лаунчера";
            Window.ProgressBarDownloading.IsIndeterminate = true;

            Uri componentUri = new Uri(_downloadingComponent == "java" ? Config.WebDir + Config.JavaName : Config.WebDir + Config.LauncherName);
            string componentPath = _downloadingComponent == "java" ? $"{Config.JavaPath}.zip" : Config.LauncherPath;
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

            Window.ProgressBarFull.Value = (int)(_javaDownload + _launcherDownload);
            Window.ProgressBarValue.Content = $"{Window.ProgressBarFull.Value:0}%";
        }

        private void DownloadedFile(object sender, AsyncCompletedEventArgs e)
        {
            _missingComponents.Remove(_downloadingComponent);

            if (_downloadingComponent == "java")
                InstallJava();

            if (_missingComponents.Count != 0)
                DownloadComponents();
            else if (!_javaInstalling)
                ComponentsExists();
            else
            {
                Window.CurrentOperation.Text = "Запуск лаунчера...";
                Window.ProgressBarDownloading.IsIndeterminate = false;
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
