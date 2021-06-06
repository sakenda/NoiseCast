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
        public double PlayerVolume => _playerVolume;
        public double SkipValue { get => _skipValue; set => SetProperty(ref _skipValue, value); }

        public PlayerSessionModel(string[] lastSelectedID, double playerVolume, double skipValue)
        {
            _lastSelectedID = lastSelectedID != null ? lastSelectedID : new string[2] { "", "" };
            _playerVolume = playerVolume;
            _skipValue = skipValue;
        }

        /// <summary>
        /// Subscribe (at the right time) to Player.PropertyChanged
        /// </summary>
        /// <param name="pVM"></param>
        public void SubscribePropertyChanged(PlayerViewModel pVM) => pVM.PropertyChanged += PlayerVM_PropertyChanged;

        /// <summary>
        /// Gather all relevant properties to save in the session.json
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayerVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is PlayerViewModel playerVM)
            {
                if (playerVM.CurrentEpisode == null)
                    return;

                _lastSelectedID[0] = playerVM.CurrentEpisode.ParentPodcastModel.Id;
                _lastSelectedID[1] = playerVM.CurrentEpisode.Id;
                _playerVolume = playerVM.Volume;
                _skipValue = playerVM.SkipAmount;
            }

            SessionSerialization.Serialize(MainViewModel.SettingsVM.AppSettings);
        }
    }
}