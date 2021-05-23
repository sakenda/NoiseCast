using Newtonsoft.Json;
using NoiseCast.Core;
using NoiseCast.MVVM.Model;
using System.IO;

namespace NoiseCast.MVVM.Core
{
    public static class SessionSerialization
    {
        /// <summary>
        /// Serializes the session setting. E.g. last played item in player
        /// </summary>
        /// <param name="settings"></param>
        public static void Serialize(SettingsModel settings)
        {
            var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            var path = ApplicationSettings.SETTINGS_PATH + "session.json";

            FileController.WriteAllText(path, json);
        }

        /// <summary>
        /// Deserialize the session settings. E.g. last played item in player
        /// </summary>
        /// <returns></returns>
        public static SettingsModel Deserialize()
        {
            SettingsModel settings;
            string path = ApplicationSettings.SETTINGS_PATH + "session.json";

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                settings = JsonConvert.DeserializeObject<SettingsModel>(json);
            }
            else
            {
                settings = new SettingsModel(null);
                Serialize(settings);
            }

            return settings;
        }
    }
}