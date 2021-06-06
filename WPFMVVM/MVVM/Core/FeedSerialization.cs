using Newtonsoft.Json;
using NoiseCast.Core;
using NoiseCast.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;

namespace NoiseCast.MVVM.Model
{
    public static class FeedSerialization
    {
        /// <summary>
        /// Serializes a <see cref="List{PodcastModel}"/>. All items in the List will get an ID, if not already set.
        /// </summary>
        /// <param name="podcastList"></param>
        public static void Serialize(this ObservableCollection<PodcastModel> podcastList)
        {
            foreach (var podcast in podcastList)
            {
                if (podcast == null) continue;
                if (podcast.Id == Guid.Empty.ToString()) podcast.SetID();

                SaveImage(podcast);

                string json = JsonConvert.SerializeObject(podcast, Formatting.Indented);
                string path = MainViewModel.ApplicationSettings.GetPodcastPath() + podcast.Id + ".json";
                FileController.WriteAllText(path, json);
            }
        }

        /// <summary>
        /// Serializes one <see cref="PodcastModel"/>. ID will be set if ID is equal to <see cref="Guid.Empty"/>
        /// </summary>
        /// <param name="podcastModel"></param>
        public static void Serialize(this PodcastModel podcastModel)
        {
            if (podcastModel.Id == Guid.Empty.ToString()) podcastModel.SetID();

            SaveImage(podcastModel);

            string json = JsonConvert.SerializeObject(podcastModel, Formatting.Indented);
            string path = MainViewModel.ApplicationSettings.GetPodcastPath() + podcastModel.Id + ".json";
            File.WriteAllText(path, json);
        }

        /// <summary>
        /// Deserializes all Files in .../podcasts/ path at startup
        /// </summary>
        /// <returns>A <see cref="List{PodcastModel}"/>Returns a List with all deserialized Podcasts if <see cref="true"/>, else a new empty List</returns>
        public static ObservableCollection<PodcastModel> Deserialize()
        {
            var feedList = new ObservableCollection<PodcastModel>();
            string podcastPath = MainViewModel.ApplicationSettings.GetPodcastPath();

            string[] files = Directory.GetFiles(podcastPath, "*.json", SearchOption.TopDirectoryOnly);

            Parallel.ForEach(files, file =>
            {
                if (!Guid.TryParse(Path.GetFileNameWithoutExtension(file), out Guid id)) return;

                string json = File.ReadAllText(file);
                PodcastModel feed = JsonConvert.DeserializeObject<PodcastModel>(json);
                feedList.Add(feed);
            });

            if (files.Length <= 0) return new ObservableCollection<PodcastModel>();

            return feedList;
        }

        /// <summary>
        /// Check if localpath and webpath ar valid and saves image
        /// </summary>
        /// <param name="podcastModel"></param>
        private static async void SaveImage(this PodcastModel podcastModel)
        {
            string imagePath = MainViewModel.ApplicationSettings.GetImagePath() + podcastModel.Id + ".jpg";
            bool isWebPath = Uri.TryCreate(podcastModel.ImagePath, UriKind.Absolute, out Uri webPath) && (webPath.Scheme == Uri.UriSchemeHttp || webPath.Scheme == Uri.UriSchemeHttps);

            if (isWebPath)
                podcastModel.SetImagePath(await FileController.DownloadImageAndSave(webPath.AbsoluteUri, imagePath));
        }
    }
}