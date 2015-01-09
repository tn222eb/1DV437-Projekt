// http://www.youtube.com/watch?v=sSbIF3dd0pQ
// http://www.youtube.com/watch?v=AAPxaqs9CQM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using GameProject.View;
using GameProject.Model;

namespace GameProject
{
    class MenuView
    {
        KeyboardState m_currentKeyboard;
        KeyboardState m_previousKeyboard;

        List<string> m_startOptionList = new List<string>();
        List<string> m_continueOptionList = new List<string>();
        List<string> m_pauseOptionList = new List<string>();
        List<string> m_gameOverOptionList = new List<string>();

        private SpriteFont m_font;
        private GraphicsDeviceManager m_graphics;
        private SpriteBatch m_spriteBatch;

        int m_optionSelected = 0;

        private SpriteFont m_biggerFont;
        private List<string> m_gameWonOptionList = new List<string>();

        private GameModel m_gameModel;

        public MenuView(SpriteBatch spriteBatch, GraphicsDeviceManager graphics, GameModel gameModel)
        {
            m_startOptionList.Add("Play");
            m_startOptionList.Add("Exit");

            m_gameWonOptionList.Add("Play again");
            m_gameWonOptionList.Add("Exit");

            m_continueOptionList.Add("Continue");
            m_continueOptionList.Add("Exit");

            m_pauseOptionList.Add("Resume");
            m_pauseOptionList.Add("Exit");

            m_gameOverOptionList.Add("Retry");
            m_gameOverOptionList.Add("Exit");

            this.m_spriteBatch = spriteBatch;
            this.m_graphics = graphics;
            this.m_gameModel = gameModel;
        }

        public void LoadContent(ContentManager Content)
        {
            m_font = Content.Load<SpriteFont>("SpriteFont");
            m_biggerFont = Content.Load<SpriteFont>("SpriteFont2");
        }

        public void Update()
        {
            m_currentKeyboard = Keyboard.GetState();

            if (CheckKeyboard(Keys.Up))
            {
                if (m_optionSelected > 0)
                {
                    m_optionSelected--;
                }
            }

            if (CheckKeyboard(Keys.Down))
            {
                if (m_optionSelected < m_startOptionList.Count - 1)
                {
                    m_optionSelected++;
                }
            }

            m_previousKeyboard = m_currentKeyboard;
        }

        public bool CheckKeyboard(Keys key)
        {
            return m_currentKeyboard.IsKeyDown(key) && !m_previousKeyboard.IsKeyDown(key);
        }

        public int GetSelected()
        {
            return m_optionSelected;
        }

        /// <summary>
        /// Draw options to choose
        /// </summary>
        /// <param name="list"></param>
        public void DrawOptions(List<String> list)
        {
            Color color;

            for (int i = 0; i < list.Count; i++)
            {
                if (i == m_optionSelected)
                {
                    color = Color.DarkOrange;
                }

                else
                {
                    color = Color.White;
                }

                m_spriteBatch.Begin();
                m_spriteBatch.DrawString(m_font, list[i], new Vector2((m_graphics.PreferredBackBufferWidth / 2)
                                        - (m_font.MeasureString(list[i]).X / 2), (m_graphics.PreferredBackBufferHeight / 2)
                                        - (m_font.LineSpacing * list.Count) / 2 + ((m_font.LineSpacing + 2) * i)), color);
                m_spriteBatch.End();
            }
        }

        /// <summary>
        /// Draw start menu screen
        /// </summary>
        public void DrawMenu()
        {
            string text = "Mustache Jump";
            string instructions = "Space to jump, move left and right with arrow";

            DrawText(text, Color.White, 8, m_biggerFont);
            DrawText(instructions, Color.White, 4, m_font);
            DrawOptions(m_startOptionList);
        }

        /// <summary>
        /// Draw between level screen
        /// </summary>
        public void DrawNextLevel()
        {
            string levelPassedText = "";
            string instructionText = "";

            switch (m_gameModel.CurrentLevel())
            {
                case Level.Levels.TWO:
                    instructionText = "You must avoid contact with bomb to complete next level!";
                    levelPassedText = "Level 1 completed!";

                    DrawText(instructionText, Color.White, 4, m_font);
                    break;

                case Level.Levels.THREE:
                    instructionText = "You must collect all coins to complete next level!";
                    levelPassedText = "Level 2 completed!";

                    DrawText(instructionText, Color.White, 4, m_font);
                    break;
            }

            DrawText(levelPassedText, Color.White, 8, m_font);
            DrawOptions(m_continueOptionList);
        }

        /// <summary>
        /// Draw pause screen
        /// </summary>
        public void DrawPause()
        {
            string pauseGameText = "Game Paused";

            DrawText(pauseGameText, Color.White, 8, m_font);
            DrawOptions(m_pauseOptionList);
        }

        /// <summary>
        /// Draw game over screen
        /// </summary>
        public void DrawGameOver()
        {
            string gameOverText = "Game Over!";

            DrawText(gameOverText, Color.White, 8, m_font);
            DrawOptions(m_gameOverOptionList);
        }

        /// <summary>
        /// Used for draw text on screen
        /// </summary>
        /// <param name="text"></param>
        /// <param name="color"></param>
        /// <param name="divideBy"></param>
        /// <param name="font"></param>
        private void DrawText(string text, Color color, int divideBy, SpriteFont font)
        {
            m_spriteBatch.Begin();
            m_spriteBatch.DrawString(font, text, new Vector2((m_graphics.PreferredBackBufferWidth / 2 - m_font.MeasureString(text).X / 2), m_graphics.PreferredBackBufferHeight / divideBy), color);
            m_spriteBatch.End();
        }

        /// <summary>
        /// Draw has won game screen
        /// </summary>
        internal void DrawGameWon()
        {
            string levelPassedText = "Level 3 completed!";
            string wonGameText = "Congratulations you have finished the game!";

            DrawText(levelPassedText, Color.White, 8, m_font);
            DrawText(wonGameText, Color.White, 4, m_font);
            DrawOptions(m_gameWonOptionList);
        }
    }
}
