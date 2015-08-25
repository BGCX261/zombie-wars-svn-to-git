using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace ZombieWars.Core.Maps
{
    /// <summary>
    /// Картинка
    /// </summary>
    [Serializable]
    [Guid("0BDC90DD-5C27-44B7-9911-01A97EFC707D")]
    public class MapImage
    {
        /// <summary>
        /// Тип
        /// </summary>
        public MapImageType Type { get; protected set; }          

        /// <summary>
        /// Данные
        /// </summary>
        public byte[] Data { get; protected set; }

        /// <summary>
        /// Картинка
        /// </summary>
        /// <param name="Type">Тип</param>        
        /// <param name="Data">Данные</param>
        public MapImage(MapImageType Type, byte[] Data)
        {            
            this.Type = Type;            
            this.Data = Data;
        }

        /// <summary>
        /// Картинка
        /// </summary>        
        public MapImage()
            : this(MapImageType.Null, new byte[0])
        {            
        }

        public static MapImage Empty { get { return new MapImage(); } }
    }

    /// <summary>
    /// Тип картинки
    /// </summary>
    public enum MapImageType : ushort
    {      
        Null = 0,
        Bmp = 1,
        Jpeg = 2, 
        Png = 3, 
        Gif = 4,
        Custom = 5
    }
}
