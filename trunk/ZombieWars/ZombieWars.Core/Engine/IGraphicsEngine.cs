using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZombieWars.Core.Game;
using ZombieWars.Core.Maps;

namespace ZombieWars.Core.Engine
{
    public interface IGraphicsEngine
    {
        void SetMap(GameMap map, MapCellRange range);
        void SetRange(MapCellRange range);
        void Render();
    }
}