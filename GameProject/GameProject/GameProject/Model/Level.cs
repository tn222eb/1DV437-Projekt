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
        public const int LEVEL_WIDTH = 22;
        public const int LEVEL_HEIGHT = 7;
        private const int LEVEL_ONE = 1;
        private const int LEVEL_TWO = 2;

        public Tile[,] m_tiles = new Tile[LEVEL_WIDTH, LEVEL_HEIGHT];

        private char m_blockedChar = 'x';
        private char m_playerStartChar = 's';
        private char m_holeChar = 'h';
        private char m_playerFinishChar = 'f';

        private Levels m_currentLevel;

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
            TWO = 1
        }

        public Vector2 PlayerStartingPosition
        {
            get;
            set;
        }

        public Levels CurrentLevel
        {
            get { return m_currentLevel; }
            set { m_currentLevel = value; }
        }

        internal void LoadLevel()
        {
            string levelString;

            switch (m_currentLevel)
            {
                case Levels.ONE:
                    levelString = ImportLevel.ReadLevel(LEVEL_ONE);
                    GenerateLevel(levelString);
                    break;

                case Levels.TWO:
                    levelString = ImportLevel.ReadLevel(LEVEL_TWO);
                    GenerateLevel(levelString);
                    break;
            }
        }

        public Level()
        {
            m_currentLevel = Levels.ONE;
            LoadLevel();
        }

        public void GenerateLevel(string levelString)
        {
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
                        PlayerStartingPosition = new Vector2(x + 0.5f, y);
                        m_tiles[x, y] = Tile.EMPTY;
                    }

                    else if (levelString[index] == m_holeChar) 
                    {
                        m_tiles[x, y] = Tile.HOLE;
                    }

                    else if (levelString[index] == m_playerFinishChar) 
                    {
                        m_tiles[x, y] = Tile.FINISH;
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
    }
}