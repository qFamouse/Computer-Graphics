using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphics.Utils.Images.Bitmap
{
    internal sealed class BitmapFileHeader
    {
        public static ushort HeaderSize => FILE_HEADER_SIZE;
        /// <summary>
        /// Bitmap file header
        /// source: https://en.wikipedia.org/wiki/BMP_file_format
        /// </summary>
        private const ushort FILE_HEADER_SIZE = 14;
        private const int OFFSET_TYPE = 0;
        private const int OFFSET_SIZE = 2;
        private const int OFFSET_RESERVED1 = 6;
        private const int OFFSET_RESERVED2 = 8;
        private const int OFFSET_BITS = 10;

        /// <summary>
        /// Header data storage
        /// </summary>
        private byte[] _header;

        public BitmapFileHeader(byte[] header)
        {
            if (header.Length != FILE_HEADER_SIZE)
            {
                throw new ArgumentException("Invalid header size", nameof(header));
            }

            _header = (byte[])header.Clone();
        }

        public BitmapFileHeader()
        {
            _header = new byte[FILE_HEADER_SIZE]
            {
                0x42, 0x4D,
                54, 0, 0, 0,
                0, 0,
                0, 0,
                54, 0, 0, 0
            };
        }

        /// <summary>
        /// The file type. 
        /// Default '0x42 0x4D' (BM)
        /// </summary>
        public byte[] Type
        {
            get => new byte[] { _header[0], _header[1] };
            set
            {
                if (value.Length != 2)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Number of bytes should be two");
                }

                value.CopyTo(_header, OFFSET_TYPE);
            }
        }

        /// <summary>
        /// The size of the BMP file in bytes.
        /// Default 54
        /// </summary>
        public uint Size
        {
            get => BitConverter.ToUInt32(_header, OFFSET_SIZE);
            set => BitConverter.GetBytes(value).CopyTo(_header, OFFSET_SIZE);
        }

        /// <summary>
        /// Reserved.
        /// Default 0
        /// </summary>
        public ushort Reserved1
        {
            get => BitConverter.ToUInt16(_header, OFFSET_RESERVED1);
            set => BitConverter.GetBytes(value).CopyTo(_header, OFFSET_RESERVED1);
        }

        /// <summary>
        /// Reserved. 
        /// Default 0
        /// </summary>
        public ushort Reserved2
        {
            get => BitConverter.ToUInt16(_header, OFFSET_RESERVED2);
            set => BitConverter.GetBytes(value).CopyTo(_header, OFFSET_RESERVED2);
        }

        /// <summary>
        /// The offset, i.e. starting address, of the byte where the bitmap image data (pixel array) can be found.
        /// Default 54
        /// </summary>
        public uint OffsetBits
        {
            get => BitConverter.ToUInt32(_header, OFFSET_BITS);
            set => BitConverter.GetBytes(value).CopyTo(_header, OFFSET_BITS);
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