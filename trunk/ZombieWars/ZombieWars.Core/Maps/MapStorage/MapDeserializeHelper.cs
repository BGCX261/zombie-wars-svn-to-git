using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ZombieWars.Core.Maps.MapStorage
{
    internal static class MapDeserializeHelper
    {
        public static MapSerializer DeserializeSerializer(BinaryReader Reader)
        {
            Guid classId = DeserializeGuid(Reader);
            Version version = DeserializeVersion(Reader);

            if ((classId == MapSerializer.Instance.ClassId) && (version == MapSerializer.Instance.Version))
                return MapSerializer.Instance;
            else
                return null;
        }

        public static Version DeserializeVersion(BinaryReader Reader)
        {            
            return new Version(Reader.ReadInt32(), Reader.ReadInt32(), Reader.ReadInt32(), Reader.ReadInt32());            
        }

        public static string DeserializeString(BinaryReader Reader)
        {
            int length = DeserializeLength(Reader);
            return (length > 0) ? Encoding.UTF8.GetString(Reader.ReadBytes(length)) : String.Empty;            
        }

        public static Guid DeserializeGuid(BinaryReader Reader)
        {
            return new Guid(Reader.ReadBytes(16));            
        }

        public static Guid DeserializeMapObjectReference(BinaryReader Reader)
        {
            return DeserializeGuid(Reader);
        }       

        public static int DeserializeLength(BinaryReader Reader)
        {
            byte bytesCount = Reader.ReadByte();
            if (bytesCount == 1)
                return Reader.ReadByte();
            else
                if (bytesCount == 2)
                    return Reader.ReadUInt16();
                else
                    return (int)Reader.ReadUInt32();
        }

        public static MapArmorType DeserializeMapArmorType(BinaryReader Reader)
        {
            return (MapArmorType)Reader.ReadUInt16();
        }

        public static MapWallMode DeserializeMapWallMode(BinaryReader Reader)
        {
            return (MapWallMode)Reader.ReadByte();
        }

        public static MapDirection DeserializeMapDirection(BinaryReader Reader)
        {
            return (MapDirection)Reader.ReadByte();
        }

        public static MapSize DeserializeMapSize(BinaryReader Reader)
        {
            return new MapSize(Reader.ReadUInt16(), Reader.ReadUInt16());
        }

        public static MapPoint DeserializeMapPoint(BinaryReader Reader)
        {
            return new MapPoint(Reader.ReadUInt16(), Reader.ReadUInt16(), Reader.ReadUInt16());
        }

        public static MapImageType DeserializeMapImageType(BinaryReader Reader)
        {
            return (MapImageType)Reader.ReadUInt16();
        }

        public static MapImage DeserializeMapImage(BinaryReader Reader)
        {
            MapImageType imageType = DeserializeMapImageType(Reader);

            if (imageType == MapImageType.Null)
            {
                return null;
            }
            else
            {
                //MapSize size = DeserializeMapSize(Reader);
                int len = DeserializeLength(Reader);
                byte[] data = (len > 0) ? Reader.ReadBytes(len) : null;
                return new MapImage(imageType, data);
            };            
        }

        public static MapTile DeserializeMapTile(BinaryReader Reader)
        {
            return new MapTile(DeserializeMapImage(Reader), DeserializeMapSize(Reader));
        }

        private static void DeserializeMapVisualObject(BinaryReader Reader, MapVisualObject Object)
        {
            Object.Tile = DeserializeMapTile(Reader);          
        }

        private static MapPlace DeserializeMapPlace(BinaryReader Reader)
        {
            MapPlace place = new MapPlace();
            DeserializeMapVisualObject(Reader, place);
            place.Passability = Reader.ReadSingle();
            return place;
        }

		private static MapPlaceState DeserializeMapPlaceState(BinaryReader Reader, MapPlace place)
		{
			return new MapPlaceState(place);
		}

        private static MapWall DeserializeMapWall(BinaryReader Reader, ref Guid DestroyedReference)
        {
            MapWall wall = new MapWall();
            DeserializeMapVisualObject(Reader, wall);
            wall.BaseDirection = DeserializeMapDirection(Reader);
            wall.CornerDirection = DeserializeMapDirection(Reader);
            wall.ImageWindow = DeserializeMapImage(Reader);
            wall.ImageDoor = DeserializeMapImage(Reader);
            wall.ImageCorner = DeserializeMapImage(Reader);
            

            //Guid destroyed = DeserializeMapObjectReference(Reader);
            //if (destroyed != Guid.Empty) DestroyedReference = destroyed;
            
            wall.Mode = DeserializeMapWallMode(Reader);
            wall.ArmorType = DeserializeMapArmorType(Reader);
            wall.Health = Reader.ReadUInt16();

            return wall;
        }

		private static MapWallState DeserializeMapWallState(BinaryReader Reader, MapWall wall, MapDirection direction)
		{
			MapWallState state = new MapWallState(wall, direction);
			state.Mode = DeserializeMapWallMode(Reader);
			state.Health = Reader.ReadUInt16();
			return state;
		}

        private static MapActiveObject DeserializeMapActiveObject(BinaryReader Reader, ref Guid DestroyedReference)
        {
            MapActiveObject activeObject = new MapActiveObject();
            DeserializeMapVisualObject(Reader, activeObject);
            
            Guid destroyed = DeserializeMapObjectReference(Reader);
            if (destroyed != Guid.Empty) DestroyedReference = destroyed;
            
            activeObject.ArmorType = DeserializeMapArmorType(Reader);
            activeObject.Health = Reader.ReadUInt16();
            activeObject.Passability = Reader.ReadSingle();
            activeObject.BaseDirection = DeserializeMapDirection(Reader);

            return activeObject;
        }		

		public static MapActiveObjectState DeserializeMapActiveObjectState(BinaryReader Reader, MapTileSet mapTileSet)
		{
			Guid activeObjectId = DeserializeMapObjectReference(Reader);
			if (activeObjectId == Guid.Empty) return null;
			MapActiveObject activeObject = null;
			if ((mapTileSet != null) && mapTileSet.ContainsId(activeObjectId)) activeObject = mapTileSet[activeObjectId] as MapActiveObject;			
			if (activeObject == null) return null;
			
			MapPoint position = DeserializeMapPoint(Reader);
			MapActiveObjectState state = new MapActiveObjectState(activeObject, position);
			state.Direction = DeserializeMapDirection(Reader);
			state.ArmorType = DeserializeMapArmorType(Reader);
			state.Health = Reader.ReadUInt16();
			
			return state;
		}

        private static MapTileSet DeserializeMapTileSet(BinaryReader Reader)
        {
            MapTileSet tileSet = new MapTileSet();
            int Count = DeserializeLength(Reader);
            Dictionary<Guid, Guid> destroyedReferences = new Dictionary<Guid, Guid>();

            for (int i = 0; i < Count; i++)
            {
                MapVisualObject tile = DeserializeMapObject(Reader, destroyedReferences) as MapVisualObject;
                if (tile != null)
                    tileSet.Add(tile);
            }

            foreach (KeyValuePair<Guid, Guid> reference in destroyedReferences)
            {
                MapVisualObject tileSource = GetTile(reference.Key, tileSet);
                MapVisualObject tileTarget = GetTile(reference.Value, tileSet);

				//if ((tileSource is MapWall) && (tileTarget is MapWall))
				//    (tileSource as MapWall).DestroyedWall = tileTarget as MapWall;

                if ((tileSource is MapActiveObject) && (tileTarget is MapActiveObject))
                    (tileSource as MapActiveObject).DestroyedActiveObject = tileTarget as MapActiveObject;
            }

            return tileSet;
        }

        private static MapArea DeserializeMapArea(BinaryReader Reader)
        {
            MapArea area = new MapArea();
            area.Position = DeserializeMapPoint(Reader);
            area.Size = DeserializeMapSize(Reader);            
            return area;
        }

		private static MapAreaState DeserializeMapAreaState(BinaryReader Reader, MapArea area)
		{
			MapAreaState state = new MapAreaState(area);

			for (int i = 0; i < area.TransitionPoints.Count; i++ )
			{
				state.TransitionPoints[i] = DeserializeMapTransitionPointState(Reader, area.TransitionPoints[i]);
			}
			return state;
		}

        private static MapAreaTransitionPoint DeserializeMapTransitionPoint(BinaryReader Reader, Dictionary<Guid, MapArea> Areas)
        {
            MapAreaTransitionPoint transitionPoint = new MapAreaTransitionPoint();
            Guid fromId = DeserializeGuid(Reader);
            transitionPoint.From = Areas[fromId];
            Guid toId = DeserializeGuid(Reader);
            transitionPoint.To = Areas[toId];
            transitionPoint.Position = DeserializeMapPoint(Reader);
            transitionPoint.Size = DeserializeMapSize(Reader);
            return transitionPoint;
        }

		private static MapAreaTransitionPointState DeserializeMapTransitionPointState(BinaryReader Reader, MapAreaTransitionPoint point)
		{
			MapAreaTransitionPointState state = new MapAreaTransitionPointState(point);
			state.IsEnabled = Reader.ReadBoolean();
			return state;
		}

		private static MapAreas DeserializeMapAreas(BinaryReader Reader)
		{
			int areasLength = DeserializeLength(Reader);
			MapAreas areas = new MapAreas();
			Dictionary<Guid, MapArea> areasDict = new Dictionary<Guid, MapArea>();
			for (int i = 0; i < areasLength; i++)
			{
				MapArea area = DeserializeMapObject(Reader) as MapArea;
				areas.Areas.Add(area);
				areasDict[area.Id] = area;
			}

			int transitionPointsLength = DeserializeLength(Reader);
			for (int i = 0; i < transitionPointsLength; i++)
			{
				MapAreaTransitionPoint transitionPoint =
					DeserializeMapObject(Reader, null, areasDict) as MapAreaTransitionPoint;
				transitionPoint.From.TransitionPoints.Add(transitionPoint);
			}

			return areas;
		}

		private static MapAreasState DeserializeMapAreasState(BinaryReader Reader, MapAreas areas)
		{
			MapAreasState state = new MapAreasState(areas);
			for (int i = 0; i < areas.Areas.Count; i++)
			{
				state.Areas[i] = DeserializeMapAreaState(Reader, areas.Areas[i]);
			}
			return state;
		}

		private static MapCellSerializationType DeserializeMapCellType(BinaryReader Reader)
        {
            return (MapCellSerializationType)Reader.ReadByte();
        }

        private static Guid DeserializeMapCellIdIndex(BinaryReader Reader, int IdIndexBytesCount, List<Guid> IdTable)
        {
            int idIndex = 0;
            if (IdIndexBytesCount == 1)
                idIndex = Reader.ReadByte();
            else
                if (IdIndexBytesCount == 2)
                    idIndex = Reader.ReadUInt16();
                else
                    idIndex = (int)Reader.ReadUInt32();
            return IdTable[idIndex];
        }

        private static MapVisualObject DeserializeMapCellObjectReference(BinaryReader Reader, int IdIndexBytesCount, List<Guid> IdTable, MapTileSet TileSet)
        {
            return GetTile(DeserializeMapCellIdIndex(Reader, IdIndexBytesCount, IdTable), TileSet);
        }

        private static MapCell DeserializeMapCell(BinaryReader Reader, List<Guid> IdTable, MapTileSet TileSet)
        {
            MapCell cell = new MapCell();
            MapCellSerializationType cellType = DeserializeMapCellType(Reader);

            if (cellType == MapCellSerializationType.Null) return null;

            int idIndexBytesCount = 4;
            if ((cellType & MapCellSerializationType.IdIndex8) != 0) idIndexBytesCount = 1; else
            if ((cellType & MapCellSerializationType.IdIndex16) != 0) idIndexBytesCount = 3; else
            if ((cellType & MapCellSerializationType.IdIndex32) != 0) idIndexBytesCount = 4;

            if ((cellType & MapCellSerializationType.Place) != 0)
                cell.Place = DeserializeMapCellObjectReference(Reader, idIndexBytesCount, IdTable, TileSet) as MapPlace;

            if ((cellType & MapCellSerializationType.NorthWall) != 0)
                cell.SetWall(MapDirection.North, DeserializeMapCellObjectReference(Reader, idIndexBytesCount, IdTable, TileSet) as MapWall);

            if ((cellType & MapCellSerializationType.SouthWall) != 0)
                cell.SetWall(MapDirection.South, DeserializeMapCellObjectReference(Reader, idIndexBytesCount, IdTable, TileSet) as MapWall);

            if ((cellType & MapCellSerializationType.WestWall) != 0)
                cell.SetWall(MapDirection.West, DeserializeMapCellObjectReference(Reader, idIndexBytesCount, IdTable, TileSet) as MapWall);

            if ((cellType & MapCellSerializationType.EastWall) != 0)
                cell.SetWall(MapDirection.East, DeserializeMapCellObjectReference(Reader, idIndexBytesCount, IdTable, TileSet) as MapWall);

            return cell;
        }

		private static MapCellState DeserializeMapCellState(BinaryReader Reader, MapCell cell, MapPoint point)
		{
            if (cell == null) return null;
			MapCellState state = new MapCellState(cell, point);            
            
            state.Place = DeserializeMapPlaceState(Reader, cell.Place);
			
			MapWall northWall = cell.GetWall(MapDirection.North);
			MapWall southWall = cell.GetWall(MapDirection.South);
			MapWall westWall = cell.GetWall(MapDirection.West);
			MapWall eastWall = cell.GetWall(MapDirection.East);

			if (northWall != null) 
                state.SetWall(MapDirection.North, DeserializeMapWallState(Reader, northWall, MapDirection.North));
            if (southWall != null) 
                state.SetWall(MapDirection.South, DeserializeMapWallState(Reader, southWall, MapDirection.South));
            if (westWall != null) 
                state.SetWall(MapDirection.North, DeserializeMapWallState(Reader, westWall, MapDirection.West));
            if (eastWall != null) 
                state.SetWall(MapDirection.North, DeserializeMapWallState(Reader, eastWall, MapDirection.East));

			return state;
		}

        private static Map DeserializeMap(BinaryReader Reader)
        {
            Map map = new Map();            

            map.Version = DeserializeVersion(Reader);
            map.Size = DeserializeMapSize(Reader);
            map.LevelsCount = Reader.ReadUInt16();
            map.TileSet = DeserializeMapObject(Reader) as MapTileSet;            

            List<Guid> idTable = new List<Guid>();
            int idTableLength = DeserializeLength(Reader);
            for (int i = 0; i < idTableLength; i++)
            {
                idTable.Add(DeserializeGuid(Reader));
            }

            for (UInt16 z = 0; z < map.Levels.Length; z++)
                for (UInt16 x = 0; x < map.Size.Width; x++)
                    for (UInt16 y = 0; y < map.Size.Height; y++)
                    {
                        map.Levels[z].Cells[x, y] = DeserializeMapCell(Reader, idTable, map.TileSet);
                    }

			map.Areas = DeserializeMapAreas(Reader);

            return map;
        }

		private static MapState DeserializeMapState(BinaryReader Reader, Map map)
		{			
			MapState state = new MapState(map);

			for (UInt16 z = 0; z < map.Levels.Length; z++)
				for (UInt16 x = 0; x < map.Size.Width; x++)
					for (UInt16 y = 0; y < map.Size.Height; y++)
					{
						state.Levels[z].Cells[x, y] = DeserializeMapCellState(Reader, map.Levels[z].Cells[x, y], new MapPoint(z, x, y));
					}

			state.Areas = DeserializeMapAreasState(Reader, map.Areas);

			int activeObjectsCount = DeserializeLength(Reader);
			for (int i = 0; i < activeObjectsCount; i++)
			{
				Guid id = DeserializeGuid(Reader);
				MapActiveObjectState activeObject = DeserializeMapActiveObjectState(Reader, map.TileSet);
				state.AddActiveObject(activeObject, id);
			}

			return state;
		}

		private static MapState DeserializeMapState(BinaryReader Reader)
		{
			Map map = DeserializeMapObject(Reader) as Map;
			return DeserializeMapState(Reader, map);
		}

        private static object DeserializeMapObject(BinaryReader Reader, Dictionary<Guid, Guid> DestroyedReferences = null, Dictionary<Guid, MapArea> Areas = null)
        {
            Guid classId = DeserializeGuid(Reader);

			if (classId == typeof(MapState).GUID)
			{
				return DeserializeMapState(Reader);
			}

            Guid id = DeserializeGuid(Reader);
            string name = DeserializeString(Reader);
            string caption = DeserializeString(Reader);
            string description = DeserializeString(Reader);

            MapObject mapObject = null;

            if (classId == typeof(MapPlace).GUID)
            {
                mapObject = DeserializeMapPlace(Reader);
            } else            

            if (classId == typeof(MapWall).GUID)
            {
                Guid destroyedReference = Guid.Empty;
                mapObject = DeserializeMapWall(Reader, ref destroyedReference);
                if ((DestroyedReferences != null) && (destroyedReference != Guid.Empty))
                    DestroyedReferences[id] = destroyedReference;
            } else

            if (classId == typeof(MapActiveObject).GUID)
            {
                Guid destroyedReference = Guid.Empty;
                mapObject = DeserializeMapActiveObject(Reader, ref destroyedReference);
                if ((DestroyedReferences != null) && (destroyedReference != Guid.Empty)) 
                    DestroyedReferences[id] = destroyedReference;
            } else

            if (classId == typeof(MapTileSet).GUID)
            {
                mapObject = DeserializeMapTileSet(Reader);
            } else

            if (classId == typeof(MapArea).GUID)
            {
                mapObject = DeserializeMapArea(Reader);
            } else

            if (classId == typeof(MapAreaTransitionPoint).GUID)
            {
                mapObject = DeserializeMapTransitionPoint(Reader, Areas);
            } else

            if (classId == typeof(Map).GUID)
            {
                mapObject = DeserializeMap(Reader);
            }

            mapObject.Id = id;
            mapObject.Name = name;
            mapObject.Caption = caption;
            mapObject.Description = description;

            return mapObject;
        }

        private static MapVisualObject GetTile(Guid Id, MapTileSet TileSet)
        {
            if ((TileSet != null) && TileSet.ContainsId(Id))
                return TileSet[Id];            

            return null;
        }

        public static object Deserialize(BinaryReader Reader)
        {
            return DeserializeMapObject(Reader);
        }
    }
}
