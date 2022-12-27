using UnityEngine;
using SMLHelper.V2.Commands;
using System.Collections.Generic;
using UWE;
using DebugHelper.Systems;

namespace DebugHelper.Commands
{
    public static class SkyApplierCommands
    {
        private static List<RenderedSkyApplier> renderedSkyAppliers = new List<RenderedSkyApplier>();

        [ConsoleCommand("showskyappliers")]
        public static void ShowSkyAppliers(float inRange, bool hideMessage = false)
        {
            HideSkyAppliers();
            var comparePosition = SNCameraRoot.main.transform.position;
            var actualDistanceThreshold = inRange < 0f ? float.MaxValue : inRange;
            var all = Object.FindObjectsOfType<SkyApplier>();
            var squareDistance = actualDistanceThreshold * actualDistanceThreshold;
            int count = 0;
            foreach (var skyApplier in all)
            {
                if (Vector3.SqrMagnitude(skyApplier.transform.position - comparePosition) < squareDistance)
                {
                    var component = skyApplier.gameObject.EnsureComponent<RenderedSkyApplier>();
                    component.attachedSkyApplier = skyApplier;
                    renderedSkyAppliers.Add(component);
                    count++;
                }
            }
            if (!hideMessage) ErrorMessage.AddMessage($"Showing all {count} SkyAppliers within a range of {actualDistanceThreshold} meters.");
        }

        [ConsoleCommand("hideskyappliers")]
        public static void HideSkyAppliers()
        {
            foreach (var rendered in renderedSkyAppliers)
            {
                if (rendered != null)
                {
                    Object.DestroyImmediate(rendered);
                }
            }
            renderedSkyAppliers.Clear();
        }

        private class RenderedSkyApplier : BasicDebugIcon
        {
            public SkyApplier attachedSkyApplier;

            public override string Label
            {
                get
                {
                    if (attachedSkyApplier == null) return "Null";
                    if (attachedSkyApplier.applySky != null)
                    {
                        return attachedSkyApplier.applySky.name;
                    }
                    return gameObject.name;
                }
            }

            public override Sprite Icon
            {
                get
                {
                    if (attachedSkyApplier == null)
                    {
                        return DebugIconManager.Icons.Question;
                    }
                    return DebugIconManager.Icons.Globe;
                }
            }

            public override Vector3 Position => transform.position;

            public override float Scale => 1f;

            private static Color notAffectedByDayNightCycleColor = new Color(0f, 184f / 255, 52f / 255);

            public override Color Color
            {
                get
                {
                    if (attachedSkyApplier == null)
                    {
                        return invalidColor;
                    }
                    var sky = attachedSkyApplier.applySky;
                    if (sky == null)
                    {
                        return Color.black;
                    }
                    if (sky.Outdoors)
                    {
                        if (!sky.AffectedByDayNightCycle)
                            return notAffectedByDayNightCycleColor;
                        return Color.blue;
                    }
                    else
                    {
                        return Color.yellow;
                    }
                }
            }
        }
    }
}
