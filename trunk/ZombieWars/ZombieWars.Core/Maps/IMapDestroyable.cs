using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZombieWars.Core.Maps
{
    /// <summary>
    /// Разрушимый объект карты
    /// </summary>
    public interface IMapDestroyable
    {
        /// <summary>
        /// Тип брони
        /// </summary>
        MapArmorType ArmorType { get; }

        /// <summary>
        /// Здоровье (0...)
        /// </summary>
        UInt16 Health { get; }
    }   
}
