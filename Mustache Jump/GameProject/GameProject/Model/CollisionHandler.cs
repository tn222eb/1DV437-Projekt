// https://code.google.com/p/1dv437arkanoid/source/browse/trunk/Collisions/Collisions2/Model/Model.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameProject.Model
{
    class CollisionHandler
    {
        public static bool didCollide(Vector2 centerBottom, Vector2 size, Level level)
        {
            FloatRectangle occupiedArea = FloatRectangle.createFromCenterBottom(centerBottom, size);

            if (level.IsCollidingAt(occupiedArea))
            {
                return true;
            }

            return false;
        }

        public static CollisionDetails getCollisionDetails(Vector2 oldPos, Vector2 newPosition, Vector2 size, Vector2 velocity, Level level)
        {
            CollisionDetails ret = new CollisionDetails(oldPos, velocity);

            Vector2 slidingXPosition = new Vector2(newPosition.X, oldPos.Y); //Y movement ignored
            Vector2 slidingYPosition = new Vector2(oldPos.X, newPosition.Y); //X movement ignored

            if (didCollide(slidingXPosition, size, level) == false)
            {
                return doOnlyXMovement(ref velocity, ret, ref slidingXPosition);
            }

            else if (didCollide(slidingYPosition, size, level) == false)
            {
                return doOnlyYMovement(ref velocity, ret, ref slidingYPosition);
            }

            else
            {
                return doStandStill(ret, velocity);
            }

        }

        private static CollisionDetails doStandStill(CollisionDetails ret, Vector2 velocity)
        {
            if (velocity.Y > 0)
            {
                ret.m_hasCollidedWithGround = true;
            }

            ret.m_speedAfterCollision = new Vector2(0, 0);

            return ret;
        }

        private static CollisionDetails doOnlyYMovement(ref Vector2 velocity, CollisionDetails ret, ref Vector2 slidingYPosition)
        {
            velocity.X *= -0.5f; //bounce from wall
            ret.m_speedAfterCollision = velocity;
            ret.m_positionAfterCollision = slidingYPosition;
            return ret;
        }

        private static CollisionDetails doOnlyXMovement(ref Vector2 velocity, CollisionDetails ret, ref Vector2 slidingXPosition)
        {
            ret.m_positionAfterCollision = slidingXPosition;
            //did we slide on ground?
            if (velocity.Y > 0)
            {
                ret.m_hasCollidedWithGround = true;
            }

            ret.m_speedAfterCollision = doSetSpeedOnVerticalCollision(velocity);
            return ret;
        }

        private static Vector2 doSetSpeedOnVerticalCollision(Vector2 velocity)
        {
            //did we collide with ground?
            if (velocity.Y > 0)
            {
                velocity.Y = 0; //no bounce
            }

            else
            {
                //collide with roof
                velocity.Y *= -1.0f;
            }

            velocity.X *= 0.10f;

            return velocity;
        }

        public static bool IsCollidingWithBomb(Player player, Bomb bomb)
        {
            Collider colliderOfPlayer = new Collider(player.Position, (player.Size.Y / 2.0f));
            Collider colliderOfBomb = new Collider(bomb.Position, (bomb.Size.Y / 2.0f));

            if (colliderOfPlayer.DoCollide(colliderOfBomb))
            {
                return true;
            }

            return false;
        }

        internal static bool IsCollidingWithCoin(Player player, Coin coin)
        {
            Collider colliderOfPlayer = new Collider(player.Position, (player.Size.Y / 2.0f));
            Collider colliderOfCoin = new Collider(coin.Position, (coin.Size.Y / 2.0f));

            if (colliderOfPlayer.DoCollide(colliderOfCoin))
            {
                return true;
            }

            return false;
        }
    }
}
