using OptimineLoader.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;

namespace OptimineLoader.Views
{
    public partial class MainWindow : Window
    {
        private Point _mouseOffset;

        public MainWindow()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();
            DataContext = viewModel;
            if (viewModel.MissingModules.Count != 0)
            {
                InitializeComponent();
                viewModel.Installer.DownloadModulesAsync();
            }
            else
                Launcher.Start();
        }

        private void Window_onMouseDown(object sender, MouseButtonEventArgs e) => _mouseOffset = e.GetPosition(this);

        private void Window_onMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var mouseScreen = PointToScreen(Mouse.GetPosition(this));
                this.Left = mouseScreen.X - _mouseOffset.X;
                this.Top = mouseScreen.Y - _mouseOffset.Y;
            }
        }

        private void Exit_onClick(object sender, RoutedEventArgs e) => Environment.Exit(0);

        private void Rollup_onClick(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
    }
}
