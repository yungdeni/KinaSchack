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
using Windows.UI.ViewManagement;
using Windows.UI.Core;

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
        private CanvasBitmap _test;
        public MainPage()
        {
            this.InitializeComponent();
            Window.Current.SizeChanged += Current_SizeChanged;
            Scaling.SetScale();
        }

        private void Current_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            Scaling.boundsScaling = ApplicationView.GetForCurrentView().VisibleBounds;
            Scaling.SetScale();

        }
        private void Canvas_Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            args.DrawingSession.DrawImage(Scaling.img(_BG));
            foreach ((BoardStatus, Rect bounds) pos in _currentGameState.GameBoard.Cells)
            {
                var scaledpoint = Scaling.GetScaledPoint((int)pos.bounds.X, (int)pos.bounds.Y);
                var cellOnHover = _currentGameState.SelectedCell;


                //args.DrawingSession.DrawRectangle(pos.bounds, Colors.Red);
                if (pos.Item1 == BoardStatus.Player2)
                {

                    args.DrawingSession.DrawImage(_piece, Scaling.GetScaledRect(pos.bounds));
                }
                else if(pos.Item1 == BoardStatus.Player1)
                {
                    if (cellOnHover == scaledpoint)
                    {
                        args.DrawingSession.DrawImage(_test, Scaling.GetScaledRect(pos.bounds));
                    }
                    else
                    {
                        args.DrawingSession.DrawImage(_piece2, Scaling.GetScaledRect(pos.bounds));
                    }
                    
                }
            }
            if (_currentGameState.PieceSelected)
            {
                foreach (var move in _currentGameState.PossibleMoves)
                {
                    Rect cellToDraw = Scaling.GetScaledRect(_currentGameState.GameBoard.Cells[move.x, move.y].bounds);
                    double radius = Math.Sqrt(Math.Pow(cellToDraw.Height, 2) + Math.Pow(cellToDraw.Width, 2));
                    args.DrawingSession.DrawCircle((float)(cellToDraw.X + (cellToDraw.Width / 2)), (float)(cellToDraw.Y + (cellToDraw.Height / 2)), (float)radius / 2, Colors.Green, 5);
                }

            }

            //Rect selectedPiece = _currentGameState.GameBoard.Cells[_currentGameState.SelectedCell.x, _currentGameState.SelectedCell.y].bounds;
            //if (_currentGameState.PieceSelected)
            //{
            //    args.DrawingSession.DrawCircle((float)(selectedPiece.X + (selectedPiece.Width / 2)), (float)(selectedPiece.Y + (selectedPiece.Height / 2)), 30, Colors.Green, 5);
            //}

        }

        private void Canvas_CreateResources(CanvasAnimatedControl sender, CanvasCreateResourcesEventArgs args)
        {
            args.TrackAsyncAction(CreateResourcesAsync(sender).AsAsyncAction());
        }

        async Task CreateResourcesAsync(CanvasAnimatedControl sender)
        {
            _BG = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/BG_Glow.png"));
            _piece = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/Pumpkin.png"));
            _piece2 = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/Pumpkin2.png"));
            _currentGameState = new GameState();
            _test = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/selectedPumpkin.png"));
        }

        private void Canvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Debug.WriteLine("PoinertPressed");
            x = (int)e.GetCurrentPoint(Canvas).Position.X;
            y = (int)e.GetCurrentPoint(Canvas).Position.Y;
            var newPoint = Scaling.GetScaledPoint(x, y);
            _currentGameState.HandleTurn(newPoint.x, newPoint.y);

        }

        private void Canvas_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            Debug.WriteLine("PoinertReleased");
        }

        private void Canvas_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            x = (int)e.GetCurrentPoint(Canvas).Position.X;
            y = (int)e.GetCurrentPoint(Canvas).Position.Y;
            var hoverPoint = Scaling.GetScaledPoint(x, y);
            if (_currentGameState != null)
            {
                if (_currentGameState.GetSelectedCell(hoverPoint.x, hoverPoint.y) == (-1, -1))
                {
                    return;
                }
                else if (_currentGameState.CheckIfPlayersPiece(_currentGameState.GetSelectedCell(hoverPoint.x, hoverPoint.y)))
                {
                    _currentGameState.SelectedCell = _currentGameState.GetSelectedCell(hoverPoint.x, hoverPoint.y);
                    Debug.WriteLine("moved!!" + _currentGameState.SelectedCell + _currentGameState.CurrentPlayer);

                }
            }

            //Debug.WriteLine("PoinertMoved");
        }

        private void HintButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Canvas_Update(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args)
        {

        }
    }
}
