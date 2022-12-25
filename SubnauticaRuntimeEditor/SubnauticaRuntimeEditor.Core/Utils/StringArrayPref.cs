using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SubnauticaRuntimeEditor.Core.Utils
{
    internal class StringArrayPref : IEnumerable<string>
    {
        public string Key { get; private set; }

        private bool doCaching;

        private string[] cache;

        public StringArrayPref(string key, bool cacheValues = true)
        {
            Key = key;
            doCaching = cacheValues;
            if (doCaching)
            {
                RefreshCache(Length);
            }
        }

        public int Length
        {
            get
            {
                if (doCaching && cache != null)
                {
                    return cache.Length;
                }
                else
                    return PlayerPrefs.GetInt(GetLengthKey(), 0);
            }
        }

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
            for (int i = newLength; i < oldLength; i++)
            {
                PlayerPrefs.DeleteKey(KeyAtIndex(i));
            }
            RefreshCache(newLength);
        }

        private void RefreshCache(int newLength)
        {
            cache = new string[newLength];
            for (int i = 0; i < cache.Length; i++)
            {
                cache[i] = Load(i);
            }
        }

        public void Clear()
        {
            SetLength(0);
        }

        public int IndexOf(string entry)
        {
            var length = Length;
            for (int i = 0; i < length; i++)
            {
                var current = this[i];
                if (string.IsNullOrEmpty(entry) || string.IsNullOrEmpty(current))
                {
                    if (string.IsNullOrEmpty(current) && string.IsNullOrEmpty(entry))
                    {
                        return i;
                    }
                }
                else if (current.Equals(entry))
                {
                    return i;
                }
            }
            return -1;
        }

        public bool IndexInRange(int index)
        {
            return index >= 0 && index < Length;
        }

        public string this[int index]
        {
            get
            {
                if (doCaching)
                {
                    if (IndexInRange(index)) return cache[index];
                    return null;
                }
                return Load(index);
            }
            set
            {
                if (doCaching) cache[index] = value;
                Save(index, value);
            }
        }

        public string Load(int index) => PlayerPrefs.GetString(KeyAtIndex(index), null);

        public void Save(int index, string value) => PlayerPrefs.SetString(KeyAtIndex(index), value);
    }
}
