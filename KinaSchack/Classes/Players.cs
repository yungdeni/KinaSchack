using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using KinaSchack.Classes;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Windows.Storage;

namespace KinaSchack.Classes
{
    /// <summary>
    /// Class <c>Players</c> sets the players name from textbox input
    /// </summary>
    public class Players : INotifyPropertyChanged
    {
        private string _player1;
        private string _player2;

        public event PropertyChangedEventHandler PropertyChanged;

        public Players()
        {
            _player1 = "Player1";
            _player2 = "Player2";
        }
        public string Player1
        {
            get => _player1;
            set
            {
                _player1 = value;
                OnPropertyChanged();
            }
        }
        public string Player2
        {
            get => _player2;
            set
            {
                _player2 = value;
                OnPropertyChanged();
            }
        }
        public void OnPropertyChanged([CallerMemberName] string playerName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(playerName));
        }
    }
}