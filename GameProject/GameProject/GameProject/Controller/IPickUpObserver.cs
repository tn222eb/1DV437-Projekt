using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameProject.Controller
{
    interface IPickUpObserver
    {
        void PlayerPickUpCoinAt(Vector2 position);
    }
}
