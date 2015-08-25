using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZombieWars.Core.Maps
{
    /// <summary>
    /// Направление (по сторонам света)
    /// </summary>
    [Serializable]
    public enum MapDirection : byte
    {
        North = 0,
        South = 1,
        West = 2,
        East = 3
    }
}
