using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace NXM_Handler
{
    public partial class AddMMWindow : Window
    {
        public AddMMWindow()
        {
            ((TextBlock)MainGrid.Children[MainGrid.Children.IndexOf(MMName) + 1]).Text = "Mod Manager Name (Leave blank for executable name)";
            ((TextBlock)MainGrid.Children[MainGrid.Children.IndexOf(MMPath) + 1]).Text = "Path to executable";
            InitializeComponent();
        }
        private void SelectPathButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                DefaultExt = "exe"
            };
            MMPath.Text = dialog.FileName;
        }
        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            Storage.AddNewModManager(new ModManager(MMName.Text, MMPath.Text));
        }
    }
}
