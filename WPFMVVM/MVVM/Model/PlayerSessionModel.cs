using NoiseCast.Core;
using NoiseCast.MVVM.ViewModel;
using System.ComponentModel;

namespace NoiseCast.MVVM.Model
{
    public class PlayerSessionModel : ObservableObject
    {
        private string[] _lastSelectedID;
        private double _playerVolume;
        private double _skipValue;

        public string[] LastSelectedID => _lastSelectedID;
        public double PlayerVolume
        {
            get => _playerVolume;
            set
            {
                if (SetProperty(ref _playerVolume, value))
                    Serialize();
            }
        }
        public double SkipValue
        {
            get => _skipValue;
            set
            {
                if (SetProperty(ref _skipValue, value))
                    Serialize();
            }
        }

        public PlayerSessionModel(string[] lastSelectedID, double playerVolume, double skipValue)
        {
            _lastSelectedID = lastSelectedID != null ? lastSelectedID : new string[2] { "", "" };
            _playerVolume = playerVolume;
            _skipValue = skipValue;
        }

        /// <summary>
        /// Gather all relevant properties to save in the session.json
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Serialize()
        {
            _lastSelectedID[0] = MainViewModel.PlayerVM.CurrentEpisode.ParentPodcastModel.Id;
            _lastSelectedID[1] = MainViewModel.PlayerVM.CurrentEpisode.Id;

            SessionSerialization.Serialize(MainViewModel.SettingsVM.AppSettings);
        }
    }
}