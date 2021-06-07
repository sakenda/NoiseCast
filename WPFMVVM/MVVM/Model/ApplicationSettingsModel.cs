using Newtonsoft.Json;
using NoiseCast.Core;
using System;
using System.IO;

namespace NoiseCast.MVVM.Model
{
    public class ApplicationSettingsModel : ObservableObject
    {
        private const string _settingsImagePath = @"\images\";
        private const string _settingsPodcastPath = @"\podcasts\";

        private string _settingsPathMain;
        private PlayerSessionModel _playerSession;

        public string SettingsPathMain { get => _settingsPathMain; set => SetProperty(ref _settingsPathMain, value); }
        public PlayerSessionModel PlayerSession { get => _playerSession; set => SetProperty(ref _playerSession, value); }

        public ApplicationSettingsModel()
        {
            _settingsPathMain = AppDomain.CurrentDomain.BaseDirectory;
            _playerSession = new PlayerSessionModel(null, 0.5, 30);

            Directory.CreateDirectory(SettingsPathMain);
            Directory.CreateDirectory(GetPodcastPath());
            Directory.CreateDirectory(GetImagePath());
        }

        [JsonConstructor]
        public ApplicationSettingsModel(string settingsPathMain, PlayerSessionModel playerSession)
        {
            _settingsPathMain = string.IsNullOrWhiteSpace(settingsPathMain) ? AppDomain.CurrentDomain.BaseDirectory : settingsPathMain;
            _playerSession = playerSession;

            Directory.CreateDirectory(SettingsPathMain);
            Directory.CreateDirectory(GetPodcastPath());
            Directory.CreateDirectory(GetImagePath());
        }

        public string GetImagePath() => _settingsPathMain + _settingsImagePath;
        public string GetPodcastPath() => _settingsPathMain + _settingsPodcastPath;
    }
}