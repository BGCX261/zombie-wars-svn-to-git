using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZombieWars.Core.Maps;

namespace ZombieWars.Core.Game
{
    /// <summary>
    /// Построитель пути внутри района
    /// </summary>
    public class LocalPathBuilder
    {
        /// <summary>
        /// Район
        /// </summary>
        public MapAreaState Area { get; private set; }
       
        /// <summary>
        /// Карта
        /// </summary>
        protected MapState Map { get; set; }
        /// <summary>
        /// Высота района
        /// </summary>
        protected UInt16 Height { get { return Area.Area.Size.Height; } }
        /// <summary>
        /// Ширина района
        /// </summary>
        protected UInt16 Width { get { return Area.Area.Size.Width; } }

        /// <summary>
        /// Откуда
        /// </summary>
        private MapCellState From { get; set; }
        /// <summary>
        /// Куда
        /// </summary>
        private MapCellState To { get; set; }
        /// <summary>
        /// Волны
        /// </summary>
        protected UInt16[,] Waves { get; set; }
        /// <summary>
        /// Закончен ли расчёт
        /// </summary>
        private bool endflag;

        /// <summary>
        /// Объект для блокировки
        /// </summary>
        private object locker = 0;

        /// <summary>
        /// Построитель пути внутри района
        /// </summary>
        /// <param name="Map">Карта</param>
        /// <param name="Area">Район</param>
        public LocalPathBuilder(MapState Map, MapAreaState Area)
        {
            this.Map = Map;
            this.Area = Area;
          // Waves = new MapCellState[Area.Area.Size.Height, Area.Area.Size.Width];
        }

        /// <summary>
        /// Построить путь из одной клетки в другую
        /// </summary>
        /// <param name="From">Откуда</param>
        /// <param name="To">Куда</param>
        /// <returns>Путь</returns>
        public MapPoint[] CalcPath(MapPoint From, MapPoint To)
        {
            if (!Area.CheckCellInArea(From)) throw new ArgumentOutOfRangeException("From", "From cannot be out of Area");
            if (!Area.CheckCellInArea(To)) throw new ArgumentOutOfRangeException("To", "To cannot be out of Area");

            lock (locker)
            {
                UInt16 level = From.Level;

                // пересчитываем волны
                Waves = new UInt16[Height, Width];
                for (int i = 0; i < Height; i++)
                    for (int j = 0; j < Width; j++)
                        Waves[i, j] = UInt16.MaxValue;
                endflag = false;
                Wave(From, 0);

                MapPoint current = To;
                List<MapPoint> path = new List<MapPoint>();

                while (!current.Equals(From))
                {
                    int X = current.X - Area.Area.Position.X;
                    int Y = current.Y - Area.Area.Position.Y;
                    if (X + 1 < Width && Waves[X, Y] > Waves[X + 1, Y]) current = new MapPoint(current, 1, 0); else
                    if (Y + 1 < Height && Waves[X, Y] > Waves[X, Y + 1]) current = new MapPoint(current, 0, 1); else
                    if (Y - 1 >= 0 && Waves[X, Y] > Waves[X, Y - 1]) current = new MapPoint(current, 0, -1); else
                    if (X - 1 >= 0 && Waves[X, Y] > Waves[X - 1, Y]) current = new MapPoint(current, -1, 0);
                    
                    path.Add(current);
                }

                path.Reverse();
                return path.ToArray();
            }
        }

        /// <summary>
        /// Обработка волны в конкретной клетке
        /// </summary>
        /// <param name="Point">Клетка</param>
        /// <param name="WaveValue">Значение волны</param>
        private void Wave(MapPoint Point, UInt16 WaveValue)
        {
            if (endflag) return;

            if (Waves[Point.X - Area.Area.Position.X, 
                      Point.Y - Area.Area.Position.Y] != UInt16.MaxValue) return;
            
            if (Point.Equals(To.Point))
            {
                endflag = true;
                return;
            }

            Waves[Point.X - Area.Area.Position.X, Point.Y - Area.Area.Position.Y] = WaveValue;

            MapCellState left = GetMapCell(Point, -1, 0);
            MapCellState right = GetMapCell(Point, +1, 0);
            MapCellState up = GetMapCell(Point, 0, -1);
            MapCellState down = GetMapCell(Point, 0, +1);

            if ((left != null) && Area.CheckCellInArea(left) && (left.Place.Passability > 0))
                Wave(left.Point, WaveValue++);

            if ((right != null) && Area.CheckCellInArea(right) && (right.Place.Passability > 0))
                Wave(right.Point, WaveValue++);

            if ((up != null) && Area.CheckCellInArea(up) && (up.Place.Passability > 0))
                Wave(up.Point, WaveValue++);

            if ((down != null) && Area.CheckCellInArea(down) && (down.Place.Passability > 0))
                Wave(down.Point, WaveValue++);
        }

        /// <summary>
        /// Взять клетку карты со смещением относительно определённой точки
        /// </summary>
        /// <param name="Point">Базовая точка</param>
        /// <param name="DeltaX">Смещение по оси X</param>
        /// <param name="DeltaY">Смещение по оси Y</param>
        /// <returns>Клетка карты</returns>
        protected MapCellState GetMapCell(MapPoint Point, int DeltaX, int DeltaY)
        {
            int newX = Point.X + DeltaX;
            int newY = Point.Y + DeltaY;

            if ((newX < 0) || (newY < 0)) return null;
            if ((newX >= Map.Levels[Point.Level].Size.Width) || (newY >= Map.Levels[Point.Level].Size.Height)) return null;

            return Map.Levels[Point.Level].Cells[newX, newY];
        }

    }
}
