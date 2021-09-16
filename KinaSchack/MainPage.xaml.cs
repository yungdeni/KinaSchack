using KinaSchack.Classes;
using KinaSchack.Enums;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Microsoft.Graphics.Canvas.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace KinaSchack
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private CanvasBitmap _BG;
        private CanvasBitmap _piece;
        private CanvasBitmap _piece2;
        private GameState _currentGameState;
        private int x, y;
        private bool debugMode;
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Canvas_Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            args.DrawingSession.DrawImage(_BG);
            foreach ((BoardStatus, Rect bounds) pos in _currentGameState.GameBoard.Cells)
            {
                //args.DrawingSession.DrawRectangle(pos.bounds, Colors.Red);
                if(pos.Item1 == BoardStatus.Player2)
                {
                    args.DrawingSession.DrawImage(_piece, pos.bounds);
                }
                else if(pos.Item1 == BoardStatus.Player1)
                {
                    args.DrawingSession.DrawImage(_piece2, pos.bounds);
                }
            }
        }

        private void Canvas_CreateResources(CanvasAnimatedControl sender, CanvasCreateResourcesEventArgs args)
        {
            args.TrackAsyncAction(CreateResourcesAsync(sender).AsAsyncAction());

        }

        async Task CreateResourcesAsync(CanvasAnimatedControl sender)
        {
            _BG = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/Pumpkin_Checkers_BG.png"));
            _piece = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/Pumpkin.png"));
            _piece2 = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/Pumpkin2.png"));
            _currentGameState = new GameState();
        }

        private void Canvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Debug.WriteLine("PoinertPressed");
            x = (int)e.GetCurrentPoint(Canvas).Position.X;
            y = (int)e.GetCurrentPoint(Canvas).Position.Y;
            _currentGameState.HandleTurn(x, y);

        }

        private void Canvas_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            Debug.WriteLine("PoinertReleased");
        }

        private void Canvas_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            //Debug.WriteLine("PoinertMoved");
        }

        private void Canvas_Update(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args)
        {

        }
    }
}
