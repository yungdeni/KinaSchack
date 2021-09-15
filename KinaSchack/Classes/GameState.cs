using System;
using System.Collections.Generic;
using Windows.Foundation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using KinaSchack.Enums;

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

        public (int x, int y) GetSelectedCell (int x, int y)
        {

            for (int i = 0; i < GameBoard.Cells.GetLength(0); i++)
            {
                for (int j = 0; j < GameBoard.Cells.GetLength(1); j++)
                {
                    if (GameBoard.Cells[i, j].bounds.Contains(new Point(x, y)))
                    {
                        return (i, j);
                        Debug.WriteLine("Ruta: " + i,j);
                    }
                }
            }
            return (0,0);
        }
        public bool CheckIfPlayersPiece(int x, int y)
        {
            if (CurrentPlayer == GameBoard.Cells[x,y].Item1)
            {
                return true;
            }
            return false;
        }
        public void Move((int x, int y) newPosition)
        {
            if (CheckIfPlayersPiece(SelectedCell.x,SelectedCell.y))
            {
                if (GameBoard.Cells[newPosition.x, newPosition.y].Item1 == BoardStatus.Empty)
                {
                    GameBoard.Cells[newPosition.x, newPosition.y].Item1 = CurrentPlayer;
                    GameBoard.Cells[SelectedCell.x,SelectedCell.y].Item1 = BoardStatus.Empty;
                }
            }
        }
    }
}
