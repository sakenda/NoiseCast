using NoiseCast.Core;
using NoiseCast.MVVM.Model;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace NoiseCast.MVVM.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        private object currentView;
        private object playerView;

        public object CurrentView
        {
            get => currentView;
            set => SetProperty(ref currentView, value);
        }
        public object PlayerView
        {
            get => playerView;
            set => SetProperty(ref playerView, value);
        }

        private static ApplicationSettingsModel _applicationSettings;
        public static ObservableCollection<PodcastModel> _podcastsList;

        public static ApplicationSettingsModel ApplicationSettings
        {
            get => _applicationSettings;
            set => _applicationSettings = value;
        }
        public static ObservableCollection<PodcastModel> PodcastsList
        {
            get => _podcastsList;
            set => _podcastsList = value;
        }
        public RelayCommand YourPodcastsViewCommand { get; set; }
        public RelayCommand NewEpisodesViewCommand { get; set; }
        public RelayCommand DiscoveryViewCommand { get; set; }
        public RelayCommand InProgressViewCommand { get; set; }
        public RelayCommand DownloadsViewCommand { get; set; }
        public RelayCommand SettingsViewCommand { get; set; }
        public RelayCommand PlayerViewCommand { get; set; }
        public RelayCommand ExitCommand { get; set; }

        public static YourPodcastsViewModel YourPodcastsVM { get; set; }
        public static NewEpisodesViewModel NewEpisodesVM { get; set; }
        public static DiscoverViewModel DiscoveryVM { get; set; }
        public static InProgressViewModel InProgressVM { get; set; }
        public static DownloadsViewModel DownloadsVM { get; set; }
        public static SettingsViewModel SettingsVM { get; set; }
        public static PlayerViewModel PlayerVM { get; set; }

        public MainViewModel()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            DeserializeData();

            YourPodcastsVM = new YourPodcastsViewModel();
            NewEpisodesVM = new NewEpisodesViewModel();
            DiscoveryVM = new DiscoverViewModel();
            InProgressVM = new InProgressViewModel();
            DownloadsVM = new DownloadsViewModel();
            SettingsVM = new SettingsViewModel();
            PlayerVM = new PlayerViewModel();

            CurrentView = YourPodcastsVM;
            PlayerView = PlayerVM;

            SessionSetup();

            YourPodcastsViewCommand = new RelayCommand(o => CurrentView = YourPodcastsVM);
            NewEpisodesViewCommand = new RelayCommand(o => CurrentView = NewEpisodesVM);
            DiscoveryViewCommand = new RelayCommand(o => CurrentView = DiscoveryVM);
            InProgressViewCommand = new RelayCommand(o => CurrentView = InProgressVM);
            DownloadsViewCommand = new RelayCommand(o => CurrentView = DownloadsVM);
            SettingsViewCommand = new RelayCommand(o => CurrentView = SettingsVM);
            PlayerViewCommand = new RelayCommand(o => PlayerView = PlayerVM);
            ExitCommand = new RelayCommand(ExitExecuted, ExitCanExecute);

            sw.Stop();
            Debug.WriteLine("Starttime: " + sw.Elapsed);
        }

        private void DeserializeData()
        {
            _applicationSettings = SessionSerialization.Deserialize();
            _podcastsList = FeedSerialization.Deserialize();
        }

        private void SessionSetup()
        {
            PlayerVM.InitializeSession(_applicationSettings.Settings);
        }

        private bool ExitCanExecute(object arg) => true;
        private void ExitExecuted(object obj) => FeedSerialization.Serialize(_podcastsList);
    }
}