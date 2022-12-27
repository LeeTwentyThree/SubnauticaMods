using DebugHelper.Systems;
using SMLHelper.V2.Commands;
using UnityEngine;
using System.Collections.Generic;

namespace DebugHelper.Commands
{
    public static class LiveMixinCommands
    {
        private static List<RenderedLiveMixin> renderedLiveMixins = new List<RenderedLiveMixin>();

        [ConsoleCommand("showhealth")]
        public static void ShowHealth(float inRange, bool hideMessage = false)
        {
            HideHealth();
            var comparePosition = SNCameraRoot.main.transform.position;
            var actualDistanceThreshold = inRange < 0f ? float.MaxValue : inRange;
            var all = Object.FindObjectsOfType<LiveMixin>();
            var squareDistance = actualDistanceThreshold * actualDistanceThreshold;
            int count = 0;
            foreach (var lm in all)
            {
                if (Vector3.SqrMagnitude(lm.transform.position - comparePosition) < squareDistance)
                {
                    var component = lm.gameObject.EnsureComponent<RenderedLiveMixin>();
                    component.liveMixin = lm;
                    renderedLiveMixins.Add(component);
                    count++;
                }
            }
            if (!hideMessage) ErrorMessage.AddMessage($"Showing CreatureActions on all {count} Creatures within a range of {actualDistanceThreshold} meters.");
        }

        [ConsoleCommand("hidehealth")]
        public static void HideHealth()
        {
            foreach (var rendered in renderedLiveMixins)
            {
                if (rendered != null)
                {
                    Object.DestroyImmediate(rendered);
                }
            }
            renderedLiveMixins.Clear();
        }

        private class RenderedLiveMixin : BasicDebugIcon
        {
            public LiveMixin liveMixin;

            private static Gradient healthGradient = new Gradient() { colorKeys = new GradientColorKey[] { new GradientColorKey(Color.red, 0f), new GradientColorKey(Color.green, 1f) } };

            private bool Invalid
            {
                get
                {
                    return liveMixin == null;
                }
            }

            public override string Label
            {
                get
                {
                    if (Invalid) return "Unknown";
                    if (liveMixin.maxHealth == 0f) return "Invalid!";
                    return Mathf.RoundToInt(liveMixin.health) + " / " + Mathf.RoundToInt(liveMixin.maxHealth) + " HP\n(" + Mathf.RoundToInt(100f * (liveMixin.health / liveMixin.maxHealth)) + "%)";
                }
            }

            public override Sprite Icon => DebugIconManager.Icons.Health;

            public override Vector3 Position => transform.position + Vector3.up;

            public override float Scale => 0.8f;

            public override Color Color
            {
                get
                {
                    if (Invalid || liveMixin.maxHealth == 0f)
                    {
                        return invalidColor;
                    }
                    return healthGradient.Evaluate(liveMixin.health / liveMixin.maxHealth);
                }
            }
        }
    }
}
