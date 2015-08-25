using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZombieWars.Core.Maps
{
    /// <summary>
    /// Матрица
    /// </summary>
    /// <typeparam name="T">Тип матрицы</typeparam>
    [Serializable]
    public class MapMatrix<T>
    {
        /// <summary>
        /// Размер
        /// </summary>
        public MapSize Size { get; private set; }
        /// <summary>
        /// Данные
        /// </summary>
        protected T[][] Data;

        /// <summary>
        /// Элемент матрицы
        /// </summary>
        /// <param name="X">X</param>
        /// <param name="Y">Y</param>
        /// <returns>Элемент матрицы</returns>
        public T this[UInt16 X, UInt16 Y]
        {
            get
            {
                if (X >= Size.Width) throw new ArgumentOutOfRangeException("X", "X cannot be out of Matrix Size");
                if (Y >= Size.Height) throw new ArgumentOutOfRangeException("Y", "Y cannot be out of Matrix Size");
                return Data[X][Y];
            }
            set
            {
                if (X >= Size.Width) throw new ArgumentOutOfRangeException("X", "X cannot be out of Matrix Size");
                if (Y >= Size.Height) throw new ArgumentOutOfRangeException("Y", "Y cannot be out of Matrix Size");
                Data[X][Y] = value;
            }
        }

        /// <summary>
        /// Элемент матрицы
        /// </summary>
        /// <param name="X">X</param>
        /// <param name="Y">Y</param>
        /// <returns>Элемент матрицы</returns>
        public T this[Int32 X, Int32 Y]
        {
            get
            {                
                if (X < 0) throw new ArgumentOutOfRangeException("X", "X cannot be < 0");
                if (Y < 0) throw new ArgumentOutOfRangeException("Y", "Y cannot be < 0");
                if (X >= Size.Width) throw new ArgumentOutOfRangeException("X", "X cannot be out of Matrix Size");
                if (Y >= Size.Height) throw new ArgumentOutOfRangeException("Y", "Y cannot be out of Matrix Size");             
                return Data[X][Y];
            }
            set
            {
                if (X < 0) throw new ArgumentOutOfRangeException("X", "X cannot be < 0");
                if (Y < 0) throw new ArgumentOutOfRangeException("Y", "Y cannot be < 0");
                if (X >= Size.Width) throw new ArgumentOutOfRangeException("X", "X cannot be out of Matrix Size");
                if (Y >= Size.Height) throw new ArgumentOutOfRangeException("Y", "Y cannot be out of Matrix Size");
                Data[X][Y] = value;
            }
        }

        /// <summary>
        /// Элемент матрицы
        /// </summary>
        /// <param name="Point">Точка</param>
        /// <returns>Элемент матрицы</returns>
        public T this[MapPoint Point]
        {
            get
            {
                return this[Point.X, Point.Y];
            }
            set
            {
                this[Point.X, Point.Y] = value;
            }
        }

        /// <summary>
        /// Матрица
        /// </summary>
        /// <param name="Size">Размер матрицы</param>
        public MapMatrix(MapSize Size)
        {      
            this.Size = Size;
            Data = new T[Size.Width][];
            for (int x = 0; x < Size.Width; x++)
            {
                Data[x] = new T[Size.Height];                
            }
        }
    }
}
