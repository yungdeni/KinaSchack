using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using KinaSchack.Classes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace KinaSchack
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainMenu : Page
    {
        public static MediaPlayer player = new MediaPlayer();
        public MainMenu()
        {
            this.InitializeComponent();
            //Background Music: https://opengameart.org/content/neocrey-jump-to-win
            player.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/Audio/neocrey - Jump to win.mp3"));
            player.Volume = 0.005;
            player.AutoPlay = true;
            player.Play();
           
        }

        private void MainMenuStartGame(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (!Instruction_Popup.IsOpen)
            {
                Instruction_Popup.Height = Window.Current.Bounds.Height;
                Instruction_Popup.IsOpen = true;
                RuleImage.Visibility = Visibility.Visible;
            }
        }

        private void MainMenuQuit(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }
    }
}
