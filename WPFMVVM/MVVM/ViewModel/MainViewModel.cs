using WPFMVVM.Core;

namespace WPFMVVM.MVVM.ViewModel
{
    internal class MainViewModel : ObservableObject
    {
        public RelayCommand YourPodcastsViewCommand { get; set; }
        public RelayCommand NewEpisodesViewCommand { get; set; }
        public RelayCommand DiscoveryViewCommand { get; set; }
        public RelayCommand InProgressViewCommand { get; set; }
        public RelayCommand DownloadsViewCommand { get; set; }
        public RelayCommand SettingsViewCommand { get; set; }
        public RelayCommand PlayerViewCommand { get; set; }

        public YourPodcastsViewModel YourPodcastsVM { get; set; }
        public NewEpisodesViewModel NewEpisodesVM { get; set; }
        public DiscoverViewModel DiscoveryVM { get; set; }
        public InProgressViewModel InProgressVM { get; set; }
        public DownloadsViewModel DownloadsVM { get; set; }
        public SettingsViewModel SettingsVM { get; set; }
        public PlayerViewModel PlayerVM { get; set; }

        private object currentView;
        public object CurrentView
        {
            get { return currentView; }
            set
            {
                currentView = value;
                OnPropertyChanged();
            }
        }
        private object playerView;
        public object PlayerView
        {
            get { return playerView; }
            set
            {
                playerView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            YourPodcastsVM = new YourPodcastsViewModel();
            NewEpisodesVM = new NewEpisodesViewModel();
            DiscoveryVM = new DiscoverViewModel();
            InProgressVM = new InProgressViewModel();
            DownloadsVM = new DownloadsViewModel();
            SettingsVM = new SettingsViewModel();
            PlayerVM = new PlayerViewModel();

            CurrentView = YourPodcastsVM;
            PlayerView = PlayerVM;

            YourPodcastsViewCommand = new RelayCommand(o => CurrentView = YourPodcastsVM);
            NewEpisodesViewCommand = new RelayCommand(o => CurrentView = NewEpisodesVM);
            DiscoveryViewCommand = new RelayCommand(o => CurrentView = DiscoveryVM);
            InProgressViewCommand = new RelayCommand(o => CurrentView = InProgressVM);
            DownloadsViewCommand = new RelayCommand(o => CurrentView = DownloadsVM);
            SettingsViewCommand = new RelayCommand(o => CurrentView = SettingsVM);
            PlayerViewCommand = new RelayCommand(o => PlayerView = PlayerVM);
        }
    }
}