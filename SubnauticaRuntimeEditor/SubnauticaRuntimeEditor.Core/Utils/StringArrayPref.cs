using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SubnauticaRuntimeEditor.Core.Utils
{
    internal class StringArrayPref : IEnumerable<string>
    {
        public string Key { get; private set; }

        public StringArrayPref(string key)
        {
            Key = key;
        }

        public int Length => PlayerPrefs.GetInt(GetLengthKey(), 0);

        private const string kLengthPropertyName = "_$LENGTH";

        private string GetLengthKey() => Key + kLengthPropertyName;

        private string KeyAtIndex(int index) => Key + index;

        public IEnumerator<string> GetEnumerator()
        {
            for (int i = 0; i < Length; i++)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void SetLength(int newLength)
        {
            int oldLength = Length;
            PlayerPrefs.SetInt(GetLengthKey(), newLength);
            for (int i = newLength - 1; i < oldLength; i++)
            {
                PlayerPrefs.DeleteKey(KeyAtIndex(i));
            }
        }

        public string this[int index]
        {
            get => PlayerPrefs.GetString(KeyAtIndex(index));
            set => PlayerPrefs.SetString(KeyAtIndex(index), value);
        }
    }
}
