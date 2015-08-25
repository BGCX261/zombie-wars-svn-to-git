using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZombieWars.Core.Maps;

namespace ZombieWars.Core.Npc
{
    public struct ScriptAction
    {
        public ITarget Target { get; set; }
        public Script Script { get; set; }        
    }    
}
