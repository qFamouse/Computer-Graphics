using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphics.UI.Utils.Coding
{
    internal class Coding
    {
        public ICodable CodingAlgorithm { get; set; }

        public Coding(ICodable codingAlgorithm)
        {
            CodingAlgorithm = codingAlgorithm;
        }

        public byte[] Encode(string text)
        {
            return CodingAlgorithm.Encode(text);
        }
        public string Decode(byte[] encodedCharacters)
        {
            return CodingAlgorithm.Decode(encodedCharacters);
        }
    }
}
