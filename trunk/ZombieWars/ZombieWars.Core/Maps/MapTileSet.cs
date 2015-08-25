using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace ZombieWars.Core.Maps
{
    /// <summary>
    /// Коллекция тайлов
    /// </summary>
    [Guid("EF6F7CA6-B26C-494B-A9D1-051425BDA7D8")]
    public class MapTileSet : MapObject, IEnumerable<MapVisualObject>
    {                
        /// <summary>
        /// Тайлы
        /// </summary>
        protected Dictionary<Guid, MapVisualObject> Tiles { get; set; }

        /// <summary>
        /// Коллекция тайлов
        /// </summary>        
        public MapTileSet()
        {
            this.Tiles = new Dictionary<Guid, MapVisualObject>();
        }
        
        /// <summary>
        /// Количество тайлов
        /// </summary>
        public int Count
        {
            get
            {
                return this.Tiles.Count;
            }
        }

        /// <summary>
        /// Очистить
        /// </summary>
        public void Clear()
        {            
            this.Tiles.Clear();
        }

        /// <summary>
        /// Добавить
        /// </summary>
        /// <param name="Tile">Тайл</param>
        public void Add(MapVisualObject Tile)
        {
            if (!this.Tiles.ContainsKey(Tile.Id))
            {
                this.Tiles[Tile.Id] = Tile;
            }
        }

        /// <summary>
        /// Удалить
        /// </summary>
        /// <param name="Tile">Тайл</param>
        public void Remove(MapVisualObject Tile)
        {
            if (this.Tiles.ContainsKey(Tile.Id))
            {
                this.Tiles.Remove(Tile.Id);
            }
        }        

        /// <summary>
        /// Содержится ли идентификатор
        /// </summary>
        /// <param name="Id">Идентификатор</param>
        /// <returns>Содержится ли идентификатор</returns>
        public bool ContainsId(Guid Id)
        {
            return this.Tiles.ContainsKey(Id);
        }

        /// <summary>
        /// Содержится ли тайл
        /// </summary>
        /// <param name="Tile">Тайл</param>
        /// <returns>Содержится ли тайл</returns>
        public bool ContainsTile(MapVisualObject Tile)
        {
            return this.Tiles.ContainsValue(Tile);
        }

        /// <summary>
        /// Тайл по идентификатору
        /// </summary>
        /// <param name="Id">Идентификатор</param>
        /// <returns>Тайл</returns>
        public MapVisualObject this[Guid Id]
        {
            get
            {                
                return this.Tiles[Id];
            }
        }


        public IEnumerator<MapVisualObject> GetEnumerator()
        {            
            return ((IEnumerable<MapVisualObject>)(Tiles.Values)).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Tiles.Values.GetEnumerator();
        }
    }
}
