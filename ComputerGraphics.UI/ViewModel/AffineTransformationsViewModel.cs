using ComputerGraphics.UI.Models;
using ComputerGraphics.UI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ComputerGraphics.UI.ViewModel
{
    internal class AffineTransformationsViewModel : BaseViewModel
    {
        public Canvas Canvas => MainCanvas.GetInstance().Canvas;
        public WriteableBitmap ImageContent { get; private set; }

        private Image NewImage()
        {
            Canvas.Children.Clear();

            ImageContent = BitmapFactory.New((int)Canvas.Width, (int)Canvas.Height);

            Image image = new Image()
            {
                Source = ImageContent,
                Width = Canvas.Width,
                Height = Canvas.Height
            };

            Canvas.Children.Add(image);

            return image;
        }

        public AffineTransformationsViewModel()
        {

        }
    }
}
