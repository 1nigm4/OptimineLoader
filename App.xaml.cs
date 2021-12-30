using OptimineLoader.ViewModels;
using OptimineLoader.Views;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Media;

namespace OptimineLoader
{
    public partial class App : Application
    {
        public App() : base()
        {
            Dispatcher.UnhandledException += (s, e) =>
            {
                var exception = e.Exception;
                while (exception.InnerException != null)
                    exception = exception.InnerException;
                MainWindow window = (MainWindow)MainWindow;
                window.Details.Foreground = Brushes.Red;
                (window.DataContext as MainWindowViewModel).ProgressBar.Details = "Ошибка соединения с сервером";
                window.DownloadBar.IsIndeterminate = false;
                e.Handled = true;
            };
        }
    }
}
