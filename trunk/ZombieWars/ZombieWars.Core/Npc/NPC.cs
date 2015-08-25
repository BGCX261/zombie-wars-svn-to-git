using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZombieWars.Core.Maps;
using System.Runtime.InteropServices;

namespace ZombieWars.Core.Npc
{
    [Serializable]
    [Guid("C5ED25D0-BD08-44AB-9740-E5B689B890D0")]
    public class Npc : MapObject
    {        
        public MapActiveObject MapActiveObject { get; set; }
        public NpcType Type { get; set; }        
        
        public Npc() { }
        public Npc(NpcType Type, MapActiveObject MapActiveObject)            
        {
            this.MapActiveObject = MapActiveObject;
            this.Type = Type;                
        }        

    }
}
