using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphics.Utils.Coding.LZW
{
    internal sealed class LzwDictionary
    {
        private Dictionary<string, ushort> _encodeDictionary = new Dictionary<string, ushort>();
        private Dictionary<ushort, string> _decodeDictionary = new Dictionary<ushort, string>();

        public ushort Count => (ushort)_encodeDictionary.Count;

        private void LoadDictionary(IList<string> dictionary)
        {
            for (int value = 0; value < dictionary.Count; value++)
            {
                Add(dictionary[value], (ushort)value);
            }
        }

        public LzwDictionary(IList<string> dictionary) => LoadDictionary(dictionary);

        public LzwDictionary()
        {
            List<string> dictionary = new List<string>()
            {
                "#", "a", "b", "w"
            };

            LoadDictionary(dictionary);
        }

        public bool ContainsKey(string key) => _encodeDictionary.ContainsKey(key);
        public bool ContainsValue(ushort value) => _encodeDictionary.ContainsValue(value);
        public void Add(string key, ushort value)
        {
            _encodeDictionary.Add(key, value);
            _decodeDictionary.Add(value, key);

            /* .NET Standart VERSION */

            //bool encode = _encodeDictionary.TryAdd(key, value);
            //bool decode = _decodeDictionary.TryAdd(value, key);

            //if (encode ^ decode) // 1 0 or 0 1
            //{
            //    throw new AggregateException($"The consistency is broken when adding a new element (encode: {encode}, decode: {decode})");
            //}

            //return encode == decode;
        }
        public bool TryGetValue(string key, out ushort value) => _encodeDictionary.TryGetValue(key, out value);
        public bool TryGetKey(ushort value, out string key) => _decodeDictionary.TryGetValue(value, out key);
    }
}
