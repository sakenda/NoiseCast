using CodeHollow.FeedReader.Feeds;
using WPFMVVM.Core;

namespace WPFMVVM.MVVM.ViewModel
{
    public class PlayerViewModel : ObservableObject
    {
        public BaseFeedItem CurrentEpisode
        {
            get => _currentEpisode;
            set
            {
                if (value.GetType() == typeof(Rss20FeedItem))
                    _url = ((Rss20FeedItem)value).Enclosure.Url;

                if (value.GetType() == typeof(MediaRssFeedItem))
                    _url = ((MediaRssFeedItem)value).Enclosure.Url;

                GetAlternativeImageLink(value);

                SetProperty(ref _currentEpisode, value);
            }
        }
        public string Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }
        public string Url
        {
            get => _url;
            set => SetProperty(ref _url, value);
        }

        private BaseFeedItem _currentEpisode;
        private string _image;
        private string _url;

        private void GetAlternativeImageLink(BaseFeedItem feed)
        {
            string iTunesImage = new CodeHollow.FeedReader.Feeds.Itunes.ItunesImage(feed.Element).Href;

            if (!string.IsNullOrEmpty(iTunesImage))
                Image = iTunesImage;
        }
    }
}