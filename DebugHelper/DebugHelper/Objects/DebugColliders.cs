using DebugHelper.Basis;
using UnityEngine;

namespace DebugHelper.Objects
{
    public enum ColliderType
    {
        Static,
        Rigidbody,
        KinematicRigidbody
    }
    public enum ColliderShape
    {
        Box,
        Sphere,
        Capsule,
        Mesh
    }
    public abstract class BaseDebugCollider
    {
        // References:
        // https://docs.unity3d.com/560/Documentation/Manual/CollidersOverview.html

        #region Fields
        private readonly Collider m_collider;
        protected GameObject m_visualObject;
        protected Renderer m_renderer;
        #endregion
        #region Getters
        #region Properties
        public bool WasDrawn
        {
            get
            {
                if (m_visualObject == null) return false;
                return true;
            }
        }
        /// <summary>
        /// Returns one of these collider type: Static, Rigidbody, KinematicRigidbody
        /// </summary>
        public ColliderType Type { get => GetColliderType(); }
        /// <summary>
        /// Returns one of these shape type: Box, Sphere, Capsule, Mesh
        /// </summary>
        public ColliderShape Shape { get => GetColliderShape(); }
        public bool IsTrigger { get => m_collider.isTrigger; }
        #endregion
        #region Methods
        /// <summary>
        /// Returns collider
        /// </summary>
        public Collider Get() => m_collider;
        /// <summary>
        /// Returns one of these collider type: Static, Rigidbody, KinematicRigidbody
        /// </summary>
        public ColliderType GetColliderType()
        {
            bool flag1 = m_collider.attachedRigidbody != null; // Has rigidbody attached
            if (flag1)
            {
                bool flag3 = m_collider.attachedRigidbody.isKinematic; // Is kinematic
                switch (flag3)
                {
                    case true: return ColliderType.KinematicRigidbody;
                    case false: return ColliderType.Rigidbody;
                }
            }
            return ColliderType.Static;
        }
        /// <summary>
        /// Returns one of these shape type: Box, Sphere, Capsule, Mesh
        /// </summary>
        public ColliderShape GetColliderShape()
        {
            switch (m_collider.GetType().Name)
            {
                case nameof(BoxCollider):
                    return ColliderShape.Box;
                case nameof(SphereCollider):
                    return ColliderShape.Sphere;
                case nameof(CapsuleCollider):
                    return ColliderShape.Capsule;
            }
            return ColliderShape.Mesh;
        }
        #endregion
        #endregion
        #region Visual
        public void SetColor(Color color)
        {
            if (!WasDrawn) return;
            m_renderer.material.color = color;
        }
        public void SetMaterial(Material material)
        {
            if (!WasDrawn) return;
            m_renderer.material = material;
        }
        public virtual void CreateVisual()
        {
            if (WasDrawn) return;
            PrimitiveType pt = PrimitiveType.Cube;
            switch (Shape)
            {
                case ColliderShape.Box: 
                    break;
                case ColliderShape.Sphere: 
                    pt = PrimitiveType.Sphere;
                    break;
                case ColliderShape.Capsule:
                    pt = PrimitiveType.Capsule;
                    break;
                default: throw new System.ArgumentException();
            }
            m_visualObject = GameObject.CreatePrimitive(pt);
            m_renderer = m_visualObject.GetComponent<Renderer>();
            Object.DestroyImmediate(m_visualObject.GetComponent<Collider>());
            m_visualObject.SetActive(true);
            Transform transform = m_visualObject.transform;
            transform.SetParent(m_collider.transform);
            transform.localEulerAngles = Vector3.zero;
        }
        public void DestroyVisual()
        {
            if (!WasDrawn) return;
            Object.DestroyImmediate(m_visualObject);
        }
        #endregion
        BaseDebugCollider() { }
        internal BaseDebugCollider(Collider collider) => this.m_collider = collider;
    }
    public abstract class BaseDebugCollider<T> : BaseDebugCollider where T : Collider
    {
        /// <summary>
        /// Returns collider
        /// </summary>
        new public T Get() => (T)base.Get();

        public BaseDebugCollider(T collider) : base(collider) { }
    }
    public sealed class DebugBoxCollider : BaseDebugCollider<BoxCollider>
    {
        public Vector3 Center { get => GetCenter(); }
        public Vector3 Size { get => GetSize(); }

        public Vector3 GetCenter() => Get().center;
        public Vector3 GetSize() => Get().size;

        public override void CreateVisual()
        {
            base.CreateVisual();
            m_visualObject.transform.localPosition = Center;
            m_visualObject.transform.localScale = Size;
        }

        internal DebugBoxCollider(BoxCollider collider) : base(collider) { }
    }
    public sealed class DebugSphereCollider : BaseDebugCollider<SphereCollider>
    {
        public Vector3 Center { get => GetCenter(); }
        public float Radius { get => GetRadius(); }

        public Vector3 GetCenter() => Get().center;
        public float GetRadius() => Get().radius;

        public override void CreateVisual()
        {
            base.CreateVisual();
            m_visualObject.transform.localPosition = Center;
            m_visualObject.transform.localScale = Vector3.one * GetRadius() * 2f;
        }

        internal DebugSphereCollider(SphereCollider collider) : base(collider) { }
    }
    public sealed class DebugCapsuleCollider : BaseDebugCollider<CapsuleCollider>
    {
        public Vector3 Center { get => GetCenter(); }
        public float Radius { get => GetRadius(); }
        public float Height { get => GetHeight(); }
        public int Direction { get => GetDirection(); }

        public Vector3 GetCenter() => Get().center;
        public float GetRadius() => Get().radius;
        public float GetHeight() => Get().height;
        public int GetDirection() => Get().direction;

        public override void CreateVisual()
        {
            base.CreateVisual();
            Transform transform = m_visualObject.transform;
            Vector3 angle = 90 * (Direction != 1 ? (Direction == 0 ? Vector3.forward : Vector3.right) : Vector3.zero);
            transform.localPosition = Center;
            transform.localEulerAngles = angle;
            transform.localScale = new Vector3(Radius * 2f, Height / 2, Radius * 2f);
        }

        internal DebugCapsuleCollider(CapsuleCollider collider) : base(collider) { }
    }
    public sealed class DebugMeshCollider : BaseDebugCollider<MeshCollider>
    {
        private MeshFilter m_filter;
        public Mesh Mesh { get => GetMesh(); }
        public bool Convex { get => IsConvex(); }

        public Mesh GetMesh() => Get().sharedMesh;
        public bool IsConvex() => Get().convex;

        public override void CreateVisual()
        {
            if (WasDrawn) return;

            m_visualObject = new GameObject("DebugMeshCollider");
            m_renderer = m_visualObject.AddComponent<MeshRenderer>();
            m_filter = m_visualObject.AddComponent<MeshFilter>();
            m_filter.mesh = Get().sharedMesh;
            m_visualObject.SetActive(true);

            Transform transform = m_visualObject.transform;
            transform.SetParent(Get().transform);
            transform.localPosition = Vector3.zero;
            transform.localEulerAngles = Vector3.zero;
            transform.localScale = Vector3.one;
        }

        internal DebugMeshCollider(MeshCollider collider) : base(collider) { }
    }
}
