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
        public static SettingsModel Settings;

        public static string SETTINGS_PATH => APPLICATION_PATH + @"/settings/";
        public static string SETTINGS_PODCAST_PATH => APPLICATION_PATH + @"/podcasts/";
        public static string SETTINGS_IMAGE_PATH => APPLICATION_PATH + @"/images/";

        static ApplicationSettings()
        {
            Directory.CreateDirectory(SETTINGS_PATH);
            Directory.CreateDirectory(SETTINGS_PODCAST_PATH);
            Directory.CreateDirectory(SETTINGS_IMAGE_PATH);
        }
    }
}