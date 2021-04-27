using System;
using System.Collections.ObjectModel;

namespace WPFMVVM.MVVM.Core
{
    public class Podcast
    {
        public string Titel { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        public string Image { get; set; }
        public string Website { get; set; }
        public DateTime LastUpdated { get; set; }
        public int EpisodesCount => EpisodesList == null ? default : EpisodesList.Count;
        public ObservableCollection<Episode> EpisodesList { get; set; }

        public Podcast(string titel, string publisher, string image, string website, ObservableCollection<Episode> episodesList, DateTime lastUpdated, string description)
        {
            Titel = titel;
            Publisher = publisher;
            Image = image;
            Website = website;
            EpisodesList = episodesList;
            LastUpdated = lastUpdated;
            Description = description;
        }
    }
}