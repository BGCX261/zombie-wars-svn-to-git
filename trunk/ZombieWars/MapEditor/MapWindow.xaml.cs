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
using System.Windows.Shapes;
using MapEditor.ViewModels;
using ZombieWars.Core.Maps.MapStorage;
using System.IO;
using ZombieWars.Core.Maps;
using ZombieWars.Core.Game;

namespace MapEditor
{
    /// <summary>
    /// Interaction logic for MapWindow.xaml
    /// </summary>
    public partial class MapWindow : Window
    {
        MapViewModel Map
        {
            get { return _Map; }
            set { _Map = value; DataContext = _Map; }
        }
        MapViewModel _Map;

        private string CurrentFileName;

        public MapWindow()
        {
            InitializeComponent();
            SetCommandBindings();
        }

        #region CommandBindings

        void SetCommandBindings()
        {
            CommandBinding newBinding = new CommandBinding(ApplicationCommands.New);
            newBinding.CanExecute += new CanExecuteRoutedEventHandler(newBinding_CanExecute);
            newBinding.Executed += new ExecutedRoutedEventHandler(newBinding_Executed);
            CommandBindings.Add(newBinding);

            CommandBinding closeBinding = new CommandBinding(ApplicationCommands.Close);
            closeBinding.CanExecute += new CanExecuteRoutedEventHandler(closeBinding_CanExecute);
            closeBinding.Executed += new ExecutedRoutedEventHandler(closeBinding_Executed);
            CommandBindings.Add(closeBinding);

            CommandBinding openBinding = new CommandBinding(ApplicationCommands.Open);
            openBinding.CanExecute += new CanExecuteRoutedEventHandler(openBinding_CanExecute);
            openBinding.Executed += new ExecutedRoutedEventHandler(openBinding_Executed);
            CommandBindings.Add(openBinding);

            CommandBinding saveBinding = new CommandBinding(ApplicationCommands.Save);
            saveBinding.CanExecute += new CanExecuteRoutedEventHandler(saveBinding_CanExecute);
            saveBinding.Executed += new ExecutedRoutedEventHandler(saveBinding_Executed);
            CommandBindings.Add(saveBinding);

            CommandBinding saveAsBinding = new CommandBinding(ApplicationCommands.SaveAs);
            saveAsBinding.CanExecute += new CanExecuteRoutedEventHandler(saveAsBinding_CanExecute);
            saveAsBinding.Executed += new ExecutedRoutedEventHandler(saveAsBinding_Executed);
            CommandBindings.Add(saveAsBinding);

            CommandBinding tilesetBinding = new CommandBinding(ApplicationCommands.CorrectionList);
            tilesetBinding.CanExecute += new CanExecuteRoutedEventHandler(tilesetBinding_CanExecute);
            tilesetBinding.Executed += new ExecutedRoutedEventHandler(tilesetBinding_Executed);
            CommandBindings.Add(tilesetBinding);
        }

        void tilesetBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TileSetWindow window = new TileSetWindow() { TileSet = Map.TileSet, IsMapTileSet = true };
            window.ShowDialog();
        }

        void tilesetBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (Map != null);
        }

        void saveAsBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Save();
        }

        void saveAsBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (Map != null);
        }

        void saveBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Save();
        }

        void saveBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (Map != null);
        }

        void openBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Open();
        }

        void openBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        void closeBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {            
            Map = null;
        }

        void closeBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (Map != null);
        }

        void newBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Map = new MapViewModel(new GameMap(new MapState(new Map(1, new MapSize(10, 10)) { Caption = "Новая карта" })));
        }

        void newBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }


        #endregion CommandBindings

        void Save(string fileName)
        {
            if (Map == null) return;
            if (String.IsNullOrEmpty(fileName)) return;

            Map.Save();

            byte[] data = MapSerializer.Instance.SerializeMapState(Map.Source.Map);
            using (var file = File.OpenWrite(fileName))
            {
                file.Write(data, 0, data.Length);
            }
        }

        void Save(bool saveAs = false)
        {
            if (Map == null) return;

            if (saveAs || String.IsNullOrEmpty(CurrentFileName))
            {
                var dialog = new Microsoft.Win32.SaveFileDialog();
                dialog.Filter = "Maps (*.map)|*.map";
                if (dialog.ShowDialog() == true)
                {
                    CurrentFileName = dialog.FileName;
                    Save(CurrentFileName);
                }
                Save(CurrentFileName);
            }
            else
            {
                Save(CurrentFileName);
            }
        }

        void Open(string fileName)
        {
            if (String.IsNullOrEmpty(fileName)) return;
            CurrentFileName = fileName;

            byte[] data;
            using (var file = File.OpenRead(fileName))
            {
                data = new byte[file.Length];
                file.Read(data, 0, data.Length);
            }
            Map = new MapViewModel(new GameMap(MapSerializer.Instance.DeserializeMapState(data)));
        }

        void Open()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "Maps (*.map)|*.map";
            if (dialog.ShowDialog() == true)
            {
                Open(dialog.FileName);
            }
        }
    }
}
