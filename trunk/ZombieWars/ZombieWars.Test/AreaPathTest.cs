using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZombieWars.Core.Maps;
using ZombieWars.Core.Game;

namespace ZombieWars.Test
{
    public class AreaPathTest
    {

        public static void PathTest()
        {
            GameMap game;
            Map map;

            Random rand = new Random();
            try
            {
                MapImage image = new MapImage(MapImageType.Bmp, null);
                MapPlace place = new MapPlace(image, (float)0.8) { Name = "1" };
                //MapWall destroyedWall = new MapWall("2", image, MapDirection.North, 200);
                MapWall wall = new MapWall(image, MapDirection.North, 200) { Name = "3" };
                Dictionary<MapDirection, MapWall> walls = new Dictionary<MapDirection, MapWall>();

                map = new Map(1, new MapSize(16, 16)) { Name = "Map" };
                for (int z = 0; z < map.LevelsCount; z++)
                    for (int x = 0; x < map.Size.Width; x++)
                        for (int y = 0; y < map.Size.Height; y++)
                        {
                            map.Levels[z].Cells[x, y] = new MapCell(place, null);
                        }
                MapAreas areas= new MapAreas();
             //   MapAreaTransitionPoint pnt1=new MapAreaTransitionPoint("", "", 
                areas.Areas.Add(new MapArea(new MapPoint(0, 0, 0), new MapSize(4, 4)));
                areas.Areas.Add(new MapArea(new MapPoint(0, 4, 0), new MapSize(4, 4)));
                areas.Areas.Add(new MapArea(new MapPoint(0, 8, 0), new MapSize(4, 4)));
                areas.Areas.Add(new MapArea(new MapPoint(0, 12, 0), new MapSize(4, 4)));
                areas.Areas.Add(new MapArea(new MapPoint(0, 0, 4), new MapSize(4, 4)));
                areas.Areas.Add(new MapArea(new MapPoint(0, 4, 4), new MapSize(4, 4)));
                areas.Areas.Add(new MapArea(new MapPoint(0, 8, 4), new MapSize(4, 4)));
                areas.Areas.Add(new MapArea(new MapPoint(0, 12, 4), new MapSize(4, 4)));
                areas.Areas.Add(new MapArea(new MapPoint(0, 0, 8), new MapSize(4, 4)));
                areas.Areas.Add(new MapArea(new MapPoint(0, 4, 8), new MapSize(4, 4)));
                areas.Areas.Add(new MapArea(new MapPoint(0, 8, 8), new MapSize(4, 4)));
                areas.Areas.Add(new MapArea(new MapPoint(0, 12, 8), new MapSize(4, 4)));
                areas.Areas.Add(new MapArea(new MapPoint(0, 0, 12), new MapSize(4, 4)));
                areas.Areas.Add(new MapArea(new MapPoint(0, 4, 12), new MapSize(4, 4)));
                areas.Areas.Add(new MapArea(new MapPoint(0, 8, 12), new MapSize(4, 4)));
                areas.Areas.Add(new MapArea(new MapPoint(0, 12, 12), new MapSize(4, 4)));
                MapAreaTransitionPoint[] tr0 = new MapAreaTransitionPoint[] { new MapAreaTransitionPoint(areas.Areas[0], areas.Areas[1], new MapPoint(0, 0, 0), new MapSize(1, 1)), new MapAreaTransitionPoint(areas.Areas[0], areas.Areas[5], new MapPoint(0, 1, 1), new MapSize(1, 1)) };
                MapAreaTransitionPoint[] tr1 = new MapAreaTransitionPoint[] { new MapAreaTransitionPoint(areas.Areas[1], areas.Areas[0], new MapPoint(0, 0, 0), new MapSize(1, 1)) };
                MapAreaTransitionPoint[] tr5 = new MapAreaTransitionPoint[] { new MapAreaTransitionPoint(areas.Areas[5], areas.Areas[0], new MapPoint(0, 0, 0), new MapSize(1, 1)), new MapAreaTransitionPoint(areas.Areas[5], areas.Areas[2], new MapPoint(0, 0, 0), new MapSize(1, 1)) };
                MapAreaTransitionPoint[] tr2 = new MapAreaTransitionPoint[] { new MapAreaTransitionPoint(areas.Areas[2], areas.Areas[5], new MapPoint(0, 0, 0), new MapSize(1, 1)), new MapAreaTransitionPoint(areas.Areas[2], areas.Areas[7], new MapPoint(0, 0, 0), new MapSize(1, 1)) };
                MapAreaTransitionPoint[] tr3 = new MapAreaTransitionPoint[] { new MapAreaTransitionPoint(areas.Areas[3], areas.Areas[6], new MapPoint(0, 0, 0), new MapSize(1, 1)) };
                MapAreaTransitionPoint[] tr4 = new MapAreaTransitionPoint[] { new MapAreaTransitionPoint(areas.Areas[4], areas.Areas[8], new MapPoint(0, 0, 0), new MapSize(1, 1)) };
                MapAreaTransitionPoint[] tr6 = new MapAreaTransitionPoint[] { new MapAreaTransitionPoint(areas.Areas[6], areas.Areas[3], new MapPoint(0, 0, 0), new MapSize(1, 1)), new MapAreaTransitionPoint(areas.Areas[6], areas.Areas[10], new MapPoint(0, 0, 0), new MapSize(1, 1)) };
                MapAreaTransitionPoint[] tr7 = new MapAreaTransitionPoint[] { new MapAreaTransitionPoint(areas.Areas[7], areas.Areas[2], new MapPoint(0, 0, 0), new MapSize(1, 1)), new MapAreaTransitionPoint(areas.Areas[7], areas.Areas[10], new MapPoint(0, 0, 0), new MapSize(1, 1)) };
                MapAreaTransitionPoint[] tr8 = new MapAreaTransitionPoint[] { new MapAreaTransitionPoint(areas.Areas[8], areas.Areas[4], new MapPoint(0, 0, 0), new MapSize(1, 1)), new MapAreaTransitionPoint(areas.Areas[8], areas.Areas[9], new MapPoint(0, 0, 0), new MapSize(1, 1)), new MapAreaTransitionPoint(areas.Areas[8], areas.Areas[12], new MapPoint(0, 0, 0), new MapSize(1, 1)) };
                MapAreaTransitionPoint[] tr13 = new MapAreaTransitionPoint[] { new MapAreaTransitionPoint(areas.Areas[13], areas.Areas[9], new MapPoint(0, 0, 0), new MapSize(1, 1)), new MapAreaTransitionPoint(areas.Areas[13], areas.Areas[14], new MapPoint(0, 0, 0), new MapSize(1, 1)), new MapAreaTransitionPoint(areas.Areas[13], areas.Areas[10], new MapPoint(0, 0, 0), new MapSize(1, 1)) };
                MapAreaTransitionPoint[] tr9 = new MapAreaTransitionPoint[] { new MapAreaTransitionPoint(areas.Areas[9], areas.Areas[8], new MapPoint(0, 0, 0), new MapSize(1, 1)), new MapAreaTransitionPoint(areas.Areas[9], areas.Areas[13], new MapPoint(0, 0, 0), new MapSize(1, 1)) };
                MapAreaTransitionPoint[] tr12 = new MapAreaTransitionPoint[] { new MapAreaTransitionPoint(areas.Areas[12], areas.Areas[8], new MapPoint(0, 0, 0), new MapSize(1, 1)) };
                MapAreaTransitionPoint[] tr14 = new MapAreaTransitionPoint[] { new MapAreaTransitionPoint(areas.Areas[14], areas.Areas[13], new MapPoint(0, 0, 0), new MapSize(1, 1)) };
                MapAreaTransitionPoint[] tr15 = new MapAreaTransitionPoint[] { new MapAreaTransitionPoint(areas.Areas[15], areas.Areas[10], new MapPoint(0, 0, 0), new MapSize(1, 1)) };
                MapAreaTransitionPoint[] tr11 = new MapAreaTransitionPoint[] { new MapAreaTransitionPoint(areas.Areas[11], areas.Areas[10], new MapPoint(0, 0, 0), new MapSize(1, 1)) };
                MapAreaTransitionPoint[] tr10 = new MapAreaTransitionPoint[] { new MapAreaTransitionPoint(areas.Areas[10], areas.Areas[6], new MapPoint(0, 0, 0), new MapSize(1, 1)), new MapAreaTransitionPoint(areas.Areas[10], areas.Areas[7], new MapPoint(0, 0, 0), new MapSize(1, 1)), new MapAreaTransitionPoint(areas.Areas[10], areas.Areas[11], new MapPoint(0, 0, 0), new MapSize(1, 1)), new MapAreaTransitionPoint(areas.Areas[10], areas.Areas[15], new MapPoint(0, 0, 0), new MapSize(1, 1)), new MapAreaTransitionPoint(areas.Areas[10], areas.Areas[13], new MapPoint(0, 0, 0), new MapSize(1, 1)) };
                areas.Areas[0].TransitionPoints.AddRange(tr0);
                areas.Areas[1].TransitionPoints.AddRange(tr1);
                areas.Areas[2].TransitionPoints.AddRange(tr2);
                areas.Areas[3].TransitionPoints.AddRange(tr3);
                areas.Areas[4].TransitionPoints.AddRange(tr4);
                areas.Areas[5].TransitionPoints.AddRange(tr5);
                areas.Areas[6].TransitionPoints.AddRange(tr6);
                areas.Areas[7].TransitionPoints.AddRange(tr7);
                areas.Areas[8].TransitionPoints.AddRange(tr8);
                areas.Areas[9].TransitionPoints.AddRange(tr9);
                areas.Areas[10].TransitionPoints.AddRange(tr10);
                areas.Areas[11].TransitionPoints.AddRange(tr11);
                areas.Areas[12].TransitionPoints.AddRange(tr12);
                areas.Areas[13].TransitionPoints.AddRange(tr13);
                areas.Areas[14].TransitionPoints.AddRange(tr14);
                areas.Areas[15].TransitionPoints.AddRange(tr15);

                map.Areas = areas;

                MapState state = new MapState(map);                
  

                game = new GameMap(new MapState(map));

            }
            catch
            {
            }

            Int64 memory = GC.GetTotalMemory(false);

            map = null;
            game = null;
            GC.Collect();


        }


    }
}
