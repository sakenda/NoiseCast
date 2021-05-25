using CodeHollow.FeedReader;
using Newtonsoft.Json;
using NoiseCast.Core;
using NoiseCast.MVVM.Core;
using NoiseCast.MVVM.ViewModel.Controller;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace NoiseCast.MVVM.Model
{
    public class PodcastModel : ObservableObject
    {
        [JsonProperty("originalDocument")] private string _originalDocument => _podcast.OriginalDocument;
        [JsonProperty("episodesList")] private ObservableCollection<EpisodeModel> _episodes;
        [JsonProperty("id")] private Guid _id;
        private Feed _podcast;
        private string _imagePath;
        private bool _isSubscribed;

        [JsonIgnore] public ObservableCollection<EpisodeModel> Episodes => _episodes;
        [JsonIgnore] public string PodcastTitle => _podcast.Title;
        [JsonIgnore] public Feed Podcast => _podcast;
        [JsonIgnore] public string ImagePath => _imagePath;
        [JsonIgnore] public string FilePath => ApplicationSettings.PODCAST_PATH + _id.ToString() + ".json";
        [JsonIgnore] public bool IsSubscribed { get => _isSubscribed; private set => SetProperty(ref _isSubscribed, value); }

        /// <summary>
        /// Constructor for deserialized feeds with saved values
        /// </summary>
        /// <param name="id">Internal id</param>
        /// <param name="originalDocument">Feed as XML-Document</param>
        /// <param name="episodesList">List of values relevant to each episode</param>
        [JsonConstructor]
        public PodcastModel(Guid id, string originalDocument, ObservableCollection<EpisodeModel> episodesList)
        {
            _id = id;
            _isSubscribed = true;
            _podcast = FeedReader.ReadFromString(originalDocument);
            _episodes = episodesList == null ? new ObservableCollection<EpisodeModel>() : episodesList;

            _imagePath = _podcast.ImageUrl;
            InitializeEpisodes();
        }

        /// <summary>
        /// Constructor for new Podcasts with no ID
        /// </summary>
        /// <param name="podcast"></param>
        public PodcastModel(Feed podcast)
        {
            _podcast = podcast;
            _id = Guid.Empty;
            _isSubscribed = false;
            _imagePath = podcast.ImageUrl;
            _episodes = new ObservableCollection<EpisodeModel>();
            InitializeEpisodes();
        }

        /// <summary>
        /// Initialize all episodes and skips if one already exists
        /// </summary>
        private void InitializeEpisodes()
        {
            foreach (var feedItem in _podcast.Items)
            {
                // Search for saved episodes, add FeedItem and continue
                var temp = _episodes.FirstOrDefault(x => x.ID == feedItem.Id);
                if (temp != null)
                {
                    temp.SetEpisodeFeed(feedItem, this);
                    continue;
                }

                // add new episode
                var episodeModel = new EpisodeModel(feedItem, feedItem.Id, _imagePath);
                _episodes.Add(episodeModel);
            }
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
            IsSubscribed = true;
            return true;
        }

        /// <summary>
        /// Sets imagepath on thee <see cref="PodcastModel"/> and all <see cref="EpisodeModel"/>
        /// </summary>
        /// <param name="path"></param>
        public void SetImagePath(string path)
        {
            _imagePath = path;
            foreach (var item in Episodes)
            {
                item.SetImagePath(path);
            }
        }

        /// <summary>
        /// On changes in feed, feedObj will be updated
        /// </summary>
        /// <param name="feed">Updated Feed</param>
        public void UpdateFeed(Feed feed) => _podcast = feed;

        /// <summary>
        /// Save Feed locally
        /// </summary>
        public void Save() => PodcastListController.SaveFeed(this);
    }
}