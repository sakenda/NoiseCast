using NoiseCast.Core;
using NoiseCast.MVVM.Model;
using System.Windows.Input;

namespace NoiseCast.MVVM.ViewModel
{
    public class SettingsViewModel : ObservableObject
    {
        private ApplicationSettingsModel _appSettings;

        public ICommand RefreshPodcastImagesCommand { get; private set; }
        public ApplicationSettingsModel AppSettings { get => _appSettings; set => SetProperty(ref _appSettings, value); }

        public SettingsViewModel()
        {
            _appSettings = MainViewModel.ApplicationSettings;
            RefreshPodcastImagesCommand = new RelayCommand(RefreshPodcastImagesExecuted);
        }

        private void RefreshPodcastImagesExecuted(object obj) => FeedSerialization.RefreshImages(MainViewModel.PodcastsList);
    }
}