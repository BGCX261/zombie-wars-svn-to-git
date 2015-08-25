using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZombieWars.Core.Engine;
using ZombieWars.Core.Game;
using System.Windows.Controls;
using ZombieWars.Core.Maps;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ZombieWars.Graphics.WPF
{
    public class GraphicsEngineWPF : IGraphicsEngine
    {
        private Canvas Canvas;
        private double WallPadding = 0.1;
        private GameMap TargetMap;
        private MapCellRange TargetRange;

        public bool IsGridLinesVisible
        {
            get { return _IsGridLinesVisible; }
            set
            {
                if (_IsGridLinesVisible == value) return;
                _IsGridLinesVisible = value;
                Render();
            }
        }
        private bool _IsGridLinesVisible;

        public Brush NullCellBackground { get; set; }
        public Brush GridLineBackground { get; set; }
        public double GridLineWidth { get; set; }

        public GraphicsEngineWPF(Canvas canvas)
        {
            Canvas = canvas;
            Canvas.SizeChanged += new SizeChangedEventHandler(Canvas_SizeChanged);
        }

        void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Render();
        }

        public void SetMap(GameMap map, MapCellRange range)
        {
            TargetMap = map;
            TargetRange = range;
        }

        public void SetRange(MapCellRange range)
        {
            TargetRange = range;
        }        

        public void Render()
        {
            if (Canvas == null) return;
            Canvas.Children.Clear();

            if (TargetMap == null) return;            

            MapState map = TargetMap.Map;
            if (map == null) return;

            double width = Canvas.ActualWidth;
            double height = Canvas.ActualHeight;
            ushort beginLevel = Math.Min(TargetRange.Begin.Level, TargetRange.End.Level);
            ushort endLevel = Math.Max(TargetRange.Begin.Level, TargetRange.End.Level);
            ushort beginX = Math.Min(TargetRange.Begin.X, TargetRange.End.X);
            ushort endX = Math.Max(TargetRange.Begin.X, TargetRange.End.X);
            ushort beginY = Math.Min(TargetRange.Begin.Y, TargetRange.End.Y);
            ushort endY = Math.Max(TargetRange.Begin.Y, TargetRange.End.Y);
            ushort cellsCountX = (ushort)(endX - beginX + 1);
            ushort cellsCountY = (ushort)(endY - beginY + 1);
            double cellWidth = width / cellsCountX;
            double cellHeight = height / cellsCountY;
            cellWidth = cellHeight = Math.Floor(Math.Min(cellWidth, cellHeight));
            Size cellSize = new Size(cellWidth, cellHeight);

            for (ushort level = beginLevel; level <= endLevel; level++)
            {
                for (ushort x = beginX; x <= endX; x++)
                {
                    for (ushort y = beginY; y <= endY; y++)
                    {
                        MapCellState cell = map[level, x, y];                        

                        Point cellLocation = new Point((x - beginX) * cellWidth, (y - beginY) * cellHeight);                        
                        Rect cellArea = new Rect(cellLocation, cellSize);
                        //cellArea.Width += 1;
                        //cellArea.Height += 1;

                        List<FrameworkElement> cellRender = null;
                        
                        if (cell != null)
                            cellRender = RenderCell(cell, cellArea);
                        else
                            cellRender = RenderNullCell(cellArea);

                        if (cellRender == null) continue;

                        foreach (FrameworkElement cellRenderItem in cellRender)
                        {
                            if (cellRenderItem == null) continue;
                            Canvas.Children.Add(cellRenderItem);
                        }
                    }
                }
            }

            if (IsGridLinesVisible)
            {
                for (ushort x = beginX; x <= endX + 1; x++)
                {
                    FrameworkElement gridLine = RenderVerticalGridLine(new Point((x - beginX) * cellWidth, 0), new Point((x - beginX) * cellWidth, cellHeight * cellsCountY));
                    if (gridLine != null) Canvas.Children.Add(gridLine);
                }

                for (ushort y = beginY; y <= endY + 1; y++)
                {
                    FrameworkElement gridLine = RenderHorizontalGridLine(new Point(0, (y - beginX) * cellHeight), new Point(cellWidth * cellsCountX, (y - beginX) * cellHeight));
                    if (gridLine != null) Canvas.Children.Add(gridLine);
                }
            }

            //foreach (MapActiveObjectState activeObject in map.ActiveObjectList)
            //{
            //    if (activeObject == null) continue;
            //    FrameworkElement renderObject = RenderActiveObject(activeObject, area);
            //    if (renderObject == null) continue;
            //    Canvas.Children.Add(renderObject);
            //}
               
        }

        public MapPoint GetMapPointByPoint(Point point)
        {
            double width = Canvas.ActualWidth;
            double height = Canvas.ActualHeight;
            ushort beginLevel = Math.Min(TargetRange.Begin.Level, TargetRange.End.Level);
            ushort endLevel = Math.Max(TargetRange.Begin.Level, TargetRange.End.Level);
            ushort beginX = Math.Min(TargetRange.Begin.X, TargetRange.End.X);
            ushort endX = Math.Max(TargetRange.Begin.X, TargetRange.End.X);
            ushort beginY = Math.Min(TargetRange.Begin.Y, TargetRange.End.Y);
            ushort endY = Math.Max(TargetRange.Begin.Y, TargetRange.End.Y);
            ushort cellsCountX = (ushort)(endX - beginX + 1);
            ushort cellsCountY = (ushort)(endY - beginY + 1);
            double cellWidth = width / cellsCountX;
            double cellHeight = height / cellsCountY;
            cellWidth = cellHeight = Math.Floor(Math.Min(cellWidth, cellHeight));
            Size cellSize = new Size(cellWidth, cellHeight);

            ushort indexX = (ushort)(point.X / cellSize.Width + beginX);
            ushort indexY = (ushort)(point.Y / cellSize.Height + beginY);

            return new MapPoint(endLevel, indexX, indexY);
        }

        private FrameworkElement RenderVerticalGridLine(Point begin, Point end)
        {
            Rectangle rect = new Rectangle(); rect.Fill = GridLineBackground;                       
            rect.Height = Math.Abs(end.Y - begin.Y);
            rect.Width = (GridLineWidth <= 0) ? 1 : GridLineWidth;
            Canvas.SetLeft(rect, begin.X - rect.Width / 2.0); Canvas.SetTop(rect, begin.Y);

            return rect;
        }

        private FrameworkElement RenderHorizontalGridLine(Point begin, Point end)
        {
            Rectangle rect = new Rectangle(); rect.Fill = GridLineBackground;
            rect.Width = Math.Abs(end.X - begin.X);
            rect.Height = (GridLineWidth <= 0) ? 1 : GridLineWidth;
            Canvas.SetLeft(rect, begin.X); Canvas.SetTop(rect, begin.Y - rect.Height / 2.0);

            return rect;
        }

        private List<FrameworkElement> RenderNullCell(Rect area)
        {
            Rectangle rect = new Rectangle();
            Canvas.SetLeft(rect, area.Left); Canvas.SetTop(rect, area.Top);
            rect.Width = area.Width; rect.Height = area.Height;
            rect.Fill = NullCellBackground;
            rect.Stroke = NullCellBackground;
            return new List<FrameworkElement> { rect };
        }

        private List<FrameworkElement> RenderCell(MapCellState cell, Rect cellArea)
        {
            List<FrameworkElement> result = new List<FrameworkElement>();
            if (cell == null) return result;            

            result.Add(RenderPlace(cell.Place, cellArea));            
            result.Add(RenderWall(cell.GetWall(MapDirection.South), cellArea));
            result.Add(RenderWall(cell.GetWall(MapDirection.West), cellArea));
            result.Add(RenderWall(cell.GetWall(MapDirection.North), cellArea));
            result.Add(RenderWall(cell.GetWall(MapDirection.East), cellArea));
            result.Add(RenderWallCorner(cell.GetWall(MapDirection.North), cell.GetWall(MapDirection.West), cellArea));
            result.Add(RenderWallCorner(cell.GetWall(MapDirection.North), cell.GetWall(MapDirection.East), cellArea));
            result.Add(RenderWallCorner(cell.GetWall(MapDirection.South), cell.GetWall(MapDirection.West), cellArea));
            result.Add(RenderWallCorner(cell.GetWall(MapDirection.South), cell.GetWall(MapDirection.East), cellArea));

            while (result.Remove(null));

            return result;
        }

        private FrameworkElement RenderPlace(MapPlaceState place, Rect area)
        {
            if (place == null) return null;
            if (place.Place == null) return null;            
            
            MapImageWPF imageSource = new MapImageWPF(place.Image);
            Image image = new Image { Source = imageSource.Image, Stretch = Stretch.Fill };
            Canvas.SetLeft(image, area.Left); Canvas.SetTop(image, area.Top); image.Width = area.Width; image.Height = area.Height;
            return image;
        }

        private FrameworkElement RenderActiveObject(MapActiveObjectState activeObject, Rect area)
        {
            if (activeObject == null) return null;
            if (activeObject.ActiveObject == null) return null;

            MapImageWPF imageSource = new MapImageWPF(activeObject.Image);
            Image image = new Image { Source = imageSource.Image, Stretch = Stretch.Fill };

            double wallWidth = area.Width * WallPadding;
            double wallHeight = area.Height * WallPadding;

            double imageWidth = area.Width - wallWidth * 2;
            double imageHeight = area.Height - wallHeight * 2;

            Canvas.SetLeft(image, area.Left + wallWidth); Canvas.SetTop(image, area.Top + wallHeight); image.Width = imageWidth; image.Height = imageHeight;
            return image;
        }

        private FrameworkElement RenderWallCorner(MapWallState wall1, MapWallState wall2, Rect area)
        {
            if (wall1 == null) return null;
            if (wall1.Wall == null) return null;
            if (wall2 == null) return null;
            if (wall2.Wall == null) return null;

            if (wall1.Wall.Id != wall2.Wall.Id) return null;

            MapImage cornerImage = wall1.Wall.ImageCorner;
            if (cornerImage == null) return null;

            double wallWidth = area.Width * WallPadding;
            double wallHeight = area.Height * WallPadding;            

            MapImageWPF imageSource = new MapImageWPF(cornerImage);
            double imageWidth = wallWidth;
            double imageHeight = wallHeight;

            Image image = new Image { Source = imageSource.Image, Width = imageWidth, Height = imageHeight, Stretch= Stretch.Uniform};
            image.RenderTransformOrigin = new Point(0.5, 0.5);

            if ((wall1.Direction == MapDirection.North) && (wall2.Direction == MapDirection.West))
            {
                Canvas.SetLeft(image, area.Left); Canvas.SetTop(image, area.Top);
            }

            if ((wall1.Direction == MapDirection.North) && (wall2.Direction == MapDirection.East))
            {
                Canvas.SetLeft(image, area.Left + area.Width - imageWidth); Canvas.SetTop(image, area.Top);
                image.RenderTransform = new RotateTransform(90);
            }

            if ((wall1.Direction == MapDirection.South) && (wall2.Direction == MapDirection.West))
            {
                Canvas.SetLeft(image, area.Left); Canvas.SetTop(image, area.Top + area.Height - imageHeight);
                image.RenderTransform = new RotateTransform(-90);
            }

            if ((wall1.Direction == MapDirection.South) && (wall2.Direction == MapDirection.East))
            {
                Canvas.SetLeft(image, area.Left + area.Width - imageWidth); Canvas.SetTop(image, area.Top + area.Height - imageHeight);
                image.RenderTransform = new RotateTransform(180);
            }

            return image;
        }

        private FrameworkElement RenderWall(MapWallState wall, Rect area)
        {
            if (wall == null) return null;
            if (wall.Wall == null) return null;

            MapImageWPF imageSource = new MapImageWPF(wall.CurrentImage);
            Image image = new Image { Source = imageSource.Image };            

            double wallWidth = area.Width * WallPadding;
            double wallHeight = area.Height * WallPadding;

            double imageWidth = area.Width;
            double imageHeight = wallHeight;

            image.Stretch = Stretch.Fill;            


            if (wall.Direction == MapDirection.North)
            {                
                Canvas.SetLeft(image, area.Left); Canvas.SetTop(image, area.Top); image.Width = imageWidth; image.Height = imageHeight;                
            }            

            if (wall.Direction == MapDirection.South)
            {
                Canvas.SetLeft(image, area.Left); Canvas.SetTop(image, area.Top + area.Height); image.Width = imageWidth; image.Height = imageHeight;
                image.RenderTransformOrigin = new Point(0.5, 0);
                image.RenderTransform = new RotateTransform(-180);
            }
                      
            if (wall.Direction == MapDirection.West)
            {
                Canvas.SetLeft(image, area.Left); Canvas.SetTop(image, area.Top + imageWidth); image.Width = imageWidth; image.Height = imageHeight;                
                image.RenderTransformOrigin = new Point(0, 0);
                image.RenderTransform = new RotateTransform(-90);               
            }

            if (wall.Direction == MapDirection.East)
            {
                Canvas.SetLeft(image, area.Left); Canvas.SetTop(image, area.Top + imageWidth); image.Width = imageWidth; image.Height = imageHeight;                
                image.RenderTransformOrigin = new Point(1, 0);
                image.RenderTransform = new RotateTransform(90);       
            }                   

            return image;
        }        
    }
    
}
