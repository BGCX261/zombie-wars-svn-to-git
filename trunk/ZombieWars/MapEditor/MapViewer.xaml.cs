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
using ZombieWars.Core.Game;
using ZombieWars.Graphics.WPF;
using ZombieWars.Core.Maps;
using MapEditor.ViewModels;

namespace MapEditor
{
    /// <summary>
    /// Interaction logic for MapViewer.xaml
    /// </summary>
    public partial class MapViewer : UserControl
    {
        private GraphicsEngineWPF Renderer;

        public static DependencyProperty TargetMapProperty = DependencyProperty.Register("TargetMap", typeof(MapViewModel), typeof(MapViewer),
            new PropertyMetadata(new PropertyChangedCallback((s, e) => { if (s is MapViewer) (s as MapViewer).OnTargetMapChanged(e.NewValue as MapViewModel); })));

        public MapViewModel TargetMap
        {
            get { return GetValue(TargetMapProperty) as MapViewModel; }
            set { SetValue(TargetMapProperty, value); }
        }        

        public void OnTargetMapChanged(MapViewModel newValue)
        {
            if (newValue == null) return;

            GameMap map = newValue.Source;

            newValue.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(map_PropertyChanged);
            Renderer.IsGridLinesVisible = newValue.IsGridLinesVisible;
            Renderer.SetMap(map, map.Map.Range);
            Render();
        }

        public static DependencyProperty TargetObjectProperty = DependencyProperty.Register("TargetObject", typeof(BaseViewModel), typeof(MapViewer),
           new PropertyMetadata(new PropertyChangedCallback((s, e) => { if (s is MapObjectViewer) (s as MapObjectViewer).OnTargetObjectChanged(e.NewValue as BaseViewModel); })));

        public BaseViewModel TargetObject
        {
            get { return GetValue(TargetObjectProperty) as BaseViewModel; }
            set { SetValue(TargetObjectProperty, value); }
        }        

        public void OnTargetObjectChanged(BaseViewModel newValue)
        {           
        }

        void map_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Renderer.IsGridLinesVisible = TargetMap.IsGridLinesVisible;            
        }

        public void Render()
        {
            Renderer.Render();
        }   

        public MapViewer()
        {
            InitializeComponent();
            canvas.MouseLeftButtonDown += new MouseButtonEventHandler(canvas_MouseLeftButtonDown);
            Renderer = new GraphicsEngineWPF(canvas) { IsGridLinesVisible = true, NullCellBackground = Brushes.Black, GridLineBackground = Brushes.White };
        }

        void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (TargetObject == null) return;

            Point p = e.MouseDevice.GetPosition(canvas);
            MapPoint cellPoint = Renderer.GetMapPointByPoint(p);            

            if (TargetObject.SourceBase is MapPlace)
            {
                MapPlace place = TargetObject.SourceBase as MapPlace;
                TargetMap.Map.SetPlace(place, cellPoint);

                Renderer.Render();
            }
        }        
    }
}
