using OptimineLoader.Services;
using OptimineLoader.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;

namespace OptimineLoader.Views
{
    public partial class MainWindow : Window
    {
        private Point windowOffset;
        private Point mouseOffset;

        private void Window_onLoaded(object sender, RoutedEventArgs e)
        {
            MainWindowViewModel viewModel = (MainWindowViewModel)DataContext;
            if (viewModel.MissingModules.Count != 0)
            {
                InitializeComponent();
                viewModel.Installer.DownloadModulesAsync();
            }
            else
                OptimineLauncher.Start();
        }

        private void Window_onMouseDown(object sender, MouseButtonEventArgs e)
        {
            windowOffset = new Point(Left, Top);
            mouseOffset = e.GetPosition(this);
        }

        private void Window_onMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point mousePosition = e.GetPosition(this);
                windowOffset += mousePosition - mouseOffset;
                this.Left = windowOffset.X;
                this.Top = windowOffset.Y;
            }
        }

        private void Exit_onClick(object sender, RoutedEventArgs e) => Environment.Exit(0);

        private void Rollup_onClick(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
    }
}
