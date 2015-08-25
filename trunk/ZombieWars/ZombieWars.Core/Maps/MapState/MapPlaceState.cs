using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZombieWars.Core.Maps
{
    /// <summary>
    /// Состояние покрытия клетки карты
    /// </summary>
    public class MapPlaceState : MapVisualObjectState, IMapPassable
    {
        /// <summary>
        /// Покрытие клетки
        /// </summary>
        public MapPlace Place { get { return this.MapObject as MapPlace; } }

        /// <summary>
        /// Проходимость (0..1)
        /// </summary>
        public MapPassability Passability { get; protected set; }

        /// <summary>
        /// Состояние покрытия клекти карты
        /// </summary>
        /// <param name="Place">Покрытие</param>
        public MapPlaceState(MapPlace Place)
            : base(Place)
        {
            this.Passability = Place.Passability;
        }
    }
}
