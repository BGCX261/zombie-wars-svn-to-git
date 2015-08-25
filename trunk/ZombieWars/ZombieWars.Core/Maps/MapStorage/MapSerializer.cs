using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Runtime.InteropServices;

namespace ZombieWars.Core.Maps.MapStorage
{
    /// <summary>
    /// Сериализатор объектов карты
    /// </summary>
    [Guid("36F10910-E43F-431E-8C8E-F4C70816E4AD")]
    public class MapSerializer
    {
        /// <summary>
        /// Версия
        /// </summary>
        public Version Version { get { return _Version; } }
        private readonly Version _Version = new Version(1, 0, 0, 0);

        /// <summary>
        /// Идентификатор класса
        /// </summary>
        public Guid ClassId
        {
            get
            {
                return this.GetType().GUID;
            }
        }

        /// <summary>
        /// Основной экземпляр сериализатора
        /// </summary>
        public static MapSerializer Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new MapSerializer();
                return _Instance;
            }
        }
        private static MapSerializer _Instance;        

        /// <summary>
        /// Сериализатор объектов карт
        /// </summary>
        protected MapSerializer()
        {
        }        

        /// <summary>
        /// Сериализация объекта карты
        /// </summary>
        /// <param name="Object">Объект</param>
        /// <returns>Сериализованные данные</returns>
        private byte[] Serialize(object Object)
        {
            if (Object == null) throw new ArgumentNullException("Object", "Object for serialization cannot be null");

            byte[] data;

            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    MapSerializeHelper.SerializeSerializer(writer, this);
                    MapSerializeHelper.Serialize(writer, Object);
                    stream.Seek(0, SeekOrigin.Begin);
                    data = new byte[stream.Length];
                    stream.Read(data, 0, data.Length);
                }
            }

            return data;
        }

        public byte[] SerializeMapState(MapState mapState)
        {
            return Serialize(mapState);
        }

        public byte[] SerializeMap(Map map)
        {
            return Serialize(map);
        }

        public byte[] SerializeMapTileSet(MapTileSet tileSet)
        {
            return Serialize(tileSet);
        }

        /// <summary>
        /// Сериализация объекта карты в поток
        /// </summary>
        /// <param name="Object">Объект карты</param>
        /// <param name="Stream">Поток</param>
        private void Serialize(object Object, Stream Stream)
        {
            if (Object == null) throw new ArgumentNullException("Object", "Object for serialization cannot be null");
            if (Stream == null) throw new ArgumentNullException("Stream", "Stream for serialization cannot be null");
            
            using (BinaryWriter writer = new BinaryWriter(Stream))
            {
                MapSerializeHelper.SerializeSerializer(writer, this);
                MapSerializeHelper.Serialize(writer, Object);                
            }            
        }

        public void Serialize(MapState mapState, Stream stream)
        {
            Serialize(mapState, stream);
        }

        public void Serialize(Map map, Stream stream)
        {
            Serialize(map, stream);
        }

        public void Serialize(MapTileSet tileSet, Stream stream)
        {
            Serialize(tileSet, stream);
        }

        /// <summary>
        /// Десериализация объекта карты
        /// </summary>
        /// <param name="Data">Сериализованные данные</param>        
        /// <returns>Объект карты</returns>
        private object Deserialize(byte[] Data)
        {
            using (MemoryStream stream = new MemoryStream(Data))
            {
                return Deserialize(stream);
            }
        }

        public MapState DeserializeMapState(byte[] data)
        {
            return Deserialize(data) as MapState;
        }

        public Map DeserializeMap(byte[] data)
        {
            return Deserialize(data) as Map;
        }

        public MapTileSet DeserializeMapTileSet(byte[] data)
        {
            return Deserialize(data) as MapTileSet;
        }

        /// <summary>
        /// Десериализация объекта карты
        /// </summary>
        /// <param name="Stream">Поток с сериализованными данными</param>        
        /// <returns>Объект карты</returns>
        private object Deserialize(Stream Stream)
        {
            using (BinaryReader reader = new BinaryReader(Stream))
            {
                MapSerializer serializer = MapDeserializeHelper.DeserializeSerializer(reader);
                if (serializer != this) return null;
                return MapDeserializeHelper.Deserialize(reader);
            }
        }

        public MapState DeserializeMapState(Stream stream)
        {
            return Deserialize(stream) as MapState;
        }

        public Map DeserializeMap(Stream stream)
        {
            return Deserialize(stream) as Map;
        }

        public MapTileSet DeserializeMapTileSet(Stream stream)
        {
            return Deserialize(stream) as MapTileSet;
        }

    }        
}