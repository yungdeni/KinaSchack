using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Media.Playback;
using Windows.Media.Core;

namespace KinaSchack.Classes
{
    public class Audio
    {
        //public static MediaPlayer jumpSound;
        public MediaPlayer mediaPlayer;
        public bool playing = false;

        public Audio()
        {
            mediaPlayer = new MediaPlayer();

            //Jump Sound: https://freesound.org/people/simoneyoh3998/sounds/500675/
            mediaPlayer.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/Audio/jumps.wav"));
           
        }
        public void PlayJumpSound()
        {
            playing = true;
            mediaPlayer.Volume = 0.5;
            mediaPlayer.Play();
        }
        public void PlayWinnerSound()
        {
            MainMenu.player.Pause();
            // Winner Music: https://opengameart.org/content/victory-1
            mediaPlayer.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/Audio/Viktor Kraus - Victory!.mp3"));       
            mediaPlayer.Volume = 0.008;
            mediaPlayer.IsLoopingEnabled = true;
            mediaPlayer.Play();
        }
        public void StartBackGrounAudio()
        {
            MainMenu.player.Play();
        }
        public void StopBackGrounAudio()
        {
            MainMenu.player.Pause();
        }
    }
}
