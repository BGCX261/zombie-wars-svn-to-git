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
using System.Reflection;
using ZombieWars.Core.Maps;
using System.ComponentModel;
using ZombieWars.Graphics.WPF;
using System.IO;

namespace MapEditor
{
    /// <summary>
    /// Interaction logic for PropertyEditor.xaml
    /// </summary>
    public partial class PropertyEditor : UserControl
    {
        public static DependencyProperty TargetObjectProperty = DependencyProperty.Register("TargetObject", typeof(object), typeof(PropertyEditor),
            new PropertyMetadata(new PropertyChangedCallback((s, e) => { if (s is PropertyEditor) (s as PropertyEditor).OnTargetObjectChanged(e.NewValue); })));                
        
        public object TargetObject
        {
            get { return GetValue(TargetObjectProperty); }
            set { SetValue(TargetObjectProperty, value); }            
        }

        public bool IsValid
        {
            get
            {
                foreach (FrameworkElement control in layoutRoot.Children)
                {
                    if (Validation.GetHasError(control)) return false;
                }
                return true;
            }
        }

        public void OnTargetObjectChanged(object newValue)
        {
            CreateLayout(layoutRoot, newValue);
        }        

        public PropertyEditor()
        {
            InitializeComponent();
        }
   
        private static void AddRow(Grid grid, FrameworkElement headerControl, FrameworkElement valueControl)
        {
            if (grid == null) return;
            if (headerControl == null) return;
            if (valueControl == null) return;
            int currentRow = grid.RowDefinitions.Count;
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                        
            Grid.SetRow(headerControl, currentRow);
            Grid.SetColumn(headerControl, 0);
            grid.Children.Add(headerControl);
                                   
            Grid.SetRow(valueControl, currentRow);
            Grid.SetColumn(valueControl, 1);
            grid.Children.Add(valueControl);            
        }

        private static void ClearRows(Grid grid)
        {
            if (grid == null) return;
            grid.Children.Clear();
            grid.RowDefinitions.Clear();
        }

        private static void CreateLayout(Grid grid, object targetObject)
        {
            if (grid == null) return;
            ClearRows(grid);
            if (targetObject == null) return;
            Type type = targetObject.GetType();
            foreach (PropertyInfo property in type.GetProperties())
            {
                if (property == null) continue;
                if (!property.CanRead) continue;
                if (!property.CanWrite) continue;
                if (property.GetGetMethod() == null) continue;
                if (property.GetSetMethod() == null) continue;
                AddRow(grid, CreateHeaderControl(property), CreateValueControl(property, targetObject));
            }
        }

        private static FrameworkElement CreateHeaderControl(PropertyInfo property)
        {
            if (property == null) return null;

            DisplayNameAttribute displayNameAttribute = property.GetCustomAttributes(typeof(DisplayNameAttribute), true).FirstOrDefault() as DisplayNameAttribute;
            if (displayNameAttribute == null) return null;
            string displayText = displayNameAttribute.DisplayName;
            TextBlock control = new TextBlock { Text = displayText, Margin = new Thickness(5), TextWrapping = TextWrapping.Wrap, TextTrimming = TextTrimming.CharacterEllipsis };
            control.VerticalAlignment = VerticalAlignment.Center;
            return control;
        }

        private static FrameworkElement CreateValueControl(PropertyInfo property, object targetObject)
        {
            if (property == null) return null;
            if (targetObject == null) return null;

            Type type = property.PropertyType;
            if (type == null) return null;
            object val = property.GetValue(targetObject, null);            

            FrameworkElement result = new FrameworkElement();

            if (type.IsEnum)
            {
                ComboBox comboBox = new ComboBox();
                comboBox.ItemsSource = type.GetEnumValues();
                Binding itemBinding = new Binding(property.Name) { Mode = BindingMode.TwoWay, Source = targetObject, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
                comboBox.SetBinding(ComboBox.SelectedItemProperty, itemBinding);
                result = comboBox;
            }
            else
            {
                if (type.Equals(typeof(MapImageWPF)) || type.IsSubclassOf(typeof(MapImageWPF)))
                {
                    ImageSource imageSource = (val as MapImageWPF).Image;
                    Image image = new Image();                    
                    image.Source = imageSource ?? new BitmapImage(new Uri("Images/no_image.gif", UriKind.Relative));
                    Viewbox box = new Viewbox();
                    box.Child = image;
                    box.HorizontalAlignment = HorizontalAlignment.Center;
                    box.VerticalAlignment = VerticalAlignment.Center;
                    box.Width = 100;
                    box.Height = 100;
                    image.MouseLeftButtonUp += (s, e) =>
                    {                        
                        var dialog = new Microsoft.Win32.OpenFileDialog() { CheckFileExists = true, CheckPathExists = true };
                        if (dialog.ShowDialog() == true)
                        {
                            string fileName = dialog.FileName;                                
                            byte[] data;
                            using (var file = File.OpenRead(fileName))
                            {
                                data = new byte[file.Length];
                                file.Read(data, 0, data.Length);
                            }
                            MapImageWPF newImage = new MapImageWPF(MapImageType.Custom, data);
                            image.Source = newImage.Image ?? new BitmapImage(new Uri("Images/no_image.gif", UriKind.Relative));
                            property.SetValue(targetObject, newImage, null);
                        }
                    };
                    result = box;
                }
                else
                {
                    if (type.Equals(typeof(bool)))
                    {
                        CheckBox checkBox = new CheckBox();
                        Binding boolBinding = new Binding(property.Name) { Mode = BindingMode.TwoWay, Source = targetObject, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, ValidatesOnDataErrors = true };
                        checkBox.SetBinding(CheckBox.IsCheckedProperty, boolBinding);
                        result = checkBox;
                    }
                    else
                    {
                        TextBox textBox = new TextBox();
                        Binding textBinding = new Binding(property.Name) { Mode = BindingMode.TwoWay, Source = targetObject, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, ValidatesOnDataErrors = true };
                        textBox.SetBinding(TextBox.TextProperty, textBinding);
                        result = textBox;
                    }
                }
            }                       

            if (result != null)
            {
                result.Margin = new Thickness(5);
            }

            return result;
        }        
    }
}
