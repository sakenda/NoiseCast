using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using WPFMVVM.MVVM.Model;

namespace WPFMVVM.MVVM.Core
{
    public class FeedSerialization
    {
        /// <summary>
        /// Serializes a <see cref="List{SerializableFeedModel}"/>. All items in the List will get an ID, if not already set.
        /// </summary>
        /// <param name="sFeeds"></param>
        public void Serialize(List<SerializableFeedModel> sFeeds)
        {
            foreach (var feed in sFeeds)
            {
                if (feed.GetID() == Guid.Empty.ToString()) feed.SetID();

                string json = JsonConvert.SerializeObject(feed, Formatting.Indented);

                string path = ApplicationSettings.SETTINGS_PODCAST_PATH + feed.GetID() + ".json";
                File.WriteAllText(path, json);
            }
        }

        /// <summary>
        /// Serializes one <see cref="SerializableFeedModel"/>. ID will be set if ID is equal to <see cref="Guid.Empty"/>
        /// </summary>
        /// <param name="feed"></param>
        public void Serialize(SerializableFeedModel feed)
        {
            if (feed.GetID() == Guid.Empty.ToString()) feed.SetID();

            string json = JsonConvert.SerializeObject(feed, Formatting.Indented);

            string path = ApplicationSettings.SETTINGS_PODCAST_PATH + feed.GetID() + ".json";
            File.WriteAllText(path, json);
        }

        /// <summary>
        /// Deserializes all Files in .../podcasts/ path at startup
        /// </summary>
        /// <returns>A <see cref="List{SerializableFeedModel}"/></returns>
        public List<SerializableFeedModel> Deserialize()
        {
            List<SerializableFeedModel> feedList = new List<SerializableFeedModel>();

            string[] files = Directory.GetFiles(ApplicationSettings.SETTINGS_PODCAST_PATH, "*.json", SearchOption.TopDirectoryOnly);

            foreach (string file in files)
            {
                if (!Guid.TryParse(Path.GetFileNameWithoutExtension(file), out Guid id)) continue;

                string json = File.ReadAllText(file);
                SerializableFeedModel feed = JsonConvert.DeserializeObject<SerializableFeedModel>(json);
                feedList.Add(feed);
            }

            return feedList;
        }
    }
}