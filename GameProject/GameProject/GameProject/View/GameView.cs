using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GameProject.Model;
using Microsoft.Xna.Framework.Content;

namespace GameProject.View
{
    class GameView
    {
        private GameModel m_gameModel;

        private SpriteBatch m_spriteBatch;
        private Rectangle m_sourceRectangle;
        private Rectangle m_destinationRectangle;

        private float m_elapsedTime;
        private float m_delayTimer = 60f;
        private int m_frames = 0;

        private Texture2D m_playerRightWalkTexture;
        private Texture2D m_playerLeftWalkTexture;
        private Texture2D m_playerTexture;
        private Texture2D m_tileTexture;
        private Texture2D m_groundTexture;
        private Texture2D m_emptyTexture;
        private Texture2D m_bombTexture;
        private Texture2D m_coinTexture;

        public enum Movement
        {
            RIGHT = 0,
            LEFT,
            STAND
        };

        public GameView(SpriteBatch spriteBatch, GameModel gameModel, ContentManager content)
        {
            this.m_spriteBatch = spriteBatch;
            this.m_gameModel = gameModel;

            // Load textures
            this.m_playerLeftWalkTexture = content.Load<Texture2D>("PlayerLeftWalkAnimation");
            this.m_playerRightWalkTexture = content.Load<Texture2D>("PlayerRightWalkAnimation");
            this.m_groundTexture = content.Load<Texture2D>("GroundTile");
            this.m_emptyTexture = content.Load<Texture2D>("EmptyTile");
            this.m_bombTexture = content.Load<Texture2D>("Bomb");
            this.m_coinTexture = content.Load<Texture2D>("Coin");
            this.m_playerTexture = m_playerRightWalkTexture;
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

                if (m_elapsedTime >= m_delayTimer)
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

        public void DrawGame(Viewport viewport, Camera camera, Level level, Vector2 playerPosition, GameProject.Model.GameModel.GameState gameState, List<Vector2> bombPositions, List<Vector2> coinPositions)
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

            for (int i = 0; i < coinPositions.Count; i++)
            {
                Vector2 coinViewPosition = camera.GetViewPosition(coinPositions[i].X, coinPositions[i].Y, viewPortSize);
                DrawCoinAt(coinViewPosition, scale);
            }

            for (int i = 0; i < bombPositions.Count; i++)
            {
                Vector2 bombViewPosition = camera.GetViewPosition(bombPositions[i].X, bombPositions[i].Y, viewPortSize);
                DrawBombAt(bombViewPosition, scale);
            }

            Vector2 playerViewPosition = camera.GetViewPosition(playerPosition.X, playerPosition.Y, viewPortSize);
            DrawPlayerAt(playerViewPosition, scale);

            m_spriteBatch.End();
        }

        private void DrawCoinAt(Vector2 coinViewPosition, float scale)
        {
            Rectangle rectangle = new Rectangle((int)(coinViewPosition.X - scale / 2.0), (int)(coinViewPosition.Y - scale), (int)scale, (int)scale);

            m_spriteBatch.Draw(m_coinTexture, rectangle, Color.White);
        }

        private void DrawTile(float x, float y, GameProject.Model.Level.Tile tile, float scale)
        {
            if (tile == Level.Tile.BLOCKED)
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

        private void DrawBombAt(Vector2 bombViewPosition, float scale)
        {
            Rectangle rectangle = new Rectangle((int)(bombViewPosition.X - scale / 2.0f), (int)(bombViewPosition.Y - scale), (int)scale, (int)scale);

            m_spriteBatch.Draw(m_bombTexture, rectangle, Color.White);
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

        public bool DidPlayerPressToPause()
        {
            return Keyboard.GetState().IsKeyDown(Keys.Escape);
        }

        internal bool DidPlayerWantToStartGame(int selected)
        {
            return Keyboard.GetState().IsKeyDown(Keys.Enter) && selected == 0;
        }

        internal bool DidPlayerWantToExitGame(int selected)
        {
            return Keyboard.GetState().IsKeyDown(Keys.Enter) && selected == 1;
        }
    }
}