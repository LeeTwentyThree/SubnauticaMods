using HarmonyLib;

namespace SeaVoyager.Patches;

[HarmonyPatch(typeof(ConstructorInput))]
public static class CraftSoundPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(ConstructorInput.OnCraftingBegin))]
    public static void OnCraftBeginPatch(TechType techType, ref float duration)
    {
        if (techType != Plugin.SeaVoyager.Info.TechType) return;
        
        duration = 20f; // Takes 20 seconds to build
        FMODUWE.PlayOneShot("event:/tools/constructor/spawn", Player.main.transform.position);
    }
}