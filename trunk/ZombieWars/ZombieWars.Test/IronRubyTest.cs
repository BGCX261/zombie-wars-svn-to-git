using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZombieWars.Core.Maps;
using ZombieWars.Core.Npc;
using ZombieWars.Core.Game;
using ZombieWars.ScriptEngines.IronRubyScript;

namespace ZombieWars.Test
{
    public class SimpleObject
    {
        public int[] mas;
        public int[] getless100()
        {
            List<int> arr = new List<int>();
            for (int i = 0; i < mas.Length; i++)
            {
                if (mas[i] < 100) arr.Add(mas[i]);
            }
            return arr.ToArray();
        }
    }
   
    [TestClass]
    public class IronRubyTest
    {
        //[TestMethod]
        public void Execute()
        {
            
            IronRubyScript script=new IronRubyScript();
            script.Content=@"m=new MoveAction(new MapPoint(0,2,2));";



            GameMap game=MapCreation(1000, 1000, 1, 0, 0);
            NpcType type = new NpcType();
            type.Description = "1";
            type.Name = "1";
            MapActiveObject obj = new MapActiveObject(null, null, new MapSize(1, 1), MapDirection.East, MapArmorType.None, 100, 0);

            Npc npc=new Npc(type, obj);
            NpcState state= new NpcState( npc,game.Map,script,new Fraction(),new MapPoint(0,1,1));
            state.PerformAction();
            //npc.Position = game.Map.Levels[0].Cells[1,1];
            //Move move = new Move(new MapPoint(0, 1, 2), npc);
            //((NPCAction)move).perform(game.Map);
            //MapCreation(1000, 1000, 1, 0, 0);


            //SimpleObject obj= new SimpleObject();
            //obj.mas=new int[50000000];
            //Random rand = new Random();
            //for (int i = 0; i < obj.mas.Length; i++)
            //  obj.mas[i] = rand.Next(100000);

//            ScriptRuntime runtime = IronRuby.Ruby.CreateRuntime();
//            ScriptEngine _rubyEngine = runtime.GetEngine("Ruby");
//            ScriptScope _scope = _rubyEngine.CreateScope();
//            _scope.SetVariable("obj", obj);
//            DateTime time1 = DateTime.Now;
//            var result1 = _rubyEngine.Execute(@" mas=[]
//                                                 (0..obj.mas.size()-1).each do |x|  
//                                                      if obj.mas[x]<100
//                                                          mas+= [obj.mas[x]]
//                                                       end                                           
//                                                  end
//                                                 m1=mas ", _scope);
//            DateTime time2 = DateTime.Now;
//            var result2 = _rubyEngine.Execute(@"  obj.mas.find_all{|elem| elem<100}  ", _scope);
//            DateTime time3 = DateTime.Now;
//            var result3 = _rubyEngine.Execute(@"  obj.getless100()  ", _scope);
//            DateTime time4 = DateTime.Now;
//            var result4 = obj.getless100();
//            DateTime time5 = DateTime.Now;
//            var result5 = obj.mas.Where(x => x < 100).ToArray();
//            DateTime time6 = DateTime.Now;

        }
        public  GameMap MapCreation(UInt16 MapWidth, UInt16 MapHeight, UInt16 LevelsCount, UInt16 WallsCountOnWalledCells, double WalledCellProbability)
        {
            GameMap game=null;
            Map map;

            Random rand = new Random();
            try
            {
                MapImage image = new MapImage(MapImageType.Bmp, null);
                MapPlace place = new MapPlace(image, (float)0.8);
                MapWall destroyedWall = new MapWall(image, MapDirection.North, 200);
                MapWall wall = new MapWall(image, MapDirection.North, 200);
                Dictionary<MapDirection, MapWall> walls = new Dictionary<MapDirection, MapWall>();

                if (WallsCountOnWalledCells > 0)
                    walls.Add(MapDirection.North, wall);
                if (WallsCountOnWalledCells > 1)
                    walls.Add(MapDirection.South, wall);
                if (WallsCountOnWalledCells > 2)
                    walls.Add(MapDirection.West, wall);
                if (WallsCountOnWalledCells > 3)
                    walls.Add(MapDirection.East, wall);

                bool isWall = false;
                int probability = (int)Math.Round(WalledCellProbability * 100.0);

                map = new Map(LevelsCount, new MapSize(MapWidth, MapHeight));
                for (int z = 0; z < map.LevelsCount; z++)
                    for (int x = 0; x < map.Size.Width; x++)
                        for (int y = 0; y < map.Size.Height; y++)
                        {
                            isWall = rand.Next(0, 100) < probability;
                            map.Levels[z].Cells[x, y] = new MapCell(place, isWall ? walls : null);
                        }
                game = new GameMap(new MapState(map));
            }
            catch
            {
            }

            return game;
        }
    }    
}
