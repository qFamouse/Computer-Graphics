using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ComputerGraphics.Core.Extensions
{
    public static class ImageExtensions
    {
        public static Image FromUri(this Image image, Uri uri)
        {
            image.Source = new BitmapImage(uri);
            return image;
        }
    }
}
