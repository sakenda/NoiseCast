using NoiseCast.Core;
using NoiseCast.MVVM.Core;
using System.Diagnostics;

namespace NoiseCast.MVVM.ViewModel
{
    public class MainViewModel : ObservableObject
    {
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

        private object currentView;
        public object CurrentView
        {
            get => currentView;
            set => SetProperty(ref currentView, value);
        }
        private object playerView;
        public object PlayerView
        {
            get => playerView;
            set => SetProperty(ref playerView, value);
        }

        public MainViewModel()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

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
            ExitCommand = new RelayCommand(ExitExecuted, ExitCanExecute);

            sw.Stop();
            Debug.WriteLine("Full MainViewModel time: " + sw.Elapsed);
        }

        private bool ExitCanExecute(object arg) => true;
        private void ExitExecuted(object obj) => new FeedSerialization().Serialize(PodcastListController.PodcastsList);
    }
}