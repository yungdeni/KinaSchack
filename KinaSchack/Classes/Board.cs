using System;
using System.Text;
using Windows.Foundation;
using KinaSchack.Enums;

namespace KinaSchack.Classes
{
    /// <summary>
    /// <c>Board </c> Represents the gameboard. Contains the cells in a 2D-array with a <c>BoardStatus</c> and a <c>Rect</c>
    /// </summary>
    public class Board
    {
        public (BoardStatus, Rect bounds)[,] Cells;
        public Board()
        {
            Cells = new (BoardStatus, Rect)[7, 7];
            for (int i = 0; i < 7; i++)
            {
                Cells[i, 0] = (BoardStatus.Empty, new Rect(new Point(100 + (115 * i), 80), new Point(160 + (115 * i), 150)));
                Cells[i, 1] = (BoardStatus.Empty, new Rect(new Point(100 + (115 * i), 195), new Point(160 + (115 * i), 270)));
                Cells[i, 2] = (BoardStatus.Empty, new Rect(new Point(100 + (115 * i), 305), new Point(160 + (115 * i), 380)));
                Cells[i, 3] = (BoardStatus.Empty, new Rect(new Point(100 + (115 * i), 420), new Point(160 + (115 * i), 500)));
                Cells[i, 4] = (BoardStatus.Empty, new Rect(new Point(100 + (115 * i), 540), new Point(160 + (115 * i), 620)));
                Cells[i, 5] = (BoardStatus.Empty, new Rect(new Point(100 + (115 * i), 655), new Point(160 + (115 * i), 735)));
                Cells[i, 6] = (BoardStatus.Empty, new Rect(new Point(100 + (115 * i), 775), new Point(160 + (115 * i), 845)));
            }
            Cells[0, 0].Item1 = BoardStatus.Player2;
            Cells[1, 1].Item1 = BoardStatus.Player2;
            Cells[1, 2].Item1 = BoardStatus.Player2;
            Cells[0, 1].Item1 = BoardStatus.Player2;
            Cells[1, 0].Item1 = BoardStatus.Player2;
            Cells[3, 0].Item1 = BoardStatus.Player2;
            Cells[2, 0].Item1 = BoardStatus.Player2;
            Cells[2, 1].Item1 = BoardStatus.Player2;
            Cells[0, 3].Item1 = BoardStatus.Player2;
            Cells[0, 2].Item1 = BoardStatus.Player2;

            Cells[6, 6].Item1 = BoardStatus.Player1;
            Cells[6, 5].Item1 = BoardStatus.Player1;
            Cells[6, 4].Item1 = BoardStatus.Player1;
            Cells[6, 3].Item1 = BoardStatus.Player1;
            Cells[5, 6].Item1 = BoardStatus.Player1;
            Cells[5, 5].Item1 = BoardStatus.Player1;
            Cells[5, 4].Item1 = BoardStatus.Player1;
            Cells[4, 5].Item1 = BoardStatus.Player1;
            Cells[4, 6].Item1 = BoardStatus.Player1;
            Cells[3, 6].Item1 = BoardStatus.Player1;
        }
        /// <summary>
        /// Gets the player positions from the Cells array
        /// </summary>
        /// <returns>A comma seperated string with the BoardStatuses</returns>
        public string GetPlayerPositions()
        {
            StringBuilder positions = new StringBuilder();
            for (int i = 0; i < Cells.GetLength(0); i++)
            {
                for (int j = 0; j < Cells.GetLength(1); j++)
                {
                    positions.Append(Cells[i, j].Item1.ToString() + ",");
                }
            }
            positions.Remove(positions.Length - 1, 1);
            return positions.ToString();
        }
        /// <summary>
        /// Gets the player positions to the Cells array
        /// </summary>
        /// <param name="saveString">A comma seperated string with the boardstatuses</param>
        public void SetPlayerPositions(string saveString)
        {
            if (saveString is null)
            {
                return;
            }
            string[] positions = saveString.Split(",");
            if (positions.Length != 49)
            {
                return;
            }
            for (int i = 0; i < Cells.GetLength(0); i++)
            {
                for (int j = 0; j < Cells.GetLength(1); j++)
                {
                    Cells[i, j].Item1 = (BoardStatus)Enum.Parse(typeof(BoardStatus), positions[(i * Cells.GetLength(0)) + j]);
                }
            }
        }
    }
}
