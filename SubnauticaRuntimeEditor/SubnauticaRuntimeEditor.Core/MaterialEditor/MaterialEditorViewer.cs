using SubnauticaRuntimeEditor.Core.Utils.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SubnauticaRuntimeEditor.Core.MaterialEditor
{
    public class MaterialEditorViewer : Window<MaterialEditorViewer>
    {
        public static MaterialEditorViewer main;

        public static readonly string DEFAULT_TITLE = "Material editor";

        public static readonly string MARMOSETUBER_SHADER_NAME = "MarmosetUBER";

        private const float SHADER_EDITOR_WIDTH = 500f;

        private readonly GUILayoutOption _propertyColumnWidth = GUILayout.Width(75f);

        private readonly GUILayoutOption _informationColumnWidth = GUILayout.Width(30f);

        private readonly GUILayoutOption _keywordColumnWidth = GUILayout.Width(400f);

        private Vector2 scrollPosition = Vector2.zero;

        private Material editingMaterial;

        private bool materialKeywordsExpanded;

        private bool materialPropertiesExpanded = true;

        protected override void Initialize(InitSettings initSettings)
        {
            main = this;
            Title = DEFAULT_TITLE;
        }

        public static void StartEditing(Material material)
        {
            main.Enabled = true;
            if (material == null)
            {
                main.Title = DEFAULT_TITLE;
            }
            else
            {
                main.editingMaterial = material;
                main.Title = material.name + " - " + material.shader.name;
            }
        }

        protected override Rect GetDefaultWindowRect(Rect screenRect)
        {
            return MakeDefaultWindowRect(screenRect, TextAlignment.Left);
        }

        protected override void DrawContents()
        {
            scrollPosition = GUILayout.BeginScrollView(this.scrollPosition, new GUILayoutOption[]
            {
                GUILayout.Width(SHADER_EDITOR_WIDTH),
                GUILayout.ExpandHeight(true)
            });
            if (editingMaterial == null)
            {
                GUILayout.BeginVertical(GUI.skin.box, Array.Empty<GUILayoutOption>());
                GUILayout.Label("No material selected!", new GUILayoutOption[]
                {
                    GUILayout.Width(SHADER_EDITOR_WIDTH)
                });
                return;
            }
            if (editingMaterial.shader.name.Equals(MaterialEditorViewer.MARMOSETUBER_SHADER_NAME))
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
                DrawProperties();
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
                this._keywordColumnWidth
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
                this._keywordColumnWidth
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
                    this.editingMaterial.EnableKeyword(keyword.ToString());
                }
                else
                {
                    this.editingMaterial.DisableKeyword(keyword.ToString());
                }
            }
            GUILayout.EndHorizontal();
        }

        private void DrawProperties()
        {
            this.DrawPropertiesTableHeader();
            foreach (KeyValuePair<MaterialEditorProperties, PropertyType> keyValuePair in from possibleProperty in MaterialEditorPropertyTypes.TYPES
                                                                                          where this.editingMaterial.HasProperty(possibleProperty.Value.Property)
                                                                                          select possibleProperty)
            {
                GUILayout.BeginHorizontal(GUI.skin.box, Array.Empty<GUILayoutOption>());
                this.DrawPropertiesTableRow(keyValuePair.Key, keyValuePair.Value);
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

        private void DrawPropertiesTableRow(MaterialEditorProperties property, PropertyType type)
        {
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label(type.Property, new GUILayoutOption[]
            {
                this._propertyColumnWidth
            });
            type.DrawGUI(this.editingMaterial);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("?", new GUILayoutOption[]
            {
                this._informationColumnWidth
            }))
            {
                this.ShowHelp(property, type);
            }
            GUILayout.EndHorizontal();
        }

        private void ShowHelp(MaterialEditorProperties property, PropertyType type)
        {
            /*new Dialog("Keywords for " + type.Property, delegate ()
			{
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.FlexibleSpace();
				GUILayout.Label("For the following property to work, you may need to enable these following keywords:", Array.Empty<GUILayoutOption>());
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.FlexibleSpace();
				MaterialEditorKeywords[] orDefault = MaterialEditorPropertyKeywords.KEYWORDS.GetOrDefault(property, Array.Empty<MaterialEditorKeywords>());
				GUILayout.Label((orDefault.Length == 0) ? "No Keywords" : string.Join<MaterialEditorKeywords>(", ", orDefault), Array.Empty<GUILayoutOption>());
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
			});*/
        }
    }
}