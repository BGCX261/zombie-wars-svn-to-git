using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using ZombieWars.Core.Maps;
using ZombieWars.Graphics.WPF;

namespace MapEditor.ViewModels
{
    public class ActiveObjectViewModel : BaseViewModel
    {
        public MapActiveObject Source { get; private set; }        

        /// <summary>
        /// "Здоровье" стены (0...)
        /// </summary>
        [DisplayName("Здоровье")]
        public UInt16 Health
        {
            get { return (Source != null) ? Source.Health : default(UInt16); }
            set { if (Source != null) { Source.Health = value; OnPropertyChanged("Health"); } }
        }

        /// <summary>
        /// Тип брони стены
        /// </summary>
        [DisplayName("Тип брони")]
        public MapArmorType ArmorType
        {
            get { return (Source != null) ? Source.ArmorType : default(MapArmorType); }
            set { if (Source != null) { Source.ArmorType = value; OnPropertyChanged("ArmorType"); } }
        }

        ///// <summary>
        ///// Ориентация картинок
        ///// </summary>
        //[DisplayName("Направление")]
        //public MapDirection Direction
        //{
        //    get { return (Source != null) ? Source.Direction : default(MapDirection); }
        //    set { if (Source != null) { Source.Direction = value; OnPropertyChanged("Direction"); } }
        //}

        [DisplayName("Требуемая проходимость (0..1)")]
        public float Passability
        {
            get { return (Source != null) ? Source.Passability : default(MapPassability); }
            set
            {
                if (Source != null)
                {
                    try
                    {
                        Source.Passability = value;
                        ClearError("Passability");
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        SetError("Passability", ex.Message);
                    }
                    OnPropertyChanged("Passability");
                }
            }
        }       

        [DisplayName("Картинка")]
        public MapImageWPF Image
        {
            get { return (Source != null) ? new MapImageWPF(Source.Image) : null; }
            set { if (Source != null) { Source.Image = value; OnPropertyChanged("Image"); } }
        }

        [DisplayName("Размер по горизонтали (в клетках)")]
        public UInt16 SizeWidth
        {
            get { return (Source != null) ? Source.Tile.Size.Width : (UInt16)1; }
            set 
            { 
                if (Source != null) 
                {
                    ClearError("SizeWidth");
                    try
                    {
                        MapSize mapSize = new MapSize(value, Source.Tile.Size.Height);
                        Source.Tile = new MapTile(Source.Tile.Image, mapSize);
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        SetError("SizeWidth", ex.Message);
                    }
                    OnPropertyChanged("SizeWidth");
                } 
            }
        }

        [DisplayName("Размер по вертикали (в клетках)")]
        public UInt16 SizeHeight
        {
            get { return (Source != null) ? Source.Tile.Size.Height : (UInt16)1; }
            set
            {
                if (Source != null)
                {
                    ClearError("SizeHeight");
                    try
                    {
                        MapSize mapSize = new MapSize(Source.Tile.Size.Width, value);
                        Source.Tile = new MapTile(Source.Tile.Image, mapSize);
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        SetError("SizeHeight", ex.Message);
                    }
                    OnPropertyChanged("SizeHeight");
                }
            }
        }

        public ActiveObjectViewModel()
            : this(new MapActiveObject() { Caption = "Новый объект" })
        {
        }

        public ActiveObjectViewModel(MapActiveObject activeObject)
            : base(activeObject)
        {
            Source = activeObject;
        }        
    }
}
