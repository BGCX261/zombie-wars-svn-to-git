using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;

namespace ZombieWars.Core.Maps.MapStorage
{
    internal static class MapSerializeHelper
    {
        public static void SerializeSerializer(BinaryWriter Writer, MapSerializer Serializer)
        {
            SerializeGuid(Writer, Serializer.ClassId);
            SerializeVersion(Writer, Serializer.Version);            
        }

        public static void SerializeVersion(BinaryWriter Writer, Version Version)
        {            
            Writer.Write(Version.Major);
            Writer.Write(Version.Minor);
            Writer.Write(Version.Build);
            Writer.Write(Version.Revision);         
        }

        public static void SerializeString(BinaryWriter Writer, string String)
        {
            int bytesCount = 0; 
            byte[] text = null;

            if (!String.IsNullOrEmpty(String))
            {
                bytesCount = Encoding.UTF8.GetByteCount(String);
                text = Encoding.UTF8.GetBytes(String);
            }

            SerializeLength(Writer, bytesCount);
            if ((text != null) && (text.Length > 0))
                Writer.Write(text);
        }

        public static void SerializeGuid(BinaryWriter Writer, Guid Guid)
        {
            Writer.Write(Guid.ToByteArray());
        }

        public static void SerializeMapObjectReference(BinaryWriter Writer, MapObject Object)
        {
            SerializeGuid(Writer, (Object != null) ? Object.Id : Guid.Empty);
        }

        public static void SerializeLength(BinaryWriter Writer, int Length)
        {
            if (Length < 0) Length = 0;
            byte bytesCount = 1;
            if (Length >= byte.MaxValue) bytesCount = 2;
            if (Length >= UInt16.MaxValue) bytesCount = 4;
            Writer.Write(bytesCount);
            if (bytesCount == 1)
                Writer.Write((byte)Length);
            else
                if (bytesCount == 2)
                    Writer.Write((UInt16)Length);
                else
                    Writer.Write((UInt32)Length);            
        }

        public static void SerializeLength(BinaryWriter Writer, Array Array)
        {
            SerializeLength(Writer, (Array == null) ? 0 : Array.Length);
        }

        public static void SerializeLength(BinaryWriter Writer, IList Array)
        {
            SerializeLength(Writer, (Array == null) ? 0 : Array.Count);
        }       

        public static void SerializeMapArmorType(BinaryWriter Writer, MapArmorType ArmorType)
        {
            Writer.Write((UInt16)ArmorType);
        }

        public static void SerializeMapWallMode(BinaryWriter Writer, MapWallMode WallMode)
        {
            Writer.Write((byte)WallMode);
        }

        public static void SerializeMapDirection(BinaryWriter Writer, MapDirection Direction)
        {
            Writer.Write((byte)Direction);
        }

        public static void SerializeMapObject(BinaryWriter Writer, MapObject Object)
        {
            SerializeGuid(Writer, Object.ClassId);
            SerializeGuid(Writer, Object.Id);
            SerializeString(Writer, Object.Name);
            SerializeString(Writer, Object.Caption);
            SerializeString(Writer, Object.Description);
        }

        public static void SerializeMapObjectState(BinaryWriter Writer, MapObjectState ObjectState)
        {     
        }

        public static void SerializeMapSize(BinaryWriter Writer, MapSize Size)
        {
            Writer.Write(Size.Width);
            Writer.Write(Size.Height);
        }

        public static void SerializeMapPoint(BinaryWriter Writer, MapPoint Point)
        {
            Writer.Write(Point.Level);
            Writer.Write(Point.X);
            Writer.Write(Point.Y);
        }       

        private static void SerializeMapCellType(BinaryWriter Writer, MapCellSerializationType CellType)
        {
            Writer.Write((byte)CellType);
        }

        private static void SerializeMapCellIdIndex(BinaryWriter Writer, int IdIndexBytesCount, int IdIndex)
        {
            if (IdIndexBytesCount == 1)
                Writer.Write((byte)IdIndex);
            else
                if (IdIndexBytesCount == 2)
                    Writer.Write((UInt16)IdIndex);
                else
                    Writer.Write((UInt32)IdIndex);
        }

        private static void SerializeMapCell(BinaryWriter Writer, MapCell Cell, List<Guid> IdTable)
        {
            if (Cell == null) { SerializeMapCellType(Writer, MapCellSerializationType.Null); return; }

            int idIndexBytesCount = 1;
            if (IdTable.Count > byte.MaxValue - 5) idIndexBytesCount = 2;
            if (IdTable.Count > UInt16.MaxValue - 5) idIndexBytesCount = 4;
            
            MapCellSerializationType type = MapCellSerializationType.Place;
            MapWall northWall = Cell.GetWall(MapDirection.North);
            MapWall southWall = Cell.GetWall(MapDirection.South);
            MapWall westWall = Cell.GetWall(MapDirection.West);
            MapWall eastWall = Cell.GetWall(MapDirection.East);
            if (northWall != null) type = type | MapCellSerializationType.NorthWall;
            if (southWall != null) type = type | MapCellSerializationType.SouthWall;
            if (westWall != null) type = type | MapCellSerializationType.WestWall;
            if (eastWall != null) type = type | MapCellSerializationType.EastWall;
            if (idIndexBytesCount == 1) type = type | MapCellSerializationType.IdIndex8; else
            if (idIndexBytesCount == 2) type = type | MapCellSerializationType.IdIndex16; else
            if (idIndexBytesCount == 4) type = type | MapCellSerializationType.IdIndex32;

            SerializeMapCellType(Writer, type);

            int placeIndex = IdTable.IndexOf(Cell.Place.Id);
            if (placeIndex < 0) { IdTable.Add(Cell.Place.Id); placeIndex = IdTable.Count - 1; }

            SerializeMapCellIdIndex(Writer, idIndexBytesCount, placeIndex);         

            if (northWall != null)
            {
                int wallIndex = IdTable.IndexOf(northWall.Id);
                if (wallIndex < 0) { IdTable.Add(northWall.Id); wallIndex = IdTable.Count - 1; }
                SerializeMapCellIdIndex(Writer, idIndexBytesCount, wallIndex);
            }

            if (southWall != null)
            {
                int wallIndex = IdTable.IndexOf(southWall.Id);
                if (wallIndex < 0) { IdTable.Add(southWall.Id); wallIndex = IdTable.Count - 1; }
                SerializeMapCellIdIndex(Writer, idIndexBytesCount, wallIndex);
            }

            if (westWall != null)
            {
                int wallIndex = IdTable.IndexOf(westWall.Id);
                if (wallIndex < 0) { IdTable.Add(westWall.Id); wallIndex = IdTable.Count - 1; }
                SerializeMapCellIdIndex(Writer, idIndexBytesCount, wallIndex);
            }

            if (eastWall != null)
            {
                int wallIndex = IdTable.IndexOf(eastWall.Id);
                if (wallIndex < 0) { IdTable.Add(eastWall.Id); wallIndex = IdTable.Count - 1; }
                SerializeMapCellIdIndex(Writer, idIndexBytesCount, wallIndex);
            }            
        }

		private static void SerializeMapCellState(BinaryWriter Writer, MapCellState CellState)
		{
            if (CellState == null) return;

            MapWallState northWall = CellState.GetWall(MapDirection.North);
            MapWallState southWall = CellState.GetWall(MapDirection.South);
            MapWallState westWall = CellState.GetWall(MapDirection.West);
            MapWallState eastWall = CellState.GetWall(MapDirection.East);

            SerializeMapPlaceState(Writer, CellState.Place);

			if (northWall != null) SerializeMapWallState(Writer, northWall);
			if (southWall != null) SerializeMapWallState(Writer, southWall);
			if (westWall != null) SerializeMapWallState(Writer, westWall);
			if (eastWall != null) SerializeMapWallState(Writer, eastWall);		
		}

        public static void SerializeMapImageType(BinaryWriter Writer, MapImageType ImageType)
        {
            Writer.Write((UInt16)ImageType);
        }

        public static void SerializeMapImage(BinaryWriter Writer, MapImage Image)
        {            
            if ((Image == null) || (Image.Data == null) || (Image.Data.Length == 0))
            {
                SerializeMapImageType(Writer, MapImageType.Null);
            }
            else
            {
                SerializeMapImageType(Writer, Image.Type);
                //SerializeMapSize(Writer, Image.Size);
                SerializeLength(Writer, Image.Data);
                if ((Image.Data != null) && (Image.Data.Length > 0))
                    Writer.Write(Image.Data);
            }            
        }

        public static void SerializeMapTile(BinaryWriter Writer, MapTile Tile)
        {            
            SerializeMapImage(Writer, Tile.Image);
            SerializeMapSize(Writer, Tile.Size);
        }

        public static void SerializeMapVisualObject(BinaryWriter Writer, MapVisualObject VisualObject)
        {
            SerializeMapObject(Writer, VisualObject);
            SerializeMapTile(Writer, VisualObject.Tile);
        }

        public static void SerializeMapVisualObjectState(BinaryWriter Writer, MapVisualObjectState VisualObjectState)
        {
            SerializeMapObjectState(Writer, VisualObjectState);
        }
       
        public static void SerializeMapPlace(BinaryWriter Writer, MapPlace Place)
        {
            SerializeMapVisualObject(Writer, Place);
            Writer.Write(Place.Passability);
        }

        public static void SerializeMapPlaceState(BinaryWriter Writer, MapPlaceState PlaceState)
        {            
            SerializeMapVisualObjectState(Writer, PlaceState);            
        }

        public static void SerializeMapWall(BinaryWriter Writer, MapWall Wall)
        {
            SerializeMapVisualObject(Writer, Wall);
            SerializeMapDirection(Writer, Wall.BaseDirection);
            SerializeMapDirection(Writer, Wall.CornerDirection);
            SerializeMapImage(Writer, Wall.ImageWindow);
            SerializeMapImage(Writer, Wall.ImageDoor);
            SerializeMapImage(Writer, Wall.ImageCorner);                            
            //SerializeMapObjectReference(Writer, Wall.DestroyedWall);
            SerializeMapWallMode(Writer, Wall.Mode);
            SerializeMapArmorType(Writer, Wall.ArmorType);
            Writer.Write(Wall.Health);            
        }

        public static void SerializeMapWallState(BinaryWriter Writer, MapWallState WallState)
        {			
            SerializeMapVisualObjectState(Writer, WallState);
            SerializeMapWallMode(Writer, WallState.Mode);            
            Writer.Write(WallState.Health);
        }

        public static void SerializeMapActiveObject(BinaryWriter Writer, MapActiveObject ActiveObject)
        {            
            SerializeMapVisualObject(Writer, ActiveObject);
            SerializeMapObjectReference(Writer, ActiveObject.DestroyedActiveObject);
            SerializeMapArmorType(Writer, ActiveObject.ArmorType);
            Writer.Write(ActiveObject.Health);
            Writer.Write(ActiveObject.Passability);
            SerializeMapDirection(Writer, ActiveObject.BaseDirection);            
        }

        public static void SerializeMapActiveObjectState(BinaryWriter Writer, MapActiveObjectState ActiveObjectState)
        {
            SerializeMapVisualObjectState(Writer, ActiveObjectState);
            SerializeMapObjectReference(Writer, ActiveObjectState.ActiveObject);
            SerializeMapPoint(Writer, ActiveObjectState.Position);
            SerializeMapDirection(Writer, ActiveObjectState.Direction);
            SerializeMapArmorType(Writer, ActiveObjectState.ArmorType);
            Writer.Write(ActiveObjectState.Health);
        }

        public static void SerializeMapAreaTransitionPoint(BinaryWriter Writer, MapAreaTransitionPoint AreaTransitionPoint)
        {
            SerializeMapObject(Writer, AreaTransitionPoint);
            SerializeMapObjectReference(Writer, AreaTransitionPoint.From);
            SerializeMapObjectReference(Writer, AreaTransitionPoint.To);
            SerializeMapPoint(Writer, AreaTransitionPoint.Position);
            SerializeMapSize(Writer, AreaTransitionPoint.Size);
        }

		public static void SerializeMapAreaTransitionPointState(BinaryWriter Writer, MapAreaTransitionPointState AreaTransitionPointState)
		{
			SerializeMapObjectState(Writer, AreaTransitionPointState);
			Writer.Write(AreaTransitionPointState.IsEnabled);
		}

        public static void SerializeMapArea(BinaryWriter Writer, MapArea Area)
        {
            SerializeMapObject(Writer, Area);
            SerializeMapPoint(Writer, Area.Position);
            SerializeMapSize(Writer, Area.Size);
        }

        public static void SerializeMapAreaState(BinaryWriter Writer, MapAreaState AreaState)
        {
			SerializeMapObjectState(Writer, AreaState);
			foreach (MapAreaTransitionPointState point in AreaState.TransitionPoints)
			{
				SerializeMapAreaTransitionPointState(Writer, point);
			}
        }

        public static void SerializeMapAreas(BinaryWriter Writer, MapAreas MapAreas)
        {
            SerializeLength(Writer, MapAreas.Areas);
            List<MapAreaTransitionPoint> transitionPoints = new List<MapAreaTransitionPoint>();
            foreach (MapArea area in MapAreas.Areas)
            {
                if (area.TransitionPoints != null)
                    transitionPoints.AddRange(area.TransitionPoints);
                SerializeMapArea(Writer, area);
            }

            SerializeLength(Writer, transitionPoints);
            if (transitionPoints != null)
            {
                foreach (MapAreaTransitionPoint transitionPoint in transitionPoints)
                    SerializeMapAreaTransitionPoint(Writer, transitionPoint);
            }
        }

        public static void SerializeMapAreasState(BinaryWriter Writer, MapAreasState MapAreasState)
        {            
            foreach (MapAreaState areaState in MapAreasState.Areas)
            {                
                SerializeMapAreaState(Writer, areaState);
            }            
        }

        public static void SerializeMap(BinaryWriter Writer, Map Map)
        {            
            SerializeMapObject(Writer, Map);            
            SerializeVersion(Writer, Map.Version);
            SerializeMapSize(Writer, Map.Size);
            Writer.Write(Map.LevelsCount);                     
            SerializeMapTileSet(Writer, Map.TileSet);            

            List<Guid> idTable = new List<Guid>();
            byte[] cellsData;

            using (MemoryStream cellsStream = new MemoryStream())
            {
                using (BinaryWriter cellsWriter = new BinaryWriter(cellsStream))
                {
                    for (UInt16 z = 0; z < Map.Levels.Length; z++)
                        for (UInt16 x = 0; x < Map.Size.Width; x++)
                            for (UInt16 y = 0; y < Map.Size.Height; y++)
                            {
                                SerializeMapCell(cellsWriter, Map.Levels[z].Cells[x, y], idTable);
                            }
                    cellsStream.Seek(0, SeekOrigin.Begin);
                    cellsData = new byte[cellsStream.Length];
                    cellsStream.Read(cellsData, 0, cellsData.Length);
                }
            }

            SerializeLength(Writer, idTable);
            foreach (Guid id in idTable)
                SerializeGuid(Writer, id);            

            Writer.Write(cellsData);

            SerializeMapAreas(Writer, Map.Areas);           
        }

        public static void SerializeMapState(BinaryWriter Writer, MapState MapState)
        {
            SerializeMapObjectState(Writer, MapState);
			SerializeGuid(Writer, MapState.ClassId);
            SerializeMap(Writer, MapState.Map);

			foreach (MapLevelState level in MapState.Levels)
			{
				for (UInt16 z = 0; z < MapState.Levels.Length; z++)
					for (UInt16 x = 0; x < MapState.Size.Width; x++)
						for (UInt16 y = 0; y < MapState.Size.Height; y++)
						{
							SerializeMapCellState(Writer, MapState.Levels[z].Cells[x, y]);
						}
			}            

            SerializeMapAreasState(Writer, MapState.Areas);

			SerializeLength(Writer, MapState.ActiveObjects.Count);
			foreach (KeyValuePair<Guid, MapActiveObjectState> activeState in MapState.ActiveObjects)
			{
				SerializeGuid(Writer, activeState.Key);
				SerializeMapActiveObjectState(Writer, activeState.Value);
			}
        }

        public static void Serialize(BinaryWriter Writer, object Object)
        {
            if (Writer == null) throw new ArgumentNullException("Writer", "Writer for serialization cannot be null");
            if (Object == null) throw new ArgumentNullException("MapObject", "MapObject for serialization cannot be null");

            if (Object is MapPlace) SerializeMapPlace(Writer, Object as MapPlace); else
            if (Object is MapWall) SerializeMapWall(Writer, Object as MapWall); else
            if (Object is MapActiveObject) SerializeMapActiveObject(Writer, Object as MapActiveObject); else
            if (Object is MapArea) SerializeMapArea(Writer, Object as MapArea); else
            if (Object is MapAreaTransitionPoint) SerializeMapAreaTransitionPoint(Writer, Object as MapAreaTransitionPoint); else
            if (Object is MapTileSet) SerializeMapTileSet(Writer, Object as MapTileSet); else
            if (Object is Map) SerializeMap(Writer, Object as Map);
			if (Object is MapState) SerializeMapState(Writer, Object as MapState);
        }

        public static void SerializeMapTileSet(BinaryWriter Writer, MapTileSet TileSet)
        {
            SerializeMapObject(Writer, TileSet);
            SerializeLength(Writer, TileSet.Count);
            foreach (MapVisualObject tile in TileSet.ToArray())
            {
                Serialize(Writer, tile);
            }
        }
    }

    [Flags]
    internal enum MapCellSerializationType : byte
    {
        Null = 0,
        Place = 1,
        NorthWall = 2,
        SouthWall = 4,
        WestWall = 8,
        EastWall = 16,
        IdIndex8 = 32,
        IdIndex16 = 64,
        IdIndex32 = 128,       
    }
}