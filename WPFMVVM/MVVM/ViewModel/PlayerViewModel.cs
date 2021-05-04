using CodeHollow.FeedReader.Feeds;
using System;
using System.Windows.Input;
using System.Windows.Media;
using WPFMVVM.Core;

namespace WPFMVVM.MVVM.ViewModel
{
    public class PlayerViewModel : ObservableObject
    {
        private MediaPlayer _mediaPlayer = new();

        private BaseFeedItem _currentEpisode;
        private string _image;
        private string _url;
        private TimeSpan _currentPosition;
        private TimeSpan _totalLength;
        private double _skipAmount;

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
        public TimeSpan CurrentPosition
        {
            get => _currentPosition;
            set => SetProperty(ref _currentPosition, value);
        }
        public TimeSpan TotalLength
        {
            get => _totalLength;
            set => SetProperty(ref _totalLength, value);
        }
        public double SkipAmount
        {
            get => _skipAmount;
            set => SetProperty(ref _skipAmount, value);
        }

        public ICommand RewindCommand { get; private set; }
        public ICommand SkipCommand { get; private set; }
        public ICommand PlayCommand { get; private set; }
        public ICommand PauseCommand { get; private set; }
        public ICommand NextCommand { get; private set; }
        public ICommand LastCommand { get; private set; }

        public PlayerViewModel()
        {
            RewindCommand = new RelayCommand(RewindExecuted, RewindCanExecute);
            SkipCommand = new RelayCommand(SkipExecuted, SkipCanExecute);
            PlayCommand = new RelayCommand(PlayExecuted, PlayCanExecute);
            PauseCommand = new RelayCommand(PauseExecuted, PauseCanExecute);
            NextCommand = new RelayCommand(NextExecuted, NextCanExecute);
            LastCommand = new RelayCommand(LastExecuted, LastCanExecute);

            _skipAmount = 30;
        }

        public void SetCurrentEpisode(Rss20FeedItem episode, string imageUrl)
        {
            CurrentEpisode = episode;
            Url = episode.Enclosure.Url;
            Image = imageUrl;
            CurrentPosition = TimeSpan.Zero;
            TotalLength = GetTotalLength(_url);
        }
        public void SetCurrentEpisode(MediaRssFeedItem episode, string imageUrl)
        {
            CurrentEpisode = episode;
            Url = episode.Enclosure.Url;
            Image = imageUrl;
            CurrentPosition = TimeSpan.Zero;
            TotalLength = GetTotalLength(_url);
        }

        private TimeSpan GetTotalLength(string file)
        {
            _mediaPlayer.Open(new Uri(file));

            if (_mediaPlayer.NaturalDuration.HasTimeSpan)
                return _mediaPlayer.NaturalDuration.TimeSpan;

            return TimeSpan.Zero;
        }

        private bool LastCanExecute(object arg) => false;
        private void LastExecuted(object obj) => throw new NotImplementedException();

        private bool NextCanExecute(object arg) => false;
        private void NextExecuted(object obj) => throw new NotImplementedException();

        private bool PauseCanExecute(object arg) => false;
        private void PauseExecuted(object obj) => throw new NotImplementedException();

        private bool PlayCanExecute(object arg) => false;
        private void PlayExecuted(object obj) => throw new NotImplementedException();

        private bool SkipCanExecute(object arg) => _currentEpisode != null && _currentPosition > TimeSpan.Zero;
        private void SkipExecuted(object obj) => _currentPosition += TimeSpan.FromSeconds(_skipAmount);

        private bool RewindCanExecute(object arg) => _currentEpisode != null && _currentPosition < _totalLength;
        private void RewindExecuted(object obj) => _currentPosition -= TimeSpan.FromSeconds(_skipAmount);
    }
}