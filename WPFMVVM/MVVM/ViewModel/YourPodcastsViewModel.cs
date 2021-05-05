using CodeHollow.FeedReader;
using CodeHollow.FeedReader.Feeds;
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
        public ObservableCollection<BaseFeed> PodcastList => _podcastList;

        private ListCollectionView _viewPodcasts;
        private ObservableCollection<BaseFeed> _podcastList;

        public YourPodcastsViewModel()
        {
            PlayCommand = new RelayCommand(PlayExecuted, PlayCanExecute);

            _podcastList = new PodcastFeed().RequestFeeds();
            _viewPodcasts = new ListCollectionView(_podcastList);
            _viewPodcasts.MoveCurrentToFirst();

            SortViewCollection(_viewPodcasts, nameof(FeedItem.Title), ListSortDirection.Ascending);
            _viewPodcasts.Refresh();
        }

        private void SortViewCollection(ListCollectionView list, string property, ListSortDirection direction)
        {
            list.SortDescriptions.Clear();
            list.SortDescriptions.Add(new SortDescription(property, direction));
        }

        private bool PlayCanExecute(object arg) => true;
        private void PlayExecuted(object obj)
        {
            var typeName = ((BaseFeedItem)obj).GetType().Name;

            switch (typeName)
            {
                case nameof(Rss20FeedItem):
                    MainViewModel.PlayerVM.SetCurrentEpisode((Rss20FeedItem)obj, ((Rss20Feed)_viewPodcasts.CurrentItem).Image.Url);
                    break;
                case nameof(MediaRssFeedItem):
                    MainViewModel.PlayerVM.SetCurrentEpisode((MediaRssFeedItem)obj, ((MediaRssFeed)_viewPodcasts.CurrentItem).Image.Url);
                    break;
                default:
                    break;
            }
        }
    }
}