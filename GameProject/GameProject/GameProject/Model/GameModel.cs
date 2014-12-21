using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameProject.Model
{
    class GameModel
    {
        Player m_player;
        Level m_level;

        public GameModel(string levelString) 
        {
            m_level = new Level(levelString);
            m_player = new Player();
            m_player.Position = m_level.PlayerStartingPosition;      
        }

        public Level GetLevel
        {
            get { return m_level; }
        }

        public Player GetPlayer
        {
            get { return m_player; }
        }

        public void MoveLeft() 
        {
            m_player.MoveLeft();
        }

        public void MoveRight() 
        {
            m_player.MoveRight();
        }

        public void Update(float totalElapsedSeconds)
        {
            m_player.Update(totalElapsedSeconds);
        }

        public void StandStill()
        {
            m_player.Velocity = new Vector2(0, m_player.Velocity.Y);
        }

        public void Jump()
        {
            m_player.DoJump();
        }

        public bool CanPlayerJump()
        {
            return m_player.CanJump;
        }
    }
}
