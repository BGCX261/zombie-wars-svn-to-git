using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZombieWars.Core.Game
{
    public interface IGame
    {
        IGameMap Map { get; }
    }
}
