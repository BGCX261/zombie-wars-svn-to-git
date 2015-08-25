using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZombieWars.Core.Maps
{
    /// <summary>
    /// Районы карты
    /// </summary>
    [Serializable]
    public class MapAreas
    {
        /// <summary>
        /// Районы
        /// </summary>
        public List<MapArea> Areas{ get; protected set; }

        /// <summary>
        /// Количество районов
        /// </summary>
        public UInt32 AreasCount { get { return (UInt32)Areas.Count; } }

        /// <summary>
        /// Районы карты
        /// </summary>
        public MapAreas()
        {
            this.Areas = new List<MapArea>();
        }
    }
}
