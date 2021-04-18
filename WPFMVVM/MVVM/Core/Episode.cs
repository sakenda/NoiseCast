using System;

namespace WPFMVVM.MVVM.Core
{
    internal class Episode
    {
        public string Titel { get; set; }
        public DateTime Release { get; set; }
        public string Description { get; set; }
        public string AudioFile { get; set; }

        public Episode(string titel, DateTime release, string description, string audioFile)
        {
            Titel = titel;
            Release = release;
            Description = description;
            AudioFile = audioFile;
        }
    }
}