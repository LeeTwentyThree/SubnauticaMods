using HarmonyLib;
using UnityEngine.UI;

namespace LongerSigns;

public static class Patches
{
    [HarmonyPatch(typeof(uGUI_SignInput), nameof(uGUI_SignInput.Awake))]
    [HarmonyPostfix]
    public static void uGUI_SignInputAwakePostfix(uGUI_SignInput __instance)
    {
        var inputField = __instance.GetComponentInChildren<uGUI_InputField>();
        if (inputField == null)
            return;

        inputField.characterLimit = int.MaxValue;

        // inputField.lineLimit = int.MaxValue;
        // inputField.lineType = TMPro.TMP_InputField.LineType.MultiLineNewline;
    }
}
