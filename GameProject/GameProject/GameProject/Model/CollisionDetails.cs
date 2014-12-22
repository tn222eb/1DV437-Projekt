using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameProject.Model
{
    class CollisionDetails
    {
        public Vector2 m_speedAfterCollision;
        public Vector2 m_positionAfterCollision;
        public bool m_hasCollidedWithGround = false;

        public CollisionDetails(Vector2 a_oldPos, Vector2 a_velocity)
        {
            m_positionAfterCollision = a_oldPos;
            m_speedAfterCollision = a_velocity;
        }
    }
}
