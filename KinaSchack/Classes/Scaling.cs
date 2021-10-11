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
    /// <summary>
    /// Contains helper methods to assist with scaling
    /// </summary>
    public static class Scaling
    {
        public static Rect BoundsScaling = ApplicationView.GetForCurrentView().VisibleBounds;
        public static float DesignWidth = 1920;
        public static float DesignHeight = 1080;
        public static float scaleWidth, scaleHeight;
        /// <summary>
        /// Sets the scale factors according to the design bounds and the current size of the application
        /// </summary>
        public static void SetScale()
        {
            scaleWidth = (float)BoundsScaling.Width / DesignWidth;
            scaleHeight = (float)BoundsScaling.Height / DesignHeight;
        }
        /// <summary>
        /// Scales an image by applying a transform to it
        /// </summary>
        /// <param name="source">The Source image</param>
        /// <returns>An image with a transform scale</returns>
        public static Transform2DEffect Img(CanvasBitmap source)
        {
            Transform2DEffect image;
            image = new Transform2DEffect() { Source = source };
            image.TransformMatrix = Matrix3x2.CreateScale(scaleWidth, scaleHeight);
            return image;
        }
        /// <summary>
        /// Scales a Rect objecty
        /// </summary>
        /// <param name="rectToScale">The rect to scale</param>
        /// <returns>A Rect with its dimesions scaled</returns>
        public static Rect GetScaledRect(Rect rectToScale)
        {
            double newWidth = rectToScale.Width * scaleWidth;
            double newHeight = rectToScale.Height * scaleHeight;
            double newX = rectToScale.Left * scaleWidth;
            double newY = rectToScale.Top  *scaleHeight;
            return new Rect(newX, newY, newWidth, newHeight);

        }
        /// <summary>
        /// Scales a point. Used for sending the values into the GameState.HandleTurn method so we dont have to save the scaled Rects in the board.
        /// </summary>
        /// <returns>An (x, y) tuple with its values scaled</returns>
        public static (int x, int y) GetScaledPoint(int x, int y)
        {
            int newX = (int)(x / scaleWidth);
            int newY = (int)(y / scaleHeight);
            return (newX, newY);
        }
    }
}
