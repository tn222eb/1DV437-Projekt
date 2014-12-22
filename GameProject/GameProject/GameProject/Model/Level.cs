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

        public Tile[,] m_tiles = new Tile[LEVEL_WIDTH, LEVEL_HEIGHT];

        private char m_blockedChar = 'x';
        private char m_playerStartChar = 's';
        private char m_holeChar = 'h';
        private string m_levelString;

        public enum Tile
        {
            EMPTY = 0,
            BLOCKED,
            HOLE
        };

        public Vector2 PlayerStartingPosition
        {
            get;
            set;
        }

        public Level(string levelString)
        {
            m_levelString = levelString;

            GenerateLevel();
        }

        public void GenerateLevel()
        {
            for (int x = 0; x < LEVEL_WIDTH; x++)
            {
                for (int y = 0; y < LEVEL_HEIGHT; y++)
                {
                    int index = y * LEVEL_WIDTH + x;

                    if (m_levelString[index] == m_blockedChar)
                    {
                        m_tiles[x, y] = Tile.BLOCKED;
                    }

                    else if (m_levelString[index] == m_playerStartChar)
                    {
                        PlayerStartingPosition = new Vector2(x + 0.5f, y);
                        m_tiles[x, y] = Tile.EMPTY;
                    }

                    else if (m_levelString[index] == m_holeChar) 
                    {
                        m_tiles[x, y] = Tile.HOLE;
                    }

                    else
                    {
                        m_tiles[x, y] = Tile.EMPTY;
                    }
                }
            }
        }

        public bool IsInHole(Vector2 a_position, Vector2 a_size)
        {
            Vector2 topLeft = new Vector2(a_position.X - a_size.X / 2.0f, a_position.Y - a_size.Y);
            Vector2 bottomRight = new Vector2(a_position.X + a_size.X / 2.0f, a_position.Y);

            for (int x = 0; x < LEVEL_WIDTH; x++)
            {
                for (int y = 0; y < LEVEL_HEIGHT; y++)
                {
                    if (bottomRight.X < (float)x)
                        continue;
                    if (bottomRight.Y < (float)y)
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