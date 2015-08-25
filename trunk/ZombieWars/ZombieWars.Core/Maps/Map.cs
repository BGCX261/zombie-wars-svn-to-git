using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace ZombieWars.Core.Maps
{
    /// <summary>
    /// Карта
    /// </summary>
    [Serializable]
    [Guid("87C049B4-6B9C-4218-92AD-D52435A29818")]
    public class Map : MapObject
    {
        /// <summary>
        /// Версия карты
        /// </summary>
        public Version Version { get; set; }

        /// <summary>
        /// Набор специфических тайлов
        /// </summary>
        public MapTileSet TileSet { get; internal set; }

        /// <summary>
        /// Размер карты (в клетках)
        /// </summary>
        public MapSize Size { get; internal set; }
        /// <summary>
        /// Количество уровней
        /// </summary>
        public UInt16 LevelsCount 
        { 
            get { return (byte)Levels.Length; }
            internal set
            {
                this.Levels = new MapLevel[value];
                for (int i = 0; i < LevelsCount; i++)
                    this.Levels[i] = new MapLevel(Size);
            }
        }

        /// <summary>
        /// Уровни
        /// </summary>
        public MapLevel[] Levels { get; internal set; }

        /// <summary>
        /// Районы, кварталы, жилые массивы
        /// </summary>
        public MapAreas Areas { get; set; }

        public MapCellRange Range
        {
            get 
            { 
                return new MapCellRange(new MapPoint(0, 0, 0), new MapPoint((ushort)(LevelsCount - 1), (ushort)(Size.Width - 1), (ushort)(Size.Height - 1))); 
            }
        }

        /// <summary>
        /// Клетка
        /// </summary>
        /// <param name="Level">Уровень</param>
        /// <param name="X">X</param>
        /// <param name="Y">Y</param>
        /// <returns>Клетка</returns>
        public MapCell this[UInt16 Level, UInt16 X, UInt16 Y]
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
        public MapCell this[Int32 Level, Int32 X, Int32 Y]
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
        public MapCell this[MapPoint Point]
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

        /// <summary>
        /// Карта
        /// </summary>
        /// <param name="Name">Название</param>
        /// <param name="Description">Описание</param>
        /// <param name="LevelsCount">Количество уровней</param>
        /// <param name="Size">Размер карты (в клетках)</param>
        public Map(UInt16 LevelsCount, MapSize Size)            
        {
            if (LevelsCount <= 0) throw new ArgumentOutOfRangeException("LevelsCount", "LevelsCount of Map cannot be 0");
         
            this.Size = Size;
            this.LevelsCount = LevelsCount;

            this.Areas = new MapAreas();
            this.TileSet = new MapTileSet();
            this.Version = new Version(0, 0, 0, 0);
        }

        /// <summary>
        /// Карта
        /// </summary>
        internal Map()
            : base()
        {
        }        
    }
}
