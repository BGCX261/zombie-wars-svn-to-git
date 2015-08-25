using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZombieWars.Core.Maps
{
    public struct MapPassability
    {
        float Value
        {
            get { return _Value; }
            set
            {
                if ((value < 0) || (value > 1)) throw new ArgumentOutOfRangeException("Passability", "Passability of Place must be in range 0..1");
                _Value = value;
            }
        }
        private float _Value;

        public MapPassability(float value)
        {
            _Value = 0;
            Value = value;
        }

        public static implicit operator float(MapPassability passability) { return passability.Value; }
        public static implicit operator MapPassability(float passability) { return new MapPassability(passability); }        

        public static MapPassability Full { get { return new MapPassability(1); } }

        public static MapPassability None { get { return new MapPassability(0); } }
    }
}
