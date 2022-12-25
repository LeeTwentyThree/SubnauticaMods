using SubnauticaRuntimeEditor.Core.Utils.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SubnauticaRuntimeEditor.Core.Utils;

namespace SubnauticaRuntimeEditor.Core.MaterialEditor
{
    public class MaterialEditorViewer : Window<MaterialEditorViewer>
    {
        public static MaterialEditorViewer main;

        public static readonly string DEFAULT_TITLE = "Material editor";

        public static readonly string MARMOSETUBER_SHADER_NAME = "MarmosetUBER";

        private static readonly string NO_MATERIAL_SELECTED = "No material selected!\nSelect the 'Open in Material Editor' button on a valid Renderer or Material.";

        private const float SHADER_EDITOR_WIDTH = 500f;

        private readonly GUILayoutOption _propertyColumnWidth = GUILayout.Width(75f);

        private readonly GUILayoutOption _pinColumnWidth = GUILayout.Width(30f);

        private readonly GUILayoutOption _keywordColumnWidth = GUILayout.Width(400f);

        private readonly GUILayoutOption _textLabelWidth = GUILayout.Width(400f);

        private readonly StringListPref pinnedProperties = StringListPref.Get("PINNEDMATERIALPROPERTIES", false);

        private Vector2 scrollPosition = Vector2.zero;

        private Material editingMaterial;

        private bool materialKeywordsExpanded;

        private bool materialPropertiesExpanded = true;

        private bool pinnedMaterialPropertiesExpanded = true;

        protected override void Initialize(InitSettings initSettings)
        {
            main = this;
            Title = DEFAULT_TITLE;
        }

        public static void StartEditing(Material material)
        {
            main.Enabled = true;
            main.editingMaterial = material;
            main.Title = main.CurrentTitle;
        }

        public string CurrentTitle
        {
            get
            {
                if (editingMaterial == null) return DEFAULT_TITLE;
                return editingMaterial.name + " - " + editingMaterial.shader.name;
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
                GUILayout.Width(SHADER_EDITOR_WIDTH),
                GUILayout.ExpandHeight(true),
            });
            // no material selected warning
            if (editingMaterial == null)
            {
                GUILayout.BeginVertical(GUI.skin.box, Array.Empty<GUILayoutOption>());
                GUILayout.Label(NO_MATERIAL_SELECTED, new GUILayoutOption[]
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
            // keywords
            if (editingMaterial.shader.name.Equals(MARMOSETUBER_SHADER_NAME))
            {
                GUILayout.BeginVertical(GUI.skin.box, Array.Empty<GUILayoutOption>());
                if (GUILayout.Button("Keywords", new GUILayoutOption[]
                {
                    GUILayout.ExpandWidth(true)
                }))
                {
                    materialKeywordsExpanded = !materialKeywordsExpanded;
                }
                if (materialKeywordsExpanded)
                {
                    DrawKeywords();
                }
                GUILayout.EndVertical();
            }
            // properties (pinned)

            GUILayout.BeginVertical(GUI.skin.box, Array.Empty<GUILayoutOption>());

            GUILayout.BeginHorizontal(GUI.skin.box);
            if (GUILayout.Button("Pinned properties", new GUILayoutOption[]
            {
                GUILayout.ExpandWidth(true)
            }))
            {
                pinnedMaterialPropertiesExpanded = !pinnedMaterialPropertiesExpanded;
            }

            if (GUILayout.Button("Unpin all", new GUILayoutOption[]
            {
                GUILayout.Width(100)
            }))
            {
                pinnedProperties.array.Clear();
            }
            GUILayout.EndHorizontal();

            if (pinnedMaterialPropertiesExpanded)
            {
                DrawProperties(true);
            }
            GUILayout.EndVertical();

            // properties (normal)

            GUILayout.BeginVertical(GUI.skin.box, Array.Empty<GUILayoutOption>());
            if (GUILayout.Button("Properties", new GUILayoutOption[]
            {
                GUILayout.ExpandWidth(true)
            }))
            {
                materialPropertiesExpanded = !materialPropertiesExpanded;
            }
            if (materialPropertiesExpanded)
            {
                DrawProperties(false);
            }
            GUILayout.EndVertical();

            GUILayout.EndScrollView();
        }

        private void DrawKeywords()
        {
            HashSet<string> shaderKeywords = new HashSet<string>(this.editingMaterial.shaderKeywords);
            this.DrawKeywordTableHeader();
            foreach (object obj in Enum.GetValues(typeof(Keywords)))
            {
                Keywords keyword = (Keywords)obj;
                GUILayout.BeginHorizontal(GUI.skin.box, Array.Empty<GUILayoutOption>());
                this.DrawKeywordTableRow(shaderKeywords, keyword);
                GUILayout.EndHorizontal();
            }
        }

        private void DrawKeywordTableHeader()
        {
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label("Keyword", GUI.skin.box, new GUILayoutOption[]
            {
                _keywordColumnWidth
            });
            GUILayout.Label("Toggled", GUI.skin.box, new GUILayoutOption[]
            {
                GUILayout.ExpandWidth(true)
            });
            GUILayout.EndHorizontal();
        }

        private void DrawKeywordTableRow(ICollection<string> shaderKeywords, Keywords keyword)
        {
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label(keyword.ToString(), new GUILayoutOption[]
            {
                _keywordColumnWidth
            });
            bool flag = shaderKeywords.Contains(keyword.ToString());
            bool flag2 = GUILayout.Toggle(flag, "", new GUILayoutOption[]
            {
                GUILayout.ExpandWidth(true)
            });
            if (flag != flag2)
            {
                if (flag2)
                {
                    editingMaterial.EnableKeyword(keyword.ToString());
                }
                else
                {
                    editingMaterial.DisableKeyword(keyword.ToString());
                }
            }
            GUILayout.EndHorizontal();
        }

        private void DrawProperties(bool pinnedOnly)
        {
            DrawPropertiesTableHeader();
            foreach (KeyValuePair<MaterialEditorProperties, PropertyType> keyValuePair in from possibleProperty in MaterialEditorPropertyTypes.TYPES
                                                                                          where editingMaterial.HasProperty(possibleProperty.Value.Property)
                                                                                          select possibleProperty)
            {
                bool pinned = pinnedProperties.Contains(keyValuePair.Key.ToString());
                if (!pinned && pinnedOnly) continue;
                GUILayout.BeginHorizontal(GUI.skin.box, Array.Empty<GUILayoutOption>());
                DrawPropertiesTableRow(keyValuePair.Key, keyValuePair.Value, pinned);
                GUILayout.EndHorizontal();
            }
        }

        private void DrawPropertiesTableHeader()
        {
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label("Name", GUI.skin.box, new GUILayoutOption[]
            {
                this._propertyColumnWidth
            });
            GUILayout.Label("Value", GUI.skin.box, new GUILayoutOption[]
            {
                GUILayout.ExpandWidth(true)
            });
            GUILayout.EndHorizontal();
        }

        private void DrawPropertiesTableRow(MaterialEditorProperties property, PropertyType type, bool pinned)
        {
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label(type.Property, new GUILayoutOption[]
            {
                _propertyColumnWidth
            });
            type.DrawGUI(editingMaterial);
            GUILayout.FlexibleSpace();
            if (pinned) GUI.color = Styling.Colors.genericSelectionColorButton;
            if (GUILayout.Button(UI.InterfaceMaker.PinIcon, new GUILayoutOption[]
            {
                _pinColumnWidth
            }))
            {
                TogglePropertyPinned(property, type);
            }
            GUI.color = Styling.Colors.defaultColor;
            GUILayout.EndHorizontal();
        }

        private void IsPinned(MaterialEditorProperties property) => pinnedProperties.Contains(property.ToString());

        private void TogglePropertyPinned(MaterialEditorProperties property, PropertyType type)
        {
            var propertyId = property.ToString();
            if (!pinnedProperties.Contains(propertyId))
            {
                pinnedProperties.Add(propertyId);
            }
            else
            {
                pinnedProperties.Remove(propertyId);
            }
        }
    }
}