using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZombieWars.Core.Maps;
using ZombieWars.Core.Db;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using ZombieWars.Core.Maps.MapStorage;
using System.Reflection;
using Db4objects.Db4o;

namespace ZombieWars.Test
{
    [TestClass] 
    public class DbTest
    {
      //  [TestMethod]
        public void TestStorage()
        {
            //MapTileSet ts = MapStorage.Instance.GeneralTileSet;
        }

        //[TestMethod]
        public void TestSaveMap()
        {
            MapImage image = new MapImage(MapImageType.Bmp, null);
            MapSize mapSize = new MapSize(1000, 1000);
            Map map = new Map(1, mapSize);
            map.Version = new Version(1, 2, 3, 4);
            MapPlace place1 = new MapPlace(image, 1);
            MapPlace place2 = new MapPlace(image, 0.8f);
            MapWall wall = new MapWall(image, MapDirection.North, 200);

            MapActiveObject testObj1 = new MapActiveObject(image, null, new MapSize(2, 3), MapDirection.West, MapArmorType.Heavy, 120, 1);
            MapActiveObject testObj2 = new MapActiveObject(image, null, new MapSize(20, 30), MapDirection.West, MapArmorType.Machine, 121, 1);
            MapActiveObject testObj3 = new MapActiveObject(image, null, new MapSize(200, 300), MapDirection.West, MapArmorType.None, 122, 1);
            MapActiveObject testObj4 = new MapActiveObject(image, null, new MapSize(259, 355), MapDirection.West, MapArmorType.Undead, 123, 1);

            MapArea area1 = new MapArea(new MapPoint(0, 20, 30), new MapSize(50, 50));
            MapArea area2 = new MapArea(new MapPoint(0, 200, 300), new MapSize(100, 100));
            MapArea area3 = new MapArea(new MapPoint(0, 100, 100), new MapSize(10, 10));

            MapAreaTransitionPoint transPoint1 = new MapAreaTransitionPoint(area1, area2, new MapPoint(0, 25, 35), new MapSize(5, 5));
            MapAreaTransitionPoint transPoint2 = new MapAreaTransitionPoint(area2, area3, new MapPoint(0, 225, 325), new MapSize(5, 5));            
            MapAreaTransitionPoint transPoint3 = new MapAreaTransitionPoint(area3, area1, new MapPoint(0, 105, 105), new MapSize(2, 2));

            area1.TransitionPoints.Add(transPoint1);
            area2.TransitionPoints.Add(transPoint2);
            area3.TransitionPoints.Add(transPoint3);



            map.TileSet.Add(place1);
            map.TileSet.Add(place2);
            map.TileSet.Add(wall);
            map.TileSet.Add(testObj1);
            map.TileSet.Add(testObj2);
            map.TileSet.Add(testObj3);
            map.TileSet.Add(testObj4);
            map.Areas.Areas.Add(area1);
            map.Areas.Areas.Add(area2);
            map.Areas.Areas.Add(area3);

            for (int x = 0; x < mapSize.Width / 2; x++)
                for (int y = 0; y < mapSize.Height; y++)
                    map.Levels[0].Cells[x, y] = new MapCell(place1, new Dictionary<MapDirection, MapWall>());

            for (int x = mapSize.Width / 2; x < mapSize.Width; x++)
                for (int y = 0; y < mapSize.Height; y++)
                    map.Levels[0].Cells[x, y] = new MapCell(place2, new Dictionary<MapDirection, MapWall>());

            map.Levels[0].Cells[5, 6].SetWall(MapDirection.South, wall);
           
            Exception exception = null;

            MapState mapState = new MapState(map);
            
            mapState.AddActiveObject(testObj1, new MapPoint(0, 1, 1));
            mapState.AddActiveObject(testObj2, new MapPoint(0, 100, 100));
            mapState.AddActiveObject(testObj3, new MapPoint(0, 200, 200));
            mapState.AddActiveObject(testObj4, new MapPoint(0, 500, 500));
            Map map2 = null;
            MapState mapState2 = null;

            try
            {
                DateTime start = DateTime.Now;
                byte[] data = MapSerializer.Instance.SerializeMapState(mapState);
                DateTime finish = DateTime.Now;
                mapState2 = MapSerializer.Instance.DeserializeMapState(data);
                map2 = mapState2.Map;

            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.IsNull(exception);

            Assert.IsNotNull(map2);
            Assert.AreEqual(map2[0, 0, 0].Place.Id, place1.Id);
            Assert.AreEqual(map2[0, 0, 0].Place.Passability, place1.Passability);
            Assert.AreEqual(map2[0, mapSize.Width - 1, 0].Place.Id, place2.Id);
            Assert.AreEqual(map2[0, mapSize.Width - 1, 0].Place.Passability, place2.Passability);
            Assert.AreEqual(map2[0, 5, 6].Walls[MapDirection.South].Id, wall.Id);
            Assert.AreEqual(map2[0, 5, 6].Walls[MapDirection.South].Health, wall.Health);
            Assert.AreEqual(map2.Areas.Areas[1].Position, map.Areas.Areas[1].Position);
            Assert.AreEqual(map2.Areas.Areas[2].TransitionPoints[0].From.Name, map.Areas.Areas[2].TransitionPoints[0].From.Name);
            Assert.AreEqual(map2.Areas.Areas[2].TransitionPoints[0].To.Name, map.Areas.Areas[2].TransitionPoints[0].To.Name);
            Assert.AreEqual(map2.Areas.Areas[2].TransitionPoints[0].Position, map.Areas.Areas[2].TransitionPoints[0].Position);
            Assert.AreEqual(mapState2.ActiveObjects.Count, mapState.ActiveObjects.Count);
        }      
    }
}
