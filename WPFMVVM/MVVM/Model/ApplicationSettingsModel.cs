using Newtonsoft.Json;
using NoiseCast.Core;
using NoiseCast.MVVM.ViewModel;
using NoiseCast.MVVM.ViewModel.Controller;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Threading;

namespace NoiseCast.MVVM.Model
{
    public class ApplicationSettingsModel : ObservableObject
    {
        private const string _settingsImagePath = @"\images\";
        private const string _settingsPodcastPath = @"\podcasts\";

        private Stopwatch _stopWatch;
        private DispatcherTimer _timer;

        private string _settingsPathMain;
        private PlayerSessionModel _playerSession;
        private double _updateInterval;

        public string SettingsPathMain { get => _settingsPathMain; set => SetProperty(ref _settingsPathMain, value); }
        public PlayerSessionModel PlayerSession { get => _playerSession; set => SetProperty(ref _playerSession, value); }
        public double UpdateInterval
        {
            get => _updateInterval;
            set
            {
                _timer.Stop();
                _timer.Interval = TimeSpan.FromMinutes(value);
                _timer.Start();

                SetProperty(ref _updateInterval, value);
                SessionSerialization.Serialize(this);
            }
        }

        public ApplicationSettingsModel()
        {
            InitializeTimer();

            _settingsPathMain = AppDomain.CurrentDomain.BaseDirectory;
            _playerSession = new PlayerSessionModel(null, 0.5, 30);
            _updateInterval = TimeSpan.FromMinutes(20).TotalMinutes;

            Directory.CreateDirectory(SettingsPathMain);
            Directory.CreateDirectory(GetPodcastPath());
            Directory.CreateDirectory(GetImagePath());
        }

        [JsonConstructor]
        public ApplicationSettingsModel(string settingsPathMain, PlayerSessionModel playerSession, double updateInterval)
        {
            _settingsPathMain = string.IsNullOrWhiteSpace(settingsPathMain) ? AppDomain.CurrentDomain.BaseDirectory : settingsPathMain;
            _playerSession = playerSession;
            _updateInterval = updateInterval;

            InitializeTimer(_updateInterval);

            Directory.CreateDirectory(SettingsPathMain);
            Directory.CreateDirectory(GetPodcastPath());
            Directory.CreateDirectory(GetImagePath());
        }
        /// <summary>
        /// Update all podcasts by the predefined <see cref="UpdateInterval"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTimerTick(object sender, EventArgs e)
        {
            // Update Podcastlist
            Debug.WriteLine("UPDATE: " + TimeSpan.FromMilliseconds(_stopWatch.ElapsedMilliseconds));
            foreach (var item in MainViewModel.PodcastsList)
                PodcastListController.UpdateFeed(item);

            _stopWatch.Restart();
        }

        /// <summary>
        /// Initializes the required <see cref="Stopwatch"/> and <see cref="DispatcherTimer"/>.
        /// If no timer interval is set, the default is 20 minutes
        /// </summary>
        /// <param name="interval"></param>
        private void InitializeTimer(double interval = 0)
        {
            _timer = new DispatcherTimer();
            _timer.Interval = interval == 0 ? TimeSpan.FromMinutes(20) : TimeSpan.FromMinutes(interval);
            _timer.Tick += OnTimerTick;
            _timer.Start();

            _stopWatch = new Stopwatch();
            _stopWatch.Start();
        }

        public string GetImagePath() => _settingsPathMain + _settingsImagePath;
        public string GetPodcastPath() => _settingsPathMain + _settingsPodcastPath;
    }
}