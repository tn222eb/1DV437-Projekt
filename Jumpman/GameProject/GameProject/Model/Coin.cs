using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameProject.Model
{
    class Coin
    {
        private Vector2 m_modelCenterPosition;

        public Coin(Vector2 position)
        {
            m_modelCenterPosition = position;
            Size = new Vector2(1f, 1f);
        }

        public Vector2 Size
        {
            get;
            set;
        }

        public Vector2 Position
        {
            get { return m_modelCenterPosition; }
            set { m_modelCenterPosition = value; }
        }
    }
}
