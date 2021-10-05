using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using KinaSchack.Enums;

namespace KinaSchack.Classes
{
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

            //positions[5, 5].occupied = true;

        }
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
        public void SetPlayerPositions(string saveString)
        {
            var positions = saveString.Split(",");
            for (int i = 0; i < Cells.GetLength(0); i++)
            {
                for (int j = 0; j < Cells.GetLength(1); j++)
                {
                    Cells[i, j].Item1 = (BoardStatus)Enum.Parse(typeof(BoardStatus), positions[i * Cells.GetLength(0) + j]);
                }
            }
        }

    }
}
