using NoiseCast.MVVM.ViewModel;
using System.Windows;
using System.Windows.Input;

namespace NoiseCast
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += MainWindow_MouseLeftButtonDown;
        }

        private void MainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => DragMove();
        private void CloseApplication_Click(object sender, RoutedEventArgs e) => Application.Current.MainWindow.Close();
        private void MinWindow_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
        private void MaxWindow_Click(object sender, RoutedEventArgs e) => WindowState = WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }
    }
}