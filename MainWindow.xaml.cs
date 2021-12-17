using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace OptimineLoader
{
    public partial class MainWindow : Window
    {
        private Point windowOffset;
        private Point mouseOffset;
        public static bool InitializedWindow { get; set; }
        public MainWindow()
        {
            Loader loader = new Loader
            {
                Window = this
            };

            var osBin = Environment.Is64BitOperatingSystem ? "64" : "86";
            Config.JavaName = $"{Config.JavaName}{osBin}.zip";
            Config.JavaPath += osBin;

            try
            {
                loader.ComponentsExists();
            }
            catch
            {
                InitializeComponent();
                CurrentOperation.Foreground = Brushes.Red;
                CurrentOperation.Text = "Ошибка соединения с сервером!";
            }
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
                windowOffset += (mousePosition - mouseOffset);
                this.Left = windowOffset.X;
                this.Top = windowOffset.Y;
            }
        }

        private void Exit_onClick(object sender, RoutedEventArgs e) => Environment.Exit(0);

        private void Rollup_onClick(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
    }
}
