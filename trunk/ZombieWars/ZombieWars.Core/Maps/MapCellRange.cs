using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace ZombieWars.Core.Maps
{
    /// <summary>
    /// Диапазон на карте
    /// </summary>
    [Serializable]
    [Guid("1AF7A7CE-D178-4B46-B56B-F0D159A96670")]
    public struct MapCellRange
    {        
        /// <summary>
        /// Начало диапазона
        /// </summary>
        public MapPoint Begin;
        /// <summary>
        /// Конец диапазона
        /// </summary>
        public MapPoint End;

        /// <summary>
        /// Диапазон на карте
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public MapCellRange(MapPoint from, MapPoint to)
        {
            Begin = from;
            End = to;
        }       
    }
}
