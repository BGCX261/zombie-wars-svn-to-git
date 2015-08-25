using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace ZombieWars.Core.Maps
{
    /// <summary>
    /// Точка перехода между районами
    /// </summary>
    [Serializable]
    [Guid("F3C01DA1-8B79-462D-B544-07BDD6043E65")]
    public class MapAreaTransitionPoint : MapObject
    {
        /// <summary>
        /// Откуда
        /// </summary>
        public MapArea From { get; set; }
        /// <summary>
        /// Куда
        /// </summary>
        public MapArea To { get; set; }
        /// <summary>
        /// Координаты левой верхней точки
        /// </summary>
        public MapPoint Position { get; set; }
        /// <summary>
        /// Размер (в клетках)
        /// </summary>
        public MapSize Size { get; set; }

        /// <summary>
        /// Точка перехода между районами
        /// </summary>        
        /// <param name="From">Откуда</param>
        /// <param name="To">Куда</param>
        /// <param name="Position">Координаты левой верхней точки</param>
        /// <param name="Size">Размер (в клетках)</param>
        public MapAreaTransitionPoint(MapArea From, MapArea To, MapPoint Position, MapSize Size)            
        {
            this.From = From;
            this.To = To;
            this.Position = Position;
            this.Size = Size;
        }

        /// <summary>
        /// Точка перехода между районами
        /// </summary>
        internal MapAreaTransitionPoint()
            : base()
        {
        }
    }
}
