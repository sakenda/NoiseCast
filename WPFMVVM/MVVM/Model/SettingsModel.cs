using NoiseCast.MVVM.Core;
using NoiseCast.MVVM.ViewModel;
using NoiseCast.MVVM.ViewModel.Controller;
using System.ComponentModel;

namespace NoiseCast.MVVM.Model
{
    public class SettingsModel
    {
        private string[] _lastSelectedID;
        private double _playerVolume;
        private double _skipValue;

        public string[] LastSelectedID => _lastSelectedID;
        public double PlayerVolume => _playerVolume;
        public double SkipValue => _skipValue;

        public SettingsModel(string[] lastSelectedID, double playerVolume, double skipValue)
        {
            _lastSelectedID = lastSelectedID != null ? lastSelectedID : new string[2] { "", "" };
            _playerVolume = playerVolume;
            _skipValue = skipValue;

            MainViewModel.PlayerVM.PropertyChanged += PlayerVM_PropertyChanged;
        }

        private void PlayerVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is PlayerViewModel playerVM)
            {
                if (playerVM.CurrentEpisode == null)
                    return;

                _lastSelectedID[1] = playerVM.CurrentEpisode.ID;
                _playerVolume = playerVM.Volume;
                _skipValue = playerVM.SkipAmount;
            }

            foreach (var podcast in PodcastListController.PodcastsList)
            {
                foreach (var episode in podcast.Episodes)
                {
                    if (episode.ID == _lastSelectedID[1])
                        _lastSelectedID[0] = podcast.GetID();
                }
            }

            SessionSerialization.Serialize(this);
        }
    }
}