using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GameProject.Model;

namespace GameProject.View
{
    class GameView
    {
        private GameModel m_gameModel;

        private SpriteBatch m_spriteBatch;
        private Rectangle m_sourceRectangle;
        private Rectangle m_destinationRectangle;

        private float m_elapsedTime;
        private float m_delayTime = 60f;
        private int m_frames = 0;
     
        private Texture2D m_playerRightWalkTexture;
        private Texture2D m_playerLeftWalkTexture;
        private Texture2D m_playerTexture;
        private Texture2D m_tileTexture;
        private Texture2D m_groundTexture;
        private Texture2D m_emptyTexture;

        public enum Movement
        {
            RIGHT = 0,
            LEFT,
            STAND
        };

        public GameView(SpriteBatch spriteBatch, GameModel gameModel, Texture2D playerWalkLeftTexture, Texture2D playerWalkRightTexture, Texture2D groundTexture, Texture2D emptyTexture)
        {
            this.m_spriteBatch = spriteBatch;
            this.m_gameModel = gameModel;

            this.m_playerLeftWalkTexture = playerWalkLeftTexture;
            this.m_playerRightWalkTexture = playerWalkRightTexture;
            this.m_groundTexture = groundTexture;
            this.m_emptyTexture = emptyTexture;

            this.m_playerTexture = playerWalkRightTexture;
        }

        /// <summary>
        /// Animate player movement in sprites
        /// </summary>
        /// <param name="timeElapsedMilliSeconds"></param>
        /// <param name="movement"></param>
        public void AnimateMovement(float timeElapsedMilliSeconds, Movement movement)
        {
            if (movement.Equals(Movement.STAND) == false)
            {
                m_elapsedTime += timeElapsedMilliSeconds;

                if (m_elapsedTime >= m_delayTime)
                {
                    if (m_frames >= 3)
                    {
                        m_frames = 0;
                    }
                    else
                    {
                        m_frames++;
                    }

                    m_elapsedTime = 0;
                }
            }

            if (movement.Equals(Movement.RIGHT))
            {
                this.m_playerTexture = m_playerRightWalkTexture;
                m_sourceRectangle = new Rectangle(m_playerTexture.Width / 4 * m_frames, 0, m_playerTexture.Width / 4, m_playerTexture.Height);
            }

            else if (movement.Equals(Movement.LEFT))
            {
                this.m_playerTexture = m_playerLeftWalkTexture;
                m_sourceRectangle = new Rectangle(m_playerTexture.Width / 4 * m_frames, 0, m_playerTexture.Width / 4, m_playerTexture.Height);
            }

            else if (movement.Equals(Movement.STAND))
            {
                m_sourceRectangle = new Rectangle(0, 0, m_playerTexture.Width / 4, m_playerTexture.Height);
            }
        }

        public void DrawLevel(Viewport viewport, Camera camera, Level level, Vector2 playerPosition)
        {
            Vector2 viewPortSize = new Vector2(viewport.Width, viewport.Height);
            float scale = camera.GetScale();

            m_spriteBatch.Begin();

            for (int x = 0; x < Level.LEVEL_WIDTH; x++)
            {
                for (int y = 0; y < Level.LEVEL_HEIGHT; y++)
                {
                    Vector2 viewPosition = camera.GetViewPosition(x, y, viewPortSize);
                    DrawTile(viewPosition.X, viewPosition.Y, level.m_tiles[x, y], scale);
                }
            }

            Vector2 playerViewPosition = camera.GetViewPosition(playerPosition.X, playerPosition.Y, viewPortSize);
            DrawPlayerAt(playerViewPosition, scale);
            m_spriteBatch.End();
        }

        private void DrawTile(float x, float y, Tile tile, float scale)
        {
            if (tile.isBlocked()) 
            {
                m_tileTexture = m_groundTexture;
            }

            else
            {
                m_tileTexture = m_emptyTexture;
            }

            Rectangle destinationRectangle = new Rectangle((int)x, (int)y, (int)scale, (int)scale);

            m_spriteBatch.Draw(m_tileTexture, destinationRectangle, Color.White);
        }

        private void DrawPlayerAt(Vector2 viewCenterPosition, float scale)
        {
            m_destinationRectangle = new Rectangle((int)(viewCenterPosition.X - scale / 2.0f), (int)(viewCenterPosition.Y - scale), (int)scale, (int)scale);

            m_spriteBatch.Draw(m_playerTexture, m_destinationRectangle, m_sourceRectangle, Color.White);
        }

        public bool DidPlayerPressGoLeft()
        {
            return Keyboard.GetState().IsKeyDown(Keys.Left);
        }

        public bool DidPlayerPressToJump()
        {
            return Keyboard.GetState().IsKeyDown(Keys.Space);
        }

        public bool DidPlayerPressGoRight()
        {
            return Keyboard.GetState().IsKeyDown(Keys.Right);
        }

        public bool DidPlayerPressToQuit()
        {
            return Keyboard.GetState().IsKeyDown(Keys.Escape);
        }

    }
}