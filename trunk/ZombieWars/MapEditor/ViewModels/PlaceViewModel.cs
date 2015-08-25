using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZombieWars.Core.Maps;
using System.ComponentModel;
using System.Windows.Media;
using ZombieWars.Graphics.WPF;

namespace MapEditor.ViewModels
{
    public class PlaceViewModel : BaseViewModel
    {
        public MapPlace Source { get; private set; }

        [DisplayName("Проходимость (0..1)")]
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

        public PlaceViewModel()
            : this(new MapPlace() { Caption = "Новая поверхность" })
        {
        }

        public PlaceViewModel(MapPlace place)
            : base(place)
        {
            Source = place;
        }       
    }
}
