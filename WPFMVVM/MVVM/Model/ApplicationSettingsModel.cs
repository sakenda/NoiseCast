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
        private SettingsModel _settings;

        public string SettingsPathMain { get => _settingsPathMain; set => SetProperty(ref _settingsPathMain, value); }
        public SettingsModel Settings { get => _settings; set => _settings = value; }

        [JsonConstructor]
        public ApplicationSettingsModel()
        {
            if (string.IsNullOrWhiteSpace(_settingsPathMain))
                _settingsPathMain = AppDomain.CurrentDomain.BaseDirectory;

            Directory.CreateDirectory(SettingsPathMain);
            Directory.CreateDirectory(GetPodcastPath());
            Directory.CreateDirectory(GetImagePath());
        }

        public string GetImagePath() => _settingsPathMain + _settingsImagePath;
        public string GetPodcastPath() => _settingsPathMain + _settingsPodcastPath;
    }
}