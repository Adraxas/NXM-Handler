using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace NXM_Handler
{
    public partial class MMAssocWindow : Window
    {
        private readonly string Game;
        public MMAssocWindow(string game)
        {
            Game = game;
            Title = $"Select Mod Manager for {game}";
            ((TextBlock)MainGrid.Children[MainGrid.Children.IndexOf(prefixArgs) + 1]).Text = "Prefix Args";
            ((TextBlock)MainGrid.Children[MainGrid.Children.IndexOf(postfixArgs) + 1]).Text = "Postfix Args";
            InitializeComponent();
        }
        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            Storage.AddNewGameAssoc(new(Game, Storage.Store!.ModManagers[MMDropdown.Text].Path, (prefixArgs.Text, postfixArgs.Text)));
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
        }
    }
}
