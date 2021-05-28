﻿using CodeHollow.FeedReader;
using NoiseCast.MVVM.Core;
using NoiseCast.MVVM.Model;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace NoiseCast.MVVM.ViewModel.Controller
{
    public static class PodcastListController
    {
        /// <summary>
        /// Main <see cref="ObservableCollection{PodcastModel}"/> to store podcasts
        /// </summary>
        public static ObservableCollection<PodcastModel> PodcastsList { get; set; }

        /// <summary>
        /// Static default constructor
        /// </summary>
        static PodcastListController()
        {
            if (PodcastsList == null) PodcastsList = FeedSerialization.Deserialize();
        }

        /// <summary>
        /// Add a feed based on an url string.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="url"></param>
        /// <returns>Returns <see cref="true"/> if adding was successful, <see cref="false"/> when podast is already added to list</returns>
        public static bool AddFeed(this ObservableCollection<PodcastModel> list, string url)
        {
            Feed feed = null;

            try
            {
                feed = Task.Run(() => FeedReader.ReadAsync(url)).Result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: Could not load Feed from:" + url, ex);
            }

            var podcastModel = new PodcastModel(feed);

            if (feed == null || PodcastsList.Contains(podcastModel)) return false;

            list.Add(podcastModel);
            return true;
        }

        /// <summary>
        /// Removes Podcast if it exist.
        /// </summary>
        /// <param name="podcast"></param>
        /// <returns>Returns <see cref="true"/> if Podcast was Removed, <see cref="false"/> if it wasn't.</returns>
        public static bool RemoveFeed(this PodcastModel podcast)
        {
            if (podcast == null) return false;
            if (!PodcastsList.Contains(podcast)) return false;

            PodcastsList.Remove(podcast);

            return true;
        }

        /// <summary>
        /// Serializes podcast locally
        /// </summary>
        /// <param name="podcast"></param>
        public static void SaveFeed(this PodcastModel podcast) => FeedSerialization.Serialize(podcast);
    }
}