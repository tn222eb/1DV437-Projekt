using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameProject.Model
{
    class GameModel
    {
        public enum GameState 
        {
            PLAY = 0,
            MAIN_MENU,
            GAME_OVER,
            LEVEL_FINISHED,
            PAUSE
        }

        Player m_player;
        Level m_level;
        GameState m_gameState = GameState.MAIN_MENU;

        bool m_hasCollidedWithGround = false;

        public GameModel()
        {
            m_level = new Level();
            m_player = new Player();

            StartGame();
        }

        public GameState GetGameState
        {
            get { return m_gameState; }
        }

        public GameState SetGameState 
        {
            set { m_gameState = value; }
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

        public void StartGame() 
        {
            m_player.Position = m_level.PlayerStartingPosition;
        }

        public void Update(float totalElapsedSeconds)
        {
            UpdatePlayer(totalElapsedSeconds);

            CheckIfInHole();
            CheckIfDead(totalElapsedSeconds);
            CheckIfLevelFinished(totalElapsedSeconds);
        }

        private void UpdatePlayer(float totalElapsedSeconds)
        {
            Vector2 oldPosition = m_player.Position;
            m_player.Update(totalElapsedSeconds);
            Vector2 newPosition = m_player.Position;

            m_hasCollidedWithGround = false;
            Vector2 velocity = m_player.Velocity;

            if (CollisionHandler.didCollide(newPosition, m_player.Size, m_level))
            {
                CollisionDetails details = CollisionHandler.getCollisionDetails(oldPosition, newPosition, m_player.Size, velocity, m_level);
                m_hasCollidedWithGround = details.m_hasCollidedWithGround;

                m_player.Position = details.m_positionAfterCollision;
                m_player.Velocity = details.m_speedAfterCollision;
            }
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
            return m_hasCollidedWithGround;
        }

        private void CheckIfLevelFinished(float totalElapsedSeconds)
        {
            if (m_level.IsAtLevelFinish(m_player.Position, m_player.Size))
            {
                m_gameState = GameState.LEVEL_FINISHED;
            }
        }

        private void CheckIfDead(float totalElapsedSeconds)
        {
            if (m_player.GetRemainingLives() <= 0)
            {
                m_gameState = GameState.GAME_OVER;
            }
        }

        public void RestartGame() 
        {
            m_player = new Player();
            StartGame();
        }

        private void CheckIfInHole()
        {
            if (m_level.IsInHole(m_player.Position, m_player.Size))
            {
                m_player.RemoveLife();
            }
        }

        public void LoadNextLevel()
        {
            m_level.CurrentLevel++;
            m_level.LoadLevel();

            StartGame();
        }
    }
}
