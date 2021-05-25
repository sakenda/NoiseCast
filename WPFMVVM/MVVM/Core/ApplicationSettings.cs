using NoiseCast.Core;
using NoiseCast.MVVM.Model;
using NoiseCast.MVVM.ViewModel;
using System;
using System.IO;

namespace NoiseCast.MVVM.Core
{
    public class ApplicationSettings
    {
        private static string APPLICATION_PATH = AppDomain.CurrentDomain.BaseDirectory;
        private static SettingsModel _settings;

        public static SettingsModel Settings { get => _settings; set => _settings = value; }
        public static string SETTINGS_PATH => APPLICATION_PATH + @"\settings\";
        public static string PODCAST_PATH => APPLICATION_PATH + @"\podcasts\";
        public static string IMAGE_PATH => APPLICATION_PATH + @"\images\";

        static ApplicationSettings()
        {
            Directory.CreateDirectory(SETTINGS_PATH);
            Directory.CreateDirectory(PODCAST_PATH);
            Directory.CreateDirectory(IMAGE_PATH);
        }
    }
}