using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZombieWars.Core.Maps
{
    /// <summary>
    /// Состояние уровня карты
    /// </summary>
    public class MapLevelState
    {
        /// <summary>
        /// Уровень
        /// </summary>
        public MapLevel Level { get; protected set; }

        /// <summary>
        /// Координата уровня
        /// </summary>
        public UInt16 Index { get; protected set; }

        /// <summary>
        /// Размер (в клетках)
        /// </summary>
        public MapSize Size { get { return Level.Size; } }

        /// <summary>
        /// Клетки уровня
        /// </summary>
        public MapMatrix<MapCellState> Cells { get; protected set; }

        /// <summary>
        /// Клетка
        /// </summary>
        /// <param name="X">X</param>
        /// <param name="Y">Y</param>
        /// <returns>Клетка</returns>
        public MapCellState this[UInt16 X, UInt16 Y]
        {
            get { return this.Cells[X, Y]; }
            set { this.Cells[X, Y] = value; }
        }

        /// <summary>
        /// Клетка
        /// </summary>
        /// <param name="X">X</param>
        /// <param name="Y">Y</param>
        /// <returns>Клетка</returns>
        public MapCellState this[Int32 X, Int32 Y]
        {
            get { return this.Cells[X, Y]; }
            set { this.Cells[X, Y] = value; }
        }

        /// <summary>
        /// Клетка
        /// </summary>
        /// <param name="Point">Точка</param>
        /// <returns>Клетка</returns>
        public MapCellState this[MapPoint Point]
        {
            get { return this.Cells[Point]; }
            set { this.Cells[Point] = value; }
        }

        /// <summary>
        /// Стена разрушена, но ещё не заменена на разрушенную
        /// </summary>
        public event MapCellEventHandler<MapWallEventArgs> WallDestroying;

        /// <summary>
        /// Стена разрушена и заменена на разрушенную
        /// </summary>
        public event MapCellEventHandler<MapWallEventArgs> WallDestroyed;

        /// <summary>
        /// Состояние уровня карты
        /// </summary>
        /// <param name="Level">Уровень</param>
        /// <param name="Index">Координата уровня</param>
        public MapLevelState(MapLevel Level, UInt16 Index)
        {
            if (Level == null) throw new ArgumentNullException("Level", "Level of MapLevelState cannot be null");

            this.Index = Index;
            this.Level = Level;

            Cells = new MapMatrix<MapCellState>(Level.Size);

            for (UInt16 x = 0; x < Size.Width; x++)
                for (UInt16 y = 0; y < Size.Height; y++)
                {
                    if (Level.Cells[x, y] == null)
                        Cells[x, y] = null;
                    else
                    {
                        Cells[x, y] = new MapCellState(Level.Cells[x, y], new MapPoint(Index, x, y));
                        Cells[x, y].WallDestroying += (sender, cell, args) => { OnWallDestroying(cell, args.Wall); };
                        Cells[x, y].WallDestroyed += (sender, cell, args) => { OnWallDestroyed(cell, args.Wall); };
                    }
                }
        }


        /// <summary>
        /// Вызов события разрушения стены
        /// </summary>
        /// <param name="Cell">Клетка, в которой содержится стена</param>
        /// <param name="Wall">Стена</param>
        protected void OnWallDestroying(MapCellState Cell, MapWallState Wall)
        {
            if (WallDestroying != null)
                WallDestroying(this, Cell, new MapWallEventArgs(Wall));
        }

        /// <summary>
        /// Вызов события разрушения стены
        /// </summary>
        /// <param name="Cell">Клетка, в которой содержится стена</param>
        /// <param name="Wall">Стена</param>
        protected void OnWallDestroyed(MapCellState Cell, MapWallState Wall)
        {
            if (WallDestroyed != null)
                WallDestroyed(this, Cell, new MapWallEventArgs(Wall));
        }
    }
}
