using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZombieWars.Core.Maps
{
    /// <summary>
    /// Состояние района карты
    /// </summary>
    public class MapAreaState : MapObjectState
    {
        /// <summary>
        /// Район
        /// </summary>
        public MapArea Area{ get; protected set; }

		/// <summary>
		/// Точки перехода
		/// </summary>
		public List<MapAreaTransitionPointState> TransitionPoints { get; internal set; }

        /// <summary>
        /// Состояние района карты
        /// </summary>
        /// <param name="Area">Район</param>
        public MapAreaState(MapArea Area)
            : base(Area)
        {
			if (Area == null) throw new ArgumentNullException("Area", "Area of MapAreaState cannot be null");
            this.Area = Area;
			this.TransitionPoints = new List<MapAreaTransitionPointState>();
			foreach (MapAreaTransitionPoint point in Area.TransitionPoints)
				this.TransitionPoints.Add(new MapAreaTransitionPointState(point));
        }

        /// <summary>
        /// Определить принадлежит ли району клетка карты
        /// </summary>
        /// <param name="Cell">Клетка карты</param>
        /// <returns>Принадлежит ли району</returns>
        public bool CheckCellInArea(MapCellState Cell)
        {
            return CheckCellInArea(Cell.Point);
        }

        /// <summary>
        /// Определить принадлежит ли району точка карты
        /// </summary>
        /// <param name="Point">Точка карты</param>
        /// <returns>Принадлежит ли району</returns>
        public bool CheckCellInArea(MapPoint Point)
        {
            return Point.InArea(Area.Position, Area.Size);
        }
    }
}
