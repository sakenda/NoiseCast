using CodeHollow.FeedReader;
using CodeHollow.FeedReader.Feeds;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using WPFMVVM.Core;
using WPFMVVM.MVVM.Core;

namespace WPFMVVM.MVVM.ViewModel
{
    public class YourPodcastsViewModel : ObservableObject
    {
        public ICommand PlayCommand { get; private set; }

        public ListCollectionView ViewPodcasts => _viewPodcasts;
        public ListCollectionView ViewEpisodes => _viewEpisodes;
        public ObservableCollection<Feed> PodcastList => _podcastList;
        public ObservableCollection<Rss20FeedItem> EpisodesList => _episodesList;
        public Rss20FeedItem CurrentEpisode => _currentEpisode;

        private ListCollectionView _viewPodcasts;
        private ListCollectionView _viewEpisodes;
        private ObservableCollection<Feed> _podcastList;
        private ObservableCollection<Rss20FeedItem> _episodesList;
        private Rss20FeedItem _currentEpisode;

        public YourPodcastsViewModel()
        {
            _podcastList = new PodcastFeed().RequestFeedsAsync();
            _viewPodcasts = new ListCollectionView(_podcastList);
            _viewPodcasts.MoveCurrentToFirst();

            _episodesList = ((Feed)_viewPodcasts.CurrentItem).Items as ObservableCollection<Rss20FeedItem>;
            if (_episodesList != null)
            {
                _viewEpisodes = new ListCollectionView(_episodesList);
                _viewEpisodes.MoveCurrentToFirst();
            }

            PlayCommand = new RelayCommand(PlayExecuted, PlayCanExecute);

            //_viewPodcasts.CurrentChanged += _viewPodcasts_CurrentChanged;
            //_viewEpisodes.CurrentChanged += _viewEpisodes_CurrentChanged;
        }

        private void _viewEpisodes_CurrentChanged(object sender, EventArgs e)
        {
            _currentEpisode = sender as Rss20FeedItem;
        }

        private void _viewPodcasts_CurrentChanged(object sender, EventArgs e)
        {
            _episodesList = ((Rss20Feed)_viewPodcasts.CurrentItem).Items as ObservableCollection<Rss20FeedItem>;
            _viewEpisodes = new ListCollectionView(_episodesList);
            _viewEpisodes.MoveCurrentToFirst();
        }

        private bool PlayCanExecute(object arg)
        {
            return true;
        }
        private void PlayExecuted(object obj)
        {
            _currentEpisode = obj as Rss20FeedItem;
        }
    }
}