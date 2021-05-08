using CodeHollow.FeedReader;
using CodeHollow.FeedReader.Feeds;
using System;
using WPFMVVM.Core;

namespace WPFMVVM.MVVM.Model
{
    public delegate void EpisodeChangedEventHandler(Episode sender, EpisodeChangedEventArgs e);
    public class EpisodeChangedEventArgs : EventArgs
    {
        public FeedType Type { get; set; }

        public EpisodeChangedEventArgs(Episode episode)
        {
            Type = episode.EpisodeFeed.GetType().Name switch
            {
                nameof(Rss20FeedItem) => FeedType.Rss_2_0,
                nameof(MediaRssFeedItem) => FeedType.MediaRss,
                _ => FeedType.Unknown
            };
        }
    }

    public class Episode : ObservableObject
    {
        public event EpisodeChangedEventHandler EpisodeChanged;

        private BaseFeedItem _episodeFeed;
        private string _imageUrl;
        private string _mediaUrl;

        public BaseFeedItem EpisodeFeed
        {
            get => _episodeFeed;
            private set => SetProperty(ref _episodeFeed, value);
        }
        public string ImageUrl
        {
            get => _imageUrl;
            private set => SetProperty(ref _imageUrl, value);
        }
        public string MediaUrl
        {
            get => _mediaUrl;
            private set => SetProperty(ref _mediaUrl, value);
        }

        public Episode()
        {
        }

        public void ChangeEpisode(BaseFeedItem episodeFeed, string mediaUrl, string imageUrl = null)
        {
            ImageUrl = imageUrl;
            EpisodeFeed = episodeFeed;
            MediaUrl = mediaUrl;
            OnEpisodeChanged(new EpisodeChangedEventArgs(this));
        }

        public virtual void OnEpisodeChanged(EpisodeChangedEventArgs e) => EpisodeChanged?.Invoke(this, e);
    }
}