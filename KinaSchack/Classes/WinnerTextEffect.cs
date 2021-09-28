using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace KinaSchack.Classes
{
    class WinnerTextEffect
    {
        GaussianBlurEffect blur;
        CanvasTextFormat textFormat;
        private string _player;
        private string _text;
        private float _fontSize = 90;
        public WinnerTextEffect(string player)
        {
            
            _player = player;
            _text = _player + " IS THE WINNER!!!";

            textFormat = new CanvasTextFormat()
            {
                FontFamily = "Monotype Corsiva",
                FontSize = _fontSize
            };

        }

        private void CreateEffect()
        {
            blur = new GaussianBlurEffect()
            {
                
                BlurAmount = 50,
                
            };
        }
       


        public string DrawText(string text)
        {
            
            return _text;
        }
        

    }
}
