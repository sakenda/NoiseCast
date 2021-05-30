using CodeHollow.FeedReader;
using Newtonsoft.Json;
using NoiseCast.Core;
using System;

namespace NoiseCast.MVVM.Model
{
    public class EpisodeModel : ObservableObject
    {
        private double _durationRemaining;
        private bool _isArchived;
        private string _title;
        private string _description;
        private string _author;
        private DateTime? _publishingDate;
        private string _link;

        public string Id { get; private set; }
        public bool IsArchived { get => _isArchived; set => SetProperty(ref _isArchived, value); }
        public double DurationRemaining { get => _durationRemaining; set => SetProperty(ref _durationRemaining, value); }
        [JsonIgnore] public PodcastModel ParentPodcastModel { get; set; }
        [JsonIgnore] public string ImagePath { get; private set; }
        [JsonIgnore] public string MediaPath { get; private set; }
        [JsonIgnore] public string Title { get => _title; set => SetProperty(ref _title, value); }
        [JsonIgnore] public string Description { get => _description; set => SetProperty(ref _description, value); }
        [JsonIgnore] public string Author { get => _author; set => SetProperty(ref _author, value); }
        [JsonIgnore] public DateTime? PublishingDate { get => _publishingDate; set => SetProperty(ref _publishingDate, value); }
        [JsonIgnore] public string Link { get => _link; set => SetProperty(ref _link, value); }

        /// <summary>
        /// Constructor for json deserializiation
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isArchived"></param>
        /// <param name="durationRemaining"></param>
        [JsonConstructor]
        public EpisodeModel(string id, bool isArchived, double durationRemaining)
        {
            Id = id;
            IsArchived = isArchived;
            DurationRemaining = durationRemaining;
        }

        /// <summary>
        /// New episode constructor
        /// </summary>
        public EpisodeModel(FeedItem feedItem, PodcastModel podcast)
        {
            if (feedItem == null || podcast == null) return;

            Id = feedItem.Id;
            _durationRemaining = 0;
            _isArchived = false;

            ParentPodcastModel = podcast;
            ImagePath = podcast.ImagePath;
            MediaPath = podcast.GetMediaPath(Id);

            InitializeEpisode(feedItem);
        }

        /// <summary>
        /// Initializes episode properties
        /// </summary>
        /// <param name="feedItem"></param>
        public void InitializeEpisode(FeedItem feedItem, PodcastModel podcast = null)
        {
            if (feedItem == null) return;

            _title = feedItem.Title;
            _description = feedItem.Description;
            _author = feedItem.Author;
            _publishingDate = feedItem.PublishingDate;
            _link = feedItem.Link;

            if (podcast != null)
            {
                ParentPodcastModel = podcast;
                MediaPath = podcast.GetMediaPath(Id);
                ImagePath = podcast.ImagePath;
            }
        }

        /// <summary>
        /// Set episode as archived
        /// </summary>
        public void SetIsArchived()
        {
            _isArchived = true;
            DurationRemaining = -1;
        }

        /// <summary>
        /// Sets imagepath
        /// </summary>
        /// <param name="path"></param>
        public void SetImagePath(string path) => ImagePath = path;
    }
}