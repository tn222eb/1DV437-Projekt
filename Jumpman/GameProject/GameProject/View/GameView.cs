using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GameProject.Model;
using Microsoft.Xna.Framework.Content;
using GameProject.Controller;

namespace GameProject.View
{
    class GameView : IPickUpObserver
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
        private Texture2D m_bombTexture;
        private Texture2D m_coinTexture;

        private Vector2 m_coinPosition;
        private bool m_showCoinSplatter = false;

        private CoinSplatterSystem m_coinSplatterSystem = new CoinSplatterSystem();
        private Texture2D m_backgroundTexture;
        private Texture2D m_levelBackgroundThree;
        private Texture2D m_levelBackgroundTwo;
        private Texture2D m_levelBackgroundOne;

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
            this.m_bombTexture = content.Load<Texture2D>("Bomb");
            this.m_coinTexture = content.Load<Texture2D>("Coin");
            this.m_levelBackgroundThree = content.Load<Texture2D>("background3");
            this.m_levelBackgroundTwo = content.Load<Texture2D>("background2");
            this.m_levelBackgroundOne = content.Load<Texture2D>("background1");

            this.m_tileTexture = m_groundTexture;
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

        /// <summary>
        /// Draw player, tiles, bombs, coins, coin particle, background and set camera on player
        /// </summary>
        /// <param name="viewport"></param>
        /// <param name="camera"></param>
        /// <param name="level"></param>
        /// <param name="playerPosition"></param>
        /// <param name="gameState"></param>
        /// <param name="bombPositions"></param>
        /// <param name="coinPositions"></param>
        public void DrawGame(Viewport viewport, Camera camera, Level level, Vector2 playerPosition, GameProject.Model.GameModel.GameState gameState, List<Vector2> bombPositions, List<Vector2> coinPositions, float elapsedTime)
        {
            Vector2 viewPortSize = new Vector2(viewport.Width, viewport.Height);
            float scale = camera.GetScale();

            m_spriteBatch.Begin();

            DrawBackground(viewPortSize, camera);

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
                DrawCoin(coinViewPosition, scale);
            }

            for (int i = 0; i < bombPositions.Count; i++)
            {
                Vector2 bombViewPosition = camera.GetViewPosition(bombPositions[i].X, bombPositions[i].Y, viewPortSize);
                DrawBomb(bombViewPosition, scale);
            }

            if (m_showCoinSplatter) 
            {
                m_coinSplatterSystem.DrawCoinSplatter(m_spriteBatch, m_coinTexture, elapsedTime ,camera.GetViewPosition(m_coinPosition.X, m_coinPosition.Y, viewPortSize));
            }

            Vector2 playerViewPosition = camera.GetViewPosition(playerPosition.X, playerPosition.Y, viewPortSize);
            DrawPlayer(playerViewPosition, scale);

            m_spriteBatch.End();
        }

        /// <summary>
        /// Draw background
        /// </summary>
        /// <param name="viewPortSize"></param>
        private void DrawBackground(Vector2 viewPortSize, Camera camera)
        {
            WhatBackgroundToShow();

            var sourceRectangle = new Rectangle(0,0 , m_backgroundTexture.Width, m_backgroundTexture.Height);
            var destionationRectangle = camera.GetViewPosition(0,0, viewPortSize);

            m_spriteBatch.Draw(m_backgroundTexture, destionationRectangle, sourceRectangle, Color.White);
        }

        private void WhatBackgroundToShow()
        {
            if (m_gameModel.GetLevel.CurrentLevel == Level.Levels.ONE)
            {
                m_backgroundTexture = m_levelBackgroundOne;
            }

            else if (m_gameModel.GetLevel.CurrentLevel == Level.Levels.TWO)
            {
                m_backgroundTexture = m_levelBackgroundTwo;
            }

            else if (m_gameModel.GetLevel.CurrentLevel == Level.Levels.THREE)
            {
                m_backgroundTexture = m_levelBackgroundThree;
            }
        }

        /// <summary>
        /// Draw coin
        /// </summary>
        /// <param name="coinViewPosition"></param>
        /// <param name="scale"></param>
        private void DrawCoin(Vector2 coinViewPosition, float scale)
        {
            Rectangle rectangle = new Rectangle((int)(coinViewPosition.X - scale / 2.0), (int)(coinViewPosition.Y - scale), (int)scale / 2, (int)scale / 2);

            m_spriteBatch.Draw(m_coinTexture, rectangle, Color.White);
        }

        /// <summary>
        /// Draw tile
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="tile"></param>
        /// <param name="scale"></param>
        private void DrawTile(float x, float y, GameProject.Model.Level.Tile tile, float scale)
        {
            if (tile == Level.Tile.BLOCKED)
            {
                Rectangle destinationRectangle = new Rectangle((int)x, (int)y, (int)scale, (int)scale);

                m_spriteBatch.Draw(m_tileTexture, destinationRectangle, Color.White);
            }

        }

        /// <summary>
        /// Draw player
        /// </summary>
        /// <param name="viewCenterPosition"></param>
        /// <param name="scale"></param>
        private void DrawPlayer(Vector2 viewCenterPosition, float scale)
        {
            m_destinationRectangle = new Rectangle((int)(viewCenterPosition.X - scale / 2.0f), (int)(viewCenterPosition.Y - scale), (int)scale, (int)scale);

            m_spriteBatch.Draw(m_playerTexture, m_destinationRectangle, m_sourceRectangle, Color.White);
        }

        /// <summary>
        /// Draw bomb
        /// </summary>
        /// <param name="bombViewPosition"></param>
        /// <param name="scale"></param>
        private void DrawBomb(Vector2 bombViewPosition, float scale)
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

        public bool DidPlayerWantToStartGame(int selected)
        {
            return Keyboard.GetState().IsKeyDown(Keys.Enter) && selected == 0;
        }

        public bool DidPlayerWantToExitGame(int selected)
        {
            return Keyboard.GetState().IsKeyDown(Keys.Enter) && selected == 1;
        }

        /// <summary>
        /// Saves position player picks up coin and sets to show coin particles
        /// </summary>
        /// <param name="coinPosition"></param>
        public void PlayerPickUpCoinAt(Vector2 coinPosition)
        {
            m_showCoinSplatter = true;
            m_coinPosition = coinPosition;

            m_coinSplatterSystem.GenerateCoinParticles();
        }

        public bool ShowCoinSplatter
        {
            get { return m_showCoinSplatter; }
            set { m_showCoinSplatter = value; }
        }
    }
}