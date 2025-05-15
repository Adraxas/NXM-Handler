using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace NXM_Handler
{
    public partial class AddMMWindow : Window
    {
        public AddMMWindow()
        {
            InitializeComponent();
            ShowDialog();
        }
        private void SelectPathButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                DefaultExt = "exe"
            };
            dialog.ShowDialog();
            MMPath.Text = dialog.FileName;
        }
        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            Storage.AddNewModManager(new ModManager(MMName.Text, MMPath.Text));
            Close();
        }
    }
}
