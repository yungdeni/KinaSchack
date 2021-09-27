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
            
            //jumpSound = new MediaPlayer();
            mediaPlayer.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/Audio/jumps.wav"));
           
        }
        public void PlayJumpSound()
        {
            playing = true;
            mediaPlayer.Play();
        }
        public void PlayWinnerSound()
        {
            MainMenu.player.Pause();        
            mediaPlayer.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/Audio/Viktor Kraus - Victory!.mp3"));       
            mediaPlayer.Volume = 0.008;
            mediaPlayer.Play();
        }
        public void StopBackGrounAudio()
        {
            MainMenu.player.Pause();
            MainMenu.player.Source = null;
        }
    }
}
