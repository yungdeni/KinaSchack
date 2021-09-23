using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.ViewManagement;

namespace KinaSchack.Classes
{
    public static class Scaling
    {
        public static Rect boundsScaling = ApplicationView.GetForCurrentView().VisibleBounds;
        public static float DesignWidth = 1920;
        public static float DesignHeight = 1080;
        public static float scaleWidth, scaleHeight;

        public static void SetScale()
        {
            scaleWidth = (float)boundsScaling.Width / DesignWidth;
            scaleHeight = (float)boundsScaling.Height / DesignHeight;
        }

        public static Transform2DEffect img(CanvasBitmap source)
        {
            Transform2DEffect image;
            image = new Transform2DEffect() { Source = source };
            image.TransformMatrix = Matrix3x2.CreateScale(scaleWidth, scaleHeight);
            return image;
        }
        public static Rect GetScaledRect(Rect rectToScale)
        {
            double newWidth = rectToScale.Width * scaleWidth;
            double newHeight = rectToScale.Height * scaleHeight;
            double newX = rectToScale.Left * scaleWidth;
            double newY = rectToScale.Top  *scaleHeight;
            return new Rect(newX, newY, newWidth, newHeight);

        }
        public static (int x, int y) GetScaledPoint(int x, int y)
        {
            int newX = (int)(x / scaleWidth);
            int newY = (int)(y / scaleHeight);
            return (newX, newY);
        }
    }
}
