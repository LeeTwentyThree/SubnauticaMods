using System;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using SubnauticaRuntimeEditor.Core;
using SubnauticaRuntimeEditor.Core.Utils.Abstractions;
using UnityEngine;
using HarmonyLib;
using System.Reflection;

namespace SubnauticaRuntimeEditor.Plugin
{
    [BepInPlugin(SubnauticaRuntimeEditorCore.GUID, "Subnautica Runtime Editor", SubnauticaRuntimeEditorCore.Version)]
    public class SubnauticaRuntimeEditorPlugin : BaseUnityPlugin
    {
        public static SubnauticaRuntimeEditorCore Instance { get; private set; }

        private void Start()
        {
            if (!TomlTypeConverter.CanConvert(typeof(Rect)))
            {
                var converter = Core.Utils.TomlTypeConverter.GetConverter(typeof(Rect));
                TomlTypeConverter.AddConverter(typeof(Rect), new TypeConverter { ConvertToObject = converter.ConvertToObject, ConvertToString = converter.ConvertToString });
            }

            Instance = new SubnauticaRuntimeEditorCore(new SubnauticaRuntimeEditorSettings(this));

            var harmony = new Harmony("Lee23.SubnauticaRuntimeEditor");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            gameObject.EnsureComponent(typeof(SceneCleanerPreserve));
        }

        private void Update()
        {
            Instance.Update();
        }

        private void LateUpdate()
        {
            Instance.LateUpdate();
        }

        private void OnGUI()
        {
            Instance.OnGUI();
        }

        private sealed class SubnauticaRuntimeEditorSettings : InitSettings
        {
            private readonly Plugin.SubnauticaRuntimeEditorPlugin _instance;

            public SubnauticaRuntimeEditorSettings(Plugin.SubnauticaRuntimeEditorPlugin instance)
            {
                _instance = instance;
                LoggerWrapper = new Logger5(_instance.Logger);
            }

            public override Action<T> RegisterSetting<T>(string category, string name, T defaultValue, string description, Action<T> onValueUpdated)
            {
                var s = _instance.Config.Bind(category, name, defaultValue, description);
                s.SettingChanged += (sender, args) => onValueUpdated(s.Value);
                onValueUpdated(s.Value);
                return x => s.Value = x;
            }

            public override MonoBehaviour PluginMonoBehaviour => _instance;
            public override ILoggerWrapper LoggerWrapper { get; }
            public override string ConfigPath => Paths.ConfigPath;
        }

        private sealed class Logger5 : ILoggerWrapper
        {
            private readonly ManualLogSource _logger;

            public Logger5(ManualLogSource logger)
            {
                _logger = logger;
            }

            public void Log(Core.Utils.Abstractions.LogLevel logLevel, object content)
            {
                _logger.Log((BepInEx.Logging.LogLevel)logLevel, content);
            }
        }
    }
}
