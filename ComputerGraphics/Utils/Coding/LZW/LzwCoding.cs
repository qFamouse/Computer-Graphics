using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphics.Utils.Coding.LZW
{
    internal sealed class LzwCoding : ICodable
    {
        private LzwDictionary _lzwDictionary;

        public LzwCoding(LzwDictionary lzwDictionary)
        {
            _lzwDictionary = lzwDictionary ?? throw new ArgumentNullException(nameof(lzwDictionary));
        }
        
        public byte[] Encode(string text)
        {
            ValidateEncode(text);
            var currentSequence = String.Empty;
            LinkedList<ushort> encodedCharacters = new LinkedList<ushort>();

            foreach (char nextChar in text)
            {
                string bufferSequence = currentSequence + nextChar;

                if (_lzwDictionary.ContainsKey(bufferSequence))
                {
                    currentSequence = bufferSequence;
                }
                else
                {
                    _lzwDictionary.Add(bufferSequence, _lzwDictionary.Count);

                    if (_lzwDictionary.TryGetValue(currentSequence, out ushort currentSequenceCode))
                    {
                        encodedCharacters.AddLast(currentSequenceCode);
                        currentSequence = nextChar.ToString();
                    }
                    else
                    {
                        throw new AggregateException($"Can't get value from dictionary (Key: {currentSequence})");
                    }
                }
            }
            // Add the last element 
            if (_lzwDictionary.TryGetValue(currentSequence, out ushort lastElementCode))
            {
                encodedCharacters.AddLast(lastElementCode);
            }

            return encodedCharacters.SelectMany(e => BitConverter.GetBytes(e)).ToArray();
        }

        public string Decode(byte[] encodedCharacters)
        {
            ValidateDecode(encodedCharacters);
            // Get firt sybmol
            ushort previousWordCode = BitConverter.ToUInt16(encodedCharacters, 0);
            _lzwDictionary.TryGetKey(previousWordCode, out string previousWordStr);

            var decodedCharacters = new StringBuilder(previousWordStr);

            for (int i = 2; i < encodedCharacters.Length; i += 2)
            {
                ushort currentWordCode = BitConverter.ToUInt16(encodedCharacters, i);
                _lzwDictionary.TryGetKey(currentWordCode, out string currentWordStr);

                if (_lzwDictionary.ContainsKey(currentWordStr))
                {
                    decodedCharacters.Append(currentWordStr);

                    string bufferSequence = previousWordStr + currentWordStr.Substring(0, 1);
                    if (!_lzwDictionary.ContainsKey(bufferSequence))
                    {
                        _lzwDictionary.Add(bufferSequence, _lzwDictionary.Count);
                    }
                }

                previousWordStr = currentWordStr;
            }

            return decodedCharacters.ToString();
        }

        private void ValidateDecode(in byte[] encodedCharacters)
        {
            if (encodedCharacters is null)
            {
                throw new NullReferenceException("No find incoming data");
            }

            if ((encodedCharacters.Length & 1) == 1)
            {
                throw new ArgumentException("Invalid number of encoded bytes");
            }
        }

        private void ValidateEncode(in string text)
        {
            if (String.IsNullOrEmpty(text))
            {
                throw new ArgumentException("Incoming data cannot be null");
            }
        }
    }
}
