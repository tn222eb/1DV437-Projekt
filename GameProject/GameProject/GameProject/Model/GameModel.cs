using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameProject.View;
using GameProject.Controller;

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
            LAST_LEVEL_FINISHED,
            PAUSE
        }

        public enum Direction
        {
            LEFT = 0,
            RIGHT
        }

        private List<Bomb> m_bombsList = new List<Bomb>();
        private List<Coin> m_coinList;

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
            m_bombsList = m_level.Bombs;
            m_coinList = m_level.Coins;
        }

        public void Update(float totalElapsedSeconds, ISoundObserver soundObserver)
        {
            UpdatePlayer(totalElapsedSeconds);
            UpdateBomb(totalElapsedSeconds);

            CheckIfInHole();
            CheckIfCollideWithBomb(soundObserver);
            CheckIfDead();
            CheckIfLevelFinished(soundObserver);
            CheckIfPlayerPickedUpCoin(soundObserver);
        }

        /// <summary>
        /// Check if player have collided with coin
        /// </summary>
        /// <param name="soundObserver"></param>
        private void CheckIfPlayerPickedUpCoin(ISoundObserver soundObserver)
        {
            for (int i = 0; i < m_coinList.Count; i++)
            {
                if (CollisionHandler.IsCollidingWithCoin(m_player, m_coinList[i]))
                {
                    m_coinList.Remove(m_coinList[i]);
                    soundObserver.PlayerPickUpCoin();
                }
            }
        }

        /// <summary>
        /// Check if player have collided with bomb
        /// </summary>
        /// <param name="soundObserver"></param>
        private void CheckIfCollideWithBomb(ISoundObserver soundObserver)
        {
            foreach (Bomb bomb in m_bombsList) 
            {
                if (CollisionHandler.IsCollidingWithBomb(m_player, bomb))
                {
                    m_player.RemoveLife();
                    soundObserver.BombExplode();
                }
            }
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

        private void UpdateBomb(float totalElapsedSeconds)
        {
            foreach (Bomb bomb in m_bombsList)
            {
                if (bomb.Direction == Direction.RIGHT)
                {
                    Vector2 bombCenterPosition = bomb.Position;
                    bombCenterPosition.X += bomb.Size.X / 2.0f;

                    if (m_level.IsBombStandingByEdgeOfBlocked(bombCenterPosition))
                    {
                        bomb.MoveLeft();
                    }
                }

                else if (bomb.Direction == Direction.LEFT)
                {
                    Vector2 bombCenterPosition = bomb.Position;
                    bombCenterPosition.X -= bomb.Size.X / 2.0f;

                    if (m_level.IsBombStandingByEdgeOfBlocked(bombCenterPosition))
                    {
                        bomb.MoveRight();
                    }
                }

                bomb.Update(totalElapsedSeconds);
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

        /// <summary>
        /// Get a bool if player can move more to right based on if have passed level finish
        /// </summary>
        /// <returns>Returns true or false</returns>
        public bool CanPlayerMoveRight() 
        {
            if (m_level.IsAtLevelFinish(m_player.Position, m_player.Size / 2.0f)) 
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Get a boolean based on if player have landed
        /// </summary>
        /// <returns>Returns true or false</returns>
        public bool CanPlayerJump()
        {
            return m_hasCollidedWithGround;
        }

        /// <summary>
        /// Check if player have passed level finish
        /// </summary>
        /// <param name="soundObserver"></param>
        private void CheckIfLevelFinished(ISoundObserver soundObserver)
        {
            if (m_level.IsAtLevelFinish(m_player.Position, m_player.Size) && CheckIfAllCoinsIsPickedUp())
            {
                m_gameState = GameState.LEVEL_FINISHED;
                
                if (m_level.CurrentLevel == Level.Levels.THREE)
                {
                    SetGameState = GameModel.GameState.LAST_LEVEL_FINISHED;
                }

                m_level.CurrentLevel++;
                soundObserver.LevelCompleted();
            }
        }

        /// <summary>
        /// Check if player have picked up all coins
        /// </summary>
        /// <returns></returns>
        private bool CheckIfAllCoinsIsPickedUp() 
        {
            return m_coinList.Count == 0;
        }

        /// <summary>
        /// Check if player is dead
        /// </summary>
        private void CheckIfDead()
        {
            if (m_player.GetRemainingLives() <= 0)
            {
                m_gameState = GameState.GAME_OVER;
            }
        }

        /// <summary>
        /// Restart current level
        /// </summary>
        public void RestartLevel() 
        {
            m_player = new Player();

            m_level.LoadLevel();

            StartGame();
        }

        /// <summary>
        /// Check if player have fallen in hole
        /// </summary>
        private void CheckIfInHole()
        {
            if (m_level.IsInHole(m_player.Position, m_player.Size))
            {
                m_player.RemoveLife();
            }
        }

        /// <summary>
        /// Load level
        /// </summary>
        public void LoadLevel()
        {
            m_level.LoadLevel();

            StartGame();
        }

        /// <summary>
        /// Reset current level back to level 1
        /// </summary>
        internal void ResetLevel()
        {
            m_level.CurrentLevel = Level.Levels.ONE;
        }

        /// <summary>
        /// Get bomb positions
        /// </summary>
        /// <returns>Returns a list with position of bombs</returns>
        public List<Vector2> GetBombPositions()
        {
            List<Vector2> bombPositions = new List<Vector2>();

            foreach (Bomb bomb in m_bombsList)
            {
                bombPositions.Add(bomb.Position);
            }

            return bombPositions;
        }

        /// <summary>
        /// Get coin positions
        /// </summary>
        /// <returns>Returns a list with position of coins</returns>
        public List<Vector2> GetCoinPositions()
        {
            List<Vector2> coinPositions = new List<Vector2>();

            foreach(Coin coin in m_coinList)
            {
                coinPositions.Add(coin.Position);
            }

            return coinPositions;
        }

        /// <summary>
        /// Get current level
        /// </summary>
        /// <returns>Returns a level object to know what level currently on</returns>
        public GameProject.Model.Level.Levels CurrentLevel() 
        {
            return m_level.CurrentLevel;
        } 
    }
}