// https://code.google.com/p/1dv437arkanoid/source/browse/trunk/Collisions/Collisions2/Model/Collider.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameProject.Model
{
    class Collider
    {
        Vector2 m_position = new Vector2();
        float m_radius = 1;


        Collider(Vector2 a_pos, float a_radius)
        {
            m_position = a_pos;
            m_radius = a_radius;
        }

        bool DoCollide(Collider a_other)
        {
            Vector2 line = m_position - a_other.m_position;
            float distance = line.Length();

            if (distance < m_radius + a_other.m_radius)
            {
                return true;
            }

            return false;
        }

    }
}
