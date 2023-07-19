﻿using SubnauticaRuntimeEditor.Core.Utils.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SubnauticaRuntimeEditor.Core.Utils;

namespace SubnauticaRuntimeEditor.Core.AnimationController
{
    public class AnimationController : Window<AnimationController>
    {
        public static AnimationController main;

        public static readonly string DEFAULT_TITLE = "Animation controller";

        private static readonly string NO_ANIMATOR_SELECTED = "No animator selected!\nSelect the 'Open In Animator Controller' button on a valid Animator component.";

        private const float ANIMATOR_CONTROLLER_WIDTH = 500f;

        private readonly GUILayoutOption _propertyColumnWidth = GUILayout.Width(150f);

        private readonly GUILayoutOption _textLabelWidth = GUILayout.Width(400f);

        private Vector2 scrollPosition = Vector2.zero;

        private Animator editingAnimator;

        private bool layersExpanded = true;

        private bool parametersExpanded = true;

        protected override void Initialize(InitSettings initSettings)
        {
            main = this;
            Title = DEFAULT_TITLE;
        }

        public static void StartEditing(Animator animator)
        {
            main.Enabled = true;
            main.editingAnimator = animator;
            main.Title = main.CurrentTitle;
        }

        public string CurrentTitle
        {
            get
            {
                if (editingAnimator == null) return DEFAULT_TITLE;
                if (editingAnimator.runtimeAnimatorController == null) return editingAnimator.name + " - No runtime animator controller!";
                return editingAnimator.name + " - " + editingAnimator.runtimeAnimatorController.name;
            }
        }

        protected override Rect GetDefaultWindowRect(Rect screenRect)
        {
            return MakeDefaultWindowRect(screenRect, Alignment.LowerLeft);
        }

        protected override void DrawContents()
        {
            // scroll view
            scrollPosition = GUILayout.BeginScrollView(this.scrollPosition, new GUILayoutOption[]
            {
                GUILayout.Width(ANIMATOR_CONTROLLER_WIDTH),
                GUILayout.ExpandHeight(true),
            });
            // no material selected warning
            if (editingAnimator == null)
            {
                GUILayout.BeginVertical(GUI.skin.box, Array.Empty<GUILayoutOption>());
                GUILayout.Label(NO_ANIMATOR_SELECTED, new GUILayoutOption[]
                {
                    _textLabelWidth,
                }) ;
                GUILayout.EndVertical();
                GUILayout.EndScrollView();
                return;
            }
            // title
            GUILayout.BeginVertical(GUI.skin.box, Array.Empty<GUILayoutOption>());
            GUILayout.Label("Selected: " + CurrentTitle, new GUILayoutOption[]
            {
                _textLabelWidth
            });
            GUILayout.EndVertical();

            // model viewer

            if (!editingAnimator.gameObject.GetComponent<ObjectView.RenderedObject>())
            {
                GUILayout.BeginVertical(GUI.skin.box, Array.Empty<GUILayoutOption>());
                if (GUILayout.Button("Show in model viewer", new GUILayoutOption[]
                {
                GUILayout.ExpandWidth(true)
                }))
                {
                    StartEditing(ObjectView.ObjectViewWindow.Instance.RenderModel(editingAnimator.gameObject, true).GetComponent<Animator>());
                }

                if (GUILayout.Button("Show in model viewer (no TrailManagers)", new GUILayoutOption[]
                {
                GUILayout.ExpandWidth(true)
                }))
                {
                    StartEditing(ObjectView.ObjectViewWindow.Instance.RenderModel(editingAnimator.gameObject, false).GetComponent<Animator>());
                }
                GUILayout.EndVertical();
            }

            // properties

            DrawMainProperties();

            // layers

            GUILayout.BeginVertical(GUI.skin.box, Array.Empty<GUILayoutOption>());
            if (GUILayout.Button("Layers", new GUILayoutOption[]
            {
                GUILayout.ExpandWidth(true)
            }))
            {
                layersExpanded = !layersExpanded;
            }
            if (layersExpanded)
            {
                DrawLayers();
            }
            GUILayout.EndVertical();

            // parameters

            GUILayout.BeginVertical(GUI.skin.box, Array.Empty<GUILayoutOption>());
            if (GUILayout.Button("Parameters", new GUILayoutOption[]
            {
                GUILayout.ExpandWidth(true)
            }))
            {
                parametersExpanded = !parametersExpanded;
            }
            if (parametersExpanded)
            {
                DrawParameters();
            }
            GUILayout.EndVertical();

            GUILayout.EndScrollView();
        }

        private void DrawMainProperties()
        {
            GUILayout.BeginHorizontal(GUI.skin.box, Array.Empty<GUILayoutOption>());

            editingAnimator.speed = DrawFloatField("Time scale", editingAnimator.speed);

            GUILayout.EndHorizontal();
        }

        private void DrawToggleTableRow(string id, bool enabled)
        {
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label(id, new GUILayoutOption[]
            {
                _propertyColumnWidth
            });
            bool wasEnabled = enabled;
            bool enabledNow = GUILayout.Toggle(wasEnabled, "", new GUILayoutOption[]
            {
                GUILayout.ExpandWidth(true)
            });
            if (wasEnabled != enabledNow)
            {
                editingAnimator.SetBool(id, enabledNow);
            }
            GUILayout.EndHorizontal();
        }

        private void DrawTriggerTableRow(string id)
        {
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label(id, new GUILayoutOption[]
            {
                _propertyColumnWidth
            });
            bool trigger = GUILayout.Button("Trigger", new GUILayoutOption[]
            {
                GUILayout.ExpandWidth(true)
            });
            if (trigger)
            {
                editingAnimator.SetTrigger(id);
            }
            GUILayout.EndHorizontal();
        }

        private float DrawFloatField(string name, float value)
        {
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label(name, new GUILayoutOption[]
            {
                _propertyColumnWidth
            });
            float oldValue = value;
            string text = GUILayout.TextField(oldValue.ToString("0.0000"), new GUILayoutOption[]
            {
                GUILayout.ExpandWidth(true)
            });
            float.TryParse(text, out float newValue);
            GUILayout.EndHorizontal();
            return newValue;
        }

        private void DrawFloatTableRow(string id, float value)
        {
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label(id, new GUILayoutOption[]
            {
                _propertyColumnWidth
            });
            float oldValue = value;
            string text = GUILayout.TextField(oldValue.ToString("0.0000"), new GUILayoutOption[]
            {
                GUILayout.ExpandWidth(true)
            });
            float.TryParse(text, out float newValue);
            if (oldValue != newValue)
            {
                editingAnimator.SetFloat(id, newValue);
            }
            GUILayout.EndHorizontal();
        }

        private void DrawIntTableRow(string id, int value)
        {
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label(id, new GUILayoutOption[]
            {
                _propertyColumnWidth
            });
            int oldValue = value;
            string text = GUILayout.TextField(oldValue.ToString(), new GUILayoutOption[]
            {
                GUILayout.ExpandWidth(true)
            });
            int.TryParse(text, out int newValue);
            if (oldValue != newValue)
            {
                editingAnimator.SetInteger(id, newValue);
            }
            GUILayout.EndHorizontal();
        }

        private void DrawParameters()
        {
            DrawPropertiesTableHeader("Name", "Value");
            foreach (var parameter in editingAnimator.parameters)
            {
                DrawParameter(parameter);
            }
        }

        private void DrawParameter(AnimatorControllerParameter parameter)
        {
            GUILayout.BeginHorizontal(GUI.skin.box, Array.Empty<GUILayoutOption>());
            switch (parameter.type)
            {
                default:
                    GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
                    GUILayout.Label($"Unable to display parameter '{parameter.name}' (of type '{parameter.type}')");
                    GUILayout.EndHorizontal();
                    break;
                case AnimatorControllerParameterType.Bool:
                    DrawToggleTableRow(parameter.name, editingAnimator.GetBool(parameter.name));
                    break;
                case AnimatorControllerParameterType.Trigger:
                    DrawTriggerTableRow(parameter.name);
                    break;
                case AnimatorControllerParameterType.Float:
                    DrawFloatTableRow(parameter.name, editingAnimator.GetFloat(parameter.name));
                    break;
                case AnimatorControllerParameterType.Int:
                    DrawIntTableRow(parameter.name, editingAnimator.GetInteger(parameter.name));
                    break;

            }
            GUILayout.EndHorizontal();
        }

        private void DrawPropertiesTableHeader(string s1, string s2)
        {
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label(s1, GUI.skin.box, new GUILayoutOption[]
            {
                _propertyColumnWidth
            });
            GUILayout.Label(s2, GUI.skin.box, new GUILayoutOption[]
            {
                GUILayout.ExpandWidth(true)
            });
            GUILayout.EndHorizontal();
        }

        private void DrawLayers()
        {
            DrawPropertiesTableHeader("Index - Name", "Weight");
            for (int i = 0; i < editingAnimator.layerCount; i++)
            {
                DrawLayer(i);
            }
        }

        private void DrawLayer(int index)
        {
            GUILayout.BeginHorizontal(GUI.skin.box, Array.Empty<GUILayoutOption>());
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());

            GUILayout.Label(index + " " + editingAnimator.GetLayerName(index), new GUILayoutOption[]
            {
                _propertyColumnWidth
            });
            if (index != 0)
            {
                float oldValue = editingAnimator.GetLayerWeight(index);
                string text = GUILayout.TextField(oldValue.ToString("0.0"), new GUILayoutOption[]
                {
                GUILayout.ExpandWidth(true)
                });
                float.TryParse(text, out float newValue);
                if (oldValue != newValue) editingAnimator.SetLayerWeight(index, newValue);
            }

            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();
        }
    }
}