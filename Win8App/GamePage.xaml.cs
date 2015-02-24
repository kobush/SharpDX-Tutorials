using Windows.UI.Xaml;
using SharpDX.Toolkit;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Win8App
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class GamePage : Page
    {
        private Game _game;

        public GamePage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var gameType = e.Parameter as Type;
            if (gameType != null)
            {
                pageTitle.Text = gameType.Name;

                _game = (Game)Activator.CreateInstance(gameType);
                _game.Run(swapChainPanel);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (_game != null)
            {
                _game.Exit();
                _game.Dispose();
                _game = null;
            }
        }

        private void BackButton_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
