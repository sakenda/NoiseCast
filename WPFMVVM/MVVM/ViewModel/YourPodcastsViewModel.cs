using NoiseCast.Core;
using NoiseCast.MVVM.Core;
using NoiseCast.MVVM.Model;
using NoiseCast.MVVM.ViewModel.Controller;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

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

            ViewPodcasts.MoveCurrentToFirst();

            PodcastListController.PodcastsList.CollectionChanged += PodcastList_CollectionChanged;
            _viewPodcasts.CurrentChanged += ViewPodcasts_CurrentChanged;
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
        }

        /// <summary>
        /// Check if Url does exist and add podcast to collection
        /// </summary>
        /// <param name="obj"></param>
        private void AddExecuted(object obj)
        {
            string text = obj as string;

            Uri uriResult;
            bool result = Uri.TryCreate(text, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if (result)
                PodcastListController.PodcastsList.AddFeed(text);
        }
        private bool AddCanExecute(object arg) => !string.IsNullOrWhiteSpace(arg.ToString());

        /// <summary>
        /// Sends current selected episode to <see cref="PlayerViewModel"/>
        /// </summary>
        /// <param name="obj"></param>
        private void PlayExecuted(object obj)
        {
            MainViewModel.PlayerVM.SetEpisode((EpisodeModel)_viewEpisodes.CurrentItem);
        }
        private bool PlayCanExecute(object arg) => true;

        /// <summary>
        /// Saves the current <see cref="PodcastModel"/> locally
        /// </summary>
        /// <param name="obj"></param>
        private void SubscribeExecuted(object obj)
        {
            ((PodcastModel)_viewPodcasts.CurrentItem).SaveFeed();
        }
        private bool SubscribeCanExecute(object arg)
        {
            var item = (PodcastModel)_viewPodcasts.CurrentItem;

            if (item == null) return false;

            return item.IsSubscribed != true;
        }

        /// <summary>
        /// Updates <see cref="EpisodesList"/> depending on the current podcast selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewPodcasts_CurrentChanged(object sender, EventArgs e)
        {
            if (_episodesList == null) _episodesList = new ObservableCollection<EpisodeModel>();

            _episodesList.Clear();
            var podcast = (PodcastModel)_viewPodcasts.CurrentItem;
            if (podcast != null)
                foreach (var item in podcast.Episodes)
                    EpisodesList.Add(item);

            SortViewCollection(_viewEpisodes, nameof(EpisodeModel.ID), ListSortDirection.Descending);
            ViewEpisodes.MoveCurrentToFirst();
        }

        /// <summary>
        /// Event: When the content of the <see cref="ListCollectionView"/> is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PodcastList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SortViewCollection(_viewPodcasts, nameof(PodcastModel.PodcastTitle), ListSortDirection.Descending);
        }
    }
}