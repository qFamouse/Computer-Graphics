using ComputerGraphics.Core.Configurations;
using ComputerGraphics.Core.Entities;
using ComputerGraphics.UI.Models;
using ComputerGraphics.UI.Models.ColorConverter;
using ComputerGraphics.UI.Utils;
using ComputerGraphics.UI.Utils.Rasterization.Primitives.Ellipse;
using ComputerGraphics.UI.Utils.Rasterization.Primitives.Line;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ComputerGraphics.UI.ViewModel
{
    internal class PrimitivesViewModel : BaseViewModel
    {
        public Canvas Canvas => MainCanvas.GetInstance().Canvas;

        #region ComboBox

        private CollectionView _options = new CollectionView(PrimitivesConfiguration.OptionsList);
        private PrimitiveOption _selectedOption;
        public CollectionView Options => _options;
        public PrimitiveOption SelectedOption
        {
            get { return _selectedOption; }
            set
            {
                _selectedOption = value;
                OnPropertyChanged(nameof(SelectedOption));
            }
        }

        #endregion

        #region Sliders 

        public int φ1 { get; set; } = 0;
        public int φ2 { get; set; } = 360;
        public double Δφ { get; set; } = 1;

        public int Scale { get; set; } = 100;

        public bool RangeValidation()
        {
            if (φ1 < 0 || φ1 > 360 ||
                φ2 < 0 || φ2 > 360 ||
                Δφ <= 0 || Δφ > 360)
            {
                return false;
            }

            if (φ1 > φ2)
            {
                return false;
            }

            if (Δφ > φ2 - φ1)
            {
                return false;
            }

            return true;
        }

        #endregion

        public RelayCommand Draw
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    Canvas.Children.Clear();
                    int dimension = 2 * Scale + 5;
                    int centerDimension = dimension / 2;
                    Canvas.Width = Canvas.Height = dimension;

                    WriteableBitmap writeableBitmap = BitmapFactory.New(dimension, dimension);
                    var image = new Image
                    {
                        Source = writeableBitmap
                    };

                    using (writeableBitmap.GetBitmapContext())
                    {
                        writeableBitmap.SetPixel(dimension - 1, dimension - 1, Colors.Red);

                        int circleR1 = int.MaxValue;
                        int circleR2 = int.MinValue;


                        for (double angle = φ1; angle < φ2; angle += Δφ)
                        {
                            var φ = ((int)angle * Math.PI) / 180; // convert to radian

                            int x0 = (int)(SelectedOption.r1(φ) * Math.Cos(φ) * Scale);
                            int y0 = (int)(SelectedOption.r1(φ) * Math.Sin(φ) * Scale);

                            int x1 = (int)(SelectedOption.r2(φ) * Math.Cos(φ) * Scale);
                            int y1 = (int)(SelectedOption.r2(φ) * Math.Sin(φ) * Scale);

                            circleR1 = Math.Min(circleR1, (int)Math.Sqrt(Math.Pow(x0, 2) + Math.Pow(y0, 2)));
                            circleR2 = Math.Max(circleR2, (int)Math.Sqrt(Math.Pow(x1, 2) + Math.Pow(y1, 2)));

                            LineBresenham.Draw(x0, y0, x1, y1, (x, y) => writeableBitmap.SetPixel(centerDimension + x, centerDimension - y, Colors.Red));
                        }

                        CircleBresenham.Draw(circleR1, centerDimension, centerDimension, (x, y) => writeableBitmap.SetPixel(x, y, Colors.Black));
                        CircleBresenham.Draw(circleR2, centerDimension, centerDimension, (x, y) => writeableBitmap.SetPixel(x, y, Colors.Black));
                    }

                    Canvas.Children.Add(image);
                });
            }
        }

        public PrimitivesViewModel()
        {



            WriteableBitmap writeableBitmap = BitmapFactory.New(400, 400);
            var image = new Image
            {
                Source = writeableBitmap
            };

            var φ1 = Math.PI / 2;
            var φ2 = Math.PI;
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
                    //var r1 = (1 + Math.Cos(2 * φ) * Math.Cos(2 * φ)) / 5;
                    var r1 = PrimitivesConfiguration.OptionsList[0].r1(φ);
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
