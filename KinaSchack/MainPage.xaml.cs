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
using System.ComponentModel;
using System.Text.Json;
using System.Text.RegularExpressions;

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
        private CanvasBitmap _winner;
        private CanvasBitmap _tombstone;
        private GameState _currentGameState;
        private int x, y;
        public static Audio audio;
        private Players _players;
        private double _currVolume;
        static public bool isWinner = false;
        private string _theWinner;
        private CanvasBitmap orangeHover;
        private CanvasBitmap blueHover;
        private (int x, int y) hoverSelect;
        private AnimatePiece _currentAnimation;
        private bool _showHints;
        private SaveState _savedGame;
        public MainPage()
        {
            this.InitializeComponent();
            Window.Current.SizeChanged += Current_SizeChanged;
            Scaling.SetScale();
            SetDefaultStartPlayerText();
            _showHints = true;
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
                if (!(_currentAnimation is null))
                {
                    if (_currentAnimation.EndPosition == pos.bounds && !_currentAnimation.Done)
                    {
                        continue;
                    }
                    if (pos.bounds == _currentGameState.GameBoard.Cells[_currentGameState.LastMove.x, _currentGameState.LastMove.y].bounds)
                    {
                        continue;
                    }
                }
                //args.DrawingSession.DrawRectangle(pos.bounds, Colors.Red);
                if (pos.Item1 == BoardStatus.Player2)
                {
                    args.DrawingSession.DrawImage(_piece, Scaling.GetScaledRect(pos.bounds));
                }
                else if (pos.Item1 == BoardStatus.Player1)
                {
                    args.DrawingSession.DrawImage(_piece2, Scaling.GetScaledRect(pos.bounds));
                }
                else if (pos.Item1 == BoardStatus.Tombstone)
                {
                    args.DrawingSession.DrawImage(_tombstone, Scaling.GetScaledRect(pos.bounds));
                }

            }
            if (hoverSelect != (-1, -1))
            {
                if (_currentGameState.CurrentPlayer == BoardStatus.Player1)
                {
                    args.DrawingSession.DrawImage(blueHover, Scaling.GetScaledRect(_currentGameState.GameBoard.Cells[hoverSelect.x, hoverSelect.y].bounds));
                }
                else
                {
                    args.DrawingSession.DrawImage(orangeHover, Scaling.GetScaledRect(_currentGameState.GameBoard.Cells[hoverSelect.x, hoverSelect.y].bounds));
                }

            }


            if (_currentGameState.PieceSelected)
            {
                if (_currentGameState.CurrentPlayer == BoardStatus.Player1)
                {
                    args.DrawingSession.DrawImage(blueHover, Scaling.GetScaledRect(_currentGameState.GameBoard.Cells[_currentGameState.SelectedCell.x, _currentGameState.SelectedCell.y].bounds));
                }
                else
                {
                    args.DrawingSession.DrawImage(orangeHover, Scaling.GetScaledRect(_currentGameState.GameBoard.Cells[_currentGameState.SelectedCell.x, _currentGameState.SelectedCell.y].bounds));
                }

                if (_showHints)
                {
                    foreach (var move in _currentGameState.PossibleMoves)
                    {
                        Rect cellToDraw = Scaling.GetScaledRect(_currentGameState.GameBoard.Cells[move.x, move.y].bounds);
                        double radius = Math.Sqrt(Math.Pow(cellToDraw.Height, 2) + Math.Pow(cellToDraw.Width, 2));
                        args.DrawingSession.DrawCircle((float)(cellToDraw.X + (cellToDraw.Width / 2)), (float)(cellToDraw.Y + (cellToDraw.Height / 2)), (float)radius / 2, Colors.Green, 5);
                    }
                }


            }
            //args.DrawingSession.DrawImage(Scaling.img(_winner));
            //Do something if a player wins
            if (isWinner)
            {
                //gets the main CoreApplicationView so it is always available
                //source: https://stackoverflow.com/questions/16477190/correct-way-to-get-the-coredispatcher-in-a-windows-store-app
                _ = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                    {         
                        if (_currentGameState.CurrentPlayer == BoardStatus.Player1)
                        {
                            _theWinner = _players.Player1;
                        }
                        else if (_currentGameState.CurrentPlayer == BoardStatus.Player2)
                        {
                            _theWinner = _players.Player2;
                        }
                        WinnerTextEffect w = new WinnerTextEffect(_theWinner);
                      
                        CanvasAnimatedControl victoryCanvas = new CanvasAnimatedControl
                        {
                            Name = "VictoryCanvas",
                        };
                        victoryCanvas.Draw += w.DrawText;
                        GameGrid.Children.Add(victoryCanvas);
                        AddBackToMenuButton();
                        Winner.Visibility = Visibility.Visible;
                    }
                );
                isWinner = false;
            }
            if (!(_currentAnimation is null))
            {
                if (!_currentAnimation.Done)
                {
                    if (_currentAnimation.Player == BoardStatus.Player1)
                    {
                        args.DrawingSession.DrawImage(_piece2, Scaling.GetScaledRect(_currentAnimation.DrawPosition));
                    }
                    else
                    {
                        args.DrawingSession.DrawImage(_piece, Scaling.GetScaledRect(_currentAnimation.DrawPosition));
                    }

                    //Debug.WriteLine("Drawing Animation");
                }
            }
            //Rect selectedPiece = _currentGameState.GameBoard.Cells[_currentGameState.SelectedCell.x, _currentGameState.SelectedCell.y].bounds;
            //if (_currentGameState.PieceSelected)
            //{
            //    args.DrawingSession.DrawCircle((float)(selectedPiece.X + (selectedPiece.Width / 2)), (float)(selectedPiece.Y + (selectedPiece.Height / 2)), 30, Colors.Green, 5);
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
            _winner = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/winner1.png"));
            _tombstone = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/tombstone.png"));
            _currentGameState = new GameState();
            audio = new Audio();
            _players = new Players();
            _currVolume = MainMenu.player.Volume * 1000;
            VolumeSlider.Value = _currVolume;
            //Content dialog with textbox to enter players name 

            ContentDialogResult result = await InputPlayersNameDialog.ShowAsync();
            orangeHover = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/selectedPumpkin.png"));
            blueHover = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/selectedPumpkin2.png"));
            //_testAnimation = new AnimatePiece(_currentGameState.GameBoard.Cells[1, 1].bounds, _currentGameState.GameBoard.Cells[5, 5].bounds, BoardStatus.Player1);
        }

        private void Canvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Debug.WriteLine("PoinertPressed");
            x = (int)e.GetCurrentPoint(Canvas).Position.X;
            y = (int)e.GetCurrentPoint(Canvas).Position.Y;
            var newPoint = Scaling.GetScaledPoint(x, y);
            _currentGameState.HandleTurn(newPoint.x, newPoint.y);
            ChangePlayerEffect();
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
                var selectedCellTemp = _currentGameState.GetSelectedCell(hoverPoint.x, hoverPoint.y);
                if (selectedCellTemp == (-1, -1) || _currentGameState.CheckIfPlayersPiece(selectedCellTemp))
                {
                    hoverSelect = selectedCellTemp;
                }
            }


            //Debug.WriteLine("PoinertMoved");
        }

        private void HintButton_Click(object sender, RoutedEventArgs e)
        {
            var item = (MenuFlyoutItem)sender;
            _showHints = !_showHints;
            item.Text = _showHints ? "Disable Hint" : "Enable Hint";
        }
        //Sets the players properties to default or input from textbox
        private void InputPlayersNameDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            P1.Text = _players.Player1;
            P2.Text = _players.Player2;
        }

        private void InputPlayersNameDialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            this.Frame.Navigate(typeof(MainMenu));
        }

        private void Player1Input_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        //Displays the Audio settings Content Dialog
        private void AudioSettings_Click(object sender, RoutedEventArgs e)
        {
           AudioSettingsDialog.ShowAsync();
        }

        private void AudioSettingsDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }

        private void AudioSettingsDialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }

        private void StartMusic_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.player.Play();
        }

        private void StopMusic_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.player.Pause();
        }
        //Volume slider changes the background music.
        private void VolumeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            Debug.WriteLine(MainMenu.player.Volume);
            Debug.WriteLine(VolumeSlider.Value / 1000 + "    slider");

            if (VolumeSlider.Value < _currVolume)
            {
                Debug.WriteLine("minskar");
                MainMenu.player.Volume = VolumeSlider.Value / 1000;
                _currVolume = VolumeSlider.Value;
            }
            else if (VolumeSlider.Value > _currVolume)
            {
                Debug.WriteLine("ökar");
                MainMenu.player.Volume = VolumeSlider.Value / 1000;
                _currVolume = VolumeSlider.Value;
            }

        }
        private void WinnerButton_Click(object sender, RoutedEventArgs e)
        {
            audio.mediaPlayer.Pause();
            this.Frame.Navigate(typeof(MainMenu));
        }

        private void Canvas_Update(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args)
        {
            

            if (_currentGameState.AnimationQueue.Count != 0 && _currentAnimation is null)
            {
                _currentAnimation = _currentGameState.AnimationQueue.Dequeue();
                Debug.WriteLine("Got animation");
            }
            if (!(_currentAnimation is null))
            {
                if (_currentAnimation.Done)
                {
                    _currentAnimation = null;
                    //Debug.WriteLine("Updating Animation");
                }
                else
                {
                    _currentAnimation.Update();
                }
            }
        }
        private void AddBackToMenuButton()
        {
            SolidColorBrush foreTransparent = new SolidColorBrush(Colors.White);
            FontFamily font = new FontFamily("Monotype Corsiva");
            Button menuButton = new Button
            {
                Content = "Back to Menu",
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Right,
                FontFamily = font,
                Foreground = foreTransparent,
                Width = 120,
                Height = 40,
                FontSize = 16
            };
            menuButton.Click += WinnerButton_Click;
            GameGrid.Children.Add(menuButton);
        }
        //Change the highlight of players turn
        private void ChangePlayerEffect()
        {
            if (_currentGameState.CurrentPlayer == BoardStatus.Player1)
            {
                P1.Visibility = Visibility.Collapsed;
                textblockTranslateMaster1.Visibility = Visibility.Visible;
                PlayerEffect1.Visibility = Visibility.Visible;
                P2.Visibility = Visibility.Visible;
                textblockTranslateMaster2.Visibility = Visibility.Collapsed;
                PlayerEffect2.Visibility = Visibility.Collapsed;
            }
            else if (_currentGameState.CurrentPlayer == BoardStatus.Player2)
            {
                P2.Visibility = Visibility.Collapsed;
                textblockTranslateMaster2.Visibility = Visibility.Visible;
                PlayerEffect2.Visibility = Visibility.Visible;
                P1.Visibility = Visibility.Visible;
                textblockTranslateMaster1.Visibility = Visibility.Collapsed;
                PlayerEffect1.Visibility = Visibility.Collapsed;        
            }
        }

        private void FlyoutBackToMenu_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainMenu));
        }

        private void FlyoutExitGame(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }


        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            _currentAnimation = null;
            if (!(_savedGame is null))
            {
                _currentGameState = _savedGame.ReturnSavedGameState();
                //_players = _savedGame.ReturnSavedPlayers();
            }
            ChangePlayerEffect();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _savedGame = new SaveState(_currentGameState, _players);
            LoadingFunction.IsEnabled = true;
        }
        private void LoadInstructionPage(object sender, RoutedEventArgs e)
        {
            if (!Instruction_Popup.IsOpen)
            {
                Instruction_Popup.Height = Window.Current.Bounds.Height;
                Instruction_Popup.IsOpen = true;
            }
        }

        private void CreditsPopup(object sender, RoutedEventArgs e)
        {
            Credits_Popup.Height = Window.Current.Bounds.Height;
            Credits_Popup.IsOpen = true;
        }

        private void SetDefaultStartPlayerText()
        {
            P1.Visibility = Visibility.Collapsed;
            textblockTranslateMaster1.Visibility = Visibility.Visible;
            PlayerEffect1.Visibility = Visibility.Visible;
        }
    }
}