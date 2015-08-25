using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace ZombieWars.Core.Maps
{
    /// <summary>
    /// Размер объекта карты
    /// </summary>
    [Serializable]
    [Guid("A70CE641-8D96-4DEE-9F31-0C2597974918")]
    public struct MapSize
    {
        /// <summary>
        /// Ширина (в клетках)
        /// </summary>
        public readonly UInt16 Width;
        /// <summary>
        /// Высота (в клетках)
        /// </summary>
        public readonly UInt16 Height;        

        /// <summary>
        /// Одна клетка
        /// </summary>
        public static MapSize One
        {
            get { return new MapSize(1, 1); }
        }

        /// <summary>
        /// Размер объекта карты
        /// </summary>
        /// <param name="width">Ширина (в клетках)</param>
        /// <param name="height">Высота (в клетках)</param>
        public MapSize(UInt16 width, UInt16 height)
        {
            if (width < 1) throw new ArgumentOutOfRangeException("width", "width cannot be <= 1");
            this.Width = width;
            if (height < 1) throw new ArgumentOutOfRangeException("height", "height cannot be <= 1");
            this.Height = height;
        }
    }
}
