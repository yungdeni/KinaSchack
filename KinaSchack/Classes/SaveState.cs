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
        private string _positions;
        private string _currentPlayer;
        private Players _playerInfo;
        public SaveState(GameState stateToLoad, Players players)
        {
            _positions = stateToLoad.GameBoard.GetPlayerPositions();
            _currentPlayer = stateToLoad.CurrentPlayer.ToString();
            _playerInfo = players;
        }
        /// <summary>
        /// Creates a new <c>GameState</c>, sets the saved positions and the current player and returns it
        /// </summary>
        /// <returns><c>GameState</c></returns>
        public GameState ReturnSavedGameState()
        {
            GameState loaded = new GameState();
            loaded.GameBoard.SetPlayerPositions(_positions);
            loaded.CurrentPlayer = (BoardStatus)Enum.Parse(typeof(BoardStatus), _currentPlayer);
            return loaded;
        }
        /// <summary>
        /// Not yet implemented properly
        /// </summary>
        public Players ReturnSavedPlayers()
        {
            return _playerInfo;
        }
        /// <summary>
        /// Not yet implemented, calling this method results in an exception
        /// </summary>
        public void SaveToFile()
        {
            //TODO
            //_positions string holds the boardstatuses of the board
            //Board class has methods to save and load from that string
            //The other info can be saved in a similiar way
            throw new NotImplementedException();
        }
        /// <summary>
        /// Not yet implemented, calling this method results in an exception
        /// </summary>
        public void LoadFromFile()
        {
            throw new NotImplementedException();
        }
    }
}
