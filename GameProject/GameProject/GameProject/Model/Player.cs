using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameProject.Model
{
    class Player
    {
        private Vector2 m_modelCenterPosition;
        private Vector2 m_velocity;
        private Vector2 m_gravityAcceleration;

        public Player()
        {
            m_gravityAcceleration = new Vector2(0, 3f);

            CanJump = true;
        }

        public Boolean CanJump
        {
            get;
            set;
        }

        public Vector2 Position
        {
            get { return m_modelCenterPosition; }
            set { m_modelCenterPosition = value;  }
        }

        public Vector2 Velocity
        {
            get { return m_velocity; }
            set { m_velocity = value; }
        }

        public void Update(float totalElapsedSeconds)
        {
            m_modelCenterPosition += Velocity * totalElapsedSeconds;
            m_velocity += m_gravityAcceleration * totalElapsedSeconds;

            if (m_modelCenterPosition.Y >= Level.LEVEL_HEIGHT - 1)
            {
                CanJump = true;
                m_velocity.Y = 0f;
            }
        }

        /// <summary>
        /// Player goes left
        /// </summary>
        public void MoveLeft()
        {
            m_velocity.X = -5f;
        }

        /// <summary>
        /// Player goes right
        /// </summary>
        public void MoveRight()
        {
            m_velocity.X = 5f;
        }

        /// <summary>
        /// Player jump
        /// </summary>
        public void DoJump() 
        {
            m_velocity.Y = -3f;
            CanJump = false;
        }
    }
}
