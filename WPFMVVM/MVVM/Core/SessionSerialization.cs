using Newtonsoft.Json;
using NoiseCast.Core;
using System;
using System.IO;

namespace NoiseCast.MVVM.Model
{
    public static class SessionSerialization
    {
        private static string _sessionSettingsPath = AppDomain.CurrentDomain.BaseDirectory + "session.json";

        /// <summary>
        /// Serializes the application settings.
        /// </summary>
        /// <param name="settings"></param>
        public static void Serialize(ApplicationSettingsModel settings)
        {
            var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            FileController.WriteAllText(_sessionSettingsPath, json);
        }

        /// <summary>
        /// Deserialize the applicationsettings.
        /// </summary>
        /// <returns></returns>
        public static ApplicationSettingsModel Deserialize()
        {
            if (File.Exists(_sessionSettingsPath))
            {
                string json = File.ReadAllText(_sessionSettingsPath);
                return JsonConvert.DeserializeObject<ApplicationSettingsModel>(json);
            }

            var settings = new ApplicationSettingsModel() { PlayerSession = new PlayerSessionModel(null, 0.5, 30) };
            Serialize(settings);

            return settings;
        }
    }
}