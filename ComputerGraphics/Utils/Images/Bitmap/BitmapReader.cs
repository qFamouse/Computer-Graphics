using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ComputerGraphics.Utils.Images.Bitmap
{
    internal sealed class BitmapReader
    {
        public static BitmapSource Read(Stream stream)
        {
            BitmapFileHeader bitmapFileHeader = ReadFileHeader(stream);
            BitmapInfoHeader bitmapInfoHeader = ReadInfoHeader(stream);

            WriteableBitmap writeableBitmap = new WriteableBitmap(
                bitmapInfoHeader.Width, bitmapInfoHeader.Height,
                96, 96, PixelFormats.Bgr24, null);

            stream.Seek(bitmapFileHeader.OffsetBits, SeekOrigin.Begin);
            int garbageWidth = bitmapInfoHeader.Width % 4;

            for (var y = bitmapInfoHeader.Height - 1; y >= 0; y--)
            {
                for (var x = 0; x < bitmapInfoHeader.Width; x++)
                {
                    var pixels = new byte[3];
                    stream.Read(pixels, 0, 3);

                    writeableBitmap.WritePixels(new Int32Rect(x, y, 1, 1), pixels, 3, 0);
                }

                stream.Read(new byte[garbageWidth], 0, garbageWidth); // Read garbage bytes
            }

            return writeableBitmap;
        }

        public static BitmapFileHeader ReadFileHeader(Stream stream)
        {
            byte[] header = new byte[BitmapFileHeader.HeaderSize];
            stream.Read(header, 0, BitmapFileHeader.HeaderSize);
            stream.Seek(0, SeekOrigin.Begin);
            return new BitmapFileHeader(header);
        }

        public static BitmapInfoHeader ReadInfoHeader(Stream stream)
        {
            byte[] header = new byte[BitmapInfoHeader.HeaderSize];
            stream.Seek(BitmapFileHeader.HeaderSize, SeekOrigin.Begin);
            stream.Read(header, 0, BitmapInfoHeader.HeaderSize);
            stream.Seek(0, SeekOrigin.Begin);
            return new BitmapInfoHeader(header);
        }
    }
}
