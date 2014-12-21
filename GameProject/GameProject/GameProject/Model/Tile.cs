// https://code.google.com/p/1dv437arkanoid/source/browse/trunk/Collisions/Collisions2/Model/Tile.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameProject.Model
{
    class Tile
    {
        private enum TileType
        {
            EMPTY = 0,
            BLOCKED,
        };

        TileType m_type;

        private Tile(TileType a_type)
        {
            m_type = a_type;
        }

        internal static Tile CreateBlocked()
        {
            return new Tile(TileType.BLOCKED);
        }

        internal static Tile CreateEmpty()
        {
            return new Tile(TileType.EMPTY);
        }
      
        internal bool isBlocked()
        {
            return m_type == TileType.BLOCKED;
        }
    }
}
