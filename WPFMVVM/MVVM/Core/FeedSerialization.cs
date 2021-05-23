using Newtonsoft.Json;
using NoiseCast.Core;
using NoiseCast.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;

namespace NoiseCast.MVVM.Core
{
    public class FeedSerialization
    {
        /// <summary>
        /// Serializes a <see cref="List{PodcastModel}"/>. All items in the List will get an ID, if not already set.
        /// </summary>
        /// <param name="podcastModels"></param>
        public void Serialize(ObservableCollection<PodcastModel> podcastModels)
        {
            foreach (var feed in podcastModels)
            {
                if (feed == null) continue;
                if (feed.GetID() == Guid.Empty.ToString()) feed.SetID();

                SaveImage(feed);

                string json = JsonConvert.SerializeObject(feed, Formatting.Indented);
                string path = ApplicationSettings.SETTINGS_PODCAST_PATH + feed.GetID() + ".json";
                FileController.WriteAllText(path, json);
            }
        }

        /// <summary>
        /// Serializes one <see cref="PodcastModel"/>. ID will be set if ID is equal to <see cref="Guid.Empty"/>
        /// </summary>
        /// <param name="podcastModel"></param>
        public void Serialize(PodcastModel podcastModel)
        {
            if (podcastModel.GetID() == Guid.Empty.ToString()) podcastModel.SetID();

            SaveImage(podcastModel);

            string json = JsonConvert.SerializeObject(podcastModel, Formatting.Indented);
            string path = ApplicationSettings.SETTINGS_PODCAST_PATH + podcastModel.GetID() + ".json";
            File.WriteAllText(path, json);
        }

        /// <summary>
        /// Deserializes all Files in .../podcasts/ path at startup
        /// </summary>
        /// <returns>A <see cref="List{PodcastModel}"/>Returns a List with all deserialized Podcasts if <see cref="true"/>, else a new empty List</returns>
        public ObservableCollection<PodcastModel> Deserialize()
        {
            var feedList = new ObservableCollection<PodcastModel>();

            string[] files = Directory.GetFiles(ApplicationSettings.SETTINGS_PODCAST_PATH, "*.json", SearchOption.TopDirectoryOnly);

            foreach (string file in files)
            {
                if (!Guid.TryParse(Path.GetFileNameWithoutExtension(file), out Guid id)) continue;

                string json = File.ReadAllText(file);
                PodcastModel feed = JsonConvert.DeserializeObject<PodcastModel>(json);
                feedList.Add(feed);
            }

            if (files.Length <= 0) return new ObservableCollection<PodcastModel>();

            return feedList;
        }

        /// <summary>
        /// Check if localpath and webpath ar valid and saves image
        /// </summary>
        /// <param name="podcastModel"></param>
        private async static void SaveImage(PodcastModel podcastModel)
        {
            string imagePath = ApplicationSettings.SETTINGS_IMAGE_PATH + podcastModel.GetID() + Path.GetExtension(podcastModel.ImagePath);
            bool isWebPath = Uri.TryCreate(podcastModel.ImagePath, UriKind.Absolute, out Uri webPath) && (webPath.Scheme == Uri.UriSchemeHttp || webPath.Scheme == Uri.UriSchemeHttps);

            if (isWebPath)
                podcastModel.SetImagePath(await FileController.DownloadImageAndSave(webPath.AbsoluteUri, imagePath));
        }
    }
}