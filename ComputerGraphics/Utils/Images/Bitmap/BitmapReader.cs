using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
namespace ComputerGraphics.Utils.Images.Bitmap
{
    internal sealed class BitmapReader
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Image Image { get; private set; }

        public BitmapReader(Stream stream) => Read(stream);

        public bool Read(Stream stream)
        {
            BitmapFileHeader bitmapFileHeader = ReadFileHeader(stream);
            BitmapInfoHeader bitmapInfoHeader = ReadInfoHeader(stream);

            Width = bitmapInfoHeader.Width;
            Height = bitmapInfoHeader.Height;

            WriteableBitmap writeableBitmap = new WriteableBitmap(Width, Height, 96, 96, PixelFormats.Bgr24, null);

            stream.Seek(bitmapFileHeader.OffsetBits, SeekOrigin.Begin);
            int garbageWidth = Width % 4;

            for (var y = Height - 1; y >= 0; y--)
            {
                for (var x = 0; x < Width; x++)
                {
                    var pixels = new byte[3];
                    stream.Read(pixels, 0, 3);

                    writeableBitmap.WritePixels(new Int32Rect(x, y, 1, 1), pixels, 3, 0);
                }

                stream.Read(new byte[garbageWidth], 0, garbageWidth); // Read garbage bytes
            }

            Image = new Image { Source = writeableBitmap };

            return true;
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
