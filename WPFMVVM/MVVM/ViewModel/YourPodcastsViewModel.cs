using CodeHollow.FeedReader;
using CodeHollow.FeedReader.Feeds;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
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

            _podcastList = new PodcastFeed().RequestFeeds();
            _viewPodcasts = new ListCollectionView(_podcastList);
            _viewPodcasts.MoveCurrentToFirst();

            Update_ViewEpisodes();

            SortViewCollection(_viewPodcasts, nameof(FeedItem.Title), ListSortDirection.Ascending);
            _viewPodcasts.Refresh();

            _viewPodcasts.CurrentChanged += Update_ViewEpisodes;
        }

        private void Update_ViewEpisodes(object sender = null, EventArgs e = null)
        {
            _episodesList = new ObservableCollection<BaseFeedItem>(((BaseFeed)_viewPodcasts.CurrentItem).Items);

            _viewEpisodes = new ListCollectionView(_episodesList);
            _viewEpisodes.MoveCurrentToFirst();
            _viewEpisodes.Refresh();
        }

        private void SortViewCollection(ListCollectionView list, string property, ListSortDirection direction)
        {
            list.SortDescriptions.Clear();
            list.SortDescriptions.Add(new SortDescription(property, direction));
        }

        private bool PlayCanExecute(object arg) => true;
        private void PlayExecuted(object obj) => MainViewModel.PlayerVM.CurrentEpisode = (BaseFeedItem)obj;
    }
}