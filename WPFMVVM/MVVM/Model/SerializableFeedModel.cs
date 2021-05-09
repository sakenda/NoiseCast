using System;

namespace WPFMVVM.MVVM.Model
{
    public class SerializableFeedModel
    {
        private Guid _id = Guid.Empty;
        private string _feedUrl;

        public Guid Id
        {
            get => _id;
            private set => _id = value;
        }
        public string FeedUrl
        {
            get => _feedUrl;
            private set => _feedUrl = value;
        }

        public SerializableFeedModel(string feedDocument, Guid guid = new Guid())
        {
            _id = guid == Guid.Empty ? GenerateNewGuid() : guid;
            _feedUrl = feedDocument;
        }

        private Guid GenerateNewGuid()
        {
            return new Guid();
        }
    }
}