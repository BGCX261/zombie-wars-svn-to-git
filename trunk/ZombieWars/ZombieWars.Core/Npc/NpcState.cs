using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZombieWars.Core.Maps;

namespace ZombieWars.Core.Npc
{
    public class NpcState : MapObjectState
    {
        public Npc Npc { get { return base.MapObject as Npc; } }
        public Guid MapActiveObjectGuid { get; set; }
        public MapState MapState { get; set; }
        public MapActiveObjectState MapActiveObjectState { get { return this.MapState.GetActiveObjectById(MapActiveObjectGuid); } }
        public Script AI { get; set; }
        public Fraction Fraction { get; set; }
        public ITarget Target { get; set; }

        public NpcState(Npc Npc, MapState MapState, Script AI, Fraction Fraction, MapPoint Position)
            : base(Npc)
        {            
            this.MapState = MapState;
            //this.MapActiveObjectGuid = this.MapState.AddActiveObject(this.Npc.MapActiveObject, Position);
            this.AI = AI;
            this.Fraction = Fraction;
        }

        public void PerformAction()
        {
            INpcAction nextAction = AI.Execute(this);
            nextAction.Perform(this);            
        }
    }
}
