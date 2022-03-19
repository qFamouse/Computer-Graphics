using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphics.Utils.Images.Bitmap
{
    enum CompressionMethod
    {
        BI_RGB = 0,
        BI_RLE8 = 1,
        BI_RLE4 = 2,
        BI_BITFIELDS = 3,
        BI_JPEG = 4,
        BI_PNG = 5,
        BI_ALPHABITFIELDS = 6,
        BI_CMYK = 11,
        BI_CMYKRLE8 = 12,
        BI_CMYKRLE4 = 13
    }

    internal class BitmapInfoHeader
    {
        /// <summary>
        /// Bitmap info header
        /// source: https://en.wikipedia.org/wiki/BMP_file_format
        /// </summary>
        private const int INFO_HEADER_SIZE = 40;
        private const int OFFSET_SIZE = 0; // Absolute 14
        private const int OFFSET_WIDTH = 4; // 18
        private const int OFFSET_HEIGHT = 8; // 22
        private const int OFFSET_PLANES = 12; // 26
        private const int OFFSET_BIT_COUNT = 14; // 28
        private const int OFFSET_COMPRESSION = 16; // 30
        private const int OFFSET_SIZE_IMAGE = 20; // 34
        private const int OFFSET_X_PELS_PER_METER = 24; // 38
        private const int OFFSET_Y_PELS_PER_METER = 28; // 42
        private const int OFFSET_CLR_USED = 32; // 46
        private const int OFFSET_CLR_IMPORTANT = 36; // 50

        /// <summary>
        /// Header data storage
        /// </summary>
        private byte[] _header = new byte[INFO_HEADER_SIZE]
        {
            40, 0, 0, 0, // Size
            0, 0, 0, 0,  // Width
            0, 0, 0, 0,  // Height
            1, 0,        // Planes
            24, 0,       // BitCount
            0, 0, 0, 0,  // Compression
            0, 0, 0, 0,  // SizeImage
            0, 0, 0, 0,  // XPelsPerMeter
            0, 0, 0, 0,  // YPelsPerMeter
            0, 0, 0, 0,  // ClrUsed
            0, 0, 0, 0   // ClrImportant
        };

        /// <summary>
        /// The number of bytes required by the structure. 
        /// Default 40
        /// </summary>
        public uint Size
        {
            get => BitConverter.ToUInt32(_header, OFFSET_SIZE);
            set => BitConverter.GetBytes(value).CopyTo(_header, OFFSET_SIZE);
        }

        /// <summary>
        /// The bitmap width in pixels (signed integer).
        /// Default 0
        /// </summary>
        public int Width
        {
            get => BitConverter.ToInt32(_header, OFFSET_WIDTH);
            set => BitConverter.GetBytes(value).CopyTo(_header, OFFSET_WIDTH);
        }

        /// <summary>
        /// The bitmap height in pixels (signed integer). 
        /// Default 0
        /// </summary>
        public int Height
        {
            get => BitConverter.ToInt32(_header, OFFSET_HEIGHT);
            set => BitConverter.GetBytes(value).CopyTo(_header, OFFSET_HEIGHT);
        }

        /// <summary>
        /// The number of color planes (must be 1).
        /// Default 1
        /// </summary>
        public ushort Planes
        {
            get => BitConverter.ToUInt16(_header, OFFSET_PLANES);
            set => BitConverter.GetBytes(value).CopyTo(_header, OFFSET_PLANES);
        }

        /// <summary>
        /// The number of bits per pixel, which is the color depth of the image. Typical values are 1, 4, 8, 16, 24 and 32.
        /// Default 24
        /// </summary>
        public ushort BitCount
        {
            get => BitConverter.ToUInt16(_header, OFFSET_BIT_COUNT);
            set => BitConverter.GetBytes(value).CopyTo(_header, OFFSET_BIT_COUNT);
        }

        /// <summary>
        /// The type of compression for a compressed bottom-up bitmap (top-down DIBs cannot be compressed).
        /// Default BI_RGB (0)
        /// </summary>
        public CompressionMethod Compression
        {
            get => (CompressionMethod)BitConverter.ToUInt16(_header, OFFSET_COMPRESSION);
            set => BitConverter.GetBytes((int)value).CopyTo(_header, OFFSET_COMPRESSION);
        }

        /// <summary>
        /// The size, in bytes, of the image. This is the size of the raw bitmap data.
        /// Default 0
        /// </summary>
        public uint SizeImage
        {
            get => BitConverter.ToUInt32(_header, OFFSET_SIZE_IMAGE);
            set => BitConverter.GetBytes(value).CopyTo(_header, OFFSET_SIZE_IMAGE);
        }

        /// <summary>
        /// The horizontal resolution of the image. (pixel per metre, signed integer).
        /// Defalut 0
        /// </summary>
        public int XPelsPerMeter
        {
            get => BitConverter.ToInt32(_header, OFFSET_X_PELS_PER_METER);
            set => BitConverter.GetBytes(value).CopyTo(_header, OFFSET_X_PELS_PER_METER);
        }

        /// <summary>
        /// The vertical resolution of the image. 
        /// Defalut 0
        /// </summary>
        public int YPelsPerMeter
        {
            get => BitConverter.ToInt32(_header, OFFSET_Y_PELS_PER_METER);
            set => BitConverter.GetBytes(value).CopyTo(_header, OFFSET_Y_PELS_PER_METER);
        }

        /// <summary>
        /// The number of color indexes in the color table that are actually used by the bitmap.
        /// Default 0
        /// </summary>
        public uint ClrUsed
        {
            get => BitConverter.ToUInt32(_header, OFFSET_CLR_USED);
            set => BitConverter.GetBytes(value).CopyTo(_header, OFFSET_CLR_USED);
        }

        /// <summary>
        /// The number of color indexes that are required for displaying the bitmap.
        /// Defalut 0
        /// </summary>
        public uint ClrImportant
        {
            get => BitConverter.ToUInt32(_header, OFFSET_CLR_IMPORTANT);
            set => BitConverter.GetBytes(value).CopyTo(_header, OFFSET_CLR_IMPORTANT);
        }

        /// <summary>
        /// Get header in byte array. There are no calculations, the data is already stored in an array
        /// </summary>
        /// <returns>Bmp header in byte array</returns>
        public byte[] ToArray()
        {
            return _header;
        }
    }
}