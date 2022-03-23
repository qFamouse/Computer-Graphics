using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphics.Utils.Coding
{
    internal interface ICodable
    {
        byte[] Encode(string text);
        string Decode(byte[] encodedCharacters);
    }
}
