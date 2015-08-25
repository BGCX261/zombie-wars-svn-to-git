using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZombieWars.Core.Maps
{
    /// <summary>
    /// Тип брони активного объекта карты
    /// </summary>
    [Serializable]    
    public enum MapArmorType : ushort
    {
        None = 0,
        Light = 1,
        Medium = 2,
        Heavy = 3,
        Undead = 4,
        Machine = 5,
        Building = 6
    }
}
