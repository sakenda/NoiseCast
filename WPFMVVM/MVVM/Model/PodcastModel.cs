using CodeHollow.FeedReader;
using CodeHollow.FeedReader.Feeds;
using Newtonsoft.Json;
using NoiseCast.Core;
using NoiseCast.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace NoiseCast.MVVM.Model
{
    [JsonObject]
    public class PodcastModel : ObservableObject
    {
        private Guid _id;
        private bool _isSubscribed;
        private int _isArchivedCount;
        private DateTime? _lastUpdatedDate;

        [JsonProperty] public string Id => _id.ToString();
        [JsonProperty] public string OriginalDocument { get; private set; }
        [JsonProperty] public string RSSLink { get; private set; }
        [JsonProperty] public ObservableCollection<EpisodeModel> Episodes { get; private set; }
        [JsonIgnore] public int IsArchivedCount { get => _isArchivedCount; set => SetProperty(ref _isArchivedCount, value); }
        [JsonIgnore] public DateTime? LastUpdatedDate { get => _lastUpdatedDate; private set => SetProperty(ref _lastUpdatedDate, value); }
        [JsonIgnore] public bool IsSubscribed { get => _isSubscribed; private set => SetProperty(ref _isSubscribed, value); }
        [JsonIgnore] public string Title { get; private set; }
        [JsonIgnore] public string Description { get; private set; }
        [JsonIgnore] public string Copyright { get; private set; }
        [JsonIgnore] public string Link { get; private set; }
        [JsonIgnore] public string ImagePath { get; private set; }
        [JsonIgnore] public string FilePath => MainViewModel.SettingsVM.AppSettings.GetPodcastPath() + _id.ToString() + ".json";

        /// <summary>
        /// Constructor for deserialized feeds with saved values
        /// </summary>
        /// <param name="id">Internal id</param>
        /// <param name="originalDocument">Feed as XML-Document</param>
        /// <param name="episodesList">List of values relevant to each episode</param>
        [JsonConstructor]
        public PodcastModel(Guid id, string originalDocument, string rSSLink, ObservableCollection<EpisodeModel> episodes)
        {
            _id = id;
            OriginalDocument = originalDocument;
            RSSLink = rSSLink;
            Episodes = episodes;
            IsSubscribed = true;
            _isArchivedCount = 0;

            Feed feed = FeedReader.ReadFromString(originalDocument);
            InitializePodcast(feed);

            int count = 0;
            Parallel.ForEach(Episodes, episode =>
            {
                FeedItem feedItem = feed.Items.FirstOrDefault(x => x.Id == episode.Id);
                episode.InitializeEpisode(feedItem, this);

                if (episode.IsArchived) _isArchivedCount++;

                Debug.WriteLine("Loaded: " + count++ + "/" + feed.Items.Count + " PODCAST: " + feed.Title);
            });
        }

        /// <summary>
        /// Constructor for new Podcasts with no ID
        /// </summary>
        /// <param name="podcast"></param>
        public PodcastModel(Feed feed, string rSSLink)
        {
            _id = Guid.Empty;
            OriginalDocument = feed.OriginalDocument;
            RSSLink = rSSLink;
            _isSubscribed = false;
            Episodes = new ObservableCollection<EpisodeModel>();

            InitializePodcast(feed);
            InitializeEpisodes(feed);
        }

        /// <summary>
        /// Initialize all podcastinfo's
        /// </summary>
        private void InitializePodcast(Feed feed)
        {
            Title = feed.Title;
            Description = feed.Description;
            ImagePath = feed.ImageUrl;
            Link = feed.Link;
            Copyright = feed.Copyright;
            LastUpdatedDate = feed.LastUpdatedDate.HasValue ? feed.LastUpdatedDate.Value : null;
        }

        /// <summary>
        /// Initializes all episodes as a EpisodeModel
        /// </summary>
        /// <param name="feed"></param>
        private void InitializeEpisodes(Feed feed)
        {
            if (feed == null) return;

            if (Episodes.Count > 0 && Episodes.Count < feed.Items.Count)
            {
                var excludedEpisodes = new HashSet<string>(Episodes.Select(p => p.Id));
                var newEpisodes = feed.Items.Where(p => !excludedEpisodes.Contains(p.Id));

                foreach (var item in newEpisodes)
                {
                    EpisodeModel episode = new EpisodeModel(item, this);
                    Episodes.Add(episode);
                }

                return;
            }

            foreach (var item in feed.Items)
            {
                if (item == null) continue;

                EpisodeModel episode = new EpisodeModel(item, this);
                Episodes.Add(episode);
            }
        }

        /// <summary>
        /// Set Path to the Enclosure URL depending on the FeedType
        /// </summary>
        public string GetMediaPath(string id)
        {
            Feed feed = FeedReader.ReadFromString(OriginalDocument);
            FeedItem feedItem = feed.Items.FirstOrDefault(x => x.Id == id);

            if (feed == null || feedItem == null) return null;

            string type = feedItem.SpecificItem.GetType().Name;

            return feed.Type switch
            {
                FeedType.Rss_2_0 => ((Rss20FeedItem)feedItem.SpecificItem).Enclosure.Url,
                FeedType.MediaRss => ((MediaRssFeedItem)feedItem.SpecificItem).Enclosure.Url,
                FeedType.Unknown => null,
                _ => null
            };
        }

        /// <summary>
        /// Sets imagepath on thee <see cref="PodcastModel"/> and all <see cref="EpisodeModel"/>
        /// </summary>
        /// <param name="path"></param>
        public void SetImagePath(string path)
        {
            ImagePath = path;

            foreach (var item in Episodes)
                item.SetImagePath(path);
        }

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
        /// Returns a <see cref="ObservableCollection{EpisodeModel}"/>. Generates a new list if no item is current attached
        /// or returns the populated list
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<EpisodeModel> GetEpisodes()
        {
            if (Episodes == null || Episodes.Count == 0) return Episodes = new ObservableCollection<EpisodeModel>();

            return Episodes;
        }

        /// <summary>
        /// Update contents from a online copy of the RSS-Content
        /// </summary>
        /// <param name="feed"></param>
        public void Update(Feed feed)
        {
            if (feed.LastUpdatedDate == LastUpdatedDate) return;

            OriginalDocument = feed.OriginalDocument;
            InitializePodcast(feed);
            InitializeEpisodes(feed);
        }
    }
}