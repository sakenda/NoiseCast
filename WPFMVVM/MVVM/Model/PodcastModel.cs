using CodeHollow.FeedReader;
using Newtonsoft.Json;
using NoiseCast.MVVM.Core;
using System;
using System.Collections.Generic;

namespace NoiseCast.MVVM.Model
{
    public class PodcastModel
    {
        [JsonProperty("originalDocument")] private string _originalDocument => _podcast.OriginalDocument;
        [JsonProperty("id")] private Guid _id;
        [JsonProperty("episodesList")] private List<EpisodeModel> _episodes;
        private Feed _podcast;
        private string _imagePath;
        private PodcastListController _controller = new PodcastListController();

        [JsonIgnore] public List<EpisodeModel> Episodes => _episodes;
        [JsonIgnore] public Feed Podcast => _podcast;
        [JsonIgnore] public string FilePath => ApplicationSettings.SETTINGS_PODCAST_PATH + _id.ToString() + ".json";
        [JsonIgnore] public bool IsSubscribed => _id != Guid.Empty;
        [JsonIgnore] public string ImagePath => _imagePath;

        /// <summary>
        /// Constructor for deserialized feeds with saved values
        /// </summary>
        /// <param name="id">Internal id</param>
        /// <param name="originalDocument">Feed as XML-Document</param>
        /// <param name="episodes">List of values relevant to each episode</param>
        [JsonConstructor]
        public PodcastModel(Guid id, string originalDocument, List<EpisodeModel> episodes)
        {
            _id = id;
            _podcast = FeedReader.ReadFromString(originalDocument);
            _episodes = episodes;

            _imagePath = _podcast.ImageUrl; // TODO: Save image locally/set imagePath locally
        }

        /// <summary>
        /// Constructor for new Podcasts with no ID
        /// </summary>
        /// <param name="podcast"></param>
        public PodcastModel(Feed podcast)
        {
            _podcast = podcast;
            _id = Guid.Empty;
            _episodes = new List<EpisodeModel>();
        }

        /// <summary>
        /// Get ID as string
        /// </summary>
        /// <returns></returns>
        public string GetID() => _id.ToString();

        /// <summary>
        /// Sets ID if ID = <see cref="Guid.Empty"/>
        /// </summary>
        /// <returns><see cref="true"/> ID is setted, <see cref="false"/> ID was already set</returns>
        public bool SetID()
        {
            if (_id != Guid.Empty) return false;

            _id = Guid.NewGuid();
            return true;
        }

        /// <summary>
        /// On changes in feed, feedObj will be updated
        /// </summary>
        /// <param name="feed">Updated Feed</param>
        public void UpdateFeed(Feed feed) => _podcast = feed;

        /// <summary>
        /// Save Feed locally
        /// </summary>
        public void Save() => _controller.SaveFeed(this);
    }
}