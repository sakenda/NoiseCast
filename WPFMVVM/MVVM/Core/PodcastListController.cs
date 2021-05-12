using CodeHollow.FeedReader;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using WPFMVVM.MVVM.Model;

namespace WPFMVVM.MVVM.Core
{
    public class PodcastListController
    {
        #region "Static Members"
        private static ObservableCollection<Feed> _feeds;
        static PodcastListController()
        {
            _feeds = new ObservableCollection<Feed>();
        }
        #endregion "Static Members"

        #region "Instance Members"
        private List<SerializableFeedModel> _sFeeds;
        private FeedSerialization _serialization;

        public ObservableCollection<Feed> PodcastList
        {
            get => _feeds;
            private set => _feeds = value;
        }

        public PodcastListController()
        {
            _serialization = new FeedSerialization();
            _sFeeds = new List<SerializableFeedModel>();
        }

        public void GetSubscribedFeeds()
        {
            _sFeeds = _serialization.Deserialize();

            foreach (var feed in _serialization.Deserialize())
                PodcastList.Add(feed.FeedObj);
        }

        public bool AddFeed(string url)
        {
            Feed feed = null;

            try
            {
                feed = Task.Run(() => FeedReader.ReadAsync(url)).Result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: Could not load Feed from:" + url, ex);
            }

            if (feed == null || _feeds.Contains(feed)) return false;

            PodcastList.Add(feed);
            _sFeeds.Add(new SerializableFeedModel(feed));

            return true;
        }

        public bool RemoveFeed(Feed feed)
        {
            if (feed == null) return false;
            if (!_feeds.Contains(feed)) return false;

            PodcastList.Remove(feed);

            foreach (var item in _sFeeds)
                if (item.FeedObj.Link == feed.Link) _sFeeds.Remove(item);

            return true;
        }

        public bool CheckFeedIsSaved(Feed feed)
        {
            if (feed == null) return false;

            foreach (var item in _sFeeds)
            {
                if (item.FeedObj.Link != feed.Link) continue;

                if (item.GetID() == Guid.Empty.ToString()) return false;

                return true;
            }

            return false;
        }

        public SerializableFeedModel GetSerializableFeedModel(Feed feed)
        {
            foreach (var item in _sFeeds)
            {
                if (item.FeedObj.Link != feed.Link) continue;

                return item;
            }

            _sFeeds.Add(new SerializableFeedModel(feed));

            return _sFeeds[_sFeeds.Count - 1];
        }

        public void SaveFeed(Feed feed)
        {
            _serialization.Serialize(GetSerializableFeedModel(feed));
        }

        public void UpdateFeedList()
        {
            try
            {
                Parallel.ForEach(_sFeeds, feed =>
                {
                    try
                    {
                        feed.UpdateFeed(
                            Task.Run(() =>
                            {
                                return FeedReader.ReadAsync(feed.FeedObj.Link);
                            }).Result
                        );
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error: Could not update feed from:" + feed.FeedObj.Link, ex);
                    }

                    Debug.WriteLine($"Done: {feed.FeedObj.Title} - {feed.FeedObj.Items.Count} episodes updated.");
                });
            }
            catch (AggregateException)
            {
                throw;
            }
        }

        #endregion "Instance Members"
    }
}