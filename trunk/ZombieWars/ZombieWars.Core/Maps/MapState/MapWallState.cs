using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZombieWars.Core.Maps
{
    /// <summary>
    /// Состояние стены
    /// </summary>
    public class MapWallState : MapVisualObjectState, IMapDestroyable, IMapPassable
    {
        /// <summary>
        /// Стена
        /// </summary>
        public MapWall Wall
        { 
            get { return this.MapObject as MapWall; }
            protected set { this.MapObject = value; }
        }

        /// <summary>
        /// Направление стены в клетке
        /// </summary>
        public MapDirection Direction { get; private set; }

        /// <summary>
        /// "Здоровье" стены (0...)
        /// </summary>
        public UInt16 Health 
        {
            get { return _Health; }
            set
            {
                _Health = value;
                if (_Health <= 0)
                {
                    Destroy();
                }
            }
        }
        private UInt16 _Health;

        /// <summary>
        /// Тип брони
        /// </summary>
        public MapArmorType ArmorType { get { return Wall.ArmorType; } }

        /// <summary>
        /// Проходимость через стену (0..1)
        /// </summary>
        public MapPassability Passability { get { return MapWall.GetPassabilityByState(this.Health, this.Mode); } }        

        /// <summary>
        /// Наличие окна
        /// </summary>
        public bool HasWindow { get { return Wall.HasWindow; } }
        /// <summary>
        /// Наличие двери
        /// </summary>
        public bool HasDoor { get { return Wall.HasDoor; } }

        /// <summary>
        /// Состояние открытости
        /// </summary>
        public MapWallMode Mode
        {
            get { return _Mode; }
            set
            {
                MapWallMode mode = value;
                if ((value == MapWallMode.Door) && (!this.HasDoor)) mode = MapWallMode.Wall;
                if ((value == MapWallMode.Window) && (!this.HasWindow)) mode = MapWallMode.Wall;
                this._Mode = mode;
            }
        }
        private MapWallMode _Mode;

        /// <summary>
        /// Текущая картинка (в зависимости от режима)
        /// </summary>
        public MapImage CurrentImage
        {
            get
            {
                if (Wall == null) return null;
                if (Mode == MapWallMode.Wall) return Wall.ImageWall;
                if (Mode == MapWallMode.Door) return Wall.ImageDoor ?? Wall.ImageWall;
                if (Mode == MapWallMode.Window) return Wall.ImageWindow ?? Wall.ImageWall;
                return Wall.ImageWall;
            }
        }

		///// <summary>
		///// Разрушенная стена
		///// </summary>
		//public MapWall DestroyedWall { get { return Wall.DestroyedWall; } }

        /// <summary>
        /// Стена разрушена, но ещё не заменена на разрушенную
        /// </summary>
        public event EventHandler<MapWallEventArgs> Destroying = null;

        /// <summary>
        /// Стена разрушена и заменена на разрушенную
        /// </summary>
        public event EventHandler<MapWallEventArgs> Destroyed = null;

        /// <summary>
        /// Состояние стены
        /// </summary>
        /// <param name="Wall">Стена</param>
        /// <param name="Direction">Направление стены в клетке</param>
        public MapWallState(MapWall Wall, MapDirection Direction)
            : base(Wall)
        {
            this._Health = Wall.Health;
            this.Mode = Wall.Mode;
            this.Direction = Direction;
        }

        /// <summary>
        /// Замещение стены на разрушенную (если есть)
        /// </summary>
        protected void Destroy()
        {
            if (Destroying != null)
                Destroying(this, new MapWallEventArgs(this));

			//if (DestroyedWall != null)
			//{
			//    this.Wall = DestroyedWall;
			//    this._Health = this.Wall.Health;
			//    this.Mode = this.Wall.Mode;
			//}
			//else
			//{
			//    this.Wall = null;
			//}

            if (Destroyed != null)
                Destroyed(this, new MapWallEventArgs(this));
        }      
    }

    /// <summary>
    /// Аргумент - стена
    /// </summary>
    public class MapWallEventArgs : EventArgs
    {        
        /// <summary>
        /// Стена
        /// </summary>
        public MapWallState Wall { get; set; }
      
        /// <summary>
        /// Аргумент - стена
        /// </summary>
        /// <param name="Wall">Стена</param>
        public MapWallEventArgs(MapWallState Wall)
        {
            this.Wall = Wall;
        }
    }
}
