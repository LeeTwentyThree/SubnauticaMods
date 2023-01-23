using SubnauticaRuntimeEditor.Core.Utils;
using SubnauticaRuntimeEditor.Core.Utils.Abstractions;
using UnityEngine;
using Type = System.Type;

namespace SubnauticaRuntimeEditor.Core.ObjectView
{
    public sealed class ObjectViewWindow : Window<ObjectViewWindow>
    {
        private object _objToDisplay;
        private Vector2 _scrollPos;
        private static GameObject _model;
        private Type[] _typesToPreserve = new Type[] { typeof(Transform), typeof(MeshFilter), typeof(Renderer), typeof(Animator), typeof(LODGroup) };

        public void SetShownObject(object objToDisplay, string objName, bool isModel = false)
        {
            // todo make more generic and support more types
            if (objToDisplay == null || objToDisplay is Texture || objToDisplay is string)
                _objToDisplay = objToDisplay;
            else
                _objToDisplay = $"Unsupported object type: {objToDisplay.GetType()}\n{new System.Diagnostics.StackTrace()}";

            Title = "Object viewer - " + (objName ?? "NULL");

            Enabled = true;

            if (!isModel) Object.Destroy(_model);
        }

        public RenderedObject RenderModel(GameObject meshParent, bool preserveTrailManagers)
        {
            if (_model != null)
            {
                Object.Destroy(_model);
            }
            _model = Object.Instantiate(meshParent);
            foreach (Component c in _model.GetComponentsInChildren<Component>(true))
            {
                if (preserveTrailManagers && c.GetType() == typeof(TrailManager)) continue;
                bool allowed = false;
                foreach (var t in _typesToPreserve)
                {
                    if (c.GetType() == t || c.GetType().IsSubclassOf(t))
                        allowed = true;
                }
                if (!allowed) Object.Destroy(c);
            }
            _model.transform.parent = null;
            _model.transform.position = new Vector3(0, 2000, 0);
            _model.transform.rotation = Quaternion.identity;
            var renderedObject = _model.AddComponent<RenderedObject>();
            SetShownObject(renderedObject.Setup(), meshParent.gameObject.name, true);
            return renderedObject;
        }

        protected override void OnVisibleChanged(bool visible)
        {
            base.OnVisibleChanged(visible);
            if (!visible)
            {
                Object.Destroy(_model);
            }
        }

        protected override Rect GetDefaultWindowRect(Rect screenRect)
        {
            return MakeDefaultWindowRect(screenRect, Alignment.UpperLeft);
        }

        protected override void DrawContents()
        {
            GUILayout.BeginVertical();
            {
                if (_objToDisplay == null)
                {
                    GUILayout.FlexibleSpace();
                    GUILayout.Label("No object selected or the object has been destroyed.\n\nYou can send objects here from other windows by clicking the \"View\" or \"V\" buttons.");
                    GUILayout.FlexibleSpace();
                }
                else
                {
                    _scrollPos = GUILayout.BeginScrollView(_scrollPos, true, true, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                    // todo make more generic and support more types
                    {
                        if (_objToDisplay is Texture tex)
                        {
                            if (GUILayout.Button("Save"))
                                tex.SaveTextureToFileWithDialog();

                            GUILayout.Label(tex, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                        }
                        else if (_objToDisplay is string str)
                        {
                            GUILayout.TextArea(str, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                        }
                    }
                    GUILayout.EndScrollView();
                }
            }
            GUILayout.EndVertical();

        }

        protected override void Initialize(InitSettings initSettings)
        {
            Title = "Object viewer - Empty";
            DisplayName = "Viewer";
            DisplayType = FeatureDisplayType.Hidden;
        }
    }
}