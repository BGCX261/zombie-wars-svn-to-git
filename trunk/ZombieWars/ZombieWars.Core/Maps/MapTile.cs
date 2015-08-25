using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZombieWars.Core.Maps
{
    /// <summary>
    /// Тайл (отображение объекта карты)
    /// </summary>
    [Serializable]
    public class MapTile
    {        
        /// <summary>
        /// Картинка
        /// </summary>
        public MapImage Image { get; protected set; }

        /// <summary>
        /// Размер (в клетках)
        /// </summary>
        public MapSize Size { get; protected set; }

        /// <summary>
        /// Объект карты, представляемый в виде прямогуольной картинки
        /// </summary>        
        /// <param name="image">Картинка</param>
        /// <param name="size">Размер (в клетках карты)</param>
        public MapTile(MapImage image, MapSize size)            
        {
            Image = image;
            Size = size;
        }
    }
}
