using System.Windows;
namespace NXM_Handler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Options_Click(object sender, RoutedEventArgs e)
        {
            Options_Grid.Visibility = Options_Grid.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
        }
        private void Game_Click(object sender, RoutedEventArgs e)
        {
            
        }
        private void Target_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}