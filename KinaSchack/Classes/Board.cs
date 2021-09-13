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
            positions = new (int, int,bool,Rect)[6, 6];
            for (int i = 0; i < 6; i++)
            {
                positions[i, 0] = (50 + (150 * i), 50 , false, new Rect(new Point(25 + (150 * i), 25), new Point(75 + (150 * i), 75)));
                //positions[i, 1] = (50 + (150 * i), 200, true);
                //positions[i, 2] = (50 + (150 * i), 350, true);
                //positions[i, 3] = (50 + (150 * i), 500, false);
                //positions[i, 4] = (50 + (150 * i), 650, false);
                //positions[i, 5] = (50 + (150 * i), 800, false);


            }

            //positions[5, 5].occupied = true;

        }

    }
}
