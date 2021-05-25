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
        }

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

                _lastSelectedID[0] = playerVM.CurrentEpisode.ParentPodcast.GetID();
                _lastSelectedID[1] = playerVM.CurrentEpisode.ID;
                _playerVolume = playerVM.Volume;
                _skipValue = 30;
                //_skipValue = playerVM.SkipAmount;
            }

            SessionSerialization.Serialize(this);
        }
    }
}