using SubnauticaRuntimeEditor.Core.Utils.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SubnauticaRuntimeEditor.Core.Utils;

namespace SubnauticaRuntimeEditor.Core.MaterialEditor
{
    public class MaterialEditorViewer : Window<MaterialEditorViewer>
    {
        public static MaterialEditorViewer main;

        public static readonly string DEFAULT_TITLE = "Material editor";

        public static readonly string MARMOSETUBER_SHADER_NAME = "MarmosetUBER";

        private static readonly string NO_MATERIAL_SELECTED =
            "No material selected!\nSelect the 'Open in Material Editor' button on a valid Renderer or Material.";

        private const float SHADER_EDITOR_WIDTH = 500f;

        private readonly GUILayoutOption _propertyColumnWidth = GUILayout.Width(75f);
        
        private readonly GUILayoutOption _texturePropertyColumnWidth = GUILayout.Width(200f);

        private readonly GUILayoutOption _propertyDescriptionColumnWidth = GUILayout.Width(400f);

        private readonly GUILayoutOption _pinColumnWidth = GUILayout.Width(30f);

        private readonly GUILayoutOption _keywordColumnWidth = GUILayout.Width(400f);

        private readonly GUILayoutOption _headerColumnWidth = GUILayout.Width(400f);
        private readonly GUILayoutOption _headerHeight = GUILayout.Height(35f);

        private readonly GUILayoutOption _textLabelWidth = GUILayout.Width(400f);

        private readonly StringListPref _pinnedProperties = StringListPref.Get("PINNEDMATERIALPROPERTIES", false);

        private Vector2 _scrollPosition = Vector2.zero;

        private Material _editingMaterial;

        private bool _materialKeywordsExpanded;

        private bool _materialPropertiesExpanded = true;

        private bool _pinnedMaterialPropertiesExpanded = true;

        private readonly Dictionary<string, bool> _propertyHeadersExpanded = new Dictionary<string, bool>();

        private const bool DefaultPropertyExpandedValue = false;

        public static string FloatPropertyFormattingString { get; private set; }

        protected override void Initialize(InitSettings initSettings)
        {
            main = this;
            Title = DEFAULT_TITLE;
            initSettings.RegisterSetting("Material Editor",
                "Material editor decimal place accuracy",
                4,
                "The decimal place accuracy of float properties in the material editor.",
                x => FloatPropertyFormattingString = $"F{x}");
        }

        public static void StartEditing(Material material)
        {
            main.Enabled = true;
            main._editingMaterial = material;
            main.Title = main.CurrentTitle;
        }

        public string CurrentTitle
        {
            get
            {
                if (_editingMaterial == null) return DEFAULT_TITLE;
                return _editingMaterial.name + " - " + _editingMaterial.shader.name;
            }
        }

        protected override Rect GetDefaultWindowRect(Rect screenRect)
        {
            return MakeDefaultWindowRect(screenRect, Alignment.LowerLeft);
        }

        protected override void DrawContents()
        {
            // scroll view
            _scrollPosition = GUILayout.BeginScrollView(this._scrollPosition, new GUILayoutOption[]
            {
                GUILayout.Width(SHADER_EDITOR_WIDTH),
                GUILayout.ExpandHeight(true),
            });
            // no material selected warning
            if (_editingMaterial == null)
            {
                GUILayout.BeginVertical(GUI.skin.box);
                GUILayout.Label(NO_MATERIAL_SELECTED, _textLabelWidth);
                GUILayout.EndVertical();
                GUILayout.EndScrollView();
                return;
            }

            // title
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label("Selected: " + CurrentTitle, _textLabelWidth);
            GUILayout.EndVertical();
            // keywords
            if (_editingMaterial.shader.name.Equals(MARMOSETUBER_SHADER_NAME))
            {
                GUILayout.BeginVertical(GUI.skin.box);
                if (GUILayout.Button("Keywords", GUILayout.ExpandWidth(true)))
                {
                    _materialKeywordsExpanded = !_materialKeywordsExpanded;
                }

                if (_materialKeywordsExpanded)
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
                _pinnedMaterialPropertiesExpanded = !_pinnedMaterialPropertiesExpanded;
            }

            if (GUILayout.Button("Unpin all", new GUILayoutOption[]
                {
                    GUILayout.Width(100)
                }))
            {
                _pinnedProperties.array.Clear();
            }

            GUILayout.EndHorizontal();

            if (_pinnedMaterialPropertiesExpanded)
            {
                DrawProperties(true);
            }

            GUILayout.EndVertical();

            // properties (normal)

            GUILayout.BeginVertical(GUI.skin.box);
            if (GUILayout.Button("Properties", GUILayout.ExpandWidth(true)))
            {
                _materialPropertiesExpanded = !_materialPropertiesExpanded;
            }

            if (_materialPropertiesExpanded)
            {
                DrawProperties(false);
            }

            GUILayout.EndVertical();

            GUILayout.EndScrollView();
        }

        private void DrawKeywords()
        {
            HashSet<string> shaderKeywords = new HashSet<string>(_editingMaterial.shaderKeywords);
            DrawKeywordTableHeader();
            foreach (object obj in Enum.GetValues(typeof(Keywords)))
            {
                Keywords keyword = (Keywords)obj;
                GUILayout.BeginHorizontal(GUI.skin.box);
                DrawKeywordTableRow(shaderKeywords, keyword);
                GUILayout.EndHorizontal();
            }
        }

        private void DrawKeywordTableHeader()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Keyword", GUI.skin.box, _keywordColumnWidth);
            GUILayout.Label("Toggled", GUI.skin.box, GUILayout.ExpandWidth(true));
            GUILayout.EndHorizontal();
        }

        private void DrawKeywordTableRow(ICollection<string> shaderKeywords, Keywords keyword)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(keyword.ToString(), _keywordColumnWidth);
            bool flag = shaderKeywords.Contains(keyword.ToString());
            bool flag2 = GUILayout.Toggle(flag, "", GUILayout.ExpandWidth(true));
            if (flag != flag2)
            {
                if (flag2)
                {
                    _editingMaterial.EnableKeyword(keyword.ToString());
                }
                else
                {
                    _editingMaterial.DisableKeyword(keyword.ToString());
                }
            }

            GUILayout.EndHorizontal();
        }

        private void DrawProperties(bool pinnedOnly)
        {
            bool opened = true;

            DrawPropertiesTableHeader();
            foreach (KeyValuePair<MaterialEditorProperties, PropertyType> keyValuePair in from possibleProperty in
                         MaterialEditorPropertyTypes.TYPES
                     where _editingMaterial.HasProperty(possibleProperty.Value.Property)
                     select possibleProperty)
            {
                bool pinned = _pinnedProperties.Contains(keyValuePair.Key.ToString());
                if (!pinned && pinnedOnly) continue;
                if (!keyValuePair.Value.IsHeader && !opened)
                {
                    continue;
                }
                GUILayout.BeginHorizontal(GUI.skin.box);
                DrawPropertiesTableRow(keyValuePair.Key, keyValuePair.Value, pinned, ref opened);
                GUILayout.EndHorizontal();
            }
        }

        private void DrawPropertiesTableHeader()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Name", GUI.skin.box, _propertyColumnWidth);
            GUILayout.Label("Value", GUI.skin.box, GUILayout.ExpandWidth(true));
            GUILayout.EndHorizontal();
        }

        private void DrawPropertiesTableRow(MaterialEditorProperties property, PropertyType type, bool pinned, ref bool opened)
        {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();

            // Color if disabled
            if (!IsPropertyActiveByKeywords(property))
            {
                GUI.color = Color.gray;
            }

            var hasDescription = TryGetPropertyDescription(type, out var description);

            // Name
            if (type.IsHeader)
            {
                GUILayout.BeginHorizontal();
                var headerText = GetFullHeaderText(hasDescription ? description : type.Property, property);
                var oldFontSize = GUI.skin.label.fontSize;
                GUI.skin.label.fontSize = 18;
                var oldAlignment = GUI.skin.label.alignment;
                GUI.skin.label.alignment = TextAnchor.MiddleCenter;
                bool pressed = GUILayout.Button($"<b>{headerText}</b>", _headerColumnWidth, _headerHeight);
                opened = ManagePropertyGroupExpansion(type.Property, pressed);
                GUI.skin.label.fontSize = oldFontSize;
                GUI.skin.label.alignment = oldAlignment;
                GUILayout.EndHorizontal();
            }
            else
            {
                GUILayout.Label(type.Property, type is PropertyTypeTexture ? _texturePropertyColumnWidth : _propertyColumnWidth);
            }

            if (!type.IsHeader)
            {
                type.DrawGUI(_editingMaterial);
                GUILayout.FlexibleSpace();
                if (pinned) GUI.color = Styling.Colors.genericSelectionColorButton;
                if (GUILayout.Button(UI.InterfaceMaker.PinIcon, _pinColumnWidth))
                {
                    TogglePropertyPinned(property, type);
                }
            }

            GUILayout.EndHorizontal();

            GUI.color = IsPropertyActiveByKeywords(property) ? Styling.Colors.defaultColor : Color.gray;

            // Description
            if (hasDescription && !type.IsHeader)
            {
                GUILayout.BeginHorizontal();
                var oldFontSize = GUI.skin.label.fontSize;
                GUI.skin.label.fontSize = 10;
                GUILayout.Label(description, _propertyDescriptionColumnWidth);
                GUI.skin.label.fontSize = oldFontSize;
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }

            GUI.color = Styling.Colors.defaultColor;

            GUILayout.EndVertical();
        }

        private bool ManagePropertyGroupExpansion(string headerPropertyName, bool pressed)
        {
            var hasValue = _propertyHeadersExpanded.TryGetValue(headerPropertyName, out var currentlyOpened);
            if (!hasValue) currentlyOpened = DefaultPropertyExpandedValue;
            
            if (pressed)
            {
                currentlyOpened = !currentlyOpened;
                if (hasValue)
                {
                    _propertyHeadersExpanded[headerPropertyName] = currentlyOpened;
                }
                else
                {
                    _propertyHeadersExpanded.Add(headerPropertyName, currentlyOpened);
                }
            }
            
            return currentlyOpened;
        }

        private string GetFullHeaderText(string name, MaterialEditorProperties propertyType)
        {
            if (MaterialEditorPropertyKeywords.KEYWORDS.TryGetValue(propertyType, out var relevantKeywords) &&
                relevantKeywords != null && relevantKeywords.Length > 0)
            {
                StringBuilder fullName = new StringBuilder(name);
                fullName.Append("\nEnabled by: ");
                for (int i = 0; i < relevantKeywords.Length; i++)
                {
                    fullName.Append(' ');
                    fullName.Append(relevantKeywords[i]);
                    if (i < relevantKeywords.Length - 1)
                    {
                        fullName.Append(",");
                    }
                }

                return fullName.ToString();
            }

            return name;
        }

        private bool TryGetPropertyDescription(PropertyType property, out string description)
        {
            int index = _editingMaterial.shader.FindPropertyIndex(property.Property);
            if (index < 0)
            {
                description = "Not found in shader property list";
                return true;
            }

            var propertyDescription = _editingMaterial.shader.GetPropertyDescription(index);
            if (propertyDescription == property.Property)
            {
                description = null;
                return false;
            }

            description = propertyDescription;
            return true;
        }

        private bool IsPropertyActiveByKeywords(MaterialEditorProperties property)
        {
            if (MaterialEditorPropertyKeywords.KEYWORDS.TryGetValue(property, out var relevantKeywords))
            {
                if (relevantKeywords == null || relevantKeywords.Length == 0) return true;
                foreach (var keyword in relevantKeywords)
                {
                    if (_editingMaterial.IsKeywordEnabled(keyword.ToString()))
                    {
                        return true;
                    }
                }

                return false;
            }

            return true;
        }

        private void IsPinned(MaterialEditorProperties property) => _pinnedProperties.Contains(property.ToString());

        private void TogglePropertyPinned(MaterialEditorProperties property, PropertyType type)
        {
            var propertyId = property.ToString();
            if (!_pinnedProperties.Contains(propertyId))
            {
                _pinnedProperties.Add(propertyId);
            }
            else
            {
                _pinnedProperties.Remove(propertyId);
            }
        }
    }
}