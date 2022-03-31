using ComputerGraphics.UI.Models;
using ComputerGraphics.UI.Utils;
using ComputerGraphics.UI.Utils.Rasterization.Primitives.Ellipse;
using ComputerGraphics.UI.Utils.Rasterization.Primitives.Line;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ComputerGraphics.UI.ViewModel
{
    internal class PrimitivesViewModel : BaseViewModel
    {
        private CollectionView _images;

        public CollectionView Images => _images;

        public Canvas Canvas => MainCanvas.GetInstance().Canvas;

        public PrimitivesViewModel()
        {
            IList<Image> list = new List<Image>();

            for (int i = 0; i< 10; i++)
            {
                var a = new Image();
                a.Source = new BitmapImage(new Uri(@"C:\Users\Famouse\Desktop\browser_prIXApGUJf.png"));
                list.Add(a);
            }

            _images = new CollectionView(list);

            WriteableBitmap writeableBitmap = BitmapFactory.New(400, 400);
            var image = new Image
            {
                Source = writeableBitmap
            };

            var φ1 = 0;
            var φ2 = 360;
            var Δφ = 0.005;

            int xcenter = (int) Canvas.Width / 2;
            int ycenter = (int) Canvas.Height / 2;

            var lol = 100;

            using (writeableBitmap.GetBitmapContext())
            {
                writeableBitmap.SetPixel(200, 200, Colors.Red);

                //var a = CircleBresenham.GetPixels((200, 200), 100);

                //CircleBresenham.Draw(100, 200, 200, (x, y) => writeableBitmap.SetPixel(x, y, Colors.Black));

                //a.ToList().ForEach(p => writeableBitmap.SetPixel(p.x, p.y, Colors.Black));

                for (double φ = φ1; φ < φ2; φ += Δφ)
                {
                    //var r1 = (1 + Math.Pow(Math.Cos(2 * φ), 2)) / 5;
                    var r1 = (1 + Math.Cos(2 * φ) * Math.Cos(2 * φ)) / 5;
                    r1 *= lol;
                    var start = (r1 * Math.Cos(φ), r1 * Math.Sin(φ));

                    //writeableBitmap.SetPixel((int)(xcenter + start.Item1), (int)(ycenter - start.Item2), Colors.Green);

                    //var r2 = (1 + Math.Pow(Math.Sin(3 * φ), 2)) / 2;
                    var r2 = (1 + Math.Sin(3 * φ) * Math.Sin(3 * φ)) / 2;

                    r2 *= lol;
                    var end = (r2 * Math.Cos(φ), r2 * Math.Sin(φ));

                    //writeableBitmap.SetPixel((int)(xcenter + end.Item1), (int)(ycenter - end.Item2), Colors.Blue);

                    //var pixels = LineBresenham.GetPixels(start, end);
                    //pixels.ToList().ForEach(p => writeableBitmap.SetPixel(xcenter + p.x, ycenter - p.y, Colors.Black));



                    LineBresenham.Draw((int)start.Item1, (int)start.Item2, (int)end.Item1, (int)end.Item2, (x, y) => writeableBitmap.SetPixel(xcenter + x, ycenter - y, Colors.Black));


                }


                //var pixels = BresenhamsLineAlgorithm.GetPixels((0, 0), (100, 30));

                //pixels.ToList().ForEach(p => writeableBitmap.SetPixel(p.x, p.y, Colors.Black));
            }

            Canvas.Children.Add(image);

        }
    }
}
