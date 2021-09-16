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

        //vems tur
        //om man har selectad en piece
        //vilken piece

        public GameState()
        {
            GameBoard = new Board();
            CurrentPlayer = BoardStatus.Player1;
            PieceSelected = false;
        }

        public (int x, int y) GetSelectedCell(int x, int y)
        {
            for (int i = 0; i < GameBoard.Cells.GetLength(0); i++)
            {
                for (int j = 0; j < GameBoard.Cells.GetLength(1); j++)
                {
                    if (GameBoard.Cells[i, j].bounds.Contains(new Point(x, y)))
                    {
                        Debug.WriteLine("Ruta: " + i, j);
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

            if (GameBoard.Cells[newPosition.x, newPosition.y].Item1 == BoardStatus.Empty)
            {
                GameBoard.Cells[newPosition.x, newPosition.y].Item1 = CurrentPlayer;
                GameBoard.Cells[SelectedCell.x, SelectedCell.y].Item1 = BoardStatus.Empty;
            }

        }
        public void HandleTurn(int x, int y)
        {
            //Check if player clicked on a cell
            
            if (GetSelectedCell(x, y) == (-1, -1))
            {
                SelectedCell = (-1, -1);
                PieceSelected = false;
                return;
            }

            if (CheckIfPlayersPiece(GetSelectedCell(x, y)) && PieceSelected == false)
            {
                SelectedCell = GetSelectedCell(x, y);
                PieceSelected = true;
                return;
            }
            if (PieceSelected)
            {
                Move(GetSelectedCell(x, y));
                PieceSelected = false;
                //Takes the integral int value behind the enum and flips it from 0 : Player1 and 1: Player2
                CurrentPlayer = (BoardStatus)(((int)CurrentPlayer) ^ 1);
            }
        }
    }
}
