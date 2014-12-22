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
        bool m_hasCollidedWithGround = false;

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
            Vector2 oldPosition = m_player.Position;
            m_player.Update(totalElapsedSeconds);
            Vector2 newPosition = m_player.Position;

            m_hasCollidedWithGround = false;
            Vector2 velocity = m_player.Velocity;

            if (didCollide(newPosition, m_player.Size))
            {
                CollisionDetails details = getCollisionDetails(oldPosition, newPosition, m_player.Size, velocity);
                m_hasCollidedWithGround = details.m_hasCollidedWithGround;

                m_player.Position = details.m_positionAfterCollision;
                m_player.Velocity = details.m_speedAfterCollision;
            }

            CheckIfInHole(totalElapsedSeconds);
            CheckIfDead();
            IsLevelFinished();
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

        private void IsLevelFinished()
        {
            if (m_player.Position.X > Level.LEVEL_WIDTH)
            {
                m_player = new Player();
                m_player.Position = m_level.PlayerStartingPosition;
            }
        }

        private void CheckIfDead()
        {
            if (m_player.GetRemainingLives() <= 0)
            {
                m_player = new Player();
                m_player.Position = m_level.PlayerStartingPosition;
            }
        }

        private void CheckIfInHole(float a_elapsedTime)
        {
            if (m_level.IsInHole(m_player.Position, m_player.Size))
            {
                m_player.RemoveLife();
            }
        }

        private bool didCollide(Vector2 a_centerBottom, Vector2 a_size)
        {
            FloatRectangle occupiedArea = FloatRectangle.createFromCenterBottom(a_centerBottom, a_size);
            if (m_level.IsCollidingAt(occupiedArea))
            {
                return true;
            }
            return false;
        }

        private CollisionDetails getCollisionDetails(Vector2 a_oldPos, Vector2 a_newPosition, Vector2 a_size, Vector2 a_velocity)
        {
            CollisionDetails ret = new CollisionDetails(a_oldPos, a_velocity);

            Vector2 slidingXPosition = new Vector2(a_newPosition.X, a_oldPos.Y); //Y movement ignored
            Vector2 slidingYPosition = new Vector2(a_oldPos.X, a_newPosition.Y); //X movement ignored

            if (didCollide(slidingXPosition, a_size) == false)
            {
                return doOnlyXMovement(ref a_velocity, ret, ref slidingXPosition);
            }
            else if (didCollide(slidingYPosition, a_size) == false)
            {

                return doOnlyYMovement(ref a_velocity, ret, ref slidingYPosition);
            }
            else
            {
                return doStandStill(ret, a_velocity);
            }

        }

        private static CollisionDetails doStandStill(CollisionDetails ret, Vector2 a_velocity)
        {
            if (a_velocity.Y > 0)
            {
                ret.m_hasCollidedWithGround = true;
            }

            ret.m_speedAfterCollision = new Vector2(0, 0);

            return ret;
        }

        private static CollisionDetails doOnlyYMovement(ref Vector2 a_velocity, CollisionDetails ret, ref Vector2 slidingYPosition)
        {
            a_velocity.X *= -0.5f; //bounce from wall
            ret.m_speedAfterCollision = a_velocity;
            ret.m_positionAfterCollision = slidingYPosition;
            return ret;
        }

        private static CollisionDetails doOnlyXMovement(ref Vector2 a_velocity, CollisionDetails ret, ref Vector2 slidingXPosition)
        {
            ret.m_positionAfterCollision = slidingXPosition;
            //did we slide on ground?
            if (a_velocity.Y > 0)
            {
                ret.m_hasCollidedWithGround = true;
            }

            ret.m_speedAfterCollision = doSetSpeedOnVerticalCollision(a_velocity);
            return ret;
        }

        private static Vector2 doSetSpeedOnVerticalCollision(Vector2 a_velocity)
        {
            //did we collide with ground?
            if (a_velocity.Y > 0)
            {
                a_velocity.Y = 0; //no bounce
            }
            else
            {
                //collide with roof
                a_velocity.Y *= -1.0f;
            }

            a_velocity.X *= 0.10f;

            return a_velocity;
        }
    }
}
