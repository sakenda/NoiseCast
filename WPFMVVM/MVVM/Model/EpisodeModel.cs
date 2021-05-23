using CodeHollow.FeedReader;
using CodeHollow.FeedReader.Feeds;
using Newtonsoft.Json;
using NoiseCast.Core;
using NoiseCast.MVVM.ViewModel;
using System;
using System.Windows.Controls;

namespace NoiseCast.MVVM.Model
{
    public class EpisodeModel : ObservableObject
    {
        public event EpisodeChangedEventHandler EpisodeChanged;
        public virtual void OnEpisodeChanged(EpisodeChangedEventArgs e) => EpisodeChanged?.Invoke(this, e);

        [JsonProperty("id")] private string _id;
        [JsonProperty("imagePath")] private string _imagePath;
        [JsonProperty("duration")] private double _durationRemaining;
        private string _mediaPath;
        private FeedItem _episode;

        [JsonIgnore] public string ID => _id;
        [JsonIgnore] public FeedItem Episode => _episode;
        [JsonIgnore] public string ImagePath => _imagePath;
        [JsonIgnore] public string MediaPath => _mediaPath;

        [JsonIgnore]
        public double DurationRemaining
        {
            get => _durationRemaining;
            set => SetProperty(ref _durationRemaining, value);
        }
        [JsonIgnore] public bool IsArchived => _durationRemaining == -1 ? true : false;

        /// <summary>
        /// Constructor for Json-Serialization
        /// </summary>
        [JsonConstructor]
        public EpisodeModel(string id, string imagePath, double duration)
        {
            _id = id;
            _imagePath = imagePath;
            _durationRemaining = duration;
        }

        /// <summary>
        /// New episode constructor
        /// </summary>
        public EpisodeModel(FeedItem episode, string id, string imagePath)
        {
            _episode = episode;
            _id = id;
            _imagePath = imagePath;
            _durationRemaining = 0;
            _mediaPath = GetMediaPath();
        }

        /// <summary>
        /// Set <see cref="DurationListened"/> to -1, so that <see cref="IsArchived"/> returns true
        /// </summary>
        public void SetIsArchived() => _durationRemaining = -1;

        /// <summary>
        /// Set <see cref="FeedItem"/>
        /// </summary>
        /// <param name="episode"></param>
        public void SetEpisodeFeed(FeedItem episode)
        {
            _episode = episode;
            _mediaPath = GetMediaPath();
        }

        /// <summary>
        /// Sets imagepath
        /// </summary>
        /// <param name="path"></param>
        public void SetImagePath(string path) => _imagePath = path;

        /// <summary>
        /// Set Path to the Enclosure URL depending on the FeedType
        /// </summary>
        private string GetMediaPath()
        {
            if (_episode == null) return null;

            string type = _episode.SpecificItem.GetType().Name;

            return type switch
            {
                nameof(Rss20FeedItem) => ((Rss20FeedItem)_episode.SpecificItem).Enclosure.Url,
                nameof(MediaRssFeedItem) => ((MediaRssFeedItem)_episode.SpecificItem).Enclosure.Url,
                _ => null
            };
        }
    }
}