using NoiseCast.Core;
using NoiseCast.MVVM.Core;
using NoiseCast.MVVM.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

namespace NoiseCast.MVVM.ViewModel
{
    public class InProgressViewModel : ObservableObject
    {
        public static ICommand PlayCommand { get; private set; }

        private ObservableCollection<EpisodeModel> _inProgressList;
        private ListCollectionView _viewInProgress;

        public ObservableCollection<EpisodeModel> InProgressList { get => _inProgressList; set => SetProperty(ref _inProgressList, value); }
        public ListCollectionView ViewInProgress { get => _viewInProgress; set => SetProperty(ref _viewInProgress, value); }

        public InProgressViewModel()
        {
            PlayCommand = new RelayCommand(PlayExecuted, PlayCanExecute);

            Initialize();
        }

        /// <summary>
        /// Initialize episodeslist at startup
        /// </summary>
        public void Initialize()
        {
            if (_inProgressList == null) _inProgressList = new ObservableCollection<EpisodeModel>();

            _inProgressList.Clear();

            foreach (PodcastModel podcast in MainViewModel.PodcastsList)
            {
                foreach (EpisodeModel episode in podcast.Episodes)
                {
                    if (!episode.IsArchived && episode.DurationRemaining != 0)
                    {
                        InProgressList.Add(episode);
                        episode.PropertyChanged += Episode_PropertyChanged;
                    }
                }
            }

            ViewInProgress = new ListCollectionView(_inProgressList);

            Helper.SortViewCollection(_viewInProgress, nameof(EpisodeModel.DurationRemaining), ListSortDirection.Ascending);
            ViewInProgress.MoveCurrentToFirst();
        }

        /// <summary>
        /// Observ if episode is fully played and remove from in progress list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Episode_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(EpisodeModel.IsArchived))
            {
                var episode = sender as EpisodeModel;
                if (episode.IsArchived)
                {
                    episode.PropertyChanged -= Episode_PropertyChanged;
                    ViewInProgress.Remove(episode);
                }
            }
        }

        /// <summary>
        /// Sends current selected episode to <see cref="PlayerViewModel"/>
        /// </summary>
        /// <param name="obj"></param>
        private void PlayExecuted(object obj)
        {
            var selectedEpisode = (EpisodeModel)_viewInProgress.CurrentItem;
            MainViewModel.PlayerVM.SetEpisode(selectedEpisode);
        }
        private bool PlayCanExecute(object arg) => true;
    }
}