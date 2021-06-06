using NoiseCast.Core;
using NoiseCast.MVVM.Model;

namespace NoiseCast.MVVM.ViewModel
{
    public class SettingsViewModel : ObservableObject
    {
        private ApplicationSettingsModel _appSettings;

        public ApplicationSettingsModel AppSettings { get => _appSettings; set => SetProperty(ref _appSettings, value); }
    }
}