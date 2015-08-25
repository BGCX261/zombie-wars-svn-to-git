using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZombieWars.Core.Maps;

namespace ZombieWars.Core.Npc.Actions
{
    public class MoveAction : INpcAction
    {        
        public MapPoint Target { get; set; }
        public MoveAction(MapPoint target) { this.Target = target; }
        
        public void Perform(NpcState NpcState)
        {
            NpcState.MapState.RelocateActiveObject(NpcState.MapActiveObjectGuid, Target);
        }
    }
}
