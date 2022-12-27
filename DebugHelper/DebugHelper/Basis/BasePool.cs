using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebugHelper.Basis
{
    public abstract class BasePool<T>
    {
        protected HashSet<T> m_list = new HashSet<T>();

        public abstract T Register(object target);
        public abstract void Unregister(T target);
        public virtual bool Contains(T target) => m_list.Contains(target);
        public abstract bool Contains(object target);
        public abstract int Clear();
        public IEnumerable<T> ToSet() => m_list.ToSet();
    }
}
