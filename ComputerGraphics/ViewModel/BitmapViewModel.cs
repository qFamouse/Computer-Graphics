using ComputerGraphics.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ComputerGraphics.ViewModel
{
    internal class BitmapViewModel : BaseViewModel
    {
        public Canvas Canvas => MainCanvas.GetInstance().Canvas;

        public BitmapViewModel()
        {
            BitmapCommand.Execute(null);
        }

        public RelayCommand BitmapCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {

                    WriteableBitmap wb = new WriteableBitmap(1921, 1081, 96, 96, PixelFormats.Bgr24, null);

                    //Int32Rect rect = new Int32Rect(0, 0, 1, 1);
                    Int32Rect rect2 = new Int32Rect(1, 0, 1, 1);

                    byte blue = 0;
                    byte green = 0;
                    byte red = 255;
                    byte[] colorData =
                    {
                        255, 0, 0,
                        0,   0, 255
                    };

                    //wb.WritePixels(rect, colorData, 3, 0);
                    //wb.WritePixels(rect2, colorData, 3, 3);


                    using (FileStream fs = new FileStream(@"C:\Users\Famouse\Desktop\1280x720.bmp", FileMode.Open,
                               FileAccess.Read))
                    {

                        //int r = fs.ReadByte();
                        //int g = fs.ReadByte();
                        //int b = fs.ReadByte();

                        //byte[] color =
                        //{
                        //    (byte)r, (byte)g, (byte)b
                        //};

                        //wb.WritePixels(rect, color, 3, 0);


                        for (int y = 720; y > 0; y--)
                        {
                            for (int x = 1280; x > 0; x--)
                            {
                                var rect = new Int32Rect(x, y, 1, 1);

                                byte[] rgb = new byte[3];

                                for (int i = 0; i < 3; i++)
                                {
                                    rgb[i] = (byte) fs.ReadByte();
                                }

                                wb.WritePixels(rect, rgb, 3, 0);
                            }
                        }


                        //int counter = 0;

                        //do
                        //{
                        //    b = fs.ReadByte();

                        //    if (counter % 3 == 0)
                        //    {

                        //    }

                        //} while (b != -1);
                    }



                    //var uri = new Uri(@"C:\Users\Famouse\Desktop\sample_640×426.bmp");

                    //byte[] buffer = File.ReadAllBytes(@"C:\Users\Famouse\Desktop\sample_640×426.bmp");

                    //for (int i = 0; i < buffer.Length; i++)
                    //{

                    //}


                    var image = new Image
                    {
                        Source = wb
                    };

                    Canvas.Children.Add(image);
                });
            }
        }
    }
}
