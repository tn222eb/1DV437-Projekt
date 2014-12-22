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
    public class MasterController : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private GameView m_gameView;
        private GameModel m_gameModel;
        private Camera m_camera;
        private SoundEffect jumpEffect;

        public MasterController()
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
            m_gameModel = new GameModel(ImportLevel.ReadLevel(1));
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
            Texture2D playerWalkLeftTexture = Content.Load<Texture2D>("PlayerLeftWalkAnimation");
            Texture2D playerWalkRightTexture = Content.Load<Texture2D>("PlayerRightWalkAnimation");
            Texture2D groundTileTexture = Content.Load<Texture2D>("GroundTile");
            Texture2D emptyTileTexture = Content.Load<Texture2D>("EmptyTile");
            jumpEffect = Content.Load<SoundEffect>("Jump");

            m_gameView = new GameView(spriteBatch, m_gameModel, playerWalkLeftTexture, playerWalkRightTexture, groundTileTexture, emptyTileTexture);
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

            bool playerQuit = m_gameView.DidPlayerPressToQuit();
            bool playerJump = m_gameView.DidPlayerPressToJump();
            bool playerGoesRight = m_gameView.DidPlayerPressGoRight();
            bool playerGoesLeft = m_gameView.DidPlayerPressGoLeft();

            if (playerQuit)
            {
                this.Exit();
            }

            if (playerJump)
            {
                if (m_gameModel.CanPlayerJump())
                {
                    m_gameModel.Jump();
                    jumpEffect.Play();
                }
            }

            if (playerGoesRight)
            {
                m_gameModel.MoveRight();
                m_gameView.AnimateMovement(elapsedTimeMilliSeconds, GameView.Movement.RIGHT);
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

            m_gameModel.Update(elapsedTimeSeconds);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Coral);

            m_camera.CenterOn(m_gameModel.GetPlayer.Position,
                GraphicsDevice.Viewport,
                new Vector2(Level.LEVEL_WIDTH, Level.LEVEL_HEIGHT));

            m_gameView.DrawGame(GraphicsDevice.Viewport, m_camera, m_gameModel.GetLevel, m_gameModel.GetPlayer.Position);

            base.Draw(gameTime);
        }
    }
}
