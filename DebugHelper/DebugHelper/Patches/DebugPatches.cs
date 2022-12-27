using DebugHelper.Systems;
using DebugHelper.Systems.Projection;
using HarmonyLib;
using UnityEngine;
using System;

namespace DebugHelper.Patches
{
    [HarmonyPatch(typeof(Debug))]
    internal static class DebugPatches
    {
        [HarmonyPatch(nameof(Debug.DrawLine))]
        [HarmonyPatch(new Type[] { typeof(Vector3), typeof(Vector3), typeof(Color), typeof(float), typeof(bool) })]
        [HarmonyPostfix()]
        public static void DrawLinePatch(Vector3 start, Vector3 end, Color color, float duration, bool depthTest)
        {
            DrawLineInternal(start, end, color, duration, depthTest);
        }

        [HarmonyPatch(nameof(Debug.DrawRay))]
        [HarmonyPatch(new Type[] { typeof(Vector3), typeof(Vector3), typeof(Color), typeof(float), typeof(bool) })]
        [HarmonyPostfix()]
        public static void DrawRayPatch(Vector3 start, Vector3 dir, Color color, float duration, bool depthTest)
        {
            DrawRayInternal(start, dir, color, duration, depthTest);
        }

        public static void DrawLineInternal(Vector3 start, Vector3 end, Color color, float duration, bool depthTest)
        {
            if (Main.config.DebugOverlayEnabled && DebugOverlay.main != null)
            {
                DebugOverlay.main.AddLineThisFrame(new Line(color, start, end, duration, Time.time, Time.frameCount));
            }
        }

        public static void DrawRayInternal(Vector3 start, Vector3 dir, Color color, float duration, bool depthTest)
        {
            DrawLineInternal(start, start + dir * 5000f, color, duration, depthTest);
        }
    }
}
