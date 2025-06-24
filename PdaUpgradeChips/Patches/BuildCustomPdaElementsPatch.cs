using HarmonyLib;

namespace PdaUpgradeChips.Patches;

[HarmonyPatch(typeof(uGUI_PDA))]
internal static class BuildCustomPdaElementsPatch
{
    public delegate void BuildUIElement(uGUI_PDA pda);
    
    public static event BuildUIElement BuildUIElementEvent; 
    
    [HarmonyPatch(nameof(uGUI_PDA.Initialize))]
    [HarmonyPostfix]
    public static void InitializePostfix(uGUI_PDA __instance)
    {
        BuildUIElementEvent?.Invoke(__instance);
    }
}