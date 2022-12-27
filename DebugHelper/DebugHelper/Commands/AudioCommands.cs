using UnityEngine;
using SMLHelper.V2.Commands;
using System.Collections.Generic;
using DebugHelper.Systems;

namespace DebugHelper.Commands
{
    public static class AudioCommands
    {
        private const float kDefaultMaxDuration = 10f;

        [ConsoleCommand("playsound")]
        public static void PlaySound(string eventPath, float maxDuration = 0f)
        {
            if (string.IsNullOrEmpty(eventPath))
            {
                ErrorMessage.AddMessage("Correct syntax: 'playsound [eventPath]'.");
                return;
            }

            FMODAsset asset = ScriptableObject.CreateInstance<FMODAsset>();
            asset.path = eventPath;
            asset.id = eventPath;

            var go = new GameObject("SoundEmitter");
            var emitter = go.EnsureComponent<FMOD_CustomEmitter>();
            emitter.followParent = true;
            emitter.transform.position = MainCameraControl.main.transform.position;
            emitter.SetAsset(asset);
            emitter.Play();

            ErrorMessage.AddMessage($"Playing FMOD EventInstance by path '{eventPath}'.");

            if (!CheckEmitterEventValid(emitter))
            {
                ErrorMessage.AddMessage("Warning: This FMOD EventInstance appears to be invalid!");
            }

            if (maxDuration == 0f)
            {
                maxDuration = kDefaultMaxDuration;
                ErrorMessage.AddMessage($"Second parameter not set; this sound will stop playing after {maxDuration} seconds.");
            }
            if (maxDuration >= 0f)
            {
                Object.Destroy(go, maxDuration);
            }
        }

        [ConsoleCommand("ps")]
        public static void PlaySoundShorthand(string eventPath, float maxDuration = 0f)
        {
            PlaySound(eventPath, maxDuration);
        }

        [ConsoleCommand("loopsound")]
        public static void PlayLoopingSound(string eventPath, float lifetime)
        {
            if (lifetime < 0f)
            {
                ErrorMessage.AddMessage("This sound will not play; lifetime parameter should be a number of seconds greater than 0! Values below zero will destroy the emitter immediately.");
            }
            if (string.IsNullOrEmpty(eventPath))
            {
                ErrorMessage.AddMessage("Correct syntax: 'loopsound [eventPath, lifetime]'.");
                return;
            }

            FMODAsset asset = ScriptableObject.CreateInstance<FMODAsset>();
            asset.path = eventPath;
            asset.id = eventPath;

            var go = new GameObject("SoundEmitter");
            var emitter = go.EnsureComponent<FMOD_CustomLoopingEmitter>();
            emitter.followParent = true;
            emitter.transform.position = MainCameraControl.main.transform.position;
            emitter.SetAsset(asset);
            emitter.Play();

            ErrorMessage.AddMessage($"Playing FMOD EventInstance by path '{eventPath}'.");

            if (!CheckEmitterEventValid(emitter))
            {
                ErrorMessage.AddMessage("Warning: This FMOD EventInstance appears to be invalid!");
            }

            if (lifetime >= 0f)
            {
                Object.Destroy(go, lifetime);
                ErrorMessage.AddMessage($"Emitter will be destroyed in '{lifetime}' seconds.");
            }
        }

        private static bool CheckEmitterEventValid(FMOD_CustomEmitter emitter)
        {
            FMOD.Studio.EventInstance foundEventInstance = default;
            bool valid = true;
            try
            {
                foundEventInstance = emitter.GetEventInstance();
            }
            finally
            {
                if (!foundEventInstance.isValid())
                {
                    valid = false;
                }
            }
            return valid;
        }

        private static List<SoundEmitterRenderer> renderedSoundEmmiters = new List<SoundEmitterRenderer>();

        [ConsoleCommand("showemitters")]
        public static void ShowEmitters(float inRange, bool hideMessage = false)
        {
            HideEmitters();
            var comparePosition = SNCameraRoot.main.transform.position;
            var actualDistanceThreshold = inRange < 0f ? float.MaxValue : inRange;
            var all = Object.FindObjectsOfType<FMOD_CustomEmitter>();
            var squareDistance = actualDistanceThreshold * actualDistanceThreshold;
            int count = 0;
            foreach (var emitter in all)
            {
                if (Vector3.SqrMagnitude(emitter.transform.position - comparePosition) < squareDistance)
                {
                    var component = emitter.gameObject.EnsureComponent<SoundEmitterRenderer>();
                    component.emitter = emitter;
                    renderedSoundEmmiters.Add(component);
                    count++;
                }
            }
            if (!hideMessage) ErrorMessage.AddMessage($"Showing all {count} FMOD emitters within a range of {actualDistanceThreshold} meters.");
        }

        [ConsoleCommand("hideemitters")]
        public static void HideEmitters()
        {
            foreach (var emitter in renderedSoundEmmiters)
            {
                if (emitter != null)
                {
                    Object.DestroyImmediate(emitter);
                }
            }
            renderedSoundEmmiters.Clear();
        }

        public class SoundEmitterRenderer : BasicDebugIcon
        {
            public FMOD_CustomEmitter emitter;

            private Color orange = new Color(252f / 255, 194f / 255, 0f);

            private bool isLooping;

            private void Start()
            {
                if (emitter != null)
                {
                    if (emitter is FMOD_CustomLoopingEmitter || emitter is FMOD_CustomLoopingEmitterWithCallback)
                    {
                        isLooping = true;
                    }
                }
            }

            private bool Invalid
            {
                get
                {
                    return emitter == null;
                }
            }

            public override string Label
            {
                get
                {
                    if (Invalid) return "Removed";
                    if (emitter.asset == null) return "No asset";
                    return emitter.asset.path;
                }
            }

            public override Sprite Icon
            {
                get
                {
                    if (Invalid)
                    {
                        return DebugIconManager.Icons.Question;
                    }
                    if (isLooping)
                    {
                        return DebugIconManager.Icons.AudioLooping;
                    }
                    return DebugIconManager.Icons.Audio;
                }
            }

            public override Vector3 Position => transform.position;

            public override float Scale
            {
                get
                {
                    if (Invalid || !emitter.playing)
                    {
                        return 1f;
                    }
                    return 1.2f;
                }
            }

            public override Color Color
            {
                get
                {
                    if (Invalid) return invalidColor;
                    if (emitter.followParent) return orange;
                    return Color.white;
                }
            }
        }
    }
}