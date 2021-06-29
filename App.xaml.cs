using System;
using System.Reflection;
using System.Windows;

namespace OptimineLoader
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App() : base()
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
        }
        private bool ProgressBarLinked;
        Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.Contains("XamlRadialProgressBar") && !ProgressBarLinked)
            {
                ProgressBarLinked = true;
                return Assembly.Load(OptimineLoader.Properties.Resources.XamlRadialProgressBar_DotNet);
            }
            return null;
        }
    }
}
