using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZombieWars.Core.Maps
{
    /// <summary>
    /// Состояние активного объекта карты
    /// </summary>
    public class MapActiveObjectState : MapVisualObjectState, IMapPassable, IMapDestroyable
    {
        /// <summary>
        /// Активный объект
        /// </summary>
        public MapActiveObject ActiveObject 
        { 
            get { return this.MapObject as MapActiveObject; }
            protected set { this.MapObject = value; }
        }

        /// <summary>
        /// Здоровье (0...)
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
        public MapArmorType ArmorType { get; set; }

        /// <summary>
        /// Проходимость (0..1)
        /// </summary>
        public MapPassability Passability { get { return ActiveObject.Passability; } }        

        /// <summary>
        /// Координаты левой верхней точки
        /// </summary>
        public MapPoint Position { get; protected set; }

        /// <summary>
        /// Расположить активный объект
        /// </summary>        
        /// <param name="Position">Левая верхняя точка</param>
        public void Locate(MapPoint Position)
        {            
            this.Position = Position;
        }

        /// <summary>
        /// Направление
        /// </summary>
        public MapDirection Direction
        {
            get { return _Direction; }
            set
            {
                if ((ActiveObject.BaseDirection == Maps.MapDirection.North) || (ActiveObject.BaseDirection == Maps.MapDirection.South))
                {
                    if ((value == Maps.MapDirection.East) || (value == Maps.MapDirection.West))
                    {
                        this.ActualSize = new MapSize(ActiveObject.Tile.Size.Height, ActiveObject.Tile.Size.Width);
                    }
                    else
                    {
                        this.ActualSize = new MapSize(ActiveObject.Tile.Size.Width, ActiveObject.Tile.Size.Height);
                    }
                }

                if ((ActiveObject.BaseDirection == Maps.MapDirection.West) || (ActiveObject.BaseDirection == Maps.MapDirection.East))
                {
                    if ((value == Maps.MapDirection.North) || (value == Maps.MapDirection.South))
                    {
                        this.ActualSize = new MapSize(ActiveObject.Tile.Size.Height, ActiveObject.Tile.Size.Width);
                    }
                    else
                    {
                        this.ActualSize = new MapSize(ActiveObject.Tile.Size.Width, ActiveObject.Tile.Size.Height);
                    }
                }

                _Direction = value;
            }
        }
        private MapDirection _Direction;

        /// <summary>
        /// Размер (в клетках)
        /// </summary>
        protected MapSize ActualSize { get; set; }

        /// <summary>
        /// Размер (в клетках)
        /// </summary>
        public override MapSize Size { get { return this.ActualSize; } }

        /// <summary>
        /// Разрушенный активный объект
        /// </summary>
        public MapActiveObject DestroyedActiveObject { get { return ActiveObject.DestroyedActiveObject; } }

        /// <summary>
        /// Активный объект разрушен, но ещё не заменён на разрушенный
        /// </summary>
        public event EventHandler<MapActiveObjectEventArgs> Destroying;

        /// <summary>
        /// Активный объект разрушен и заменён на разрушенный
        /// </summary>
        public event EventHandler<MapActiveObjectEventArgs> Destroyed;

        /// <summary>
        /// Состояние активного объекта карты
        /// </summary>
        /// <param name="ActiveObject">Активный объект</param>        
        /// <param name="Position">Координаты левой верхней точки</param>
        public MapActiveObjectState(MapActiveObject ActiveObject, MapPoint Position)
            : base(ActiveObject)
        {            
            this.Position = Position;            
            this.ArmorType = ActiveObject.ArmorType;
            this._Health = ActiveObject.Health;
            this._Direction = Direction;
            this.ActualSize = this.Size;
        }

        /// <summary>
        /// Замещение активного объекта на разрушенный (если есть)
        /// </summary>
        protected void Destroy()
        {
            if (Destroying != null)
                Destroying(this, new MapActiveObjectEventArgs(this));
            
            if (DestroyedActiveObject != null)
            {
                this.ActiveObject = DestroyedActiveObject;
                this.ArmorType = this.ActiveObject.ArmorType;
                this._Health = this.ActiveObject.Health;
                this.ActualSize = this.Size;
            }
            else
            {
                this.ActiveObject = null;
            }

            if (Destroyed != null)
                Destroyed(this, new MapActiveObjectEventArgs(this));
        }
     
    }

    /// <summary>
    /// Аргумент - активный объект
    /// </summary>
    public class MapActiveObjectEventArgs : EventArgs
    {
        /// <summary>
        /// Активный объект
        /// </summary>
        public MapActiveObjectState ActiveObject { get; set; }

        /// <summary>
        /// Аргумент - активный объект
        /// </summary>
        /// <param name="Wall">Активный объект</param>
        public MapActiveObjectEventArgs(MapActiveObjectState ActiveObject)
        {
            this.ActiveObject = ActiveObject;
        }
    }
}