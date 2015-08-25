using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using ZombieWars.Core.Maps;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace MapEditor.ViewModels
{
    public class TileSetViewModel : BaseViewModel
    {
        public MapTileSet Source { get; private set; }        
        
        public ListCollectionView Places { get; private set; }

        public ListCollectionView Walls { get; private set; }

        public ListCollectionView ActiveObjects { get; private set; }

        public TileSetViewModel(MapTileSet mapTileSet)
            : base(mapTileSet)
        {
            if (mapTileSet == null) throw new ArgumentNullException("mapTileSet");

            Source = mapTileSet;            
            Places = new ListCollectionView(Source.Where(t => t is MapPlace).Select(t => new PlaceViewModel(t as MapPlace)).ToList());            
            Walls = new ListCollectionView(Source.Where(t => t is MapWall).Select(t => new WallViewModel(t as MapWall)).ToList());
            ActiveObjects = new ListCollectionView(Source.Where(t => t is MapActiveObject).Select(t => new ActiveObjectViewModel(t as MapActiveObject)).ToList());
        }

        public TileSetViewModel(string title, string description) :
            this(new MapTileSet() { Name = title, Caption = title, Description = description })
        {
        }

        public event EventHandler Saved;

        public void Save()
        {
            if (Source == null) return;

            Source.Clear();            
            foreach (object place in Places)
            {
                PlaceViewModel placeVM = place as PlaceViewModel;
                if (placeVM != null) Source.Add(placeVM.Source);
            }
            foreach (object wall in Walls)
            {
                WallViewModel wallVM = wall as WallViewModel;
                if (wallVM != null) Source.Add(wallVM.Source);
            }
            foreach (object activeObject in ActiveObjects)
            {
                ActiveObjectViewModel activeObjectVM = activeObject as ActiveObjectViewModel;
                if (activeObjectVM != null) Source.Add(activeObjectVM.Source);
            }

            var savedHanlder = Saved;
            if (savedHanlder != null)
                savedHanlder(this, EventArgs.Empty);
        }
    }
}
