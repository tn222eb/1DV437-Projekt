using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using GameProject.View;
using GameProject.Model;

namespace GameProject
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameController : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private GameView m_gameView;
        private GameModel m_gameModel;
        private Camera m_camera;

        private MenuView m_menuView;
        private SoundView m_sound;

        public GameController()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 400;
            graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            m_camera = new Camera();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            m_gameModel = new GameModel();
            m_gameView = new GameView(spriteBatch, m_gameModel, Content);
            m_sound = new SoundView(Content);
            m_menuView = new MenuView(spriteBatch, graphics, m_gameModel);
            m_menuView.LoadContent(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            float elapsedTimeSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float elapsedTimeMilliSeconds = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            bool playerJump = m_gameView.DidPlayerPressToJump();
            bool playerGoesRight = m_gameView.DidPlayerPressGoRight();
            bool playerGoesLeft = m_gameView.DidPlayerPressGoLeft();
            bool playerPause = m_gameView.DidPlayerPressToPause();

            switch (m_gameModel.GetGameState)
            {
                case GameModel.GameState.MAIN_MENU:

                    if (m_sound.BackGroundSongPlaying)
                    {
                        m_sound.StopGameBackgroundSong();
                    }
    
                    OptionSelected();

                    break;

                case GameModel.GameState.PLAY:

                    if (m_sound.BackGroundSongPlaying == false)
                    {
                        m_sound.StartGameBackgroundSong();
                    }

                    if (playerPause)
                    {
                        m_gameModel.SetGameState = GameModel.GameState.PAUSE;
                    }

                    if (playerJump)
                    {
                        if (m_gameModel.CanPlayerJump())
                        {
                            m_gameModel.Jump();
                            m_sound.PlayerJump();
                        }
                    }

                    if (playerGoesRight)
                    {
                        if (m_gameModel.CanPlayerMoveRight())
                        {
                            m_gameModel.MoveRight();
                            m_gameView.AnimateMovement(elapsedTimeMilliSeconds, GameView.Movement.RIGHT);
                        }
                    }

                    else if (playerGoesLeft)
                    {
                        m_gameModel.MoveLeft();
                        m_gameView.AnimateMovement(elapsedTimeMilliSeconds, GameView.Movement.LEFT);
                    }

                    else
                    {
                        m_gameModel.StandStill();
                        m_gameView.AnimateMovement(elapsedTimeMilliSeconds, GameView.Movement.STAND);
                    }

                    m_gameModel.Update(elapsedTimeSeconds, m_sound);

                    break;

                case GameModel.GameState.GAME_OVER:

                    if (m_sound.BackGroundSongPlaying)
                    {
                        m_sound.StopGameBackgroundSong();
                    }

                    OptionSelected();

                    m_gameModel.RestartLevel();

                    break;

                case GameModel.GameState.LEVEL_FINISHED:

                    if (m_sound.BackGroundSongPlaying)
                    {
                        m_sound.StopGameBackgroundSong();
                    }

                    OptionSelected();

                    m_gameModel.LoadLevel();

                    break;

                case GameModel.GameState.LAST_LEVEL_FINISHED:

                    if (m_sound.BackGroundSongPlaying)
                    {
                        m_sound.StopGameBackgroundSong();
                    }

                    OptionSelected();

                    m_gameModel.ResetLevel();
                    m_gameModel.LoadLevel();

                    break;

                case GameModel.GameState.PAUSE:

                    if (m_sound.BackGroundSongPlaying)
                    {
                        m_sound.StopGameBackgroundSong();
                    }

                    OptionSelected();
                    break;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.MediumSlateBlue);

            switch (m_gameModel.GetGameState)
            {
                case GameModel.GameState.PLAY:
                    m_camera.CenterOn(m_gameModel.GetPlayer.Position,
                        GraphicsDevice.Viewport,
                        new Vector2(Level.LEVEL_WIDTH, Level.LEVEL_HEIGHT));

                    m_gameView.DrawGame(GraphicsDevice.Viewport, m_camera, m_gameModel.GetLevel, m_gameModel.GetPlayer.Position, m_gameModel.GetGameState, m_gameModel.GetBombPositions(), m_gameModel.GetCoinPositions());
                    break;

                case GameModel.GameState.MAIN_MENU:
                    m_menuView.DrawMenu();
                    break;

                case GameModel.GameState.GAME_OVER:
                    m_menuView.DrawGameOver();
                    break;

                case GameModel.GameState.PAUSE:
                    m_menuView.DrawPause();
                    break;

                case GameModel.GameState.LEVEL_FINISHED:
                    m_menuView.DrawNextLevel();
                    break;

                case GameModel.GameState.LAST_LEVEL_FINISHED:
                    m_menuView.DrawGameWon();
                    break;
            }

            base.Draw(gameTime);
        }

        /// <summary>
        /// Play or exit depending on what player selected from option menu
        /// </summary>
        private void OptionSelected() 
        {
            m_menuView.Update();

            bool playerPlay = m_gameView.DidPlayerWantToStartGame(m_menuView.GetSelected());
            bool playerExit = m_gameView.DidPlayerWantToExitGame(m_menuView.GetSelected());

            if (playerPlay)
            {
                m_gameModel.SetGameState = GameModel.GameState.PLAY;
            }

            else if (playerExit)
            {
                this.Exit();
            }
        }
    }
}
