using OptimineLoader.ViewModels.Base;

namespace OptimineLoader.Models
{
    internal class ProgressBar : ViewModel
    {
        private double currentValue;
        public double CurrentValue
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
