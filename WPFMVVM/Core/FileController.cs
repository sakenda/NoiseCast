using CodeHollow.FeedReader.Feeds;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace WPFMVVM.Core
{
    public class FileController
    {
        private const string SETTINGS_PATH = @"/settings/";
        private const string SETTINGS_PODCASTURL_FILE = @"podcasts.xml";

        public async void WritePodcastsToFile(ICollection<Rss20Feed> feeds)
        {
            using FileStream fs = File.Create(SETTINGS_PATH + SETTINGS_PODCASTURL_FILE);
            foreach (Rss20Feed item in feeds)
                await JsonSerializer.SerializeAsync(fs, item);
        }
    }
}