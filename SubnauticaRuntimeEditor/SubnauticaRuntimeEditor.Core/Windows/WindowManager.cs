using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SubnauticaRuntimeEditor.Core.Utils;
using SubnauticaRuntimeEditor.Core.Utils.Abstractions;
using UnityEngine;

namespace SubnauticaRuntimeEditor.Core
{
    public class WindowManager : IFeature
    {
        private int _windowId;
        private Rect _windowRect;
        private List<IFeature> _orderedFeatures;
        private string _title;

        public static WindowManager Instance { get; private set; }

        protected string GetTitle() => SubnauticaRuntimeEditorCore.Instance.ShowHotkey == KeyCode.None ? _title : _title + $" / Press {SubnauticaRuntimeEditorCore.Instance.ShowHotkey} to show/hide";
        public int Height => (int)_windowRect.height;

        public WindowManager()
        {
            Instance = this;
        }

        void IFeature.OnInitialize(InitSettings initSettings)
        {
            _windowId = GetHashCode();
            _title = $"{SubnauticaRuntimeEditorCore.GUID} v{SubnauticaRuntimeEditorCore.Version}";
        }

        public void SetFeatures(List<IFeature> initializedFeatures)
        {
            //var groups = initializedFeatures.ToLookup(x => x.DisplayType);
            //groups[FeatureDisplayType.Window]
            _orderedFeatures = initializedFeatures.OrderByDescending(x => x.DisplayType).ThenBy(x => x.DisplayName).ToList();
        }

        void IFeature.OnOnGUI()
        {
            _windowRect = GUILayout.Window(_windowId, _windowRect, DrawTaskbar, GetTitle(), GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(false), GUILayout.MaxWidth(Screen.width));
            IMGUIUtils.EatInputInRect(_windowRect);
            _windowRect.x = (int)((Screen.width - _windowRect.width) / 2);
            _windowRect.y = (int)(Screen.height - _windowRect.height);
        }

        private void DrawTaskbar(int id)
        {
            GUILayout.BeginHorizontal();

            var firstFeature = true;
            foreach (var feature in _orderedFeatures)
            {
                if (feature.DisplayType == FeatureDisplayType.Window)
                {
                    GUI.color = feature.Enabled ? Styling.Colors.windowManagerSelectedColor : Styling.Colors.defaultColor;
                    if (GUILayout.Button(feature.DisplayName))
                        feature.Enabled = !feature.Enabled;
                }
                else if (feature.DisplayType == FeatureDisplayType.Feature)
                {
                    if (firstFeature)
                    {
                        GUI.color = Styling.Colors.defaultTranparentColor;
                        if (GUILayout.Button("Reset"))
                        {
                            foreach (var window in _orderedFeatures.OfType<IWindow>())
                                window.ResetWindowRect();
                        }
                        firstFeature = false;
                        GUI.color = Styling.Colors.defaultColor;
                        GUILayout.Label("|");
                    }
                    feature.Enabled = GUILayout.Toggle(feature.Enabled, feature.DisplayName);
                }
            }

            GUILayout.Label("|");

            GUILayout.Label("Time", GUILayout.ExpandWidth(false));

            if (GUILayout.Button(">", GUILayout.ExpandWidth(false)))
                Time.timeScale = 1;
            if (GUILayout.Button("||", GUILayout.ExpandWidth(false)))
                Time.timeScale = 0;

            if (float.TryParse(GUILayout.TextField(Time.timeScale.ToString("F2", CultureInfo.InvariantCulture), GUILayout.Width(38)), NumberStyles.Any, CultureInfo.InvariantCulture, out var newVal))
                Time.timeScale = newVal;

            GUI.changed = false;
            var n = GUILayout.Toggle(Application.runInBackground, "in BG");
            if (GUI.changed) Application.runInBackground = n;

            GUILayout.Label("|");

            if (GUILayout.Button("Log", GUILayout.ExpandWidth(false)))
                UnityFeatureHelper.OpenLog();

            AssetBundleManagerHelper.DrawButtonIfAvailable();

            GUILayout.EndHorizontal();
        }

        public bool Enabled { get; set; }
        void IFeature.OnUpdate() { }
        void IFeature.OnLateUpdate() { }
        void IFeature.OnEditorShownChanged(bool visible) { }
        FeatureDisplayType IFeature.DisplayType => FeatureDisplayType.Hidden;
        string IFeature.DisplayName => "WindowManager";
    }
}
