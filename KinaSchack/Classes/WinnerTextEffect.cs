using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
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
        private float _fontSize = 60;
        public WinnerTextEffect(string player)
        {
            _player = player;
            _text = _player + " is the winner ! !";

            textFormat = new CanvasTextFormat()
            {
                FontFamily = "Monotype Corsiva",
                FontSize = _fontSize
            };
        }
        
        public void CreateEffect()
        {
            blur = new GaussianBlurEffect()
            {
                Source = blur,
                BlurAmount = 1.5f,
                BorderMode = EffectBorderMode.Soft
            };
        }

        public void SetupText(ICanvasAnimatedControl sender)
        {
            CanvasCommandList textCmdList = new CanvasCommandList(sender);
            using (CanvasDrawingSession cmdlist = textCmdList.CreateDrawingSession())
            {
                cmdlist.DrawText(_text, 800, 300, Colors.Turquoise, textFormat);
            }
            blur.Source = textCmdList;

        }

        public void ApplyEffectToText(ICanvasAnimatedControl sender)
        {
            CreateEffect();
            SetupText(sender);
        }
        public void DrawText(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            ApplyEffectToText(sender);
            args.DrawingSession.DrawImage(blur);
            args.DrawingSession.DrawText(_text,800, 300, Colors.DarkGray, textFormat);
        }
        public void DrawPlayerTurn(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            ApplyEffectToText(sender);
            args.DrawingSession.DrawImage(blur);
            args.DrawingSession.DrawText(_player, 50, 50, Colors.DarkGray, textFormat);
        }
    }
}
