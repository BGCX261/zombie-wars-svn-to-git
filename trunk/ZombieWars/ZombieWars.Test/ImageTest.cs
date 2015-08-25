using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZombieWars.Graphics.WPF;
using ZombieWars.Core.Maps;
using System.IO;

namespace ZombieWars.Test
{
    [TestClass]
    public class ImageTest
    {
        [TestMethod]
        public void TestConversion()
        {
            try
            {
                byte[] data = new byte[] { 1, 2, 3, 4, 5 };
                MapImage tile = new MapImage(MapImageType.Bmp, data);
                MapImageWPF image = new MapImageWPF(tile);                
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(InvalidDataException));
            }          
        }
    }
}
