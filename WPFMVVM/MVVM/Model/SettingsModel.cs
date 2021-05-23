using NoiseCast.MVVM.Core;
using NoiseCast.MVVM.ViewModel;

namespace NoiseCast.MVVM.Model
{
    public class SettingsModel
    {
        private string _playerEpisodeID;

        public string PlayerEpisodeID => _playerEpisodeID;

        public SettingsModel(string playerEpisodeID)
        {
            _playerEpisodeID = playerEpisodeID;
            MainViewModel.PlayerVM.MediaChanged += PlayerVM_MediaChanged;
        }

        private void PlayerVM_MediaChanged(object sender, MediaChangedEventArgs e)
        {
            _playerEpisodeID = e.Episode.ID;
            SessionSerialization.Serialize(this);
        }
    }
}