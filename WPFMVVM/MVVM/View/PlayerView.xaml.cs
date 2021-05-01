using CodeHollow.FeedReader.Feeds;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using WPFMVVM.MVVM.ViewModel;

namespace WPFMVVM.MVVM.View
{
    public partial class PlayerView : UserControl
    {
        private bool mediaPlayerIsPlaying = false;
        private bool userIsDraggingSlider = false;

        public PlayerView()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Clock zum zählen der sekunden (Playbacktimer)
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);           // Intervall setzen
            timer.Tick += timer_Tick;                           // Event auf den Intervall
            timer.Start();                                      // Intervall-Timer starten
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if ((me_player.Source != null) && (me_player.NaturalDuration.HasTimeSpan) && (!userIsDraggingSlider))
            {
                sli_progress.Minimum = 0;
                sli_progress.Maximum = me_player.NaturalDuration.TimeSpan.TotalSeconds;
                sli_progress.Value = me_player.Position.TotalSeconds;
            }
        }

        private void sliProgress_DragStarted(object sender, DragStartedEventArgs e)
        {
            userIsDraggingSlider = true;
        }
        private void sliProgress_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            userIsDraggingSlider = false;
            me_player.Position = TimeSpan.FromSeconds(sli_progress.Value);
        }
        private void sliProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            tbl_progressStatus.Text = TimeSpan.FromSeconds(sli_progress.Value).ToString(@"hh\:mm\:ss");
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            PlayerViewModel playerVM = (PlayerViewModel)DataContext;

            if (me_player.Source != new Uri(playerVM.Url))
            {
                me_player.Source = new Uri(playerVM.Url);
            }

            if (!mediaPlayerIsPlaying)
            {
                me_player.Play();
                mediaPlayerIsPlaying = true;
            }
            else
            {
                me_player.Pause();
                mediaPlayerIsPlaying = false;
            }
        }
    }
}