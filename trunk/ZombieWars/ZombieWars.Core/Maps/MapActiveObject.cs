using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZombieWars.Core.Maps
{
    /// <summary>
    /// Активный объект карты
    /// </summary>
    [Serializable]
    public class MapActiveObject : MapVisualObject, IMapPassable, IMapDestroyable
    {
        /// <summary>
        /// Здоровье (0...)
        /// </summary>
        public UInt16 Health { get; set; }

        /// <summary>
        /// Требуемая проходимость (0..1)
        /// </summary>
        public MapPassability Passability { get; set; }

        /// <summary>
        /// Тип брони
        /// </summary>
        public MapArmorType ArmorType { get; set; }

        /// <summary>
        /// Направление картинки
        /// </summary>
        public MapDirection BaseDirection { get; set; }

        /// <summary>
        /// Разрушенный активный объект
        /// </summary>
        public MapActiveObject DestroyedActiveObject { get; set; }

        /// <summary>
        /// Картинка
        /// </summary>
        public MapImage Image { get { return this.Tile.Image; } set { Tile = new MapTile(value, Tile.Size); } }

        /// <summary>
        /// Активный объект карты
        /// </summary>        
        /// <param name="Image">Картинка</param>
        /// <param name="DestroyedActiveObject">Разрушенный активный объект (если нет - null)</param>
        /// <param name="Size">Размер (в клетках карты)</param>
        /// <param name="Direction">Направление</param>
        /// <param name="ArmorType">Тип брони</param>
        /// <param name="Health">Здоровье (0...)</param>
        /// <param name="Passability">Проходимость (0..1)</param>
        public MapActiveObject(MapImage Image, MapActiveObject DestroyedActiveObject, MapSize Size, MapDirection Direction, MapArmorType ArmorType, UInt16 Health, MapPassability Passability)
            :base(new MapTile(Image, Size))
        {
            if ((Passability < 0) || (Passability > 1)) throw new ArgumentOutOfRangeException("Passability", "Passability of Place must be in range 0..1");

            this.DestroyedActiveObject = DestroyedActiveObject;
            this.BaseDirection = Direction;
            this.Health = Health;
            this.Passability = Passability;
            this.ArmorType = ArmorType;
        }

        public MapActiveObject()
            : this(MapImage.Empty, null, new MapSize(1, 1), MapDirection.North, MapArmorType.None, 1, 0)
        {
        }      
    }
}
