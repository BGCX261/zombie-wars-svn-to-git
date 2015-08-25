using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace ZombieWars.Core.Maps
{
    /// <summary>
    /// Состояние карты
    /// </summary>
	[Guid("261DEAD8-8C83-444D-A048-3ECF537E2E9B")]
    public class MapState : MapObjectState
    {
		/// <summary>
		/// Идентификатор класса
		/// </summary>
		public Guid ClassId { get { return this.GetType().GUID; } }

        /// <summary>
        /// Карта
        /// </summary>
        public Map Map { get; protected set; }

        public MapCellRange Range { get { return Map.Range; } }

        /// <summary>
        /// Размер уровней карты (в клетках)
        /// </summary>
        public MapSize Size { get { return Map.Size; } }
        /// <summary>
        /// Количество уровней
        /// </summary>
        public UInt16 LevelsCount { get { return Map.LevelsCount; } }

        /// <summary>
        /// Состояние уровней
        /// </summary>
        public MapLevelState[] Levels { get; protected set; }

        /// <summary>
        /// Районы, кварталы, жилые массивы
        /// </summary>
        public MapAreasState Areas { get; set; }

        /// <summary>
        /// Активные объекты
        /// </summary>
        public Dictionary<Guid, MapActiveObjectState> ActiveObjects { get; protected set; }

        /// <summary>
        /// Стена разрушена, но ещё не заменена на разрушенную
        /// </summary>
        public event MapCellEventHandler<MapWallEventArgs> WallDestroying;
        /// <summary>
        /// Стена разрушена и заменена на разрушенную
        /// </summary>
        public event MapCellEventHandler<MapWallEventArgs> WallDestroyed;
        /// <summary>
        /// Активный объект разрушен, но ещё не заменен на разрушенную
        /// </summary>
        public event EventHandler<MapActiveObjectEventArgs> ActiveObjectDestroying;
        /// <summary>
        /// Активный объект разрушен
        /// </summary>
        public event EventHandler<MapActiveObjectEventArgs> ActiveObjectDestroyed;

        /// <summary>
        /// Клетка
        /// </summary>
        /// <param name="Level">Уровень</param>
        /// <param name="X">X</param>
        /// <param name="Y">Y</param>
        /// <returns>Клетка</returns>
        public MapCellState this[UInt16 Level, UInt16 X, UInt16 Y]
        {
            get
            {
                if (Level >= LevelsCount) throw new ArgumentOutOfRangeException("Level", "Level cannot be out of Map");
                return this.Levels[Level].Cells[X, Y];
            }
            set
            {
                if (Level >= LevelsCount) throw new ArgumentOutOfRangeException("Level", "Level cannot be out of Map");
                this.Levels[Level].Cells[X, Y] = value;
            }
        }

        /// <summary>
        /// Клетка
        /// </summary>
        /// <param name="Level">Уровень</param>
        /// <param name="X">X</param>
        /// <param name="Y">Y</param>
        /// <returns>Клетка</returns>
        public MapCellState this[Int32 Level, Int32 X, Int32 Y]
        {
            get
            {
                if (Level < 0) throw new ArgumentOutOfRangeException("Level", "Level cannot be < 0");
                if (Level >= LevelsCount) throw new ArgumentOutOfRangeException("Level", "Level cannot be out of Map");
                return this.Levels[Level].Cells[X, Y];
            }
            set
            {
                if (Level < 0) throw new ArgumentOutOfRangeException("Level", "Level cannot be < 0");
                this.Levels[Level].Cells[X, Y] = value;
            }
        }

        /// <summary>
        /// Клетка
        /// </summary>
        /// <param name="Point">Точка</param>
        /// <returns>Клетка</returns>
        public MapCellState this[MapPoint Point]
        {
            get
            {
                if (Point.Level >= LevelsCount) throw new ArgumentOutOfRangeException("Point.Level", "Point.Level cannot out of Map");
                return this.Levels[Point.Level].Cells[Point];
            }
            set
            {
                if (Point.Level >= LevelsCount) throw new ArgumentOutOfRangeException("Point.Level", "Point.Level cannot out of Map");
                this.Levels[Point.Level].Cells[Point] = value;
            }
        }

        public MapCellState CreateCell(MapPlace place, Dictionary<MapDirection, MapWall> walls, MapPoint point)
        {
            MapCellState currentCell = this[point];
            if (currentCell != null)
            {
                if (currentCell.ActiveObjects != null)
                {
                    foreach (var activeObject in currentCell.ActiveObjects)
                        this.RemoveActiveObject(GetIdOfActiveObject(activeObject));
                }
            }
            MapCell cell = Map[point] = new MapCell(place, walls);
            return (this[point] = new MapCellState(cell, point));
        }

        public MapCellState SetPlace(MapPlace place, MapPoint point)
        {
            MapCellState cell = this[point];
            if (cell == null)
            {
                return (cell = CreateCell(place, null, point));
            }
            else
            {
                return (cell = CreateCell(place, cell.Cell.Walls, point));
            }
        }

        /// <summary>
        /// Состояние карты
        /// </summary>
        /// <param name="Map">Карта</param>
        public MapState(Map Map)
            : base(Map)
        {
            if (Map == null) throw new ArgumentOutOfRangeException("Map", "Map of MapState cannot be null");

            this.Map = Map;
            try
            {
                Levels = new MapLevelState[LevelsCount];
                for (UInt16 i = 0; i < LevelsCount; i++)
                {
                    Levels[i] = new MapLevelState(Map.Levels[i], i);
                    Levels[i].WallDestroying += (sender, cell, args) => { OnWallDestroying(cell, args.Wall); };
                    Levels[i].WallDestroyed += (sender, cell, args) => { OnWallDestroyed(cell, args.Wall); };
                }
                Areas = new MapAreasState(Map.Areas);
                ActiveObjects = new Dictionary<Guid, MapActiveObjectState>();
            }
            catch
            {
                throw;
            }
        }
       
        /// <summary>
        /// Вызов события разрушения стены
        /// </summary>
        /// <param name="Cell">Клетка, в которой расположена стена</param>
        /// <param name="Wall">Стена</param>
        protected void OnWallDestroying(MapCellState Cell, MapWallState Wall)
        {
            if (WallDestroying != null)
                WallDestroying(this, Cell, new MapWallEventArgs(Wall));
        }

        /// <summary>
        /// Вызов события разрушения стены
        /// </summary>
        /// <param name="Cell">Клетка, в которой расположена стена</param>
        /// <param name="Wall">Стена</param>
        protected void OnWallDestroyed(MapCellState Cell, MapWallState Wall)
        {            
            if (WallDestroyed != null)
                WallDestroyed(this, Cell, new MapWallEventArgs(Wall));
        }

        /// <summary>
        /// Вызов события разрушения активного объекта
        /// </summary>
        /// <param name="ActiveObject">Активный объект</param>
        protected void OnActiveObjectDestroying(MapActiveObjectState ActiveObject)
        {
            DeAttachActiveObject(ActiveObject);
            if (ActiveObjectDestroying != null)
                ActiveObjectDestroying(this, new MapActiveObjectEventArgs(ActiveObject));
        }

        /// <summary>
        /// Вызов события разрушения активного объекта
        /// </summary>
        /// <param name="ActiveObject">Активный объект</param>
        protected void OnActiveObjectDestroyed(MapActiveObjectState ActiveObject)
        {
            if (ActiveObject.ActiveObject == null)
            {
                RemoveActiveObject(GetIdOfActiveObject(ActiveObject));
            }
            else
            {
                if (!CheckPlaceForActiveObject(ActiveObject.ActiveObject, ActiveObject.Position))
                {
                    throw new OverflowException("New ActiveObject cannot be located to place of destroyed old ActiveObject");
                }
                AttachActiveObject(ActiveObject, ActiveObject.Position);
            }
            if (ActiveObjectDestroyed != null)
                ActiveObjectDestroyed(this, new MapActiveObjectEventArgs(ActiveObject));
        }

        #region ActiveObjects attachment

        /// <summary>
        /// Идентификатор активных объектов
        /// </summary>
        public Guid[] ActiveObjectIds { get { return ActiveObjects.Keys.ToArray(); } }
        /// <summary>
        /// Активные объекты
        /// </summary>
        public MapActiveObjectState[] ActiveObjectList { get { return ActiveObjects.Values.ToArray(); } }

        /// <summary>
        /// Получить активный объект по идентификатору
        /// </summary>
        /// <param name="Id">Идентификатор</param>
        /// <returns>Объект</returns>
        public MapActiveObjectState GetActiveObjectById(Guid Id)
        {
            if (Id == null) throw new ArgumentNullException("Id", "Id cannot be null");
            if (!ActiveObjects.ContainsKey(Id)) throw new ArgumentException("ActiveObject", "This ActiveObject not found");
            return ActiveObjects[Id];
        }

        /// <summary>
        /// Проверить возможность размещения объекта на карт
        /// </summary>
        /// <param name="ActiveObject">Активный объект</param>        
        /// <param name="Position">Координаты левой верхней точки</param>
        public bool CheckPlaceForActiveObject(MapActiveObject ActiveObject, MapPoint Position)
        {
            if (ActiveObject == null) throw new ArgumentNullException("ActiveObject", "ActiveObject cannot be null");

            if (Position.Level >= this.LevelsCount)
                throw new ArgumentOutOfRangeException("Position.Level", "Position.Level cannot be out of map");

            if ((Position.X + ActiveObject.Tile.Size.Width > this.Size.Width) 
                || (Position.Y + ActiveObject.Tile.Size.Height > this.Size.Height))
                throw new ArgumentOutOfRangeException("Position", "Position cannot be out of map");

            for (UInt16 x = Position.X; x < Position.X + ActiveObject.Tile.Size.Width; x++)
                for (UInt16 y = Position.Y; y < Position.Y + ActiveObject.Tile.Size.Height; y++)
                {
                    bool North = (y == Position.Y);
                    bool South = (y == (Position.Y + ActiveObject.Tile.Size.Height - 1));
                    bool West = (x == Position.X);
                    bool East = (x == (Position.Y + ActiveObject.Tile.Size.Width - 1));
                    if (!CheckCell(ActiveObject, Position, North, South, West, East)) return false;
                }

            return true;
        }
      

        /// <summary>
        /// Добавить новый объект на карту
        /// </summary>
        /// <param name="ActiveObject">Активный объект</param>
        /// <param name="Position">Координаты левой верхней точки</param>
        /// <param name="Id">Идентификатор (для десериализации)</param>
        /// <returns>Идентификатор</returns>
        public Guid AddActiveObject(MapActiveObject ActiveObject, MapPoint Position)
        {
            if (ActiveObject == null) throw new ArgumentNullException("ActiveObject", "ActiveObject cannot be null");

            if (Position.Level >= this.LevelsCount)
                throw new ArgumentOutOfRangeException("Position.Level", "Level cannot be out of map");

            if ((Position.X + ActiveObject.Tile.Size.Width > this.Size.Width)
                || (Position.Y + ActiveObject.Tile.Size.Height > this.Size.Height))
                throw new ArgumentOutOfRangeException("Position", "Position cannot be out of map");

            if (!CheckPlaceForActiveObject(ActiveObject, Position))
                throw new ArgumentException("ActiveObject", "ActiveObject cannot be located to this Position");

            Guid Id = Guid.NewGuid();

            MapActiveObjectState state = new MapActiveObjectState(ActiveObject, Position);
            state.Destroying += (sender, args) => { OnActiveObjectDestroying(args.ActiveObject); };
            state.Destroyed += (sender, args) => { OnActiveObjectDestroyed(args.ActiveObject); };
            this.ActiveObjects.Add(Id, state);
			
			AttachActiveObject(state, Position);

            return Id;
        }

		/// <summary>
		/// Добавить новый объект на карту
		/// </summary>
		/// <param name="ActiveObjectState">Активный объект</param>
		/// <param name="Position">Координаты левой верхней точки</param>
		/// <param name="Id">Идентификатор (для десериализации)</param>		
		/// <returns>Получилось ли добавить</returns>
		public bool AddActiveObject(MapActiveObjectState ActiveObjectState, Guid Id)
		{
			if (ActiveObjectState == null) throw new ArgumentNullException("ActiveObjectState", "ActiveObjectState cannot be null");

			if (ActiveObjects.ContainsKey(Id)) return false;

            if (ActiveObjectState.Position.Level >= this.LevelsCount)
				throw new ArgumentOutOfRangeException("Position.Level", "Level cannot be out of map");

            if ((ActiveObjectState.Position.X + ActiveObjectState.Tile.Size.Width >= this.Size.Width)
                || (ActiveObjectState.Position.Y + ActiveObjectState.Tile.Size.Height >= this.Size.Height))
				throw new ArgumentOutOfRangeException("Position", "Position cannot be out of map");

            if (!CheckPlaceForActiveObject(ActiveObjectState.ActiveObject, ActiveObjectState.Position))
				throw new ArgumentException("ActiveObject", "ActiveObject cannot be located to this Position");			

			ActiveObjectState.Destroying += (sender, args) => { OnActiveObjectDestroying(args.ActiveObject); };
			ActiveObjectState.Destroyed += (sender, args) => { OnActiveObjectDestroyed(args.ActiveObject); };
			this.ActiveObjects.Add(Id, ActiveObjectState);

            AttachActiveObject(ActiveObjectState, ActiveObjectState.Position);

			return true;
		}

        /// <summary>
        /// Удалить активный объект с карты
        /// </summary>
        /// <param name="Id">Идентификатор объекта</param>
        public void RemoveActiveObject(Guid Id)
        {                        
            MapActiveObjectState ActiveObject = GetActiveObjectById(Id);
            DeAttachActiveObject(ActiveObject);
            ActiveObjects.Remove(Id);
        }		

        /// <summary>
        /// Переместить объект в другое место карты
        /// </summary>
        /// <param name="Id">Идентификатор объекта</param>        
        /// <param name="Position">Координаты левой верхней точки</param>
        public void RelocateActiveObject(Guid Id, MapPoint Position)
        {
            MapActiveObjectState ActiveObject = GetActiveObjectById(Id);

            if (Position.Level >= this.LevelsCount)
                throw new ArgumentOutOfRangeException("Position.Level", "Position.Level cannot be out of map");            

            if ((Position.X + ActiveObject.Size.Width >= this.Size.Width)
                || (Position.Y + ActiveObject.Size.Height >= this.Size.Height))
                throw new ArgumentOutOfRangeException("Position", "Position cannot be out of map");

            if (!CheckPlaceForActiveObject(ActiveObject.ActiveObject, Position))
                throw new ArgumentException("ActiveObject", "ActiveObject cannot be located to this Position");

            DeAttachActiveObject(ActiveObject);
            AttachActiveObject(ActiveObject, Position);
        }

        /// <summary>
        /// Идентификатор размещения активного объекта
        /// </summary>
        /// <param name="ActiveObject">Активный объект</param>
        /// <returns>Идентифкатор размещения</returns>
        public Guid GetIdOfActiveObject(MapActiveObjectState ActiveObject)
        {
            if (ActiveObject == null) throw new ArgumentNullException("ActiveObject", "ActiveObject cannot be null");
            if (!ActiveObjects.ContainsValue(ActiveObject)) throw new ArgumentException("ActiveObject", "This ActiveObject not found");
            Guid id = ActiveObjects.Where(ao => ao.Value == ActiveObject).FirstOrDefault().Key;
            return id;
        }

        /// <summary>
        /// "Отклеить" объект от карты
        /// </summary>
        /// <param name="ActiveObject">Активный объект</param>
        private void DeAttachActiveObject(MapActiveObjectState ActiveObject)
        {
            for (UInt16 x = ActiveObject.Position.X; x < ActiveObject.Position.X + ActiveObject.Size.Width; x++)
                for (UInt16 y = ActiveObject.Position.Y; y < ActiveObject.Position.Y + ActiveObject.Size.Height; y++)
                    Levels[ActiveObject.Position.Level].Cells[x, y].RemoveActiveObject(ActiveObject); 
        }

        /// <summary>
        /// "Приклеить" объект к карте
        /// </summary>
        /// <param name="ActiveObject">Активный объект</param>        
        /// <param name="Position">Координаты левой верхней точки</param>
        private void AttachActiveObject(MapActiveObjectState ActiveObject, MapPoint Position)
        {
            ActiveObject.Locate(Position);
            for (UInt16 x = ActiveObject.Position.X; x < ActiveObject.Position.X + ActiveObject.Size.Width; x++)
                for (UInt16 y = ActiveObject.Position.Y; y < ActiveObject.Position.Y + ActiveObject.Size.Height; y++)
                    Levels[Position.Level].Cells[x, y].AddActiveObject(ActiveObject);
        }       

        /// <summary>
        /// Проверка возможности размещения в данной клетке объекта
        /// </summary>
        /// <param name="ActiveObject">Объект</param>
        /// <param name="Position">Координаты</param>
        /// <param name="IgnoreNorth">Игнорируется ли северная стена</param>
        /// <param name="IgnoreSouth">Мгнорируется ли южная стена</param>
        /// <param name="IgnoreWest">Игнорируется ли западная стена</param>
        /// <param name="IgnoreEast">Игнорируется ли восточная стена</param>
        /// <returns>Возможно ли разместить объект в данной клетке</returns>
        private bool CheckCell(MapActiveObject ActiveObject, MapPoint Position, bool IgnoreNorth, bool IgnoreSouth, bool IgnoreWest, bool IgnoreEast)
        {
            MapCellState cell = Levels[Position.Level].Cells[Position];
            if ((cell.Place != null) && (cell.Place.Passability < ActiveObject.Passability)) return false;          
            if (!IgnoreNorth && (cell.GetPassbilityByDirection(MapDirection.North) < ActiveObject.Passability)) return false;
            if (!IgnoreSouth && (cell.GetPassbilityByDirection(MapDirection.South) < ActiveObject.Passability)) return false;
            if (!IgnoreWest && (cell.GetPassbilityByDirection(MapDirection.West) < ActiveObject.Passability)) return false;
            if (!IgnoreEast && (cell.GetPassbilityByDirection(MapDirection.East) < ActiveObject.Passability)) return false;
            return true;
        }

        #endregion ActiveObjects attachment

    }
}
