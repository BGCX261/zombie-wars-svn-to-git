using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZombieWars.Core.Maps;

namespace ZombieWars.Core.Game
{
    public interface IGameMap
    {
        MapState Map { get; }
    }
}
