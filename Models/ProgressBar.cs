using OptimineLoader.ViewModels.Base;

namespace OptimineLoader.Models
{
    class ProgressBar : ViewModel
    {
        private double javaInstallingValue;

        public double JavaInstallingValue
        {
            get { return javaInstallingValue; }
            set
            {
                javaInstallingValue = value;
                CurrentValue = (int)(value + LauncherDownloadingValue);
            }
        }

        private double launcherDownloadingValue;

        public double LauncherDownloadingValue
        {
            get { return launcherDownloadingValue; }
            set
            {
                launcherDownloadingValue = value;
                CurrentValue = (int)(value + JavaInstallingValue);
            }
        }


        private int currentValue;
        public int CurrentValue
        {
            get => currentValue;
            set => Set(ref currentValue, value);
        }

        private string details;
        public string Details
        {
            get => details;
            set => Set(ref details, value);
        }
    }
}
