using CodeHollow.FeedReader;
using CodeHollow.FeedReader.Feeds;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

        private ListCollectionView _viewPodcasts;
        private ListCollectionView _viewEpisodes;
        private ObservableCollection<BaseFeed> _podcastList;
        private ObservableCollection<BaseFeedItem> _episodesList;

        public YourPodcastsViewModel()
        {
            PlayCommand = new RelayCommand(PlayExecuted, PlayCanExecute);

            // Podcasts List Setup
            _podcastList = new PodcastFeed().RequestFeeds();
            _viewPodcasts = new ListCollectionView(_podcastList);
            SortViewCollection(_viewPodcasts, nameof(FeedItem.Title), ListSortDirection.Ascending);
            _viewPodcasts.MoveCurrentToFirst();

            // Episodes List Setup
            _episodesList = new ObservableCollection<BaseFeedItem>();
            _viewEpisodes = new ListCollectionView(_episodesList);
            UpdateEpisodesList_PodcastChanged();
            _viewEpisodes.MoveCurrentToFirst();

            _viewPodcasts.Refresh();
            _viewEpisodes.Refresh();

            _viewPodcasts.CurrentChanged += UpdateEpisodesList_PodcastChanged;
        }

        private void UpdateEpisodesList_PodcastChanged(object sender = null, EventArgs e = null)
        {
            EpisodesList.Clear();

            foreach (var item in ((BaseFeed)_viewPodcasts.CurrentItem).Items)
                _episodesList.Add(item);

            ViewEpisodes.Refresh();
        }

        private void SortViewCollection(ListCollectionView list, string property, ListSortDirection direction)
        {
            list.SortDescriptions.Clear();
            list.SortDescriptions.Add(new SortDescription(property, direction));
            list.Refresh();
        }

        private bool PlayCanExecute(object arg) => true;
        private void PlayExecuted(object obj)
        {
            BaseFeedItem feed = _viewEpisodes.CurrentItem as BaseFeedItem;
            string mediaUrl = null;
            string imageUrl = null;

            switch (feed.GetType().Name)
            {
                case nameof(Rss20FeedItem):
                    mediaUrl = ((Rss20FeedItem)feed).Enclosure.Url;
                    imageUrl = ((Rss20Feed)_viewPodcasts.CurrentItem).Image.Url;
                    break;
                case nameof(MediaRssFeedItem):
                    mediaUrl = ((MediaRssFeedItem)feed).Enclosure.Url;
                    imageUrl = ((MediaRssFeed)_viewPodcasts.CurrentItem).Image.Url;
                    break;
                default:
                    break;
            }

            MainViewModel.PlayerVM.CurrentEpisode.ChangeEpisode(feed, mediaUrl, imageUrl);
        }
    }
}