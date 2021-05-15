using CodeHollow.FeedReader;
using CodeHollow.FeedReader.Feeds;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using NoiseCast.Core;
using NoiseCast.MVVM.Core;

namespace NoiseCast.MVVM.ViewModel
{
    public class YourPodcastsViewModel : ObservableObject
    {
        private ListCollectionView _viewPodcasts;
        private ListCollectionView _viewEpisodes;
        private ObservableCollection<FeedItem> _episodesList;
        private PodcastListController _podcastFeed;

        public static ICommand AddCommand { get; private set; }
        public static ICommand PlayCommand { get; private set; }
        public static ICommand SubscribeCommand { get; private set; }
        public static ICommand UnsubCommand { get; private set; }
        public ListCollectionView ViewPodcasts => _viewPodcasts;
        public ListCollectionView ViewEpisodes => _viewEpisodes;
        public ObservableCollection<FeedItem> EpisodesList => _episodesList;

        public YourPodcastsViewModel()
        {
            AddCommand = new RelayCommand(AddExecuted, AddCanExecute);
            PlayCommand = new RelayCommand(PlayExecuted, PlayCanExecute);
            SubscribeCommand = new RelayCommand(SubscribeExecuted, SubscribeCanExecute);
            UnsubCommand = new RelayCommand(UnsubExecuted, UnsubCanExecute);

            _podcastFeed = new PodcastListController();
            _podcastFeed.GetSubscribedFeeds();

            // Podcasts List Setup
            _viewPodcasts = new ListCollectionView(_podcastFeed.PodcastList);

            // Episodes List Setup
            _episodesList = new ObservableCollection<FeedItem>();
            _viewEpisodes = new ListCollectionView(_episodesList);

            UpdateEpisodesList_PodcastChanged();
            SortViewCollection(ViewPodcasts, nameof(Feed.LastUpdatedDate), ListSortDirection.Descending);

            ViewPodcasts.MoveCurrentToFirst();
            ViewEpisodes.MoveCurrentToFirst();

            _viewPodcasts.CurrentChanged += UpdateEpisodesList_PodcastChanged;
            _podcastFeed.PodcastList.CollectionChanged += UpdatePodcastList_CollectionChanged;
        }

        private void UpdatePodcastList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            _viewPodcasts.Refresh();
            SortViewCollection(_viewPodcasts, nameof(Feed.LastUpdatedDate), ListSortDirection.Descending);
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

        private bool AddCanExecute(object arg) => !string.IsNullOrWhiteSpace(arg.ToString());
        private void AddExecuted(object obj)
        {
            string text = obj as string;

            Uri uriResult;
            bool result = Uri.TryCreate(text, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if (result)
                _podcastFeed.AddFeed(text);
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

        private bool SubscribeCanExecute(object arg)
        {
            return true;
        }
        private void SubscribeExecuted(object obj)
        {
            _podcastFeed.SaveFeed((Feed)_viewPodcasts.CurrentItem);
        }

        private bool UnsubCanExecute(object arg)
        {
            return false;
        }
        private void UnsubExecuted(object obj)
        {
        }
    }
}