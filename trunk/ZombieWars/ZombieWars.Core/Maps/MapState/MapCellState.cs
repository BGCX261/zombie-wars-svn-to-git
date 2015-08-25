using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZombieWars.Core.Maps
{
    /// <summary>
    /// Состояние клетки карты
    /// </summary>
    public class MapCellState
    {
        /// <summary>
        /// Клетка
        /// </summary>
        public MapCell Cell { get; protected set; }

        /// <summary>
        /// Координаты клетки
        /// </summary>
        public MapPoint Point { get; private set; }

        /// <summary>
        /// Состояние покрытия клетки
        /// </summary>
        public MapPlaceState Place { get; set; }

        #region Walls

        /// <summary>
        /// Северная стена
        /// </summary>
        protected MapWallState NorthWall;
        /// <summary>
        /// Южная стена
        /// </summary>
        protected MapWallState SouthWall;
        /// <summary>
        /// Западная стена
        /// </summary>
        protected MapWallState WestWall;
        /// <summary>
        /// Восточная стена
        /// </summary>
        protected MapWallState EastWall;

        /// <summary>
        /// Получить стену по направлению
        /// </summary>
        /// <param name="Direction">Навправление</param>
        /// <returns></returns>
        public MapWallState GetWall(MapDirection Direction)
        {
            if (Direction == Maps.MapDirection.North) return NorthWall;
            if (Direction == Maps.MapDirection.South) return SouthWall;
            if (Direction == Maps.MapDirection.West) return WestWall;
            if (Direction == Maps.MapDirection.East) return EastWall;
            return null;
        }

        /// <summary>
        /// Задать стену по направлению
        /// </summary>
        /// <param name="Direction">Направление</param>
        /// <param name="Wall">Стена</param>
        public void SetWall(MapDirection Direction, MapWallState Wall)
        {
            if (Direction == Maps.MapDirection.North) NorthWall = Wall;
            if (Direction == Maps.MapDirection.South) SouthWall = Wall;
            if (Direction == Maps.MapDirection.West) WestWall = Wall;
            if (Direction == Maps.MapDirection.East) EastWall = Wall;            
        }

        /// <summary>
        /// Массив стен
        /// </summary>
        public Dictionary<MapDirection, MapWallState> Walls
        {
            get
            {
                Dictionary<MapDirection, MapWallState> walls = new Dictionary<MapDirection, MapWallState>();
                if (NorthWall != null) walls.Add(MapDirection.North, NorthWall);
                if (SouthWall != null) walls.Add(MapDirection.South, SouthWall);
                if (WestWall != null) walls.Add(MapDirection.West, WestWall);
                if (EastWall != null) walls.Add(MapDirection.East, EastWall);
                return walls;
            }
        }

        /// <summary>
        /// Есть ли стена по данному направлению
        /// </summary>
        /// <param name="Direction">Направление</param>
        /// <returns>Есть ли стена</returns>
        public bool HasWall(MapDirection Direction)
        {
            return GetWall(Direction) != null;
        }

        /// <summary>
        /// Есть ли данная стена
        /// </summary>
        /// <param name="Wall">Стена</param>
        /// <returns>Есть ли стена</returns>
        public bool HasWall(MapWallState Wall)
        {
            return Walls.ContainsValue(Wall);
        }

        /// <summary>
        /// Удалить стену по направлению
        /// </summary>
        /// <param name="Direction">Направление</param>
        public void RemoveWall(MapDirection Direction)
        {
            SetWall(Direction, null);
        }

        #endregion Walls

        #region ActiveObjects

        /// <summary>
        /// Активные объекты в данной клетке
        /// </summary>
        public MapActiveObjectState[] ActiveObjects { get; private set; }

        /// <summary>
        /// Добавить активный объект
        /// </summary>
        /// <param name="ActiveObject">Активный объект</param>
        public void AddActiveObject(MapActiveObjectState ActiveObject)
        {
            if (ActiveObject == null) return;
            List<MapActiveObjectState> objects;
            if (ActiveObjects == null)
            {
                objects = new List<MapActiveObjectState>();
                objects.Add(ActiveObject);
            }
            else
            {
                if (ActiveObjects.Contains(ActiveObject)) return;
                objects = new List<MapActiveObjectState>(ActiveObjects);
                objects.Add(ActiveObject);                
            }
            ActiveObjects = objects.ToArray();
        }

        /// <summary>
        /// Удалить активный объект
        /// </summary>
        /// <param name="ActiveObject">Активный объект</param>
        public void RemoveActiveObject(MapActiveObjectState ActiveObject)
        {
            if (ActiveObject == null) return;
            if (ActiveObjects == null) return;
            if (!ActiveObjects.Contains(ActiveObject)) return;
            List<MapActiveObjectState> objects = new List<MapActiveObjectState>(ActiveObjects);
            objects.Remove(ActiveObject);
            ActiveObjects = objects.ToArray();
        }

        #endregion ActiveObjects

        /// <summary>
        /// Стена разрушена, но ещё не заменена на разрушенную
        /// </summary>
        public MapCellEventHandler<MapWallEventArgs> WallDestroying;

        /// <summary>
        /// Стена разрушена и заменена на разрушенную
        /// </summary>
        public MapCellEventHandler<MapWallEventArgs> WallDestroyed;

        /// <summary>
        /// Проходимость с определённой стороны
        /// </summary>
        /// <param name="Direction">Сторона</param>
        /// <returns>Проходимость</returns>
        public float GetPassbilityByDirection(MapDirection Direction)
        {
            float passablity = (this.Place != null) ? this.Place.Passability : 1;
            MapWallState wall = GetWall(Direction);
            if (wall != null)
                passablity -= wall.Passability;
            foreach (MapActiveObjectState active in ActiveObjects)
                passablity -= active.Passability;
            if (passablity < 0) passablity = 0;
            return passablity;
        }

        /// <summary>
        /// Состояние клетки карты
        /// </summary>
        /// <param name="Cell">Клетка</param>
        /// <param name="Point">Координаты</param>
        public MapCellState(MapCell Cell, MapPoint Point)
        {
            if (Cell == null) throw new ArgumentNullException("Cell", "Cell of MapCellState cannot be null");
            
            this.Point = Point;
            this.Cell = Cell;
            this.Place = (Cell.Place != null) ? new MapPlaceState(Cell.Place) : null;

            if (Cell.Walls != null)
            {                
                foreach (KeyValuePair<MapDirection, MapWall> wall in Cell.Walls)
                {
                    if (wall.Value == null) continue;
                    MapWallState wallState = new MapWallState(wall.Value, wall.Key);
                    wallState.Destroying += (sender, args) => { OnWallDestroying(args.Wall); };
                    wallState.Destroyed += (sender, args) => { OnWallDestroyed(args.Wall); };
                    this.SetWall(wall.Key, wallState);
                }
            }           
        }        

        /// <summary>
        /// Вызов события разрушения стены
        /// </summary>
        /// <param name="Wall">Стена</param>
        protected void OnWallDestroying(MapWallState Wall)
        {
            if (WallDestroying != null)
                WallDestroying(this, this, new MapWallEventArgs(Wall));
        }    

        /// <summary>
        /// Вызов события разрушения стены
        /// </summary>
        /// <param name="Wall">Стена</param>
        protected void OnWallDestroyed(MapWallState Wall)
        {
            if (Wall.Wall == null)
            {
                if (this.HasWall(Wall))
                    this.RemoveWall(Wall.Direction);
            }

            if (WallDestroyed != null)
                WallDestroyed(this, this, new MapWallEventArgs(Wall));
        }        
    }

    /// <summary>
    /// Событие клетки карты
    /// </summary>
    /// <typeparam name="T">Тип атрибутов</typeparam>
    /// <param name="Sender">Источник</param>
    /// <param name="Cell">Клетка</param>
    /// <param name="EventArgs">Атрибуты</param>
    public delegate void MapCellEventHandler<T> (object Sender, MapCellState Cell, T EventArgs) where T: EventArgs;
}
