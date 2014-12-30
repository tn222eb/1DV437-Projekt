using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameProject.Controller
{
    interface ISoundObserver
    {
        void PlayerJump();
        void BombExplode();
        void LevelCompleted();
    }
}
