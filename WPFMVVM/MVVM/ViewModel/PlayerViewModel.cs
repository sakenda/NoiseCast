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
                _url = ((Rss20FeedItem)value).Enclosure.Url;
                _image = new CodeHollow.FeedReader.Feeds.Itunes.ItunesImage(value.Element).Href;
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
    }
}