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

        public MenuView(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)   
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

        public void DrawOptions(List<String> list)
        {
            Color color;
 
            for (int i = 0; i < list.Count; i++)
            {
                if (i == m_optionSelected)
                {
                    color = Color.Yellow;
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

        public void DrawMenu()
        {
            string text = "Frank Jump";
            string instructions = "Space to jump, move left and right with arrow";

            DrawText(text, Color.White, 8, m_biggerFont);
            DrawText(instructions, Color.White, 4, m_font);
            DrawOptions(m_startOptionList);
        }

        public void DrawNextLevel()
        {
            string text = "Level Completed!";

            DrawText(text, Color.White, 4, m_font);
            DrawOptions(m_continueOptionList);
        }

        public void DrawPause()
        {
            string text = "Game Paused";

            DrawText(text, Color.White, 4, m_font);
            DrawOptions(m_pauseOptionList);
        }

        public void DrawGameOver()
        {
            string text = "Game Over!";

            DrawText(text, Color.White, 4, m_font);
            DrawOptions(m_gameOverOptionList);
        }

        private void DrawText(string text, Color color, int divideBy, SpriteFont font) 
        {
            m_spriteBatch.Begin();
            m_spriteBatch.DrawString(font, text, new Vector2((m_graphics.PreferredBackBufferWidth / 2 - m_font.MeasureString(text).X / 2), m_graphics.PreferredBackBufferHeight / divideBy), color);
            m_spriteBatch.End();
        }

        internal void DrawGameWon()
        {
            string text = "Congratulations you have completed the game!";

            DrawText(text, Color.White, 4, m_font);
            DrawOptions(m_gameWonOptionList);
        }
    }
}
