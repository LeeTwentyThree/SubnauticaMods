using DebugHelper.Interfaces;
using UnityEngine;

namespace DebugHelper.Systems
{
    public abstract class BasicDebugIcon : MonoBehaviour, IDebugIcon
    {
        public Color invalidColor = new Color(1f, 0f, 0f, DebugIconManager.kInactiveComponentAlpha);

        private void OnEnable()
        {
            DebugIconManager.Main.Register(this);
        }

        private void OnDisable()
        {
            DebugIconManager.Main.Unregister(this);
        }

        public abstract string Label { get; }

        public abstract Sprite Icon { get; }
        public abstract Vector3 Position { get; }
        public abstract float Scale { get; }
        public abstract Color Color { get; }

        public virtual void OnCreation(DebugIconInstance instance)
        {

        }
    }
}
