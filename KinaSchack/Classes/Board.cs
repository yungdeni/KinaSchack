using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace KinaSchack.Classes
{
    public class Board
    {
        public (boardstatus, Rect bounds)[,] positions;
        public enum boardstatus
        {
            Player1, Player2, Empty
        }
        public Board()
        {
            positions = new (boardstatus, Rect)[7, 7];
            for (int i = 0; i < 7; i++)
            {
                positions[i, 0] = (boardstatus.Empty, new Rect(new Point(100 + (115 * i), 80), new Point(160 + (115 * i), 150)));
                positions[i, 1] = (boardstatus.Empty, new Rect(new Point(100 + (115 * i), 195), new Point(160 + (115 * i), 270)));
                positions[i, 2] = (boardstatus.Empty, new Rect(new Point(100 + (115 * i), 305), new Point(160 + (115 * i), 380)));
                positions[i, 3] = (boardstatus.Empty, new Rect(new Point(100 + (115 * i), 420), new Point(160 + (115 * i), 500)));
                positions[i, 4] = (boardstatus.Empty, new Rect(new Point(100 + (115 * i), 540), new Point(160 + (115 * i), 620)));
                positions[i, 5] = (boardstatus.Empty, new Rect(new Point(100 + (115 * i), 655), new Point(160 + (115 * i), 735)));
                positions[i, 6] = (boardstatus.Empty, new Rect(new Point(100 + (115 * i), 775), new Point(160 + (115 * i), 845)));


            }
            positions[0, 0].Item1 = boardstatus.Player2;
            positions[1, 1].Item1 = boardstatus.Player2;
            positions[1, 2].Item1 = boardstatus.Player2;
            positions[0, 1].Item1 = boardstatus.Player2;
            positions[1, 0].Item1 = boardstatus.Player2;
            positions[3, 0].Item1 = boardstatus.Player2;
            positions[2, 0].Item1 = boardstatus.Player2;
            positions[2, 1].Item1 = boardstatus.Player2;
            positions[0, 3].Item1 = boardstatus.Player2;
            positions[0, 2].Item1 = boardstatus.Player2;

            positions[6, 6].Item1 = boardstatus.Player1;
            positions[6, 5].Item1 = boardstatus.Player1;
            positions[6, 4].Item1 = boardstatus.Player1;
            positions[6, 3].Item1 = boardstatus.Player1;
            positions[5, 6].Item1 = boardstatus.Player1;
            positions[5, 5].Item1 = boardstatus.Player1;
            positions[5, 4].Item1 = boardstatus.Player1;
            positions[4, 5].Item1 = boardstatus.Player1;
            positions[4, 6].Item1 = boardstatus.Player1;
            positions[3, 6].Item1 = boardstatus.Player1;

            //positions[5, 5].occupied = true;

        }

    }
}
