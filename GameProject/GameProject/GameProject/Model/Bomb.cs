using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameProject.Model
{
    class Bomb
    {
        private Vector2 m_modelCenterPosition;
        private Vector2 m_velocity;

        private GameProject.Model.GameModel.Direction m_direction = GameProject.Model.GameModel.Direction.RIGHT;

        public Bomb(Vector2 position)
        {
            m_modelCenterPosition = position;
            Size = new Vector2(1.0f, 1.0f);
            MoveRight();
        }

        public GameProject.Model.GameModel.Direction Direction
        { 
            get { return m_direction; }
            set { m_direction = value; }
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

        public void Update(float elapsedTime)
        {
            Position += new Vector2(m_velocity.X * elapsedTime, 0);
        }

        public void MoveRight()
        {
            m_velocity.X = 2.5f;
            Direction = GameModel.Direction.RIGHT;
        }

        public void MoveLeft()
        {
            m_velocity.X = -2.5f;
            Direction = GameModel.Direction.LEFT;
        }
    }
}
