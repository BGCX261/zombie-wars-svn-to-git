using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace ZombieWars.Core.Maps
{
    /// <summary>
    /// Район карты
    /// </summary>
    [Serializable]
    [Guid("2B5730F0-3973-4FE2-8F63-BB3F687E68B8")]
    public class MapArea : MapObject
    {
        /// <summary>
        /// Координаты левой верхней точки
        /// </summary>
        public MapPoint Position { get; set; }
        /// <summary>
        /// Размер (в клетках)
        /// </summary>
        public MapSize Size { get; set; }

        /// <summary>
        /// Точки перехода
        /// </summary>
        public List<MapAreaTransitionPoint> TransitionPoints { get; internal set; }

        /// <summary>
        /// Район карты
        /// </summary>        
        /// <param name="Position">Координаты левой верхней точки</param>
        /// <param name="Size">Размер (в клетках)</param>
        /// <param name="TransitionPoints">Точки перехода</param>
        public MapArea(MapPoint Position, MapSize Size)            
        {
            this.Position = Position;
            this.Size = Size;
            this.TransitionPoints = new List<MapAreaTransitionPoint>();
        }

        /// <summary>
        /// Район карты
        /// </summary>
        internal MapArea()
            : base()
        {
            TransitionPoints = new List<MapAreaTransitionPoint>();
        }

    }
}
