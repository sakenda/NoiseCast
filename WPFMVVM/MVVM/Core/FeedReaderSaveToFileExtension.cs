﻿using CodeHollow.FeedReader.Feeds;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace WPFMVVM.MVVM.Core
{
    public static class FeedReaderSaveToFileExtension
    {
        public static void SaveToFile(this BaseFeed feed)
        {
            string fileName = ApplicationSettings.SETTINGS_PODCAST_PATH + feed.Title + ".xml";

            if (!File.Exists(fileName))
            {
                using FileStream fs = File.OpenWrite(fileName);

                var content = feed.OriginalDocument;
                byte[] bytes = Encoding.UTF8.GetBytes(content);

                fs.Write(bytes, 0, bytes.Length);
            }

            //XDocument xdoc = XDocument.Parse(feed.OriginalDocument);
            //xdoc.Root.Save(fs);
        }

        private static Guid GetGuid()
        {
            return new Guid();
        }
    }
}