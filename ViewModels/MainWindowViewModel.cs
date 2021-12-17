using OptimineLoader.Models;
using OptimineLoader.Services;
using OptimineLoader.ViewModels.Base;
using System.Collections.Generic;

namespace OptimineLoader.ViewModels
{
    class MainWindowViewModel : ViewModel
    {
        public ProgressBar ProgressBar { get; }
        public List<Module> MissingModules { get; }
        public ModulesInstaller Installer { get; }

        public MainWindowViewModel()
        {
            ProgressBar = new ProgressBar() { Details = "Загрузка..." };
            MissingModules = ModulesChecker.CheckModulesExist();
            Installer = new ModulesInstaller(ProgressBar, MissingModules);
        }
    }
}
