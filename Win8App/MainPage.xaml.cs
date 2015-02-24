using System;
using Windows.UI.Xaml.Controls;
using WinFormsApp;

namespace Win8App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            var gameTypes = new Type[]
            {
                typeof(Game0),
                typeof(Game1),
                typeof(Game2),
                typeof(Game3),
                typeof(Game4),
                typeof(Game5),
                typeof(Game6),
                typeof(Game7),
                typeof(Game8),
            };

            gamesGrid.ItemsSource = gameTypes;
        }

        private void GamesGrid_ItemClick(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof (GamePage), e.ClickedItem);
        }
    }
}
