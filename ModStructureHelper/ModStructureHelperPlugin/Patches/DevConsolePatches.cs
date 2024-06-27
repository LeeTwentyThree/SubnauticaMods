using HarmonyLib;
using ModStructureHelperPlugin.UI;

namespace ModStructureHelperPlugin.Patches;

[HarmonyPatch(typeof(DevConsole))]
public static class DevConsolePatches
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(DevConsole.SetState))]
    public static bool SetStatePrefix(DevConsole __instance, bool value)
    {
        var entityWindow = UIEntityWindow.Main;
        return entityWindow == null || !entityWindow.isActiveAndEnabled;
    }
}