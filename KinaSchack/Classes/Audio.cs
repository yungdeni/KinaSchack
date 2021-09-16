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
        public static MediaPlayer jumpSound;
        private static bool playing = false;

        public Audio()
        {
            jumpSound = new MediaPlayer();
            jumpSound.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/Audio/jumps.wav"));
        }
        public void PlayJumpSound()
        {
            playing = true;
            jumpSound.Play();
        }
    }
}
