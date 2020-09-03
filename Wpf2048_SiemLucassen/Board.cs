using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Wpf2048_SiemLucassen
{
    /// <summary>
    /// The board class represents the positions of all the tiles.
    /// It also provides methods to move the tiles and gets the current score.
    /// </summary>
    public class Board : INotifyPropertyChanged
    {
        //The random source for tile generation.
        private static readonly Random _random = new Random();

        //The board contains a row and column of 4 tiles each. (this is a 2D array)
        private readonly Tile[,] _tiles = new Tile[4, 4];

        //To easily bind the array of tiles as a list.
        public IEnumerable<Tile> Tiles => _tiles.OfType<Tile>();

        //Score of all tiles summed up.
        public int AllScore => CalculateScore();

        public event PropertyChangedEventHandler PropertyChanged;

        public Board()
        {
            InitializeBoard();
        }
        
        public void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool MoveTo(Move move)
        {
            bool moved = false;
            switch (move)
            {
                case Move.Left:
                    moved = MoveLeft();
                    break;
                case Move.Right:
                    moved = MoveRight();
                    break;
                case Move.Up:
                    moved = MoveUp();
                    break;
                case Move.Down:
                    moved = MoveDown();
                    break;

                default:
                    //There is no other Direction to move than Left, Right, Up and Down.
                    throw new ArgumentOutOfRangeException();
            }

            SetTilesToMoveable();

            if (moved)
            {
                GenerateTile();
                RaisePropertyChanged("AllScore");
            }
            return moved;
        }

        private void SetTilesToMoveable()
        {
            for (int i = 0; i < 3; i++)
            {
                //Loops over each row
                for (int row = 0; row < 4; row++)
                {
                    //Loops over all column besides the first column because the first tile cannot move to the left.
                    for (int col = 0; col < 4; col++)
                    {
                        _tiles[row, col].HasMoved = false;
                    }
                }
            }
        }

        private bool MoveLeft()
        {
            bool moved = false;
            //Checks if all tiles have the ability to move to the left tile 
            //and add together if they are the same score.

            //Forces the tile to move 3 times if possible.
            for (int i = 0; i < 3; i++)
            {
                //Loops over each row
                for (int row = 0; row < 4; row++)
                {
                    //Loops over all column besides the first column because the first tile cannot move to the left.
                    for (int col = 1; col < 4; col++)
                    {
                        //If the current tile is a free tile it cannot move so it continues with the next position.
                        if (_tiles[row, col].Score == 0 || _tiles[row, col].HasMoved || _tiles[row, col - 1].HasMoved)
                        {
                            continue;
                        }

                        //Checks if the tile to the left of the current column if the score is free (equal to 0).
                        if (_tiles[row, col - 1].Score == 0)
                        {
                            moved = true;
                            //Sets the score from the current position to the left tile.
                            _tiles[row, col - 1].Score = _tiles[row, col].Score;
                           
                            //Resets the score from the current position to a free tile (0 score).
                            _tiles[row, col].Score = 0;
                        }
                        //If the tile on the left is equal to the current score, it sums together on the left tile.
                        else if (_tiles[row, col - 1].Score == _tiles[row, col].Score)
                        {
                            _tiles[row, col - 1].HasMoved = true;
                            moved = true;
                            _tiles[row, col - 1].Score += _tiles[row, col].Score;
                            _tiles[row, col].Score = 0;

                        }
                    }
                }
            }

            return moved;
        }

        private bool MoveRight()
        {
            bool moved = false;
            //Checks if all tiles have the ability to move to the right tile 
            //and add together if they are the same score.

            //Forces the tile to move 3 times if possible.
            for (int i = 0; i < 3; i++)
            {
                //Loops over each row
                for (int row = 0; row < 4; row++)
                {
                    for (int col = 2; col >= 0; col--)
                    {
                        if (_tiles[row, col].Score == 0)
                        {
                            continue;
                        }

                        if (_tiles[row, col + 1].Score == 0)
                        {
                            moved = true;
                            _tiles[row, col + 1].Score = _tiles[row, col].Score;
                            _tiles[row, col].Score = 0;
                        }
                        else if (_tiles[row, col + 1].Score == _tiles[row, col].Score)
                        {
                            moved = true;
                            _tiles[row, col + 1].Score += _tiles[row, col].Score;
                            _tiles[row, col].Score = 0;
                        }
                    }
                }
            }

            return moved;
        }

        private bool MoveUp()
        {
            bool moved = false;
            //Checks if all tiles have the ability to move to the upper tile 
            //and add together if they are the same score.

            //Forces the tile to move 3 times if possible.
            for (int i = 0; i < 3; i++)
            {
                //Loops over each row
                for (int row = 1; row < 4; row++)
                {
                    for (int col = 0; col < 4; col++)
                    {
                        if (_tiles[row, col].Score == 0)
                        {
                            continue;
                        }

                        if (_tiles[row - 1, col].Score == 0)
                        {
                            moved = true;
                            _tiles[row - 1, col].Score = _tiles[row, col].Score;
                            _tiles[row, col].Score = 0;
                        }
                        else if (_tiles[row - 1, col].Score == _tiles[row, col].Score)
                        {
                            moved = true;
                            _tiles[row - 1, col].Score += _tiles[row, col].Score;
                            _tiles[row, col].Score = 0;
                        }
                    }
                }
            }

            return moved;
        }

        private bool MoveDown()
        {
            bool moved = false;
            //Checks if all tiles have the ability to move to the tile below
            //and add together if they are the same score.

            //Forces the tile to move 3 times if possible.
            for (int i = 0; i < 3; i++)
            {
                //Loops over each row
                for (int row = 2; row >= 0; row--)
                {
                    for (int col = 0; col < 4; col++)
                    {
                        if (_tiles[row, col].Score == 0)
                        {
                            continue;
                        }

                        if (_tiles[row + 1, col].Score == 0)
                        {
                            moved = true;
                            _tiles[row + 1, col].Score = _tiles[row, col].Score;
                            _tiles[row, col].Score = 0;
                        }
                        else if (_tiles[row + 1, col].Score == _tiles[row, col].Score)
                        {
                            moved = true;
                            _tiles[row + 1, col].Score += _tiles[row, col].Score;
                            _tiles[row, col].Score = 0;
                        }
                    }
                }
            }

            return moved;
        }

        private void GenerateTile()
        {
            //Collects all tiles where the free tiles (score is 0) and puts them in a list.
            List<Tile> freeTiles = Tiles.Where(t => t.Score == 0).ToList();

            //If there are no free tiles return, because the game is over when there are no free tiles left.          
            if (!freeTiles.Any())
            {
                //Game should allready be over.
                return;
            }

            //Generates a tile on a random position on the board with a 80% chance being 2 points
            //and a 20% chance being 4 points.

            int score = 4;

            //The random generates a number between 0 and 100 
            //and if the value is lesser than or equal to 80, sets score to 2.
            if (_random.Next(0, 100) <= 80)
            {
                score = 2;
            }

            //Gets the random position from the free tiles.
            int randomindex = _random.Next(freeTiles.Count);

            //Sets the score on a tile using the randomindex. 
            freeTiles[randomindex].Score = score;
        }

        private int CalculateScore()
        {
            int score = 0;

            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    score += _tiles[row, col].Score;
                }
            }
            return score;
        }

        private void InitializeBoard()
        {
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    _tiles[row, col] = new Tile
                    {
                        Row = row,
                        Column = col
                    };
                }
            }
            GenerateTile();
            GenerateTile();
        }
    }
}
