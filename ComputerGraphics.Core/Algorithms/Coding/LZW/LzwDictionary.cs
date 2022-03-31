using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphics.UI.Utils.Coding.LZW
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
                " ",
                // English //
                "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z",
                "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
                // Russian //
                "а", "б", "в", "г", "д", "е", "ё", "ж", "з", "и", "й", "к", "л", "м", "н", "о", "п", "р", "с", "т", "у", "ф", "х", "ц", "ч", "ш", "щ", "ъ", "ы", "ь", "э", "ю", "я",
                "А", "Б", "В", "Г", "Д", "Е", "Ё", "Ж", "З", "И", "Й", "К", "Л", "М", "Н", "О", "П", "Р", "С", "Т", "У", "Ф", "Х", "Ц", "Ч", "Ш", "Щ", "Ъ", "Ы", "Ь", "Э", "Ю", "Я",
                // Spesial //
                "!", "@", "#", "$", "\"", "№", ";", ":", "?", "*", "(", ")", "_", "-", "+", "=", "{", "}", "[", "]", "'", "/", "\\", ",", ".",
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
