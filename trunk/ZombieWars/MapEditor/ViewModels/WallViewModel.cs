using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using ZombieWars.Core.Maps;
using ZombieWars.Graphics.WPF;

namespace MapEditor.ViewModels
{
    public class WallViewModel : BaseViewModel
    {
        public MapWall Source { get; private set; }        

        /// <summary>
        /// Режим стены (закрыто, открыто окно, открыта дверь)
        /// </summary>
        [DisplayName("Режим")]        
        public MapWallMode Mode
        {
            get { return (Source != null) ? Source.Mode : default(MapWallMode); }
            set { if (Source != null) { Source.Mode = value; OnPropertyChanged("Mode"); } }
        }

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
        //[DisplayName("Ориентация картинок")]
        //public MapDirection BaseDirection
        //{
        //    get { return (Source != null) ? Source.BaseDirection : default(MapDirection); }
        //    set { if (Source != null) { Source.BaseDirection = value; OnPropertyChanged("BaseDirection"); } }
        //}

        ///// <summary>
        ///// Ориентация угла относительно картинок
        ///// </summary>
        //[DisplayName("Ориентация картинки угла")]
        //public MapDirection CornerDirection
        //{
        //    get { return (Source != null) ? Source.CornerDirection : default(MapDirection); }
        //    set { if (Source != null) { Source.CornerDirection = value; OnPropertyChanged("CornerDirection"); } }
        //}      

        [DisplayName("Картинка (режим стены)")]
        public MapImageWPF ImageWall
        {
            get { return (Source != null) ? new MapImageWPF(Source.ImageWall) : null; }
            set { if (Source != null) { Source.ImageWall = value; OnPropertyChanged("ImageWall"); } }
        }

        [DisplayName("Картинка (режим окна)")]
        public MapImageWPF ImageWindow
        {
            get { return (Source != null) ? new MapImageWPF(Source.ImageWindow) : null; }
            set { if (Source != null) { Source.ImageWindow = value; OnPropertyChanged("ImageWindow"); } }
        }

        [DisplayName("Картинка (режим двери)")]
        public MapImageWPF ImageDoor
        {
            get { return (Source != null) ? new MapImageWPF(Source.ImageDoor) : null; }
            set { if (Source != null) { Source.ImageDoor = value; OnPropertyChanged("ImageDoor"); } }
        }

        [DisplayName("Картинка угла")]
        public MapImageWPF ImageCorner
        {
            get { return (Source != null) ? new MapImageWPF(Source.ImageCorner) : null; }
            set { if (Source != null) { Source.ImageCorner = value; OnPropertyChanged("ImageCorner"); } }
        }

        public WallViewModel()
            : this(new MapWall() { Caption = "Новая стена" })
        {
        }

        public WallViewModel(MapWall wall)
            : base(wall)
        {
            Source = wall;
        }       
    }
}
