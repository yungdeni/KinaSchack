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
        public (int x, int y, bool occupied, Rect bounds)[,] positions;

        public Board()
        {
            positions = new (int, int,bool,Rect)[7, 7];
            for (int i = 0; i < 7; i++)
            {
                positions[i, 0] = (127 + (115 * i), 117 , false, new Rect(new Point(100 + (115 * i), 80), new Point(160 + (115 * i), 150)));
                positions[i, 1] = (127 + (115 * i), 232, false, new Rect(new Point(100 + (115 * i), 195), new Point(160 + (115 * i), 270)));
                positions[i, 2] = (127 + (115 * i), 348, false, new Rect(new Point(100 + (115 * i), 305), new Point(160 + (115 * i), 380)));
                positions[i, 3] = (127 + (115 * i), 464, false, new Rect(new Point(100 + (115 * i), 420), new Point(160 + (115 * i), 500)));
                positions[i, 4] = (127 + (115 * i), 580, false, new Rect(new Point(100 + (115 * i), 540), new Point(160 + (115 * i), 620)));
                positions[i, 5] = (127 + (115 * i), 696, false, new Rect(new Point(100 + (115 * i), 655), new Point(160 + (115 * i), 735)));
                positions[i, 6] = (127 + (115 * i), 812, false, new Rect(new Point(100 + (115 * i), 775), new Point(160 + (115 * i), 845)));


            }

            //positions[5, 5].occupied = true;

        }

    }
}
