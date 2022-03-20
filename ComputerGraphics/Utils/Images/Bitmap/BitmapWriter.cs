using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ComputerGraphics.Utils.Images.Bitmap
{
    internal class BitmapWriter
    {
        private WriteableBitmap _writeableBitmap;
        public int Width{ get; }
        public int Height { get; }

        public BitmapWriter(BitmapSource bitmapSource)
        {
            _writeableBitmap = BitmapFactory.ConvertToPbgra32Format(bitmapSource);

            Width = (int)_writeableBitmap.Width;
            Height = (int)_writeableBitmap.Height;
        }

        public void Write(Stream stream)
        {
            CalculateSize(out uint fileSize, out uint imageSize);

            var bitmapFileHeader = new BitmapFileHeader()
            {
                Size = fileSize
            };

            var bitmapInfoHeader = new BitmapInfoHeader()
            {
                Width = Width,
                Height = Height,
                SizeImage = imageSize
            };

            stream.Write(bitmapFileHeader.ToArray(), 0, BitmapFileHeader.HeaderSize);
            stream.Write(bitmapInfoHeader.ToArray(), 0, BitmapInfoHeader.HeaderSize);

            int garbageWidth = Width % 4;
            for (var y = Height - 1; y >= 0; y--)
            {
                for (var x = 0; x < Width; x++)
                {
                    Color color = _writeableBitmap.GetPixel(x, y);

                    stream.WriteByte(color.B);
                    stream.WriteByte(color.G);
                    stream.WriteByte(color.R);
                }

                stream.Write(new byte[garbageWidth], 0, garbageWidth); // Write garbage bytes
            }
        }

        private bool CalculateSize(out uint fileSize, out uint imageSize)
        {
            int colorsCount = 3;
            // image size //
            uint actualSize = (uint)(Width * Height * colorsCount); // Size without garbage
            uint garbageSize = (uint)(Width % 4 * Height);
            imageSize = actualSize + garbageSize;

            // file size //
            uint headerSize = (uint)(BitmapInfoHeader.HeaderSize + BitmapFileHeader.HeaderSize);
            fileSize = imageSize + headerSize;
            return true;
        }
    }
}
