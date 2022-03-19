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
using System.Xml.Serialization;
using ComputerGraphics.Models;
using ComputerGraphics.Utils.Images.Bitmap;
using Microsoft.Win32;

namespace ComputerGraphics.ViewModel
{
    internal class OpenSaveViewModel : BaseViewModel
    {
        public Canvas Canvas => MainCanvas.GetInstance().Canvas;

        public RelayCommand SaveCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.FileName = "Image"; // Default file name
                    saveFileDialog.DefaultExt = ".bmp"; // Default file extension
                    saveFileDialog.Filter = "Bitmap Image (.bmp)|*.bmp"; // Filter files by extension

                    bool? result = saveFileDialog.ShowDialog();

                    if (result == true)
                    {
                        using (FileStream fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write))
                        {
                            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(
                                (int)Canvas.RenderSize.Width,
                                (int)Canvas.RenderSize.Height,
                                96d, 96d,
                                PixelFormats.Pbgra32);

                            renderTargetBitmap.Render(Canvas);

                            BitmapWriter bitmapWriter = new BitmapWriter(renderTargetBitmap);
                            bitmapWriter.Write(fileStream);
                        }
                    }
                });
            }
        }

        public RelayCommand OpenCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    byte[] imageBytes;

                    if (openFileDialog.ShowDialog() == true)
                    {
                        using (FileStream fs = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
                        {
                            imageBytes = new byte[fs.Length];

                            for (int i = 0; i < imageBytes.Length; i++)
                            {
                                imageBytes[i] = (byte) fs.ReadByte();
                            }
                        }



  

                        var width = BitConverter.ToInt32(new []{ imageBytes[18], imageBytes[19], imageBytes[20], imageBytes[21] }, 0);
                        var height = BitConverter.ToInt32(new []{ imageBytes[22], imageBytes[23], imageBytes[24], imageBytes[25] }, 0);
                        var pixelOffset = BitConverter.ToInt32(new[] { imageBytes[10], imageBytes[11], imageBytes[12], imageBytes[13] }, 0);

                        //pixelOffset += 1;
                        WriteableBitmap writeableBitmap =
                            new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgr24, null); // TODO: We need define PixelFormat for pixelOffset += 4; (we skip 1 byte, because get 32byte

                        for (int y = 0; y < height; y++)
                        {
                            for (int x = 0; x < width; x++)
                            {
                                var rect = new Int32Rect(x, y, 1, 1);
                                writeableBitmap.WritePixels(rect, imageBytes, 3, pixelOffset);
                                pixelOffset += 4;
                            }
                        }

                        //for (int y = height - 1; y > 0; y--)
                        //{
                        //    for (int x = width - 1; x > 0; x--)
                        //    {
                        //        var rect = new Int32Rect(x, y, 1, 1);
                        //        writeableBitmap.WritePixels(rect, imageBytes, 3, pixelOffset);
                        //        pixelOffset += 3;
                        //    }
                        //}

                        var image = new Image
                        {
                            Source = writeableBitmap
                        };

                        Canvas.Children.Add(image);

                        //for (int y = 720; y > 0; y--)
                        //{
                        //    for (int x = 1280; x > 0; x--)
                        //    {
                        //        var rect = new Int32Rect(x, y, 1, 1);

                        //        byte[] rgb = new byte[3];

                        //        for (int i = 0; i < 3; i++)
                        //        {
                        //            rgb[i] = (byte)fs.ReadByte();
                        //        }

                        //        wb.WritePixels(rect, rgb, 3, 0);
                        //    }
                        //}
                    }



                });
            }
        }

        private BitmapEncoder DefineEncoder(string extension)
        {
            switch (extension.ToLower())
            {
                case ".bmp":
                    return new BmpBitmapEncoder();
                default:
                    throw new ArgumentOutOfRangeException(extension, "Unsupported extension");
            }
        }
    }
}