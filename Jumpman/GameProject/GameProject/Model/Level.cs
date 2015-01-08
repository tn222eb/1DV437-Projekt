using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;

namespace GameProject.Model
{
    class Level
    {
        public const int LEVEL_WIDTH = 40;
        public const int LEVEL_HEIGHT = 10;

        public Tile[,] m_tiles;

        private char m_blockedChar = 'x';
        private char m_playerStartChar = 's';
        private char m_holeChar = 'h';
        private char m_playerFinishChar = 'f';
        private char m_bombChar = 'e';
        private char m_coinChar = 'c';
        
        private Levels m_currentLevel;
        private List<Bomb> m_bombList = new List<Bomb>();
        private List<Coin> m_coinList = new List<Coin>(); 

        public enum Tile
        {
            EMPTY = 0,
            BLOCKED,
            HOLE,
            FINISH
        };

        public enum Levels
        {
            ONE = 0,
            TWO,
            THREE
        }

        public Vector2 PlayerStartingPosition
        {
            get;
            set;
        }

        public List<Bomb> Bombs
        {
            get
            {
                return m_bombList;
            }
        }

        public List<Coin> Coins
        {
            get
            {
                return m_coinList;
            }
        }

        public Levels CurrentLevel
        {
            get { return m_currentLevel; }
            set { m_currentLevel = value; }
        }

        public Level()
        {
            m_tiles = new Tile[LEVEL_WIDTH, LEVEL_HEIGHT];

            m_currentLevel = Levels.ONE;
            LoadLevel();
        }

        public void LoadLevel()
        {
            string levelString;

            switch (m_currentLevel)
            {
                case Levels.ONE:
                    levelString = ImportLevel.ReadLevel(1);
                    GenerateLevel(levelString);
                    break;

                case Levels.TWO:
                    levelString = ImportLevel.ReadLevel(2);
                    GenerateLevel(levelString);
                    break;

                case Levels.THREE:
                    levelString = ImportLevel.ReadLevel(3);
                    GenerateLevel(levelString);
                    break;
            }
        }

        public void GenerateLevel(string levelString)
        {
            m_bombList.Clear();
            m_coinList.Clear();

            for (int x = 0; x < LEVEL_WIDTH; x++)
            {
                for (int y = 0; y < LEVEL_HEIGHT; y++)
                {
                    int index = y * LEVEL_WIDTH + x;

                    if (levelString[index] == m_blockedChar)
                    {
                        m_tiles[x, y] = Tile.BLOCKED;
                    }

                    else if (levelString[index] == m_playerStartChar)
                    {
                        PlayerStartingPosition = new Vector2(x + 0.5f, y + 0.99f);
                        m_tiles[x, y] = Tile.EMPTY;
                    }

                    else if (levelString[index] == m_holeChar) 
                    {
                        m_tiles[x, y] = Tile.HOLE;
                    }

                    else if (levelString[index] == m_playerFinishChar) 
                    {
                        m_tiles[x - 1, y] = Tile.FINISH;
                    }

                    else if (levelString[index] == m_bombChar) 
                    {
                        m_bombList.Add(new Bomb(new Vector2(x + 0.5f, y + 1)));
                        m_tiles[x, y] = Tile.EMPTY;
                    }

                    else if (levelString[index] == m_coinChar) 
                    {
                        m_coinList.Add(new Coin(new Vector2(x + 0.5f, y + 1.5f))); 
                        m_tiles[x, y] = Tile.EMPTY;
                    }

                    else
                    {
                        m_tiles[x, y] = Tile.EMPTY;
                    }
                }
            }
        }

        public bool IsInHole(Vector2 position, Vector2 size)
        {
            Vector2 topLeft = new Vector2(position.X - size.X / 2.0f, position.Y - size.Y);
            Vector2 bottomRight = new Vector2(position.X + size.X / 2.0f, position.Y);

            for (int x = 0; x < LEVEL_WIDTH; x++)
            {
                for (int y = 0; y < LEVEL_HEIGHT; y++)
                {
                    if (bottomRight.X < (float)x)
                        continue;
                    if (bottomRight.Y < (float)y + 1.0f)
                        continue;
                    if (topLeft.X > (float)x + 1.0f)
                        continue;
                    if (topLeft.Y > (float)y + 1.0f)
                        continue;

                    if (m_tiles[x, y] == Tile.HOLE)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool IsAtLevelFinish(Vector2 position, Vector2 size)
        {
            Vector2 topLeft = new Vector2(position.X - size.X / 2.0f, position.Y - size.Y);
            Vector2 bottomRight = new Vector2(position.X + size.X / 2.0f, position.Y);

            for (int x = 0; x < LEVEL_WIDTH; x++)
            {
                for (int y = 0; y < LEVEL_HEIGHT; y++)
                {
                    if (bottomRight.X < (float)x + 1.0f)
                        continue;
                    if (bottomRight.Y < (float)y)
                        continue;
                    if (topLeft.X > (float)x + 1.0f)
                        continue;
                    if (topLeft.Y > (float)y + 1.0f)
                        continue;

                    if (m_tiles[x, y] == Tile.FINISH)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool IsCollidingAt(FloatRectangle a_rect)
        {
            Vector2 tileSize = new Vector2(1, 1);

            for (int x = 0; x < LEVEL_WIDTH; x++)
            {
                for (int y = 0; y < LEVEL_HEIGHT; y++)
                {
                    FloatRectangle rect = FloatRectangle.createFromTopLeft(new Vector2(x, y), tileSize);
                    if (a_rect.isIntersecting(rect))
                    {
                        if (m_tiles[x, y] == Tile.BLOCKED)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public bool IsBombStandingByEdgeOfBlocked(Vector2 position)
        {
            if ((m_tiles[(int)position.X, (int)position.Y] == Tile.HOLE) || m_tiles[(int)position.X, (int)position.Y] == Tile.EMPTY)
            {
                return true;
            }
            return false;
        }
    }
}