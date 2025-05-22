using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
