using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubnauticaRuntimeEditor.Core.Utils
{
    internal class StringListPref : IEnumerable<string>
    {
        public readonly StringArrayPref array;

        private bool _cleanEmptyEntries;

        public StringListPref(StringArrayPref array, bool allowEmptyEntries)
        {
            this.array = array;
            _cleanEmptyEntries = !allowEmptyEntries;
        }

        public static StringListPref Get(string key, bool allowEmptyEntries)
        {
            return new StringListPref(new StringArrayPref(key), allowEmptyEntries);
        }

        public void Add(string entry)
        {
            array.SetLength(array.Length + 1);
            array[array.Length - 1] = entry;
            if (_cleanEmptyEntries) RemoveUnused();
        }

        public IEnumerator<string> GetEnumerator()
        {
            return array.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Contains(string entry) => array.IndexOf(entry) >= 0;

        public bool Remove(int index)
        {
            int length = array.Length;
            if (index < 0 || index >= length)
            {
                return false;
            }
            for (int i = index; i < length - 1; i++)
            {
                array[i] = array[i + 1];
            }
            array.SetLength(length - 1);
            if (_cleanEmptyEntries) RemoveUnused();
            return true;
        }

        public bool Remove(string entry)
        {
            var index = array.IndexOf(entry);
            if (index < 0) return false;
            return Remove(index);
        }

        private void RemoveUnused()
        {
            while (true)
            {
                int next = array.IndexOf(null);
                if (next < 0) return;
                Remove(next);
            }
        }
    }
}
