using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZombieWars.Core.Maps;

namespace ZombieWars.Core.Npc
{
    public interface INpcAction
    {
        void Perform(NpcState NpcState);
    }
}
