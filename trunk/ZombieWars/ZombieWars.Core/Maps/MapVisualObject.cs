using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZombieWars.Core.Maps
{
    /// <summary>
    /// Объект карты имеющий отображение
    /// </summary>
    public class MapVisualObject : MapObject
    {
        /// <summary>
        /// Тайл объекта для отображения на карте
        /// </summary>
        public MapTile Tile { get; set; }

        /// <summary>
        /// Объект карты имеющий отображение
        /// </summary>
        /// <param name="Name">Название</param>
        /// <param name="Tile">Тайл объекта для отображения на карте</param>
        public MapVisualObject(MapTile Tile)            
        {
            this.Tile = Tile;
        }

        /// <summary>
        /// Объект карты имеющий отображение
        /// </summary>
        internal MapVisualObject()
            : base()
        {
        }
    }
}
