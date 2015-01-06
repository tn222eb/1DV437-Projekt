using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GameProject.View
{
    class CoinSplatterSystem
    {
        private int m_coinParticleSize = 10;
        private const int NUM_PARTICLES = 50;
        private CoinParticle[] m_coinParticles;

        private Random m_random;

        public CoinSplatterSystem()
        {
            m_random = new Random();
            m_coinParticles = new CoinParticle[NUM_PARTICLES];
        }

        internal void GenerateCoinParticles()
        {
            for (int i = 0; i < NUM_PARTICLES; i++)
            {
                Vector2 velocity = new Vector2((2.0f * (float)m_random.NextDouble() - 1.0f), (2.0f * (float)m_random.NextDouble() - 1.0f));

                m_coinParticles[i] = new CoinParticle(velocity);
            }
        }

        internal void DrawCoinSplatter(SpriteBatch spriteBatch, Texture2D coinTexture, float elapsedTime, Vector2 coinSpawnPosition)
        {
            for (int i = 0; i < NUM_PARTICLES; i++)
            {
                m_coinParticles[i].Update(elapsedTime);

                Vector2 coinModelStartPosition = m_coinParticles[i].Position;

                Vector2 coinViewPosition = coinModelStartPosition + coinSpawnPosition + new Vector2(4f, -20f);

                Rectangle destinationRectangle = new Rectangle((int)coinViewPosition.X - m_coinParticleSize / 2, (int)coinViewPosition.Y - m_coinParticleSize / 2, m_coinParticleSize, m_coinParticleSize);

                float visibility = m_coinParticles[i].Visibility;

                Color color = new Color(visibility, visibility, visibility, visibility);

                spriteBatch.Draw(coinTexture, destinationRectangle, color);
            }
        }
    }
}
