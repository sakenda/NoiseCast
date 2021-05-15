using CodeHollow.FeedReader;
using Newtonsoft.Json;
using System;

namespace NoiseCast.MVVM.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class SerializableFeedModel
    {
        [JsonProperty("id")]
        private Guid _ID;
        [JsonProperty("originalDocument")]
        private string _originalDocument => _feedObj.OriginalDocument;

        private Feed _feedObj;

        public bool IsSaved => _ID != Guid.Empty;
        public Feed FeedObj => _feedObj;

        public SerializableFeedModel(Feed feedObj) : this(feedObj, Guid.Empty)
        {
        }
        public SerializableFeedModel(Feed feedObj, Guid id)
        {
            _ID = id;
            _feedObj = feedObj;
        }

        [JsonConstructor]
        public SerializableFeedModel(string originalDocument, Guid id)
        {
            _ID = id;
            _feedObj = FeedReader.ReadFromString(originalDocument);
        }

        public string GetID() => _ID.ToString();
        public bool SetID()
        {
            if (_ID != Guid.Empty) return false;

            _ID = Guid.NewGuid();
            return true;
        }
        public void UpdateFeed(Feed feed) => _feedObj = feed;
    }
}