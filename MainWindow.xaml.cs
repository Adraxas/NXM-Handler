using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.IO;
using Microsoft.Win32;

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
            //this.Scale();
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
            OpenFileDialog openFileDialog = new()
            {
                Filter = "(*.exe)|*.exe"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                //Get the path of specified file
                string filePath = openFileDialog.FileName;
                string MMName = Microsoft.VisualBasic.Interaction.InputBox("Input name of Mod Manager (or leave blank for EXE name)", "Enter Name", Path.GetFileNameWithoutExtension(filePath));
                Storage.AddNewModManager(new ModManager(MMName, filePath));
            }
        }


        private void Scale()
        {

            Main_Window.Height = Scalar.Wheight * Scalar.Mult;
            Main_Window.Width = Scalar.Wwidth * Scalar.Mult;
            Row_1.Height = new GridLength(Scalar.R1 * Scalar.Mult, GridUnitType.Star);
            Row_3.Height = new GridLength(Scalar.R3 * Scalar.Mult, GridUnitType.Star);
            Row_2.Height = new GridLength(Scalar.R2 * Scalar.Mult, GridUnitType.Star);
            Game_Button.Height = Scalar.GBheight * Scalar.Mult;
            Game_Button.Width = Scalar.GBwidth * Scalar.Mult;
            Target_Button.Height = Scalar.TBheight * Scalar.Mult;
            Target_Button.Width = Scalar.TBwidth * Scalar.Mult;
            Options_Button.Height = Scalar.OBheight * Scalar.Mult;
            Options_Button.Width = Scalar.OBwidth * Scalar.Mult;
        }
    }
    struct Scalar
    {
        internal const double Mult = 1.5;
        internal const int Wheight = 900;
        internal const int Wwidth = 300;
        internal const int R1 = 600;
        internal const int R2 = 160;
        internal const int R3 = 80;
        internal const int GBheight = 50;
        internal const int GBwidth = 160;
        internal const int TBheight = 50;
        internal const int TBwidth = 160;
        internal const int OBheight = 60;
        internal const int OBwidth = 60;
    }
}