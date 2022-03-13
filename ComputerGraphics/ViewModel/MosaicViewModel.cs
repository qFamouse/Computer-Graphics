using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ColorPicker;
using ComputerGraphics.Models;

namespace ComputerGraphics.ViewModel
{
    internal class MosaicViewModel : BaseViewModel
    {
        public Canvas Canvas => MainCanvas.GetInstance().Canvas;

        public double Width
        {
            get => Canvas.Width;
            set => Canvas.Width = value;
        }

        public double Height
        {
            get => Canvas.Height;
            set => Canvas.Height = value;
        }

        public int BlockSize { get; set; } = 2;

        public ObservableCollection<int> BlockSizes { get; set; } = new ObservableCollection<int>() {2, 4, 8};


        public List<Color> MosaicColors { get; set; }

        public MosaicViewModel()
        {

            MosaicColors = new List<Color>()
            {
                Colors.DarkGreen,
                Colors.DarkOrange,
                Colors.DarkRed,
                Colors.DarkKhaki
            };
        }

        public RelayCommand DrawCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    Canvas.Children.Clear();

                    // Save colors to array (for the most performance)
                    var colors = new SolidColorBrush[MosaicColors.Count];
                    for (int i = 0; i < colors.Length; i++)
                    {
                        colors[i] = new SolidColorBrush(MosaicColors[i]);
                    }

                    // Drawing
                    Random random = new Random();
                    for (int i = 0; i < Height; i += BlockSize)
                    {
                        for (int j = 0; j < Width; j += BlockSize)
                        {
                            var rect = new Rectangle()
                            {
                                Width = BlockSize,
                                Height = BlockSize,
                                Fill = colors[random.Next(0, colors.Length)]
                            };

                            Canvas.SetTop(rect, i);
                            Canvas.SetLeft(rect, j);

                            Canvas.Children.Add(rect);
                            
                        }
                    }
                });
            }
        }

        public RelayCommand OptimizedDrawCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    Canvas.Children.Clear();

                    WriteableBitmap writeableBitmap = BitmapFactory.New((int)Width, (int)Height);
                    var image = new Image
                    {
                        Source = writeableBitmap
                    };

                    using (writeableBitmap.GetBitmapContext())
                    {
                        Random random = new Random();
                        for (int y = 0; y < Height; y += BlockSize)
                        {
                            for (int x = 0; x < Width; x += BlockSize)
                            {
                                var color = MosaicColors[random.Next(0, MosaicColors.Count)];
                                writeableBitmap.FillRectangle(x, y, x + BlockSize, y + BlockSize, color);
                            }
                        }
                    }

                    Canvas.Children.Add(image);



                    // Stride = (width) x (bytes per pixel)
                    //int stride = (int)writeableBitmap.PixelWidth * (writeableBitmap.Format.BitsPerPixel / 8);
                    //byte[] pixels = writeableBitmap.ToByteArray();

                    ////writeableBitmap.CopyPixels(pixels, stride, 0);


                    //using (FileStream fs = new FileStream(@"C:\Users\Famouse\Desktop\aboba.bmp", FileMode.Create, FileAccess.ReadWrite))
                    //{


                    //    //fs.WriteByte(0x42);
                    //    fs.WriteByte(0x4D);
                    //    for (int i = 0; i < pixels.Length; i++)
                    //    {
                    //        fs.WriteByte(pixels[i]);
                    //    }
                    //}


                    MemoryStream memoryStream = new MemoryStream();
                    BitmapEncoder encoder = new BmpBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(writeableBitmap));
                    encoder.Save(memoryStream);
                    byte[] bytes = memoryStream.ToArray();



                    using (FileStream fs = new FileStream(@"C:\Users\Famouse\Desktop\lol.bmp", FileMode.Create,
                               FileAccess.ReadWrite))
                    {
                        foreach (var t in bytes)
                        {
                            fs.WriteByte(t);
                        }
                    }

                });
            }
        }
    }
}
