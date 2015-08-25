using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace ZombieWars.Core.Maps
{
    /// <summary>
    /// Координаты на карте
    /// </summary>
    [Serializable]
    [Guid("2F28E24F-2A2C-49D1-81BB-4FB6C426202D")]
    public struct MapPoint
    {
        /// <summary>
        /// Уровень
        /// </summary>
        public UInt16 Level;
        /// <summary>
        /// X
        /// </summary>
        public UInt16 X;
        /// <summary>
        /// Y
        /// </summary>
        public UInt16 Y;

        /// <summary>
        /// Координаты на карте
        /// </summary>
        /// <param name="Level">Level</param>
        /// <param name="X">X</param>
        /// <param name="Y">Y</param>
        public MapPoint(UInt16 Level, UInt16 X, UInt16 Y)
        {
            this.Level = Level;
            this.X = X;
            this.Y = Y;
        }

        public MapPoint(MapPoint Point, Int16 DeltaX, Int16 DeltaY)
        {
            this.Level = Point.Level;
            this.X = (UInt16)(Point.X + DeltaX);
            this.Y = (UInt16)(Point.Y + DeltaY);
        }

        /// <summary>
        /// Точка находится на заданной площади
        /// </summary>
        /// <param name="LeftTopPoint">Левая верхняя точка площади</param>
        /// <param name="Size">Размер</param>
        /// <returns>Находиться ли точка на заданной площади</returns>
        public bool InArea (MapPoint LeftTopPoint, MapSize Size)
        {
            return this.X >= LeftTopPoint.X &&
                   this.X <= LeftTopPoint.X + Size.Width &&
                   this.Y >= LeftTopPoint.Y &&
                   this.Y <= LeftTopPoint.Y + Size.Height;
        }
    }
}
