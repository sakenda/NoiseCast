using CodeHollow.FeedReader;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using NoiseCast.Core;
using NoiseCast.MVVM.Core;
using NoiseCast.MVVM.Model;

namespace NoiseCast.MVVM.ViewModel
{
    public class YourPodcastsViewModel : ObservableObject
    {
        private ListCollectionView _viewPodcasts;
        private ListCollectionView _viewEpisodes;
        private ObservableCollection<EpisodeModel> _episodesList;

        public static ICommand AddCommand { get; private set; }
        public static ICommand PlayCommand { get; private set; }
        public static ICommand SubscribeCommand { get; private set; }
        public ListCollectionView ViewPodcasts => _viewPodcasts;
        public ListCollectionView ViewEpisodes => _viewEpisodes;
        public ObservableCollection<EpisodeModel> EpisodesList => _episodesList;

        public YourPodcastsViewModel()
        {
            AddCommand = new RelayCommand(AddExecuted, AddCanExecute);
            PlayCommand = new RelayCommand(PlayExecuted, PlayCanExecute);
            SubscribeCommand = new RelayCommand(SubscribeExecuted, SubscribeCanExecute);
            _episodesList = new ObservableCollection<EpisodeModel>();

            _viewPodcasts = new ListCollectionView(PodcastListController.PodcastsList);
            _viewEpisodes = new ListCollectionView(_episodesList);
            UpdateEpisodesList();

            SortViewCollection(ViewPodcasts, nameof(Feed.LastUpdatedDate), ListSortDirection.Descending);
            ViewPodcasts.MoveCurrentToFirst();

            PodcastListController.PodcastsList.CollectionChanged += UpdatePodcastList_CollectionChanged;
            _viewPodcasts.CurrentChanged += UpdateEpisodesList;
        }

        /// <summary>
        /// Updates <see cref="EpisodesList"/> depending on the current podcast selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateEpisodesList(object sender = null, EventArgs e = null)
        {
            // PERFORMANCE!!!

            if (_episodesList == null) _episodesList = new ObservableCollection<EpisodeModel>();

            _episodesList.Clear();

            var podcast = (PodcastModel)_viewPodcasts.CurrentItem;

            if (podcast != null)
            {
                foreach (var item in podcast.Episodes)
                    _episodesList.Add(item);
            }

            ViewEpisodes.Refresh();
            ViewEpisodes.MoveCurrentToFirst();
        }

        /// <summary>
        /// Event: When the content of the <see cref="ListCollectionView"/> is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdatePodcastList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ViewPodcasts.Refresh();
            SortViewCollection(_viewPodcasts, nameof(Feed.LastUpdatedDate), ListSortDirection.Descending);
        }

        /// <summary>
        /// Sort a <see cref="ListCollectionView"/> dependen on a Property
        /// </summary>
        /// <param name="list"></param>
        /// <param name="property">The name of the property to sort</param>
        /// <param name="direction"><see cref="ListSortDirection.Ascending"/> or <see cref="ListSortDirection.Descending"/></param>
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
                PodcastListController.PodcastsList.AddFeed(text);
        }

        private bool PlayCanExecute(object arg) => true;
        private void PlayExecuted(object obj)
        {
            MainViewModel.PlayerVM.SetEpisode((EpisodeModel)_viewEpisodes.CurrentItem);
        }

        private bool SubscribeCanExecute(object arg)
        {
            var item = (PodcastModel)_viewPodcasts.CurrentItem;

            if (item == null) return false;

            return item.IsSubscribed != true;
        }
        private void SubscribeExecuted(object obj)
        {
            ((PodcastModel)_viewPodcasts.CurrentItem).SaveFeed();
        }
    }
}