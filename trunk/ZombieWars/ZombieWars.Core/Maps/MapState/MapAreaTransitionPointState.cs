using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZombieWars.Core.Maps
{
	public class MapAreaTransitionPointState : MapObjectState
	{
		 /// <summary>
        /// Состояние точки перехода
        /// </summary>
        public MapAreaTransitionPoint TransitionPoint{ get; protected set; }

		/// <summary>
		/// Доступна ли точка перехода
		/// </summary>
		public bool IsEnabled { get; set; }

        /// <summary>
        /// Состояние точки перехода
        /// </summary>
        /// <param name="transitionPoint">Точка перехода</param>
        public MapAreaTransitionPointState(MapAreaTransitionPoint transitionPoint)
            : base(transitionPoint)
        {
			IsEnabled = true;
        }
	}
}
