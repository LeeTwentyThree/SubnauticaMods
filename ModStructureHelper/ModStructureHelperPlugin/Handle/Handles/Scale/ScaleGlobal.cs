using ModStructureHelperPlugin.Handle.Utils;
using UnityEngine;

namespace ModStructureHelperPlugin.Handle.Handles.Scale
{
    /**
     * Created by Peter @sHTiF Stefcek 20.10.2020
     */
    public class ScaleGlobal : HandleBase
    {
        protected Vector3 _axis;
        protected Vector3 _startScale;
        
        public ScaleGlobal Initialize(RuntimeTransformHandle p_parentTransformHandle, Vector3 p_axis, Color p_color)
        {
            _parentTransformHandle = p_parentTransformHandle;
            _axis = p_axis;
            _defaultColor = p_color;
            
            InitializeMaterial();

            transform.SetParent(p_parentTransformHandle.transform, false);

            GameObject o = new GameObject();
            o.transform.SetParent(transform, false);
            MeshRenderer mr = o.AddComponent<MeshRenderer>();
            mr.material = _material;
            MeshFilter mf = o.AddComponent<MeshFilter>();
            mf.mesh = MeshUtils.CreateBox(.35f, .35f, .35f);
            MeshCollider mc = o.AddComponent<MeshCollider>();

            return this;
        }

        public override void Interact(Vector3 p_previousPosition)
        {
            Vector3 mouseVector = (RuntimeTransformHandle.GetMousePosition() - p_previousPosition);
            float d = (mouseVector.x + mouseVector.y) * Time.unscaledDeltaTime * 2;
            delta += d;
            var newScale = _startScale + Vector3.Scale(_startScale,_axis) * delta;
            if (GameInput.GetButtonHeld(StructureHelperInput.ScaleUniform))
            {
                newScale = Vector3.one * ((newScale.x + newScale.y + newScale.z) / 3);
            }

            _parentTransformHandle.Target.localScale = newScale;
            
            base.Interact(p_previousPosition);
        }

        public override void StartInteraction(Vector3 p_hitPoint)
        {
            base.StartInteraction(p_hitPoint);
            _startScale = _parentTransformHandle.Target.localScale;
        }
    }
}