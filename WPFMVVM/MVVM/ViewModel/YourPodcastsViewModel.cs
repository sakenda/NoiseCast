using System;
using System.Collections.ObjectModel;
using WPFMVVM.Core;
using WPFMVVM.MVVM.Core;

namespace WPFMVVM.MVVM.ViewModel
{
    internal class YourPodcastsViewModel : ObservableObject
    {
        public ObservableCollection<Podcast> PodcastList { get; set; }

        public YourPodcastsViewModel()
        {
            PodcastList = MockData.GetPodcastMockData();
        }
    }
}