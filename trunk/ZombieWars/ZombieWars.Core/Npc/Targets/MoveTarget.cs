using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZombieWars.Core.Maps;

namespace ZombieWars.Core.Npc.Targets
{
    public class MoveTarget : ITarget
    {
        public MapState MapState { get; set; }
        public MapPoint GlobalTarget { get; set; }
        public MapPoint LocalTarget{ get; set; }
    }
}
