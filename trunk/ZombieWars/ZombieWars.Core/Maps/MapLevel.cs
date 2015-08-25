using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZombieWars.Core.Maps
{
    /// <summary>
    /// Уровень карты
    /// </summary>
    [Serializable]
    public class MapLevel
    {
        /// <summary>
        /// Размер (в клетках)
        /// </summary>
        public MapSize Size { get; protected set; }
        
        /// <summary>
        /// Клетки уровня
        /// </summary>
        public MapMatrix<MapCell> Cells { get; protected set; }

        /// <summary>
        /// Клетка
        /// </summary>
        /// <param name="X">X</param>
        /// <param name="Y">Y</param>
        /// <returns>Клетка</returns>
        public MapCell this[UInt16 X, UInt16 Y]
        {
            get { return this.Cells[X, Y]; }
            set { this.Cells[X, Y] = value; }
        }

        /// <summary>
        /// Клетка
        /// </summary>
        /// <param name="X">X</param>
        /// <param name="Y">Y</param>
        /// <returns>Клетка</returns>
        public MapCell this[Int32 X, Int32 Y]
        {
            get { return this.Cells[X, Y]; }
            set { this.Cells[X, Y] = value; }
        }

        /// <summary>
        /// Клетка
        /// </summary>
        /// <param name="Point">Точка</param>
        /// <returns>Клетка</returns>
        public MapCell this[MapPoint Point]
        {
            get { return this.Cells[Point]; }
            set { this.Cells[Point] = value; }
        }

        /// <summary>
        /// Уровень карты
        /// </summary>
        /// <param name="Size">Размер (в клетках)</param>
        public MapLevel(MapSize Size)
        {
            this.Size = Size;

            Cells = new MapMatrix<MapCell>(Size);
        }
    }
}
