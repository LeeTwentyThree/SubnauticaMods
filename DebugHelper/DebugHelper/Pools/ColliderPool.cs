using DebugHelper.Basis;
using DebugHelper.Objects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DebugHelper.Pools
{
    public sealed class ColliderPool : BasePool<BaseDebugCollider>
    {
        public override BaseDebugCollider Register(object target)
        {
            Collider collider = target as Collider;
            BaseDebugCollider pc = null;
            switch (collider.GetType().Name)
            {
                case nameof(BoxCollider):
                    pc = new DebugBoxCollider((BoxCollider)collider);
                    break;
                case nameof(SphereCollider):
                    pc = new DebugSphereCollider((SphereCollider)collider);
                    break;
                case nameof(CapsuleCollider):
                    pc = new DebugCapsuleCollider((CapsuleCollider)collider);
                    break;
                case nameof(MeshCollider):
                    pc = new DebugMeshCollider((MeshCollider)collider);
                    break;
                default:
                    Debug.Log(collider.GetType().Name + " is not supported!");
                    return null;
            }
            m_list.Add(pc);
            return pc;
        }
        public override void Unregister(BaseDebugCollider collider)
        {
            if (!m_list.Contains(collider)) return;
            collider.DestroyVisual();
            m_list.Remove(collider);
        }
        public void Unregister(Collider collider) => m_list.RemoveWhere(x => x.Get().GetInstanceID() == collider.GetInstanceID());
        public override int Clear()
        {
            int destroyed = 0;
            foreach (var obj in m_list)
            {
                obj.DestroyVisual();
            }
            m_list.Clear();
            return destroyed;
        }

        public override bool Contains(object target)
        {
            Collider c = target as Collider;
            if (c == null) return false;
            return m_list.Where(x => x.Get()).Count() > 0;
        }
    }
}
