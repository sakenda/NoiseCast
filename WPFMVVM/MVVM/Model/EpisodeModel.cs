using CodeHollow.FeedReader;
using CodeHollow.FeedReader.Feeds;
using Newtonsoft.Json;
using NoiseCast.Core;

namespace NoiseCast.MVVM.Model
{
    public class EpisodeModel : ObservableObject
    {
        private string _id;
        private string _imagePath;
        private double _durationRemaining;
        private string _mediaPath;
        private bool _isArchived;
        private FeedItem _episode;
        private PodcastModel _parentPodcast;

        [JsonProperty("id")] public string ID => _id;
        [JsonProperty("imagePath")] public string ImagePath => _imagePath;
        [JsonProperty("duration")] public double DurationRemaining { get => _durationRemaining; set => SetProperty(ref _durationRemaining, value); }
        [JsonProperty("isArchived")] public bool IsArchived { get => _isArchived; set => SetProperty(ref _isArchived, value); }
        [JsonIgnore] public string MediaPath => _mediaPath;
        [JsonIgnore] public FeedItem Episode => _episode;
        [JsonIgnore] public PodcastModel ParentPodcast => _parentPodcast;

        /// <summary>
        /// Json-Deserialization Constructor
        /// </summary>
        [JsonConstructor]
        public EpisodeModel(string id, string imagePath, double duration, bool isArchived)
        {
            _id = id;
            _imagePath = imagePath;
            _durationRemaining = duration;
            _isArchived = isArchived;
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
        /// Set episode as archived
        /// </summary>
        public void SetIsArchived()
        {
            _isArchived = true;
            DurationRemaining = -1;
            _parentPodcast.Save();
        }

        /// <summary>
        /// Set <see cref="FeedItem"/>
        /// </summary>
        /// <param name="episode"></param>
        public void SetEpisodeFeed(FeedItem episode, PodcastModel podcastModel)
        {
            _episode = episode;
            _mediaPath = GetMediaPath();
            _parentPodcast = podcastModel;
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