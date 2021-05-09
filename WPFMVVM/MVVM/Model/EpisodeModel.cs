using CodeHollow.FeedReader;
using CodeHollow.FeedReader.Feeds;
using System;
using WPFMVVM.Core;

namespace WPFMVVM.MVVM.Model
{
    public delegate void EpisodeChangedEventHandler(EpisodeModel sender, EpisodeChangedEventArgs e);
    public class EpisodeChangedEventArgs : EventArgs
    {
        public FeedType Type { get; set; }

        public EpisodeChangedEventArgs(EpisodeModel episode)
        {
            Type = episode.EpisodeFeed.GetType().Name switch
            {
                nameof(Rss20FeedItem) => FeedType.Rss_2_0,
                nameof(MediaRssFeedItem) => FeedType.MediaRss,
                _ => FeedType.Unknown
            };
        }
    }

    public class EpisodeModel : ObservableObject
    {
        public event EpisodeChangedEventHandler EpisodeChanged;

        private FeedItem _episodeFeed;
        private string _imageUrl;
        private string _mediaUrl;

        public FeedItem EpisodeFeed
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

        public EpisodeModel()
        {
        }

        public void ChangeEpisode(FeedItem episodeFeed, string mediaUrl, string imageUrl = null)
        {
            ImageUrl = imageUrl;
            EpisodeFeed = episodeFeed;
            MediaUrl = mediaUrl;
            OnEpisodeChanged(new EpisodeChangedEventArgs(this));
        }

        public virtual void OnEpisodeChanged(EpisodeChangedEventArgs e) => EpisodeChanged?.Invoke(this, e);
    }
}