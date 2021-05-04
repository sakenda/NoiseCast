using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using WPFMVVM.MVVM.ViewModel;

namespace WPFMVVM.MVVM.View
{
    public partial class PlayerView : UserControl
    {
        private PlayerViewModel _playerVM = MainViewModel.PlayerVM;
        private DispatcherTimer _timer = new DispatcherTimer()
        {
            Interval = TimeSpan.FromSeconds(1)
        };

        private bool _isSliderDragging = false;

        public PlayerView()
        {
            InitializeComponent();

            _timer.Tick += timer_Tick;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if ((me_player.Source != null) && (me_player.NaturalDuration.HasTimeSpan) && (!_isSliderDragging))
            {
                sli_progress.Minimum = 0;
                sli_progress.Maximum = me_player.NaturalDuration.TimeSpan.TotalSeconds;
                sli_progress.Value = me_player.Position.TotalSeconds;
            }
        }

        private void sli_Progress_DragStarted(object sender, DragStartedEventArgs e) => _isSliderDragging = true;
        private void sli_Progress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) => _playerVM.CurrentPosition = TimeSpan.FromSeconds(me_player.Position.TotalSeconds);
        private void sli_Progress_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            _isSliderDragging = false;
            me_player.Position = TimeSpan.FromSeconds(sli_progress.Value);
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            me_player.Source = me_player.Source != new Uri(_playerVM.Url)
                               ? new Uri(_playerVM.Url)
                               : me_player.Source;

            if (_timer.IsEnabled)
            {
                me_player.Pause();
                _timer.Stop();
                return;
            }

            me_player.Play();
            _timer.Start();
        }
    }
}