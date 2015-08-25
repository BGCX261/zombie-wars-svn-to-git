using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZombieWars.Core.Maps;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

namespace ZombieWars.Graphics.WPF
{
    public class MapImageWPF : MapImage, IDisposable
    {
        public ImageSource Image
        {
            get { return _Image; }
        }
        private ImageSource _Image;        

        public MapImageWPF(MapImage mapImage)
            : this((mapImage != null) ? mapImage.Type : MapImageType.Null, (mapImage != null) ? mapImage.Data : null)
        {            
        }

        public MapImageWPF(MapImageType type, byte[] data)
            : base(type, data)
        {
            if ((data == null) || (data.Length == 0))
            {                
                _Image = null;
                return;
            }            

            try
            {
                MemoryStream stream = new MemoryStream(Data);
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = stream;
                image.EndInit();
                _Image = image;
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidDataException("Invalid image data", ex);
            }
            catch (NotSupportedException ex)
            {
                throw new InvalidDataException("Invalid image data", ex);
            }
        }

        public static implicit operator ImageSource(MapImageWPF image) { return (image != null) ? image.Image : null; }        

        public void Dispose()
        {
            if (_Image != null)
            {
                BitmapImage bitmap = _Image as BitmapImage;
                if (bitmap != null)
                {
                    if (bitmap.StreamSource != null) bitmap.StreamSource.Dispose();
                    bitmap.StreamSource = null;                    
                }
                _Image = null;
            }
        }
    }
}
