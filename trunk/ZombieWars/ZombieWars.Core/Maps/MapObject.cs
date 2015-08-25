using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZombieWars.Core.Maps
{
    /// <summary>
    /// Объект карты
    /// </summary>
    [Serializable]
    public class MapObject
    {
        /// <summary>
        /// Идентификатор класса
        /// </summary>
        public Guid ClassId { get { return this.GetType().GUID;  } }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; internal set; }

        /// <summary>
        /// Внутреннее имя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }        

        /// <summary>
        /// Объект карты
        /// </summary>
        /// <param name="Name">Название</param>
        public MapObject()
        {            
            Id = Guid.NewGuid();            
            this.Caption = Id.ToString();
        }       

        /// <summary>
        /// Сравнение с другим объектом карты
        /// </summary>
        /// <param name="MapObject">Объект карты</param>
        /// <returns>Идентичны ли объекты карты</returns>
        public bool Equals(MapObject MapObject)
        {
            if (MapObject == null) return false;
            return this.Id.Equals(MapObject.Id);
        }

        /// <summary>
        /// Сравнение с другим объектом
        /// </summary>
        /// <param name="obj">Объект</param>
        /// <returns>Идентичны ли объекты</returns>
        public override bool Equals(object obj)
        {
            return this.Equals(obj as MapObject);
        }

        /// <summary>
        /// Вычислить хеш-код
        /// </summary>
        /// <returns>Хеш-код</returns>
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public override string ToString()
        {
            return Caption ?? Name;
        }
    }
}
