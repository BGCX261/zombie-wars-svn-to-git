using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZombieWars.Core.Maps
{
    /// <summary>
    /// Через объект возможно прохождение с определённым коэффициентом
    /// </summary>
    public interface IMapPassable
    {
        /// <summary>
        /// Проходимость (0..1)
        /// </summary>
        MapPassability Passability { get; }
    }
}
