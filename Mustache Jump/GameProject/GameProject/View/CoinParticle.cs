using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameProject.View
{
    class CoinParticle
    {
        private Vector2 m_position;
        private Vector2 m_velocity;
        private float m_maxTimeToLive = 5.0f;
        private float m_remainingLifeToLive;

        public CoinParticle(Vector2 velocity)
        {
            m_position = new Vector2(0, 0);
            m_velocity = velocity;
            m_remainingLifeToLive = m_maxTimeToLive;
        }

        public float Visibility
        {
            get { return m_remainingLifeToLive / m_maxTimeToLive; }
        }

        public Vector2 Position
        {
            get { return m_position; }
        }

        public void Update(float elapsedTime)
        {
            m_remainingLifeToLive -= 0.15f;

            m_position += m_velocity * elapsedTime;
        }
    }
}
