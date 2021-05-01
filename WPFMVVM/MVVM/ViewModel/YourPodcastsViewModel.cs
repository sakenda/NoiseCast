using CodeHollow.FeedReader;
using CodeHollow.FeedReader.Feeds;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using WPFMVVM.Core;
using WPFMVVM.MVVM.Core;

namespace WPFMVVM.MVVM.ViewModel
{
    public class YourPodcastsViewModel : ObservableObject
    {
        public static ICommand PlayCommand { get; private set; }

        public ListCollectionView ViewPodcasts => _viewPodcasts;
        public ListCollectionView ViewEpisodes => _viewEpisodes;
        public ObservableCollection<BaseFeed> PodcastList => _podcastList;
        public ObservableCollection<BaseFeedItem> EpisodesList => _episodesList;
        public BaseFeedItem CurrentEpisode
        {
            get => _currentEpisode;
            set => SetProperty(ref _currentEpisode, value);
        }

        private ListCollectionView _viewPodcasts;
        private ListCollectionView _viewEpisodes;
        private ObservableCollection<BaseFeed> _podcastList;
        private ObservableCollection<BaseFeedItem> _episodesList;
        private BaseFeedItem _currentEpisode;

        public YourPodcastsViewModel()
        {
            _podcastList = new ObservableCollection<BaseFeed>();
            _episodesList = new ObservableCollection<BaseFeedItem>();

            PlayCommand = new RelayCommand(PlayExecuted, PlayCanExecute);

            _podcastList = new PodcastFeed().RequestFeeds();
            UpdateView_Podcasts();

            ViewPodcasts.CurrentChanged += UpdateView_Podcasts;
        }

        private void ChangeCurrentPlayerItem() => MainViewModel.PlayerVM.CurrentEpisode = _viewEpisodes.CurrentItem as BaseFeedItem;

        private void UpdateView_Podcasts(object sender = null, EventArgs e = null)
        {
            if (_viewPodcasts == null || (_viewPodcasts.Count == 0 && _episodesList.Count != 0))
            {
                _viewPodcasts = new ListCollectionView(_podcastList);
                _viewPodcasts.MoveCurrentToFirst();
            }
            else return;

            _episodesList = new ObservableCollection<BaseFeedItem>(((BaseFeed)(_viewPodcasts.CurrentItem)).Items);

            _viewEpisodes = new ListCollectionView(_episodesList);
            _viewEpisodes.MoveCurrentToLast();
        }

        private bool PlayCanExecute(object arg)
        {
            return true;
        }
        private void PlayExecuted(object obj)
        {
            MainViewModel.PlayerVM.CurrentEpisode = _viewEpisodes.CurrentItem as BaseFeedItem;
        }
    }
}