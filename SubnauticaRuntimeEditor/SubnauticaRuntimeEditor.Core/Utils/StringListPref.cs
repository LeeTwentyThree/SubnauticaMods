using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubnauticaRuntimeEditor.Core.Utils
{
    internal class StringListPref : IEnumerable
    {
        public readonly StringArrayPref array;

        public StringListPref(StringArrayPref array)
        {
            this.array = array;
        }

        public static StringListPref Get(string key)
        {
            return new StringListPref(new StringArrayPref(key));
        }

        public void Add(string entry)
        {
            array.SetLength(array.Length + 1);
            array[array.Length - 1] = entry;
        }

        public IEnumerator GetEnumerator()
        {
            yield return array.GetEnumerator();
        }

        public int IndexOf(string entry)
        {
            for (int i = 0; i < entry.Length; i++)
            {
                if (array[i].Equals(entry))
                {
                    return i;
                }
            }
            return -1;
        }

        public bool Contains(string entry) => IndexOf(entry) >= 0;

        public bool Remove(int index)
        {
            if (index < 0 || index >= array.Length)
            {
                return false;
            }
            for (int i = index; i < array.Length; i++)
            {
                array[i - 1] = array[i];
            }
            array.SetLength(array.Length - 1);
            return true;
        }

        public bool Remove(string entry)
        {
            var index = IndexOf(entry);
            if (index < 0) return false;
            return Remove(index);
        }
    }
}
