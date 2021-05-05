using CodeHollow.FeedReader.Feeds;
using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WPFMVVM.Core;

namespace WPFMVVM.MVVM.ViewModel
{
    public class PlayerViewModel : ObservableObject
    {
        private MediaElement _mediaElement;

        private BaseFeedItem _currentEpisode;
        private string _image;
        private string _url;
        private double _skipAmount;

        public MediaElement MediaElement
        {
            get => _mediaElement;
            set => SetProperty(ref _mediaElement, value);
        }

        public BaseFeedItem CurrentEpisode
        {
            get => _currentEpisode;
            set => SetProperty(ref _currentEpisode, value);
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
        public double SkipAmount
        {
            get => _skipAmount;
            set => SetProperty(ref _skipAmount, value);
        }

        public ICommand RewindCommand { get; private set; }
        public ICommand SkipCommand { get; private set; }
        public ICommand PlayCommand { get; private set; }
        public ICommand NextCommand { get; private set; }
        public ICommand LastCommand { get; private set; }

        public PlayerViewModel()
        {
            RewindCommand = new RelayCommand(RewindExecuted, RewindCanExecute);
            SkipCommand = new RelayCommand(SkipExecuted, SkipCanExecute);
            PlayCommand = new RelayCommand(PlayExecuted, PlayCanExecute);
            NextCommand = new RelayCommand(NextExecuted, NextCanExecute);
            LastCommand = new RelayCommand(LastExecuted, LastCanExecute);

            _mediaElement = new MediaElement();
            _mediaElement.LoadedBehavior = MediaState.Manual;
            _mediaElement.UnloadedBehavior = MediaState.Manual;

            _skipAmount = 30;
        }

        public void SetCurrentEpisode(Rss20FeedItem episode, string imageUrl)
        {
            CurrentEpisode = episode;
            Url = episode.Enclosure.Url;
            Image = imageUrl;

            MediaElement.Source = new Uri(_url);
        }
        public void SetCurrentEpisode(MediaRssFeedItem episode, string imageUrl)
        {
            CurrentEpisode = episode;
            Url = episode.Enclosure.Url;
            Image = imageUrl;

            MediaElement.Source = new Uri(_url);
        }

        private bool LastCanExecute(object arg) => false;
        private void LastExecuted(object obj) => throw new NotImplementedException();

        private bool NextCanExecute(object arg) => false;
        private void NextExecuted(object obj) => throw new NotImplementedException();

        private bool PlayCanExecute(object arg) => true;
        private void PlayExecuted(object obj)
        {
            if (_mediaElement.CanPause)
            {
                _mediaElement.Play();
                return;
            }

            _mediaElement.Pause();
        }

        private bool SkipCanExecute(object arg) => _mediaElement.Source != null && _mediaElement.Position > TimeSpan.Zero;
        private void SkipExecuted(object obj) => _mediaElement.Position += TimeSpan.FromSeconds(_skipAmount);

        private bool RewindCanExecute(object arg) => _mediaElement.Source != null && _mediaElement.Position < _mediaElement.NaturalDuration;
        private void RewindExecuted(object obj) => _mediaElement.Position -= TimeSpan.FromSeconds(_skipAmount);
    }
}