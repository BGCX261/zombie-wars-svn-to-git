using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZombieWars.Core.Maps;
using System.ComponentModel;
using ZombieWars.Core.Game;

namespace MapEditor.ViewModels
{
    public class MapViewModel : BaseViewModel
    {
        public GameMap Source { get; private set; }

        public MapState Map { get { return (Source != null) ? Source.Map : null; } }

        public TileSetViewModel TileSet { get; private set; }

        [DisplayName("Сетка")]
        public bool IsGridLinesVisible
        {
            get { return _IsGridLinesVisible; }
            set
            {
                if (_IsGridLinesVisible == value) return;
                _IsGridLinesVisible = value;
                OnPropertyChanged("IsGridLinesVisible");
            }
        }
        private bool _IsGridLinesVisible;

        public MapViewModel(GameMap map)
            :base(map.Map.Map)
        {
            Source = map;
            TileSet = new TileSetViewModel(Map.Map.TileSet);
            TileSet.Saved += (s, e) => OnPropertyChanged("TileSet");
        }

        public void Save()
        {
        }
    }
}
