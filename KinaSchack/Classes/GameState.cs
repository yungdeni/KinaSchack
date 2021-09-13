using System;
using System.Collections.Generic;
using Windows.Foundation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace KinaSchack.Classes
{
    public class GameState
    {
        public Board gameBoard;

        public (int x, int y) selectedCell;

        public GameState()
        {
            gameBoard = new Board();

        }

        public void CheckIfSelectedCell (int x, int y)
        {

            for (int i = 0; i < gameBoard.positions.GetLength(0); i++)
            {
                for (int j = 0; j < gameBoard.positions.GetLength(1); j++)
                {
                    if (gameBoard.positions[i, j].bounds.Contains(new Point(x, y)))
                    {
                        selectedCell = (i, j);
                        gameBoard.positions[i, j].occupied = true;
                        Debug.WriteLine("Ruta: " + selectedCell);
                    }

                }
            }
        }
    }
}
