using System.Windows;

namespace NXM_Handler
{
    public partial class Tray : Window
    {
        public Tray()
        {
            InitializeComponent();
        }

        private void MainWindow_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                var _ = new MainWindow();
            });
        }

        private void AddMM_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                var _ = new AddMMWindow();
            });
        }
        /*
        private void AddAssoc_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                var _ = new MMAssocWindow();
            });
        }
        */
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }
    }
}
