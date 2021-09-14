using System;
using System.Collections.Generic;
using Windows.Foundation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace KinaSchack.Classes
{
    /// <summary>
    /// Class <c>GameState</c> models the state of the current game
    /// </summary>
    public class GameState
    {
        public Board GameBoard;

        public (int x, int y) SelectedCell;

        public GameState()
        {
            GameBoard = new Board();

        }
        public void GetSelectedCell (int x, int y)
        {

            for (int i = 0; i < GameBoard.positions.GetLength(0); i++)
            {
                for (int j = 0; j < GameBoard.positions.GetLength(1); j++)
                {
                    if (GameBoard.positions[i, j].bounds.Contains(new Point(x, y)))
                    {
                        SelectedCell = (i, j);
                        GameBoard.positions[i, j].occupied = true;
                        Debug.WriteLine("Ruta: " + SelectedCell);
                    }

                }
            }
        }
    }
}
