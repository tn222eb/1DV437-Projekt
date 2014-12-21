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
        public const int LEVEL_WIDTH = 20;
        public const int LEVEL_HEIGHT = 5;

        public Tile[,] m_tiles = new Tile[LEVEL_WIDTH, LEVEL_HEIGHT];

        private char m_blockedChar = 'x';
        private char m_playerStartChar = 's';
        private string m_levelString;

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
                        m_tiles[x, y] = Tile.CreateBlocked();
                    }

                    else if (m_levelString[index] == m_playerStartChar) 
                    {
                        PlayerStartingPosition = new Vector2(x + 0.5f, y + 1);
                        m_tiles[x, y] = Tile.CreateEmpty();
                    }

                    else
                    {
                        m_tiles[x, y] = Tile.CreateEmpty();
                    }
                }
            }
        }
    }
}
