using System;
using System.Collections.Generic;
using Windows.Foundation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using KinaSchack.Enums;
using Windows.Media.Playback;

namespace KinaSchack.Classes
{
    /// <summary>
    /// Class <c>GameState</c> models the state of the current game
    /// </summary>
    public class GameState
    {
        public Board GameBoard;
        public BoardStatus CurrentPlayer;
        public (int x, int y) SelectedCell;
        public (int a, int b) NewSelectedCell;
        public bool PieceSelected;
        public List<(int x, int y)> PossibleMoves;
        private List<(int x, int y)> _jumps;
        public Queue<AnimatePiece> AnimationQueue;
        

        public GameState()
        {
            
            GameBoard = new Board();
            CurrentPlayer = BoardStatus.Player1;
            PieceSelected = false;
            PossibleMoves = new List<(int x, int y)>();
            _jumps = new List<(int x, int y)>()
            {
                (2,0),(0,2),(-2,0),(0,-2),(2,2),(-2,-2)
            };
            AnimationQueue = new Queue<AnimatePiece>();
        }
        public (int x, int y) GetSelectedCell(int x, int y)
        {
            for (int i = 0; i < GameBoard.Cells.GetLength(0); i++)
            {
                for (int j = 0; j < GameBoard.Cells.GetLength(1); j++)
                {
                    if (GameBoard.Cells[i, j].bounds.Contains(new Point(x, y)))
                    {
                        Debug.WriteLine("Ruta: " + i + ", " + j);
                        return (i, j);
                    }
                }
            }
            return (-1, -1);
        }
        public bool CheckIfPlayersPiece((int x, int y) cellPosition)
        {
            return CurrentPlayer == GameBoard.Cells[cellPosition.x, cellPosition.y].Item1;
        }
        public void Move((int x, int y) newPosition)
        {
            AnimationQueue.Enqueue(new AnimatePiece(GameBoard.Cells[SelectedCell.x, SelectedCell.y].bounds, GameBoard.Cells[newPosition.x, newPosition.y].bounds, CurrentPlayer));
            GameBoard.Cells[newPosition.x, newPosition.y].Item1 = CurrentPlayer;
            GameBoard.Cells[SelectedCell.x, SelectedCell.y].Item1 = BoardStatus.Empty;
            
        }
        public bool CheckIfNewPositionIsEmpty((int x, int y) newPosition)      {
            return GameBoard.Cells[newPosition.x, newPosition.y].Item1 == BoardStatus.Empty;
        }
        //TODO: This doesnt need to be so general. We only check one jump now.
        public bool CheckIfJumpIsLegal((int x, int y) newPosition, (int x, int y) startPosition)
        {
            //int diffX = newPosition.x - SelectedCell.x;
            //int diffY = newPosition.y - SelectedCell.y;
            //Debug.WriteLine("Direction: ({0},{1})", Math.Sign(diffX), Math.Sign(diffY));
            //(int x, int y) direction = (Math.Sign(diffX), Math.Sign(diffY));
            (int x, int y) midPoint = ((newPosition.x + startPosition.x) / 2, (newPosition.y + startPosition.y) / 2);
            if (CheckIfNewPositionIsEmpty(midPoint))
            {
                return false;
            }
            if (!CheckIfNewPositionIsEmpty(newPosition))
            {
                return false;
            }
            Debug.WriteLine("Correct");
            return true;
        }
        public List<(int x, int y)> GetPossibleMoves()
        {
            //Seed the list with possible jumps from starting position
            List<(int x, int y)> moves = new List<(int x, int y)>();
            foreach ((int x, int y) in _jumps)
            {
                int newX = x + SelectedCell.x;
                int newY = y + SelectedCell.y;
                if (newX >= GameBoard.Cells.GetLength(0) || newX < 0 || newY >= GameBoard.Cells.GetLength(1) || newY < 0)
                {
                    continue;
                }
                if (CheckIfNewPositionIsEmpty((newX, newY)) && CheckIfJumpIsLegal((newX, newY), (SelectedCell.x, SelectedCell.y)))
                {
                    moves.Add((newX, newY));
                }
            }
            //Do the same thing with the new possible jumps as starting points
            List<(int x, int y)> copyOfPossibleMoves;
            do
            {
                copyOfPossibleMoves = moves.ToList();
                foreach (var startPos in copyOfPossibleMoves)
                {
                    foreach ((int x, int y) in _jumps)
                    {
                        int newX = x + startPos.x;
                        int newY = y + startPos.y;
                        if (newX >= GameBoard.Cells.GetLength(0) || newX < 0 || newY >= GameBoard.Cells.GetLength(1) || newY < 0)
                        {
                            continue;
                        }
                        if (CheckIfNewPositionIsEmpty((newX, newY)) && CheckIfJumpIsLegal((newX, newY), (startPos.x, startPos.y)))
                        {
                            if (!moves.Contains((newX, newY)))
                            {
                                moves.Add((newX, newY));
                            }
                        }
                    }
                }
                //We are done when there are no more possible jumps to make
            } while (copyOfPossibleMoves.Count() != moves.Count());
            //Add the single jumps
            foreach ((int x, int y) in _jumps)
            {
                int newX = (x / 2) + SelectedCell.x;
                int newY = (y / 2) + SelectedCell.y;
                if (newX >= GameBoard.Cells.GetLength(0) || newX < 0 || newY >= GameBoard.Cells.GetLength(1) || newY < 0)
                {
                    continue;
                }
                if (CheckIfNewPositionIsEmpty((newX, newY)))
                {
                    moves.Add((newX, newY));
                }
            }
            return moves;
        }
        public void HandleTurn(int x, int y)
        {
            //Check if player clicked on a cell
            if (GetSelectedCell(x, y) == (-1, -1))
            {
                SelectedCell = (-1, -1);
                PieceSelected = false;
                PossibleMoves.Clear();
                return;
            }
            if (CheckIfPlayersPiece(GetSelectedCell(x, y)))
            {
                PossibleMoves.Clear();
                SelectedCell = GetSelectedCell(x, y);
                PossibleMoves = GetPossibleMoves();
                PieceSelected = true;
                return;
            }
            if (PieceSelected)
            {
                (int x, int y) newPos = GetSelectedCell(x, y);
                if (PossibleMoves.Contains(newPos))
                {
                    Move(newPos);
                    PieceSelected = false;

                    if (CheckIfVictory())
                    {
                        Debug.WriteLine("Winner");
                        MainPage.audio.PlayJumpSound();
                        MainPage.audio.PlayWinnerSound();
                    }
                    else
                    {
                        Debug.WriteLine("Loser");

                        //Takes the integral int value behind the enum and flips it from 0 : Player1 and 1: Player2
                        CurrentPlayer = (BoardStatus)(((int)CurrentPlayer) ^ 1);
                        MainPage.audio.PlayJumpSound();
                        
                        PossibleMoves.Clear();

                    }
                }
            }
        }
        public bool CheckIfVictory()
        {
            int a = 4;
            if (CurrentPlayer == BoardStatus.Player1)
            {
                for (int i = 0; i <= 3; i++)
                {
                    a--;
                    for (int j = 0; j <= a; j++)
                    {
                        Debug.WriteLine(a);
                        Debug.WriteLine(i + ", " + j);
                        if (GameBoard.Cells[j, i].Item1 != BoardStatus.Player1)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            else if (CurrentPlayer == BoardStatus.Player2)
            {
                a = 2;
                for (int b = 6; b >= 3; b--)
                {
                    a++;
                    for (int c = 6; c >= a; c--)
                    {
                        Debug.WriteLine(a);
                        Debug.WriteLine(b + ", " + c);
                        if (GameBoard.Cells[c, b].Item1 != BoardStatus.Player2)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
    }
}
