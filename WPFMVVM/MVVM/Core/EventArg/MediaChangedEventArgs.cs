using CodeHollow.FeedReader;
using NoiseCast.MVVM.Model;
using System;

namespace NoiseCast.MVVM.Core
{
    public delegate void MediaChangedEventHandler(object sender, MediaChangedEventArgs e);

    public class MediaChangedEventArgs : EventArgs
    {
        public EpisodeModel Episode { get; set; }

        public MediaChangedEventArgs(EpisodeModel episode)
        {
            Episode = episode;
        }
    }
}