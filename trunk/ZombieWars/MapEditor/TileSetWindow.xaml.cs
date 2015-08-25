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
using MapEditor.ViewModels;
using ZombieWars.Core.Maps.MapStorage;
using System.IO;

namespace MapEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class TileSetWindow : Window
    {
        public TileSetViewModel TileSet
        {
            get { return _TileSet; }
            set { _TileSet = value; DataContext = _TileSet; }
        }
        TileSetViewModel _TileSet;

        public bool IsMapTileSet { get; set; }

        private string CurrentFileName;

        public TileSetWindow()
        {
            InitializeComponent();
            Closed += new EventHandler(TileSetWindow_Closed);
            
            SetCommandBindings();            
        }

        void TileSetWindow_Closed(object sender, EventArgs e)
        {
            Save();
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
        }        
        
        void saveAsBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Save();
        }

        void saveAsBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !IsMapTileSet && (TileSet != null);
        }

        void saveBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Save();     
        }

        void saveBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !IsMapTileSet && (TileSet != null);
        }

        void openBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Open();    
        }

        void openBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !IsMapTileSet;
        }

        void closeBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (IsMapTileSet) { Close(); return; }
            TileSet = null;
        }

        void closeBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (TileSet != null);
        }
        
        void newBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TileSet = new TileSetViewModel("Новый тайлсет", "Описание нового");
        }

        void newBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !IsMapTileSet;
        }


        #endregion CommandBindings

        void Save(string fileName)
        {
            if (TileSet == null) return;
            if (String.IsNullOrEmpty(fileName)) return;

            TileSet.Save();

            byte[] data = MapSerializer.Instance.SerializeMapTileSet(TileSet.Source);
            using (var file = File.OpenWrite(fileName))
            {
                file.Write(data, 0, data.Length);
            }
        }

        void Save(bool saveAs = false)
        {
            if (TileSet == null) return;
            if (IsMapTileSet)
            {
                TileSet.Save();
                return;
            }


            if (saveAs || String.IsNullOrEmpty(CurrentFileName)) 
            {
                var dialog = new Microsoft.Win32.SaveFileDialog();
                dialog.Filter = "Tile sets (*.tileset)|*.tileset";
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
            TileSet = new TileSetViewModel(MapSerializer.Instance.DeserializeMapTileSet(data));
        }

        void Open()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "Tile sets (*.tileset)|*.tileset";
            if (dialog.ShowDialog() == true)
            {                
                Open(dialog.FileName);
            }
        }
    }
}
