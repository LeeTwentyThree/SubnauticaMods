using System;
using UnityEngine;

namespace ModStructureHelperPlugin.Handle.Handles
{
    /**
     * Created by Peter @sHTiF Stefcek 20.10.2020
     */
    public abstract class HandleBase : MonoBehaviour
    {
        public event Action InteractionStart;
        public event Action InteractionEnd;
        public event Action<float> InteractionUpdate;
        
        protected RuntimeTransformHandle _parentTransformHandle;

        protected Color _defaultColor;

        protected Material _material;

        protected Vector3 _hitPoint;

        protected bool _isInteracting = false;

        public bool IsHovering { get; set; }

        public float delta;

        protected virtual void InitializeMaterial()
        {
            _material = new Material(Plugin.AssetBundle.LoadAsset<Shader>("HandleShader"));
            _material.color = _defaultColor;
        }
        
        public void SetDefaultColor()
        {
            _material.color = _defaultColor;
        }
        
        public void SetColor(Color p_color)
        {
            _material.color = p_color;
        }
        
        public virtual void StartInteraction(Vector3 p_hitPoint)
        {
            _hitPoint = p_hitPoint;
            InteractionStart?.Invoke();
            _isInteracting = true;
        }

        public virtual bool CanInteract(Vector3 p_hitPoint)
        {
            return true;
        }
        
        public virtual void Interact(Vector3 p_previousPosition)
        {
            InteractionUpdate?.Invoke(delta);
        }

        public virtual void EndInteraction()
        {
            _isInteracting = false;
            InteractionEnd?.Invoke();
            delta = 0;
            SetDefaultColor();
            IsHovering = false;
        }

        static public Vector3 GetVectorFromAxes(HandleAxes p_axes)
        {
            switch (p_axes)
            {
                case HandleAxes.X:
                    return new Vector3(1,0,0);
                case HandleAxes.Y:
                    return new Vector3(0,1,0);
                case HandleAxes.Z:
                    return new Vector3(0,0,1);
                case HandleAxes.XY:
                    return new Vector3(1,1,0);
                case HandleAxes.XZ:
                    return new Vector3(1,0,1);
                case HandleAxes.YZ:
                    return new Vector3(0,1,1);
                default:
                    return new Vector3(1,1,1);
            }
        }
    }
}