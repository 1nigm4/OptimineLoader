using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace OptimineLoader
{
    public partial class MainWindow : Window
    {
        public static bool InitializedWindow { get; set; }
        public MainWindow()
        {
            Loader loader = new Loader
            {
                Window = this
            };

            var osBin = Environment.Is64BitOperatingSystem ? "64" : "32";
            Config.JavaName = $"{Config.JavaName}{osBin}.zip";
            Config.JavaPath += osBin;

            Hashes.DownloadHashes();
            
            if (Loader.Connection)
                loader.ComponentsExists();
            else
            {
                InitializeComponent();
                CurrentOperation.Foreground = Brushes.Red;
                CurrentOperation.Text = "Нет соединения с сервером";
            }
        }

        private Point _mouseOffset;

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
