using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using WPFMVVM.Core;
using WPFMVVM.MVVM.Model;

namespace WPFMVVM.MVVM.ViewModel
{
    public class PlayerViewModel : ObservableObject
    {
        private MediaElement _mediaElement;
        private DispatcherTimer _timer;
        private Episode _currentEpisode;
        private double _skipAmount;
        private double _position;
        private double _positionMaximum;
        private double _tempVolume;

        public MediaElement MediaElement
        {
            get => _mediaElement;
            set => SetProperty(ref _mediaElement, value);
        }
        public Episode CurrentEpisode
        {
            get => _currentEpisode;
            set => SetProperty(ref _currentEpisode, value);
        }
        public double SkipAmount
        {
            get => _skipAmount;
            set => SetProperty(ref _skipAmount, value);
        }
        public double Position
        {
            get => _position;
            set
            {
                if (value != _position + 1)
                    MediaElement.Position = TimeSpan.FromSeconds(value);

                SetProperty(ref _position, value);
            }
        }
        public double PositionMaximum
        {
            get => _positionMaximum;
            set => SetProperty(ref _positionMaximum, value);
        }

        public ICommand RewindCommand { get; private set; }
        public ICommand SkipCommand { get; private set; }
        public ICommand PlayCommand { get; private set; }
        public ICommand NextCommand { get; private set; }
        public ICommand LastCommand { get; private set; }
        public ICommand MuteCommand { get; private set; }

        public PlayerViewModel()
        {
            RewindCommand = new RelayCommand(RewindExecuted, RewindCanExecute);
            SkipCommand = new RelayCommand(SkipExecuted, SkipCanExecute);
            PlayCommand = new RelayCommand(PlayExecuted, PlayCanExecute);
            NextCommand = new RelayCommand(NextExecuted, NextCanExecute);
            LastCommand = new RelayCommand(LastExecuted, LastCanExecute);
            MuteCommand = new RelayCommand(MuteExecuted, MuteCanExecute);

            _currentEpisode = new Episode();
            _currentEpisode.EpisodeChanged += _currentEpisode_EpisodeChanged;

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Stop();
            _timer.Tick += OnTimerTick;

            _mediaElement = new MediaElement();
            _mediaElement.LoadedBehavior = MediaState.Manual;
            _mediaElement.UnloadedBehavior = MediaState.Manual;
            _mediaElement.MediaOpened += MediaElement_MediaOpened;

            _skipAmount = 30;
            _mediaElement.Volume = .2;
        }

        private void OnTimerTick(object sender, EventArgs e) => Position++;
        private void MediaElement_MediaOpened(object sender, System.Windows.RoutedEventArgs e)
        {
            Position = 0;
            PositionMaximum = _mediaElement.NaturalDuration.TimeSpan.TotalSeconds;
        }
        private void _currentEpisode_EpisodeChanged(Episode sender, EpisodeChangedEventArgs e)
        {
            MediaElement.Source = new Uri(sender.MediaUrl);
            _mediaElement.Pause();
        }

        private bool LastCanExecute(object arg) => false;
        private void LastExecuted(object obj) => throw new NotImplementedException();

        private bool NextCanExecute(object arg) => false;
        private void NextExecuted(object obj) => throw new NotImplementedException();

        private bool PlayCanExecute(object arg) => _mediaElement.Source != null;
        private void PlayExecuted(object obj)
        {
            if (_timer.IsEnabled)
            {
                _timer.Stop();
                _mediaElement.Pause();
                return;
            }

            _timer.Start();
            _mediaElement.Play();
        }

        private bool SkipCanExecute(object arg) => _mediaElement.Source != null;
        private void SkipExecuted(object obj)
        {
            TimeSpan targetTime = _mediaElement.Position + TimeSpan.FromSeconds(_skipAmount);

            if (targetTime > _mediaElement.NaturalDuration.TimeSpan)
            {
                _mediaElement.Position = _mediaElement.NaturalDuration.TimeSpan;
                Position = _mediaElement.Position.TotalSeconds;
                return;
            }

            _mediaElement.Position = targetTime;
            Position = targetTime.TotalSeconds;
        }

        private bool RewindCanExecute(object arg) => _mediaElement.Source != null;
        private void RewindExecuted(object obj)
        {
            TimeSpan targetTime = _mediaElement.Position - TimeSpan.FromSeconds(_skipAmount);

            if (targetTime <= TimeSpan.Zero)
            {
                _mediaElement.Position = TimeSpan.Zero;
                Position = 0;
                return;
            }

            _mediaElement.Position = targetTime;
            Position = targetTime.TotalSeconds;
        }

        private bool MuteCanExecute(object arg) => true;
        private void MuteExecuted(object obj)
        {
            if (_mediaElement.Volume > 0)
            {
                _tempVolume = _mediaElement.Volume;
                _mediaElement.Volume = 0;
                return;
            }

            _mediaElement.Volume = _tempVolume;
        }
    }
}