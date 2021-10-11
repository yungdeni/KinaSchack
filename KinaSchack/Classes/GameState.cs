using System;
using System.Collections.Generic;
using Windows.Foundation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using KinaSchack.Enums;
using Windows.Media.Playback;

namespace KinaSchack.Classes
{
    /// <summary>
    /// Class <c>GameState</c> models the state of the current game
    /// </summary>
    public class GameState
    {
        public Board GameBoard;
        public BoardStatus CurrentPlayer;
        public (int x, int y) SelectedCell;
        public (int a, int b) NewSelectedCell;
        public (int x, int y) LastMove;
        public bool PieceSelected;
        public int PlayerCount;
        public bool ShowTombstone;
        public List<(int x, int y)> PossibleMoves;
        private List<(int x, int y)> _jumps;
        public Queue<AnimatePiece> AnimationQueue;
        private Dictionary<(int x, int y), List<(int x, int y)>> _sequentialJumps;

        public GameState()
        {

            GameBoard = new Board();
            CurrentPlayer = BoardStatus.Player1;
            PieceSelected = false;
            PossibleMoves = new List<(int x, int y)>();
            _jumps = new List<(int x, int y)>()
            {
                (2,0),(0,2),(-2,0),(0,-2),(2,2),(-2,-2)
            };
            AnimationQueue = new Queue<AnimatePiece>();
            _sequentialJumps = new Dictionary<(int x, int y), List<(int x, int y)>>();
            ShowTombstone = false;
            PlayerCount = 0;
        }
        /// <summary>
        /// Method <c>GetSelectedCell returns the index array of the gameboard from absolute mouse positions</c>
        /// </summary>
        /// <param name="x">x-coordinate of window position</param>
        /// <param name="y">y-coordinate of window position</param>
        /// <returns>(x,y) tuple</returns>
        public (int x, int y) GetSelectedCell(int x, int y)
        {
            for (int i = 0; i < GameBoard.Cells.GetLength(0); i++)
            {
                for (int j = 0; j < GameBoard.Cells.GetLength(1); j++)
                {
                    if (GameBoard.Cells[i, j].bounds.Contains(new Point(x, y)))
                    {
                        //Debug.WriteLine("Ruta: " + i + ", " + j);
                        return (i, j);
                    }
                }
            }
            return (-1, -1);
        }
        /// <summary>
        /// Method <c>CheckIfPlayersPiece</c> checks if the gameboard cell position has a piece that belongs to the current player
        /// </summary>
        /// <param name="cellPosition">Tuple of the index into the gameboard</param>
        /// <returns></returns>
        public bool CheckIfPlayersPiece((int x, int y) cellPosition)
        {
            return CurrentPlayer == GameBoard.Cells[cellPosition.x, cellPosition.y].Item1;
        }
        /// <summary>
        /// Method <c>FillAnimationQueue</c> sets the queue of animations to sequential jumps
        /// </summary>
        /// <param name="newPosition">Tuple that indexes into the gameboard for the end position of a piece</param>
        public void FillAnimationQueue((int x, int y) newPosition)
        {
            //A single move is trivial, just enqueue a straight path and return
            if (Math.Abs(newPosition.x - SelectedCell.x) == 1 || Math.Abs(newPosition.y - SelectedCell.y) == 1)
            {
                AnimationQueue.Enqueue(new AnimatePiece(GameBoard.Cells[SelectedCell.x, SelectedCell.y].bounds, GameBoard.Cells[newPosition.x, newPosition.y].bounds, CurrentPlayer));
                return;
            }
            var animationPositions = _sequentialJumps[newPosition];
            var startPos = SelectedCell;
            foreach (var pos in animationPositions)
            {
                AnimationQueue.Enqueue(new AnimatePiece(GameBoard.Cells[startPos.x, startPos.y].bounds, GameBoard.Cells[pos.x, pos.y].bounds, CurrentPlayer));
                startPos = pos;
            }
        }
        /// <summary>
        /// Method <c>Move</c> Moves the piece from SelectedCell position to the new position
        /// </summary>
        /// <param name="newPosition">Tuple that indexes into the gameboard for the end position of a piece</param>
        public void Move((int x, int y) newPosition)
        {
            LastMove = newPosition;
            FillAnimationQueue(newPosition);
            GameBoard.Cells[newPosition.x, newPosition.y].Item1 = CurrentPlayer;
            GameBoard.Cells[SelectedCell.x, SelectedCell.y].Item1 = BoardStatus.Empty;

        }
        /// <summary>
        /// Method <c>CheckIfNewPositionIsEmpty</c> checks if the gameboard cell position is empty
        /// </summary>
        /// <param name="newPosition">Tuple of the index into the gameboard</param>
        /// <returns></returns>
        public bool CheckIfNewPositionIsEmpty((int x, int y) newPosition)
        {
            return GameBoard.Cells[newPosition.x, newPosition.y].Item1 == BoardStatus.Empty;
        }
        /// <summary>
        /// Method <c>CheckIfJumpIsLegal</c> checks if a jump is valid
        /// </summary>
        /// <param name="newPosition">Endposition of the jump</param>
        /// <param name="startPosition">Startposition of the jump</param>
        /// <returns></returns>
        public bool CheckIfJumpIsLegal((int x, int y) newPosition, (int x, int y) startPosition)
        {
            (int x, int y) midPoint = ((newPosition.x + startPosition.x) / 2, (newPosition.y + startPosition.y) / 2);
            if (CheckIfNewPositionIsEmpty(midPoint) || GameBoard.Cells[midPoint.x, midPoint.y].Item1 == BoardStatus.Tombstone)
            {
                return false;
            }
            if (!CheckIfNewPositionIsEmpty(newPosition))
            {
                return false;
            }
            Debug.WriteLine("Correct");
            return true;
        }
        /// <summary>
        /// Method <c>GetPossibleMoves</c> returns all possible moves that the piece in SelectedCell can do. Adds the path for each end position in _sequentialjumps
        /// </summary>
        /// <returns>A list of tuples containing all possible moves</returns>
        public List<(int x, int y)> GetPossibleMoves()
        {
            _sequentialJumps.Clear();
            //Seed the list with possible jumps from starting position
            List<(int x, int y)> moves = new List<(int x, int y)>();
            foreach ((int x, int y) in _jumps)
            {
                int newX = x + SelectedCell.x;
                int newY = y + SelectedCell.y;
                if (newX >= GameBoard.Cells.GetLength(0) || newX < 0 || newY >= GameBoard.Cells.GetLength(1) || newY < 0)
                {
                    continue;
                }
                if (CheckIfNewPositionIsEmpty((newX, newY)) && CheckIfJumpIsLegal((newX, newY), (SelectedCell.x, SelectedCell.y)))
                {

                    moves.Add((newX, newY));
                }
            }
            foreach (var move in moves)
            {
                _sequentialJumps.Add(move, new List<(int x, int y)>() { move });
            }
            //Do the same thing with the new possible jumps as starting points
            List<(int x, int y)> copyOfPossibleMoves;
            do
            {
                copyOfPossibleMoves = moves.ToList();
                foreach (var startPos in copyOfPossibleMoves)
                {
                    foreach ((int x, int y) in _jumps)
                    {
                        int newX = x + startPos.x;
                        int newY = y + startPos.y;
                        if (newX >= GameBoard.Cells.GetLength(0) || newX < 0 || newY >= GameBoard.Cells.GetLength(1) || newY < 0)
                        {
                            continue;
                        }
                        if (CheckIfNewPositionIsEmpty((newX, newY)) && CheckIfJumpIsLegal((newX, newY), (startPos.x, startPos.y)))
                        {
                            if (!moves.Contains((newX, newY)))
                            {
                                moves.Add((newX, newY));
                                var jumpsToAdd = _sequentialJumps[startPos].ToList();
                                jumpsToAdd.Add((newX, newY));
                                _sequentialJumps.Add((newX, newY), jumpsToAdd);
                            }
                        }
                    }
                }
                //We are done when there are no more possible jumps to make
            } while (copyOfPossibleMoves.Count() != moves.Count());
            //Add the single moves
            foreach ((int x, int y) in _jumps)
            {
                int newX = (x / 2) + SelectedCell.x;
                int newY = (y / 2) + SelectedCell.y;
                if (newX >= GameBoard.Cells.GetLength(0) || newX < 0 || newY >= GameBoard.Cells.GetLength(1) || newY < 0)
                {
                    continue;
                }
                if (CheckIfNewPositionIsEmpty((newX, newY)))
                {
                    moves.Add((newX, newY));
                }
            }
            return moves;
        }
        /// <summary>
        /// Method <c>Gets x, y coordinate from a mouseclick/movement in the gui and handles them according to the internal state of the game</c>
        /// </summary>
        /// <param name="x">x-coordinate</param>
        /// <param name="y">y-coordinate</param>
        public void HandleTurn(int x, int y)
        {
            //Check if player clicked on a cell
            if (GetSelectedCell(x, y) == (-1, -1))
            {
                SelectedCell = (-1, -1);
                PieceSelected = false;
                PossibleMoves.Clear();
                return;
            }
            if (CheckIfPlayersPiece(GetSelectedCell(x, y)))
            {
                PossibleMoves.Clear();
                SelectedCell = GetSelectedCell(x, y);
                PossibleMoves = GetPossibleMoves();
                PieceSelected = true;
                return;
            }
            if (PieceSelected)
            {
                (int x, int y) newPos = GetSelectedCell(x, y);
                if (PossibleMoves.Contains(newPos))
                {
                    Move(newPos);
                    PieceSelected = false;

                    if (CheckIfVictory())
                    {
                        //Debug.WriteLine("Winner");
                        MainPage.Audio.PlayJumpSound();
                        MainPage.Audio.PlayWinnerSound();
                        MainPage.IsWInner = true;
                    }
                    else
                    {
                        PlayerCount++;
                        //Takes the integral int value behind the enum and flips it from 0 : Player1 and 1: Player2
                        CurrentPlayer = (BoardStatus)(((int)CurrentPlayer) ^ 1);
                        MainPage.Audio.PlayJumpSound();

                        //Set to random turns?
                        //int randomNo2 = rand.Next(0, 6);
                        if (PlayerCount % 5 == 0)
                        {
                            ShowTombstone = true;
                        }
                        else
                        {
                            ShowTombstone = false;
                        }
                        TombStoneFeature();
                        PossibleMoves.Clear();
                    }
                }
            }
        }
        /// <summary>
        /// Loops through the gameboard and checks if a player has won the game
        /// </summary>
        /// <returns></returns>
        public bool CheckIfVictory()
        {
            int a = 4;
            if (CurrentPlayer == BoardStatus.Player1)
            {
                for (int i = 0; i <= 3; i++)
                {
                    a--;
                    for (int j = 0; j <= a; j++)
                    {
                        //Debug.WriteLine(a);
                        //Debug.WriteLine(i + ", " + j);
                        if (GameBoard.Cells[j, i].Item1 != BoardStatus.Player1)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            else if (CurrentPlayer == BoardStatus.Player2)
            {
                a = 2;
                for (int b = 6; b >= 3; b--)
                {
                    a++;
                    for (int c = 6; c >= a; c--)
                    {
                        //Debug.WriteLine(a);
                        //Debug.WriteLine(b + ", " + c);
                        if (GameBoard.Cells[c, b].Item1 != BoardStatus.Player2)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// Sets a random gameboardcell to a tombstone
        /// </summary>
        public void TombStoneFeature()
        {
            var random = new Random();
            List<(int x, int y)> emptyCells;
            emptyCells = new List<(int x, int y)>();

            if (ShowTombstone)
            {
                for (int i = 0; i <= 6; i++)
                {
                    for (int j = 0; j <= 6; j++)
                    {
                        Debug.WriteLine(i + ", " + j);
                        if (GameBoard.Cells[j, i].Item1 == BoardStatus.Empty)
                        {
                            emptyCells.Add((j, i));
                        }
                    }
                }
                var index = emptyCells[random.Next(emptyCells.Count)];
                int randX = index.x;
                int randY = index.y;
                GameBoard.Cells[randX, randY].Item1 = BoardStatus.Tombstone;
            }
            else
            {
                for (int i = 0; i <= 6; i++)
                {
                    for (int j = 0; j <= 6; j++)
                    {
                        Debug.WriteLine(i + ", " + j);
                        if (GameBoard.Cells[j, i].Item1 == BoardStatus.Tombstone)
                        {
                            GameBoard.Cells[j, i].Item1 = BoardStatus.Empty;
                        }
                    }
                }
            }
        }
    }
}
