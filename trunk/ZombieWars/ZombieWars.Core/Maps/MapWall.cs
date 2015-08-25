using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace ZombieWars.Core.Maps
{
    /// <summary>
    /// Стена
    /// </summary>
    [Serializable]
    [Guid("D43CB601-E898-43D2-8DCA-BAFDBE9BA26B")]
    public class MapWall : MapVisualObject, IMapDestroyable, IMapPassable
    {
        /// <summary>
        /// "Здоровье" стены (0...)
        /// </summary>
        public UInt16 Health { get; set; }

        /// <summary>
        /// Тип брони стены
        /// </summary>
        public MapArmorType ArmorType { get; set; }

        /// <summary>
        /// Проходимость через стену (0..1)
        /// </summary>
        public MapPassability Passability { get { return GetPassabilityByState(this.Health, this.Mode); } }

        /// <summary>
        /// Проходимость в зависимости от количества здоровья и режима
        /// </summary>
        /// <param name="Heatlh">Здоровье</param>
        /// <param name="Mode">Режим</param>
        /// <returns>Проходимость</returns>
        public static MapPassability GetPassabilityByState(UInt16 Heatlh, MapWallMode Mode)
        {
            if (Mode == MapWallMode.Wall)
            {
                if (Heatlh == 0) return MapPassability.Full;
                else return MapPassability.None;
            }
            if (Mode == MapWallMode.Window) return MapPassability.None; // TODO включить влезание через окно
            if (Mode == MapWallMode.Door) return MapPassability.Full;
            return MapPassability.None;
        }

        /// <summary>
        /// Наличие окна
        /// </summary>
        public bool HasWindow { get { return (ImageWindow != null); } }

        /// <summary>
        /// Наличие двери
        /// </summary>
        public bool HasDoor { get { return (ImageDoor != null); } }

        /// <summary>
        /// Наличие угла
        /// </summary>
        public bool HasCorner { get { return (ImageCorner != null); } }

        /// <summary>
        /// Картинка стены
        /// </summary>
        public MapImage ImageWall { get { return this.Tile.Image; } set { Tile = new MapTile(value, Tile.Size); } }
        /// <summary>
        /// Картинка окна
        /// </summary>
        public MapImage ImageWindow { get; set; }
        /// <summary>
        /// Картинка двери
        /// </summary>
        public MapImage ImageDoor { get; set; }

        /// <summary>
        /// Картинка угла
        /// </summary>
        public MapImage ImageCorner { get; set; }

        /// <summary>
        /// Режим стены (закрыто, открыто окно, открыта дверь)
        /// </summary>
        public MapWallMode Mode { get; set; }

		///// <summary>
		///// Разрушенная стена
		///// </summary>
		//public MapWall DestroyedWall { get; set; }

        /// <summary>
        /// Ориентация картинок
        /// </summary>
        public MapDirection BaseDirection { get; set; }

        /// <summary>
        /// Ориентация угла относительно картинок
        /// </summary>
        public MapDirection CornerDirection { get; set; }        

        /// <summary>
        /// Стена
        /// </summary>        
        /// <param name="ImageWall">Картинка в закрытом состоянии</param>        
        /// <param name="Health">Здовровье стены</param>
        /// <param name="Mode">Режим стены</param>        
        public MapWall(MapImage ImageWall, MapDirection BaseDirection, UInt16 Health)
            : base(new MapTile(ImageWall, new MapSize(1, 1)))
        {            
            this.ImageWindow = ImageWindow;
            this.ImageDoor = ImageDoor;            
            this.Health = Health;
            this.ArmorType = MapArmorType.Building;
            this.Mode = MapWallMode.Wall;
        }

        public MapWall()
            : this(MapImage.Empty, MapDirection.North, 1)
        {
        }
       
    }
}
