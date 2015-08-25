using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZombieWars.Core.Npc.Actions;

namespace ZombieWars.Core.Npc
{
    public class Script
    {
        public String Content { get; set; }
        public String Language { get; set; }
        public String Description { get; set; }
        public String Name { get; set; }

        public virtual INpcAction Execute(NpcState NPCState)
        {
            return new NothingAction();
        }
    }
}
