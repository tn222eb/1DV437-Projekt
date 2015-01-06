// https://code.google.com/p/1dv437arkanoid/source/browse/trunk/Collisions/Collisions2/View/Camera.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.View
{
    class Camera
    {
        private Vector2 m_modelCenterPosition = new Vector2(0,0);

        private float m_scale = 80f;

        public float GetScale()
        {
            return m_scale;
        }

        public Vector2 GetViewPosition(float x, float y, Vector2 viewScreenSize)
        {
            Vector2 modelPosition = new Vector2(x, y);

            Vector2 modelScreenSize = new Vector2(viewScreenSize.X / m_scale, viewScreenSize.Y / m_scale);

            Vector2 modelTopLeftPosition = m_modelCenterPosition - (modelScreenSize / 2.0f);

            return (modelPosition - modelTopLeftPosition) * m_scale;
        }

        public void CenterOn(Vector2 newCenterPosition, Viewport viewScreenSize, Vector2 levelSize)
        {
            m_modelCenterPosition = newCenterPosition;

            Vector2 modelScreenSize = new Vector2(viewScreenSize.Width / m_scale, viewScreenSize.Height / m_scale);

            // Look if the camera is outside the left side of the level
            if (m_modelCenterPosition.X < modelScreenSize.X / 2.0f)
            {
                m_modelCenterPosition.X = modelScreenSize.X / 2.0f;
            }

            // Look if the camera is outside the right side of the level
            if (m_modelCenterPosition.X > levelSize.X - modelScreenSize.X / 2.0f)
            {
                m_modelCenterPosition.X = levelSize.X - modelScreenSize.X / 2.0f;
            }

            // Look if the top of the camera is outside the bottom of the level
            if (m_modelCenterPosition.Y > levelSize.Y - modelScreenSize.Y / 2.0f)
            {
                m_modelCenterPosition.Y = levelSize.Y - modelScreenSize.Y / 2.0f;
            }

            // Look if the top of the camera is outside the top of the level
            if (m_modelCenterPosition.Y < modelScreenSize.Y / 2.0f)
            {
                m_modelCenterPosition.Y = modelScreenSize.Y / 2.0f;
            }
        }

    }
}
