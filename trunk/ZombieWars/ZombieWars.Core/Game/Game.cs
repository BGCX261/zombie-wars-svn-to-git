using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZombieWars.Core.Game
{
    public class Game : IGame
    {
        public IGameMap Map { get; private set; }        
    }
}
