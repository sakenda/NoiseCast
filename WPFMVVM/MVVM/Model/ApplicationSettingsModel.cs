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
        public PlayerSessionModel PlayerSession { get => _playerSession; set => _playerSession = value; }

        public ApplicationSettingsModel()
        {
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