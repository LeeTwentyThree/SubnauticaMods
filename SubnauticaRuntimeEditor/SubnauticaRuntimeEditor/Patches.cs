using HarmonyLib;
using UnityEngine;

namespace SubnauticaRuntimeEditor.Plugin
{
    [HarmonyPatch(typeof(UWE.Utils))]
    internal class UtilsPatches
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(UWE.Utils.UpdateCusorLockState))]
        public static bool UpdateCusorLockStatePrefix()
        {
            if (SubnauticaRuntimeEditorPlugin.Instance.Show)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                return false;
            }
            return true;
        }
    }
}
