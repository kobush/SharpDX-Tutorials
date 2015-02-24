using System;
using System.Windows;
using System.Windows.Controls;
using SharpDX.Toolkit;
using WinFormsApp;

namespace WpfApp
{
    public partial class MainWindow
    {
        private Game _game;

        public MainWindow()
        {
            InitializeComponent();

            _game = new Game6();
            _game.Run(surface2);
        }

    }
}
