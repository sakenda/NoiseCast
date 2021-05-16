using CodeHollow.FeedReader;
using CodeHollow.FeedReader.Feeds;
using System;

namespace NoiseCast.MVVM.Model
{
    public delegate void EpisodeChangedEventHandler(EpisodeModel sender, EpisodeChangedEventArgs e);
    public class EpisodeChangedEventArgs : EventArgs
    {
        public FeedType Type { get; set; }

        public EpisodeChangedEventArgs(EpisodeModel episode)
        {
            //Type = episode.Episode.GetType().Name switch
            //{
            //    nameof(Rss20FeedItem) => FeedType.Rss_2_0,
            //    nameof(MediaRssFeedItem) => FeedType.MediaRss,
            //    _ => FeedType.Unknown
            //};
        }
    }
}