using System;
using System.Reflection;
using SubnauticaRuntimeEditor.Core.Utils;
using SubnauticaRuntimeEditor.Core.Utils.Abstractions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SubnauticaRuntimeEditor.Core.UI
{
    public static class InterfaceMaker
    {
        // These all need to be held as static properties, including textures, to prevent UnloadUnusedAssets from destroying them
        private static Texture2D _boxBackground;
        private static Texture2D _winBackground;
        private static Texture2D _buttonBackgroundNormal;
        private static Texture2D _buttonBackgroundOn;
        private static Texture2D _buttonBackgroundHover;
        private static Texture2D _buttonBackgroundActive;
        private static Texture2D _buttonBackgroundOnHover;
        private static GUISkin _customSkin;

        public static void EatInputInRect(Rect eatRect)
        {
            var mousePos = UnityInput.Current.mousePosition;
            if (eatRect.Contains(new Vector2(mousePos.x, Screen.height - mousePos.y)))
                UnityInput.Current.ResetInputAxes();
        }

        public static GUISkin CustomSkin
        {
            get
            {
                if (_customSkin == null)
                {
                    try
                    {
                        _customSkin = CreateSkin();
                    }
                    catch (Exception ex)
                    {
                        SubnauticaRuntimeEditorCore.Logger.Log(LogLevel.Warning, "Could not load custom GUISkin - " + ex.Message);
                        _customSkin = GUI.skin;
                    }
                }

                return _customSkin;
            }
        }

        private static void CreateTextures()
        {
            // Load the custom skin from resources

            var texData = ResourceUtils.GetEmbeddedResource("guisharp-box.png");
            _boxBackground = UnityFeatureHelper.LoadTexture(texData);
            Object.DontDestroyOnLoad(_boxBackground);

            texData = ResourceUtils.GetEmbeddedResource("guisharp-window.png");
            _winBackground = UnityFeatureHelper.LoadTexture(texData);
            Object.DontDestroyOnLoad(_winBackground);

            // butons

            texData = ResourceUtils.GetEmbeddedResource("guisharp-button-normal.png");
            _buttonBackgroundNormal = UnityFeatureHelper.LoadTexture(texData);
            Object.DontDestroyOnLoad(_buttonBackgroundNormal);

            texData = ResourceUtils.GetEmbeddedResource("guisharp-button-on.png");
            _buttonBackgroundOn = UnityFeatureHelper.LoadTexture(texData);
            Object.DontDestroyOnLoad(_buttonBackgroundOn);

            texData = ResourceUtils.GetEmbeddedResource("guisharp-button-active.png");
            _buttonBackgroundActive = UnityFeatureHelper.LoadTexture(texData);
            Object.DontDestroyOnLoad(_buttonBackgroundActive);

            texData = ResourceUtils.GetEmbeddedResource("guisharp-button-hover.png");
            _buttonBackgroundHover = UnityFeatureHelper.LoadTexture(texData);
            Object.DontDestroyOnLoad(_buttonBackgroundHover);

            texData = ResourceUtils.GetEmbeddedResource("guisharp-button-onhover.png");
            _buttonBackgroundOnHover = UnityFeatureHelper.LoadTexture(texData);
            Object.DontDestroyOnLoad(_buttonBackgroundOnHover);

        }
        private static GUISkin CreateSkin()
        {
            CreateTextures();

            // Reflection because unity 4.x refuses to instantiate if built with newer versions of UnityEngine
            var newSkin = typeof(Object).GetMethod("Instantiate", BindingFlags.Static | BindingFlags.Public, null, new []{typeof(Object)}, null).Invoke(null, new object[] {GUI.skin}) as GUISkin;
            Object.DontDestroyOnLoad(newSkin);

            newSkin.box.onNormal.background = null;
            newSkin.box.normal.background = _boxBackground;
            newSkin.box.normal.textColor = Color.white;

            newSkin.window.onNormal.background = null;
            newSkin.window.normal.background = _winBackground;
            newSkin.window.padding = new RectOffset(6, 6, 22, 6);
            newSkin.window.border = new RectOffset(10, 10, 20, 10);
            newSkin.window.normal.textColor = Color.white;

            newSkin.button.padding = new RectOffset(4, 4, 3, 3);
            newSkin.button.normal.textColor = Color.white;
            newSkin.button.onHover.textColor = Color.white;
            newSkin.button.normal.background = _buttonBackgroundNormal;
            newSkin.button.onNormal.background = _buttonBackgroundOn;
            newSkin.button.hover.background = _buttonBackgroundHover;
            newSkin.button.onHover.background = _buttonBackgroundOnHover;
            newSkin.button.active.background = _buttonBackgroundActive;

            newSkin.textField.normal.textColor = Color.white;

            newSkin.label.normal.textColor = Color.white;

            newSkin.toggle.stretchWidth = false;
            newSkin.label.stretchWidth = false;

            return newSkin;
        }
    }
}
