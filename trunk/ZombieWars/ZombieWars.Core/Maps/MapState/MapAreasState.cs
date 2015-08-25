using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ZombieWars.Core.Maps
{
    /// <summary>
    /// Состояние районов
    /// </summary>
    public class MapAreasState
    {
        /// <summary>
        /// Состояние районов
        /// </summary>
        public MapAreaState[] Areas { get; protected set; }


        /// <summary>
        /// Матрица достижимости районов (пересечение - следующий шаг, либо null если не достижимо)
        /// </summary>
        protected MapArea[,] MapAreasReach { get; set; }

        /// <summary>
        /// Следующий шаг при переходе на пути из района в район
        /// </summary>
        /// <param name="From">Район отправления</param>
        /// <param name="To">Район назначения</param>
        /// <returns>Следущий район в пути из района отправления в район назначения, если null - пути нет</returns>
        public MapArea GetNextStep(MapArea From, MapArea To)
        {            
            if (From == null) throw new ArgumentNullException("From", "From cannot be null");
            if (To == null) throw new ArgumentNullException("To", "To cannot be null");
            
            List<MapAreaState> areas = Areas.ToList();
            int FromIndex = areas.FindIndex(area => area.Area.Equals(From));
            int ToIndex = areas.FindIndex(area => area.Area.Equals(From));
            
            if (FromIndex < 0) throw new ArgumentOutOfRangeException("From", "From cannot be out of MapAreas");
            if (ToIndex < 0) throw new ArgumentOutOfRangeException("To", "To cannot be out of MapAreas");

            if (MapAreasReach == null) CalcMapAreasReach();

            return MapAreasReach[FromIndex, ToIndex];
        }

        /// <summary>
        /// Состояние районов
        /// </summary>
        /// <param name="MapAreas">Районы</param>
        public MapAreasState(MapAreas Areas)
        {
            if (Areas == null) throw new ArgumentNullException("Areas", "Areas of MapAreasState cannot be null");

            this.Areas = new MapAreaState[Areas.AreasCount];
            for (int i = 0; i < Areas.AreasCount; i++)
            {
                this.Areas[i] = new MapAreaState(Areas.Areas[i]);
            }
        }

        /// <summary>
        /// Определить район, которому принадлежит клетка карты
        /// </summary>
        /// <param name="Cell">Клетка карты</param>
        /// <returns>Район</returns>
        public MapAreaState GetAreaByCell(MapPoint Cell)
        {
            foreach (MapAreaState MapAreaState in Areas)
            {
                if (Cell.InArea(MapAreaState.Area.Position, MapAreaState.Area.Size)) return MapAreaState;
            }
            return null;
        }

        /// <summary>
        /// Вычисление матрицы достижимости районов
        /// </summary>
        protected void CalcMapAreasReach()
        {

            MapAreasReach = new MapArea[Areas.Length, Areas.Length];
            for (int i = 0; i < Areas.Length; i++)
            {
                for (int j = 0; j < Areas.Length; j++)
                {
                    MapAreasReach[i, j] = null;
                }
            }

            for (int i = 0; i < Areas.Length; i++)
            {
                foreach (MapAreaTransitionPoint tpoint in Areas[i].Area.TransitionPoints)
                {
                    for (int j = 0; j < Areas.Length; j++)
                    {
                        if (Areas[j].Area.Id == tpoint.To.Id)
                        {
                            MapAreasReach[i, j] = Areas[j].Area;
                        }
                    }
                }
            }
            bool endflag = true;
            while (endflag)
            {
                endflag = false;
                for (int i = 0; i < Areas.Length; i++)
                {
                    for (int j = 0; j < Areas.Length; j++)
                    {
                        if (MapAreasReach[i, j] != null)
                        {
                            for (int k = 0; k < Areas.Length; k++)
                            {
                                foreach (MapAreaTransitionPoint tpoint in Areas[k].Area.TransitionPoints)
                                {
                                    if (i == k) continue;
                                    if (Areas[j].Area.Id == tpoint.To.Id)
                                    {
                                        if (MapAreasReach[i, k] == null) endflag = true;
                                        if (MapAreasReach[i, k] == null)
                                            MapAreasReach[i, k] = MapAreasReach[i, j];
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

    }
}
