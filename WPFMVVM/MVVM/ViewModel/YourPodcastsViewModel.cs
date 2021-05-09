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
        public static ICommand PlayCommand { get; private set; }    // static damit das style für ListBox diesen Command aufrufen kann
        public static ICommand UnsubCommand { get; private set; }

        public ListCollectionView ViewPodcasts => _viewPodcasts;
        public ListCollectionView ViewEpisodes => _viewEpisodes;
        public ObservableCollection<FeedItem> EpisodesList => _episodesList;

        private ListCollectionView _viewPodcasts;
        private ListCollectionView _viewEpisodes;
        private ObservableCollection<FeedItem> _episodesList;

        public YourPodcastsViewModel()
        {
            PlayCommand = new RelayCommand(PlayExecuted, PlayCanExecute);
            UnsubCommand = new RelayCommand(UnsubExecuted, UnsubCanExecute);

            // Podcasts List Setup
            //_podcastList = new PodcastFeed().RequestFeedList();
            _viewPodcasts = new ListCollectionView(new PodcastFeed().PodcastList);

            // Episodes List Setup
            _episodesList = new ObservableCollection<FeedItem>();
            _viewEpisodes = new ListCollectionView(_episodesList);

            UpdateEpisodesList_PodcastChanged();
            SortViewCollection(ViewPodcasts, nameof(Feed.LastUpdatedDate), ListSortDirection.Descending);

            ViewPodcasts.MoveCurrentToFirst();
            ViewEpisodes.MoveCurrentToFirst();

            _viewPodcasts.CurrentChanged += UpdateEpisodesList_PodcastChanged;
        }

        private void UpdateEpisodesList_PodcastChanged(object sender = null, EventArgs e = null)
        {
            EpisodesList.Clear();

            Feed feed = (Feed)_viewPodcasts.CurrentItem;
            if (feed != null)
            {
                foreach (var item in ((Feed)_viewPodcasts.CurrentItem).Items)
                    _episodesList.Add(item);
            }

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
            // Add to Playlist, if CurrentEpisode is not null

            Feed feed = _viewPodcasts.CurrentItem as Feed;
            FeedItem feedItem = _viewEpisodes.CurrentItem as FeedItem;

            string mediaUrl = null;
            string imageUrl = feed.ImageUrl;

            switch (feed.Type)
            {
                case FeedType.Rss_2_0:
                    mediaUrl = ((Rss20FeedItem)feedItem.SpecificItem).Enclosure.Url;
                    break;
                case FeedType.MediaRss:
                    mediaUrl = ((MediaRssFeedItem)feedItem.SpecificItem).Enclosure.Url;
                    break;
                default:
                    break;
            }

            MainViewModel.PlayerVM.CurrentEpisode.ChangeEpisode(feedItem, mediaUrl, imageUrl);
        }

        private bool UnsubCanExecute(object arg) => true;
        private void UnsubExecuted(object obj)
        {
            Feed feed = (Feed)_viewPodcasts.CurrentItem;
            new PodcastFeed().RemoveFeed(feed);
        }
    }
}