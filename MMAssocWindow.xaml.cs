using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace NXM_Handler
{
    public partial class MMAssocWindow : Window
    {
        private readonly string Game;
        public MMAssocWindow(string game)
        {
            Game = game;
            InitializeComponent();
            TitleBar.Text = $"Select Mod Manager for {game}";
            ShowDialog();
        }
        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            if (MMDropdown.Text == "Add new game...")
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    var _ = new AddMMWindow();
                });
                //This is probably the wrong way to do this
                var DropdownData = MMDropdown.ItemsSource as MMList;
                DropdownData?.Update();
            }
            else
            {
                Storage.AddNewGameAssoc(new(Game, MMDropdown.Text, (prefixArgs.Text, postfixArgs.Text)));
                Close();
            }
        }
    }
    internal class MMList : ObservableCollection<string>
    {
        public MMList()
        {
            Update();
        }
        internal void Update()
        {
            Clear();
            foreach(var mm in Storage.Store!.ModManagers)
            {
                Add(mm.Value.Name);
            }
            Add("Add new game...");
        }
    }
}
