using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using NoiseCast.Core;
using NoiseCast.MVVM.Model;

namespace NoiseCast.MVVM.ViewModel
{
    public class PlayerViewModel : ObservableObject
    {
        private ApplicationSettingsModel _appSettings;
        private MediaElement _mediaElement;
        private DispatcherTimer _timer;
        private EpisodeModel _currentEpisode;
        private double _position;
        private double _positionMaximum;
        private double _tempVolume;
        private int _tickCounter;

        public ApplicationSettingsModel AppSettings => _appSettings;
        public DispatcherTimer Timer => _timer;
        public MediaElement MediaElement { get => _mediaElement; set => SetProperty(ref _mediaElement, value); }
        public EpisodeModel CurrentEpisode { get => _currentEpisode; set => SetProperty(ref _currentEpisode, value); }
        public double PositionMaximum { get => _positionMaximum; set => SetProperty(ref _positionMaximum, value); }
        public double Position
        {
            get => _position;
            set
            {
                // On changes higher than 1 second tick
                if (value != _position + 1)
                    MediaElement.Position = TimeSpan.FromSeconds(value);

                SetProperty(ref _position, value);
            }
        }

        public ICommand RewindCommand { get; private set; }
        public ICommand SkipCommand { get; private set; }
        public ICommand PlayCommand { get; private set; }
        public ICommand NextCommand { get; private set; }
        public ICommand LastCommand { get; private set; }
        public ICommand MuteCommand { get; private set; }

        public PlayerViewModel()
        {
            _appSettings = MainViewModel.ApplicationSettings;

            RewindCommand = new RelayCommand(RewindExecuted, RewindCanExecute);
            SkipCommand = new RelayCommand(SkipExecuted, SkipCanExecute);
            PlayCommand = new RelayCommand(PlayExecuted, PlayCanExecute);
            NextCommand = new RelayCommand(NextExecuted, NextCanExecute);
            LastCommand = new RelayCommand(LastExecuted, LastCanExecute);
            MuteCommand = new RelayCommand(MuteExecuted);

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += OnTimerTick;
            _tickCounter = 0;

            _mediaElement = new MediaElement();
            _mediaElement.LoadedBehavior = MediaState.Manual;
            _mediaElement.UnloadedBehavior = MediaState.Manual;
            _mediaElement.MediaOpened += MediaElement_MediaOpened;

            _appSettings.PlayerSession.PropertyChanged += PlayerSession_PropertyChanged;
        }

        /// <summary>
        /// Listen to changed properties, such as volume and sets it inside the player
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayerSession_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ApplicationSettingsModel.PlayerSession.PlayerVolume))
            {
                var session = (PlayerSessionModel)sender;
                SetVolume(session.PlayerVolume);
            }
        }

        /// <summary>
        /// Setup sessionvalues on startup
        /// </summary>
        /// <param name="session">The deserialized values</param>
        public void InitializeSession(PlayerSessionModel session)
        {
            if (string.IsNullOrWhiteSpace(session.LastSelectedID[0]) || string.IsNullOrWhiteSpace(session.LastSelectedID[1])) return;

            var podcast = MainViewModel.PodcastsList.FirstOrDefault(x => x.Id == session.LastSelectedID[0]);
            var episode = podcast.Episodes.FirstOrDefault(x => x.Id == session.LastSelectedID[1]);

            SetEpisode(episode);
        }

        /// <summary>
        /// Set episode and mediaelement source
        /// </summary>
        /// <param name="episode"></param>
        public void SetEpisode(EpisodeModel episode)
        {
            if (episode == null) return;
            CurrentEpisode = episode;
            MediaElement.Source = new Uri(episode.MediaPath);

            // To trigger MediaOpened
            _mediaElement.Play();
            _mediaElement.Stop();
        }

        /// <summary>
        /// Set Volume
        /// </summary>
        /// <param name="value">A value between 0 and 1</param>
        private void SetVolume(double value) => _mediaElement.Volume = value;

        /// <summary>
        /// Timer for mediaplayer. If the remaining duration is lower than 1, episode will
        /// set archived and player position set to 0.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTimerTick(object sender, EventArgs e)
        {
            AutoSave();
            if (CheckDurationFull()) return;

            Position++;
            _currentEpisode.DurationRemaining = _positionMaximum - _position;
        }

        /// <summary>
        /// Checks if <see cref="_tickCounter"/> is dividable by 60 (one minute) and saves podcast.
        /// </summary>
        private void AutoSave()
        {
            if (_tickCounter++ % 60 == 0) FeedSerialization.Serialize(MainViewModel.PodcastsList);
        }

        /// <summary>
        /// When Episodes duration ist full, set episode as IsArchived and stop timer and player
        /// </summary>
        /// <returns>True if duration is full</returns>
        private bool CheckDurationFull()
        {
            if (_currentEpisode.DurationRemaining < 1)
            {
                if (!_currentEpisode.IsArchived) _currentEpisode.SetIsArchived();

                Position = 0;

                _timer.Stop();
                _mediaElement.Stop();

                return true;
            }

            return false;
        }

        /// <summary>
        /// Executes after Media is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            PositionMaximum = _mediaElement.NaturalDuration.TimeSpan.TotalSeconds;

            if (_currentEpisode.DurationRemaining <= 0)
            {
                Position = 0;
                _currentEpisode.DurationRemaining = _positionMaximum;
            }
            else Position = _positionMaximum - _currentEpisode.DurationRemaining;

            MediaElement.Position = TimeSpan.FromSeconds(_position);
        }

        private void LastExecuted(object obj) => throw new NotImplementedException();
        private bool LastCanExecute(object arg) => false;

        private void NextExecuted(object obj) => throw new NotImplementedException();
        private bool NextCanExecute(object arg) => false;

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
        private bool PlayCanExecute(object arg) => _mediaElement.Source != null;

        private void SkipExecuted(object obj)
        {
            TimeSpan targetTime = _mediaElement.Position + TimeSpan.FromSeconds(_appSettings.PlayerSession.SkipValue);

            if (targetTime > _mediaElement.NaturalDuration.TimeSpan)
            {
                _mediaElement.Position = _mediaElement.NaturalDuration.TimeSpan;
                Position = _mediaElement.Position.TotalSeconds;
                return;
            }

            _mediaElement.Position = targetTime;
            Position = targetTime.TotalSeconds;
        }
        private bool SkipCanExecute(object arg) => _mediaElement.Source != null;

        private void RewindExecuted(object obj)
        {
            TimeSpan targetTime = _mediaElement.Position - TimeSpan.FromSeconds(_appSettings.PlayerSession.SkipValue);

            if (targetTime <= TimeSpan.Zero)
            {
                _mediaElement.Position = TimeSpan.Zero;
                Position = 0;
                return;
            }

            _mediaElement.Position = targetTime;
            Position = targetTime.TotalSeconds;
        }
        private bool RewindCanExecute(object arg) => _mediaElement.Source != null;

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