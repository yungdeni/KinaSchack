using KinaSchack.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinaSchack.Classes
{
    /// <summary>
    /// Holds information to save and restore a <c>GameState</c>
    /// </summary>
    public class SaveState
    {
        string _positions;
        string _currentPlayer;
        Players _playerInfo;
        public SaveState(GameState stateToLoad, Players players)
        {
            _positions = stateToLoad.GameBoard.GetPlayerPositions();
            _currentPlayer = stateToLoad.CurrentPlayer.ToString();
            _playerInfo = players;
        }
        public GameState ReturnSavedGameState()
        {
            GameState loaded = new GameState();
            loaded.GameBoard.SetPlayerPositions(_positions);
            loaded.CurrentPlayer = (BoardStatus)Enum.Parse(typeof(BoardStatus), _currentPlayer);
            return loaded;
        }
        //Doesnt do anything untill we implement saving/loading from file
        public Players ReturnSavedPlayers()
        {
            return _playerInfo;
        }
        //NYI
        public void SaveToFile()
        {
            //TODO
            //_positions string holds the boardstatuses of the board
            //Board class has methods to save and load from that string
            //The other info can be saved in a similliar way
            throw new NotImplementedException();
        }
        //NYI
        public void LoadFromFile()
        {
            throw new NotImplementedException();
        }

    }
    
}
