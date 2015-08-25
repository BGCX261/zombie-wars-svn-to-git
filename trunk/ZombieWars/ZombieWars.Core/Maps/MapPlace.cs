using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace ZombieWars.Core.Maps
{
    /// <summary>
    /// Покрытие клетки карты
    /// </summary>
    [Serializable]
    [Guid("36F10910-E43F-431E-8C8E-F4C70816E4AD")]
    public class MapPlace : MapVisualObject, IMapPassable
    {
        /// <summary>
        /// Проходимость (0..1)
        /// </summary>
        public MapPassability Passability { get; set; }

        /// <summary>
        /// Картинка
        /// </summary>
        public MapImage Image { get { return this.Tile.Image; } set { Tile = new MapTile(value, Tile.Size); } }

        /// <summary>
        /// Покрытие клетки карты
        /// </summary>        
        /// <param name="Image">Картинка</param>
        /// <param name="Passbility">Проходимость (0..1)</param>
        public MapPlace(MapImage Image, MapPassability Passability)
            : base(new MapTile(Image, MapSize.One))
        {            
            this.Passability = Passability;
        }

        public MapPlace()
            : this(MapImage.Empty, MapPassability.Full)
        {
        }
       
    }
}
