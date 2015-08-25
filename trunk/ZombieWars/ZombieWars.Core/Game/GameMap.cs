using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZombieWars.Core.Maps;

namespace ZombieWars.Core.Game
{
    public class GameMap : IGameMap
    {
        public virtual MapState Map { get; protected set; }        

        public GameMap(MapState mapState)
        {
            this.Map = mapState;
        }
    }
}
