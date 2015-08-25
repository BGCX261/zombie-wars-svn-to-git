using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZombieWars.Core.Maps
{
    /// <summary>
    /// Состояние открытости стены
    /// </summary>
    [Serializable]
    public enum MapWallMode : byte
    {
        Wall = 0, Window = 1, Door = 2
    }
}
