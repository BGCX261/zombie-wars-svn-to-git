using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZombieWars.Core.Game;
using ZombieWars.Core.Maps;

namespace ZombieWars.Test
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class MemoryTest
    {       

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion
        

        /// <summary>
        /// Тест создания карты
        /// </summary>
        /// <param name="MapWidth">Ширина карты (в клетках)</param>
        /// <param name="MapHeight">Высота карты (в клетках)</param>
        /// <param name="LevelsCount">Количество уровней</param>
        /// <param name="WallsCountOnWalledCells">Количество стен в клетке, на которой будут размещаться стены (0..4)</param>
        /// <param name="WalledCellProbability">Вероятность того, что на клетке будут размещаться стены (0..1)</param>
        /// <returns>Занимаемая память</returns>        
        public static Int64 MapCreation(UInt16 MapWidth, UInt16 MapHeight, UInt16 LevelsCount, UInt16 WallsCountOnWalledCells, double WalledCellProbability)
        {
            GameMap game;
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

            Int64 memory = GC.GetTotalMemory(false);

            map = null;
            game = null;
            GC.Collect();

            return memory;
        }
        
        public void TestMapCreation()
        {
            Int64 memory = MapCreation(1000, 1000, 1, 0, 0);
            Assert.IsTrue(memory < 200 * 1024 * 1024);
        }
        
        public void TestCellsMatrix()
        {
            MapSize size = new MapSize(1000, 1000);
            MapPlace place = new MapPlace(new MapImage(MapImageType.Bmp, null), 1);
            MapCell cell = new MapCell(place, null);

            MapMatrix<MapCell> matrix1 = new MapMatrix<MapCell>(size);
            MapCell[,] matrix2 = new MapCell[size.Width, size.Height];

            for (int x = 0; x < size.Width; x++)
                for (int y = 0; y < size.Height; y++)
                {
                    matrix1[x, y] = cell;
                    matrix2[x, y] = cell;
                }

            DateTime start1 = DateTime.Now;

            for (int i = 0; i < 1000000; i++)
            {
                cell = matrix1[25, 68];
            }

            DateTime finish1 = DateTime.Now;

            DateTime start2 = DateTime.Now;

            for (int i = 0; i < 1000000; i++)
            {
                cell = matrix2[25, 68];
            }

            DateTime finish2 = DateTime.Now;

            double result1 = (finish1 - start1).TotalMilliseconds;
            double result2 = (finish2 - start2).TotalMilliseconds;

            Assert.IsTrue(result1 < 100);

        }

        //[TestMethod]
        public void LinqTest()
        {
            List<string> testArray = new List<string>(new string[1000000]);

            DateTime start1 = DateTime.Now;

            int count1 = 0;
            for (int i = 0; i < testArray.Count; i++)
                if (i == 100) count1++;
            
            DateTime finish1 = DateTime.Now;

            DateTime start2 = DateTime.Now;

            int count2 = testArray.Where(i => i == null).Count();

            DateTime finish2 = DateTime.Now;

            double result1 = (finish1 - start1).TotalMilliseconds;
            double result2 = (finish2 - start2).TotalMilliseconds;            
        }
    }
}
