using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZombieWars.Core.Maps;
using ZombieWars.Graphics.WPF;
using ZombieWars.Core.Game;
using MapEditor.ViewModels;

namespace MapEditor
{
    /// <summary>
    /// Interaction logic for MapObjetViewer.xaml
    /// </summary>
    public partial class MapObjectViewer : UserControl
    {
        public static DependencyProperty TargetObjectProperty = DependencyProperty.Register("TargetObject", typeof(BaseViewModel), typeof(MapObjectViewer),
            new PropertyMetadata(new PropertyChangedCallback((s, e) => { if (s is MapObjectViewer) (s as MapObjectViewer).OnTargetObjectChanged(e.NewValue as BaseViewModel); })));

        public BaseViewModel TargetObject
        {
            get { return GetValue(TargetObjectProperty) as BaseViewModel; }
            set { SetValue(TargetObjectProperty, value); }
        }

        private GraphicsEngineWPF Renderer;

        public void OnTargetObjectChanged(BaseViewModel newValue)
        {
            if (newValue == null) return;

            newValue.PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(TargetObject_PropertyChanged);
            newValue.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(TargetObject_PropertyChanged);

            MapObject mapObject = newValue.SourceBase;
            Render(mapObject);
        }

        public void Render(MapObject mapObject)
        {
            MapPoint cellPoint = new MapPoint(0, 0, 0);
            Map map = new Map(1, MapSize.One);
            map[cellPoint] = new MapCell(new MapPlace(), null);
            if (mapObject is MapPlace)
            {
                map[cellPoint].Place = mapObject as MapPlace;
            }
            if (mapObject is MapWall)
            {
                map[cellPoint].SetWall(MapDirection.North, mapObject as MapWall);
                map[cellPoint].SetWall(MapDirection.East, mapObject as MapWall);
                map[cellPoint].SetWall(MapDirection.West, mapObject as MapWall);
                map[cellPoint].SetWall(MapDirection.South, mapObject as MapWall);
            }
            MapState mapState = new MapState(map);
            if (mapObject is MapActiveObject)
            {
                mapState.AddActiveObject(mapObject as MapActiveObject, cellPoint);
            }
            GameMap gameMap = new GameMap(mapState);
            Renderer.SetMap(gameMap, new MapCellRange(cellPoint, cellPoint));
            Renderer.Render();
        }

        void TargetObject_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (TargetObject == null) return;
            MapObject mapObject = TargetObject.SourceBase; ;
            Render(mapObject);
        }

        public MapObjectViewer()
        {
            InitializeComponent();
            Renderer = new GraphicsEngineWPF(canvas);         
        }        
    }
}
