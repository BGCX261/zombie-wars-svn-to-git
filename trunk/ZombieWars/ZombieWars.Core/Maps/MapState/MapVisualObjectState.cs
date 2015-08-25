using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZombieWars.Core.Maps
{
    /// <summary>
    /// Состояние тайла (отображаемого объекта карты)
    /// </summary>
    public class MapVisualObjectState : MapObjectState
    {       
        /// <summary>
        /// Тайл
        /// </summary>
        public MapTile Tile { get { return (this.VisualObject.Tile); } }

        /// <summary>
        /// Картинка
        /// </summary>
        public MapImage Image { get { return (this.Tile != null) ? Tile.Image : null; } }

        /// <summary>
        /// Отображаемый объект
        /// </summary>
        protected MapVisualObject VisualObject { get { return this.MapObject as MapVisualObject; } }

        /// <summary>
        /// Размер
        /// </summary>
        public virtual MapSize Size { get { return Tile.Size; } }

        /// <summary>
        /// Состояние тайла (отображаемого объекта карты)
        /// </summary>
        /// <param name="Tile">Тайл</param>
        public MapVisualObjectState(MapVisualObject Object)
            : base(Object)
        {
        }
    }
}
