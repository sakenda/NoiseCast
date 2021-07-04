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
        public ListCollectionView ViewEpisodes { get => _viewEpisodes; set => SetProperty(ref _viewEpisodes, value); }
        public ObservableCollection<EpisodeModel> EpisodesList { get => _episodesList; set => SetProperty(ref _episodesList, value); }

        public YourPodcastsViewModel()
        {
            AddCommand = new RelayCommand(AddExecuted, AddCanExecute);
            PlayCommand = new RelayCommand(PlayExecuted, PlayCanExecute);
            SubscribeCommand = new RelayCommand(SubscribeExecuted, SubscribeCanExecute);

            _episodesList = new ObservableCollection<EpisodeModel>();
            _viewPodcasts = new ListCollectionView(MainViewModel.PodcastsList);
            _viewEpisodes = new ListCollectionView(_episodesList);

            ViewPodcasts.MoveCurrentToFirst();

            MainViewModel.PodcastsList.CollectionChanged += PodcastList_CollectionChanged;
            _viewPodcasts.CurrentChanged += ViewPodcasts_CurrentChanged;
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
            {
                var podcastList = (ObservableCollection<PodcastModel>)ViewPodcasts.SourceCollection;
                podcastList.AddFeed(text);
            }
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
            PodcastModel podcast = (PodcastModel)_viewPodcasts.CurrentItem;

            if (!podcast.IsSubscribed)
            {
                podcast.SaveFeed();
                return;
            }

            PodcastListController.RemoveFeed(podcast);
            ViewPodcasts.Refresh();
        }
        private bool SubscribeCanExecute(object arg) => true;

        /// <summary>
        /// Updates <see cref="EpisodesList"/> depending on the current podcast selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewPodcasts_CurrentChanged(object sender, EventArgs e)
        {
            if (_episodesList == null) _episodesList = new ObservableCollection<EpisodeModel>();

            if (_viewPodcasts.CurrentItem is PodcastModel podcast && podcast != null)
            {
                EpisodesList = null;
                EpisodesList = podcast.GetEpisodes();
            }

            ViewEpisodes = new ListCollectionView(_episodesList);
            Helper.SortViewCollection(_viewEpisodes, nameof(EpisodeModel.PublishingDate), ListSortDirection.Descending);
            ViewEpisodes.MoveCurrentToFirst();
        }

        /// <summary>
        /// Event: When the content of the <see cref="ListCollectionView"/> is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PodcastList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Helper.SortViewCollection(_viewPodcasts, nameof(PodcastModel.Title), ListSortDirection.Descending);
        }
    }
}