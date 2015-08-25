using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZombieWars.Core.Maps
{
    /// <summary>
    /// Состояние объекта карты
    /// </summary>
    public class MapObjectState
    {
        /// <summary>
        /// Объект карты
        /// </summary>
        protected MapObject MapObject { get; set; }

        /// <summary>
        /// Состояние объекта карты
        /// </summary>
        /// <param name="MapObject">Объект карты</param>
        public MapObjectState(MapObject MapObject)
        {
            if (MapObject == null) throw new ArgumentNullException("MapObject", "MapObject of MapObjectState cannot be null");
            this.MapObject = MapObject;
        }

        /// <summary>
        /// Сравнение с другим состоянием объекта карты
        /// </summary>
        /// <param name="MapObjectState">Состояние объекта карты</param>
        /// <returns>Являются ли состояния состояниями одного и того же объекта карты</returns>
        public bool Equals(MapObjectState MapObjectState)
        {
            if (MapObjectState == null) return false;
            return this.MapObject.Equals(MapObjectState.MapObject);
        }

        /// <summary>
        /// Сравнение с другим объектом
        /// </summary>
        /// <param name="obj">Объект</param>
        /// <returns>Идентичные ли объекты</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as MapObjectState);
        }

        /// <summary>
        /// Вычислить хеш-код
        /// </summary>
        /// <returns>Хеш-код</returns>
        public override int GetHashCode()
        {
            return this.MapObject.GetHashCode();
        }
    }
}
