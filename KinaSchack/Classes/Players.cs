using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinaSchack.Classes
{
    class Players
    {
        public string Player1 { get; set; }
        public string Player2 { get; set; }

        public Players()
        {
            Player1 = "Player1";
            Player2 = "Player2";
        }

        //If textbox is empty, keep default value else set input from player
        public void SetPlayersName(string player1, string player2)
        {
            if (string.IsNullOrEmpty(player1) == false && string.IsNullOrEmpty(player2) == false)
            {
                Player1 = player1;
                Player2 = player2;
            }
            else if (string.IsNullOrEmpty(player1) && string.IsNullOrEmpty(player2) == false)
            {
                Player2 = player2;
            }
            else if (string.IsNullOrEmpty(player2) && string.IsNullOrEmpty(player1) == false)
            {
                Player1 = player1;
            }
            Debug.WriteLine(Player1 + " " + Player2);
        }
    }
}
