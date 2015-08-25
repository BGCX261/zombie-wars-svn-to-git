using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZombieWars.Core.Game;

namespace ZombieWars.Core.Engine
{
    public interface IGameEngine
    {
        void Start(IGame game, IGraphicsEngine graphicsEngine);
    }
}
