using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZombieWars.Core.Maps
{
    /// <summary>
    /// Клетка карты
    /// </summary>
    [Serializable]
    public class MapCell
    {
        /// <summary>
        /// Покрытие
        /// </summary>
        public MapPlace Place { get; set; }

        #region Walls

        /// <summary>
        /// Северная стена
        /// </summary>
        protected MapWall NorthWall;
        /// <summary>
        /// Южная стена
        /// </summary>
        protected MapWall SouthWall;
        /// <summary>
        /// Западная стена
        /// </summary>
        protected MapWall WestWall;
        /// <summary>
        /// Восточная стена
        /// </summary>
        protected MapWall EastWall;

        /// <summary>
        /// Получить стену по направлению
        /// </summary>
        /// <param name="Direction">Навправление</param>
        /// <returns></returns>
        public MapWall GetWall(MapDirection Direction)
        {
            if (Direction == Maps.MapDirection.North) return NorthWall; else
            if (Direction == Maps.MapDirection.South) return SouthWall; else
            if (Direction == Maps.MapDirection.West) return WestWall; else
            if (Direction == Maps.MapDirection.East) return EastWall;
            return null;
        }

        /// <summary>
        /// Задать стену по направлению
        /// </summary>
        /// <param name="Direction">Направление</param>
        /// <param name="Wall">Стена</param>
        public void SetWall(MapDirection Direction, MapWall Wall)
        {
            if (Direction == Maps.MapDirection.North) NorthWall = Wall; else
            if (Direction == Maps.MapDirection.South) SouthWall = Wall; else
            if (Direction == Maps.MapDirection.West) WestWall = Wall; else
            if (Direction == Maps.MapDirection.East) EastWall = Wall;
        }

        /// <summary>
        /// Массив стен
        /// </summary>
        public Dictionary<MapDirection, MapWall> Walls
        {
            get
            {
                Dictionary<MapDirection, MapWall> walls = new Dictionary<MapDirection, MapWall>();
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
        public bool HasWall(MapWall Wall)
        {
            return Walls.ContainsValue(Wall);
        }

        /// <summary>
        /// Удалить стену по направлению
        /// </summary>
        /// <param name="Direction">Направление</param>
        protected void RemoveWall(MapDirection Direction)
        {
            SetWall(Direction, null);
        }

        #endregion Walls  

        #region ActiveObjects

        /// <summary>
        /// Активные объекты в данной клетке
        /// </summary>
        public MapActiveObject[] ActiveObjects { get; private set; }

        /// <summary>
        /// Добавить активный объект
        /// </summary>
        /// <param name="ActiveObject">Активный объект</param>
        public void AddActiveObject(MapActiveObject ActiveObject)
        {
            if (ActiveObject == null) return;
            List<MapActiveObject> objects;
            if (ActiveObjects == null)
            {
                objects = new List<MapActiveObject>();
                objects.Add(ActiveObject);
            }
            else
            {
                if (ActiveObjects.Contains(ActiveObject)) return;
                objects = new List<MapActiveObject>(ActiveObjects);
                objects.Add(ActiveObject);
            }
            ActiveObjects = objects.ToArray();
        }

        /// <summary>
        /// Удалить активный объект
        /// </summary>
        /// <param name="ActiveObject">Активный объект</param>
        public void RemoveActiveObject(MapActiveObject ActiveObject)
        {
            if (ActiveObject == null) return;
            if (ActiveObjects == null) return;
            if (!ActiveObjects.Contains(ActiveObject)) return;
            List<MapActiveObject> objects = new List<MapActiveObject>(ActiveObjects);
            objects.Remove(ActiveObject);
            ActiveObjects = objects.ToArray();
        }

        #endregion ActiveObjects

        /// <summary>
        /// Проходимость с определённой стороны
        /// </summary>
        /// <param name="Direction">Сторона</param>
        /// <returns>Проходимость</returns>
        public float GetPassbilityByDirection(MapDirection Direction)
        {
            float passablity = this.Place.Passability;
            MapWall wall = GetWall(Direction);
            if (wall != null)
                passablity -= wall.Passability;
            foreach (MapActiveObject active in ActiveObjects)
                passablity -= active.Passability;
            if (passablity < 0) passablity = 0;
            return passablity;
        }

        /// <summary>
        /// Клетка карты
        /// </summary>
        /// <param name="Place">Покрытие</param>
        /// <param name="Walls">Стены</param>
        public MapCell(MapPlace Place, Dictionary<MapDirection, MapWall> Walls)
        {
            if (Place == null) throw new ArgumentNullException("Place");
            this.Place = Place;
            if (Walls != null)
            {
                foreach (KeyValuePair<MapDirection, MapWall> wall in Walls)
                {
                    this.SetWall(wall.Key, wall.Value);
                }
            }
        }

        /// <summary>
        /// Клетка карты
        /// </summary>
        internal MapCell()
        {
        }
    }
}
