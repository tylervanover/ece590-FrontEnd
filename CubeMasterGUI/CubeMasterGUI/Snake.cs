﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CubeMasterGUI
{
    public class Snake
    {
        private const int DIMENSION = 8;
        private CubeController.Cube _cube;
        private Dictionary<string, DIFFICULTY> _difficultyDictionary;
        private int _score;

        private enum DIRECTION { POSITIVE_X, POSITIVE_Y, NEGATIVE_X, NEGATIVE_Y };
        private enum DIFFICULTY { EASY, MEDIUM, HARD };

        private DIRECTION _currentDirection;
        private Timer _gameTimer;
        private Timer _foodBlinkTimer;

        private SnakeSection _head;
        private SnakeSection _food;
        private List<SnakeSection> _snake;

        private bool _foodIsOnTheTable = false;
        private bool _eating = false;

        private Random _random;
        
        public Snake(ref CubeController.Cube cube)
        {
            _cube = cube;
            _currentDirection = DIRECTION.POSITIVE_X;
            _head = new SnakeSection();
            _food = new SnakeSection();
            _snake = new List<SnakeSection>();
            _random = new Random();
            _gameTimer = new Timer();
            _foodBlinkTimer = new Timer();
            _difficultyDictionary = new Dictionary<string, DIFFICULTY>
            {
                {"btnEasy", DIFFICULTY.EASY},
                {"btnMedium", DIFFICULTY.MEDIUM},
                {"btnHard", DIFFICULTY.HARD}
            };
        }

        public void StartNewGame()
        {
            _score = 0;

            _snake.Add(_head);

            _gameTimer.Interval = 750; // needs to be based off of selected speed
            _gameTimer.Tick += GameTimerTick;
            _foodBlinkTimer.Interval = 1000 / 4;
            _foodBlinkTimer.Tick += FoodTimerTick;

            _gameTimer.Start();
            _foodBlinkTimer.Start();
        }

        private void FoodTimerTick(object sender, EventArgs e)
        {
            if (_foodIsOnTheTable)
                _cube.SwapVoxel(_food.X, _food.Y, 0);
        }

        private void SpawnFood()
        {
            bool valid = false;
            while (!valid)
            {
                _food.X = _random.Next(DIMENSION);
                _food.Y = _random.Next(DIMENSION);
                valid = true;
                foreach (var s in _snake)
                {
                    if (_food == s)
                        valid = false;
                }
            }
            _foodIsOnTheTable = true;
            _cube.SetVoxel(_food.X, _food.Y, 0);
        }

        public void ChangeDifficultySetting(string s)
        {
            _gameTimer.Stop();
            DIFFICULTY diff = _difficultyDictionary[s];
            switch(diff)
            {
                case DIFFICULTY.EASY:
                    _gameTimer.Interval = 700;
                    break;
                case DIFFICULTY.MEDIUM:
                    _gameTimer.Interval = 400;
                    break;
                case DIFFICULTY.HARD:
                    _gameTimer.Interval = 175;
                    break;
                default:
                    break;
            }
            _gameTimer.Start();
        }

        private void GameTimerTick(object sender, EventArgs e)
        {
            if (!_foodIsOnTheTable)
                SpawnFood();
            
            if (_food == _head)
            {
                _snake.Add(new SnakeSection(_head.X, _head.Y));
                _eating = true;
                _score++;
                SpawnFood();
            }

            DisplaySnake();

            if (CheckForCollision())
            {
                _gameTimer.Stop();
                _foodBlinkTimer.Stop();
                _snake.Clear();
                _cube.ClearEntireCube();
                MessageBox.Show("Game Over!");
            }
        }

        private void DisplaySnake()
        {
            int count = _snake.Count;
            _cube.SetVoxel(_head.X, _head.Y, 0);

            if (!_eating)
                _cube.ClearVoxel(_snake[count - 1].X, _snake[count - 1].Y, 0);
            else
            {
                _cube.SetVoxel(_snake[count - 1].X, _snake[count - 1].Y, 0);
                _eating = false;
            }

            for (int i = count - 1; i > 0; i--)
            {
                _snake[i].X = _snake[i - 1].X;
                _snake[i].Y = _snake[i - 1].Y;

            }
            MoveHeadOfSnake();
            _cube.SetVoxel(_head.X, _head.Y, 0);
        }

        private void MoveHeadOfSnake()
        {
            switch (_currentDirection)
            {
                case DIRECTION.POSITIVE_X:
                    _head.X++;
                    _head.X %= DIMENSION;
                    break;
                case DIRECTION.POSITIVE_Y:
                    _head.Y++;
                    _head.Y %= DIMENSION;
                    break;
                case DIRECTION.NEGATIVE_X:
                    if (_head.X == 0)
                        _head.X = DIMENSION - 1;
                    else
                        _head.X--;
                    break;
                case DIRECTION.NEGATIVE_Y:
                    if (_head.Y == 0)
                        _head.Y = DIMENSION - 1;
                    else
                        _head.Y--;
                    break;
                default:
                    break;
            }
        }

        private bool CheckForCollision()
        {
            for (int i = 1; i < _snake.Count; i++)
            {
                if (_head == _snake[i])
                    return true;
            }
            return false;
        }
        
        public void ChangeInputState(Keys key)
        {
            switch (key)
            {
                case Keys.Left:
                case Keys.A:
                    if (_currentDirection == DIRECTION.NEGATIVE_Y)
                        _currentDirection = DIRECTION.POSITIVE_X;
                    else
                        _currentDirection++;
                    break;
                case Keys.Right:
                case Keys.D:
                    if (_currentDirection == DIRECTION.POSITIVE_X)
                        _currentDirection = DIRECTION.NEGATIVE_Y;
                    else
                        _currentDirection--;
                    break;
                default:
                    break;
            }
        }
                
        public bool[][] GetPlane(int plane)
        {
            return _cube.GetPlane(CubeController.Cube.AXIS.AXIS_Z, plane);
        }

        public string GetScore()
        {
            return _score.ToString();
        }
    }

    internal class SnakeSection
    {
        public int X { get; set; }
        public int Y { get; set; }

        public SnakeSection()
        {
            X = 0;
            Y = 0;
        }

        public SnakeSection(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static bool operator ==(SnakeSection a, SnakeSection b)
        {
            if (Object.ReferenceEquals(a,b))
                return true;
            if (((object)a == null) || ((object)b == null)) 
                return false;
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(SnakeSection a, SnakeSection b)
        {
            return !(a == b);
        }
    }
}