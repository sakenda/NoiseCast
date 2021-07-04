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
    public class NewEpisodesViewModel : ObservableObject
    {
        public static ICommand PlayCommand { get; private set; }

        private ObservableCollection<EpisodeModel> _episodesList;
        private ListCollectionView _viewNewEpisodes;

        public ListCollectionView ViewNewEpisodes { get => _viewNewEpisodes; set => SetProperty(ref _viewNewEpisodes, value); }

        public NewEpisodesViewModel()
        {
            PlayCommand = new RelayCommand(PlayExecuted, PlayCanExecute);

            InitializeNewEpisodes();
        }

        /// <summary>
        /// Observes if episode is startet playing, then remove from list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectedEpisode_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is EpisodeModel episode)
            {
                if (e.PropertyName == nameof(EpisodeModel.DurationRemaining))
                {
                    if (episode.DurationRemaining != 0)
                    {
                        episode.PropertyChanged -= SelectedEpisode_PropertyChanged;
                        ViewNewEpisodes.Remove(episode);
                    }
                }
            }
        }

        /// <summary>
        /// Initialize episodeslist at startup
        /// </summary>
        private void InitializeNewEpisodes()
        {
            if (_episodesList == null) _episodesList = new ObservableCollection<EpisodeModel>();

            _episodesList.Clear();

            foreach (PodcastModel podcast in MainViewModel.PodcastsList)
            {
                foreach (EpisodeModel episode in podcast.Episodes)
                {
                    if (!episode.IsArchived && episode.DurationRemaining == 0)
                    {
                        _episodesList.Add(episode);
                    }
                }
            }

            ViewNewEpisodes = new ListCollectionView(_episodesList);
            Helper.SortViewCollection(_viewNewEpisodes, nameof(EpisodeModel.PublishingDate), ListSortDirection.Descending);
            ViewNewEpisodes.MoveCurrentToFirst();
        }

        /// <summary>
        /// Sends current selected episode to <see cref="PlayerViewModel"/>
        /// </summary>
        /// <param name="obj"></param>
        private void PlayExecuted(object obj)
        {
            var selectedEpisode = (EpisodeModel)_viewNewEpisodes.CurrentItem;

            MainViewModel.PlayerVM.SetEpisode(selectedEpisode);

            selectedEpisode.PropertyChanged += SelectedEpisode_PropertyChanged;
        }
        private bool PlayCanExecute(object arg) => true;
    }
}