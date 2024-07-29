using HarmonyLib;
using TheRedPlague.Mono;
using TheRedPlague.Mono.PlagueCyclops;
using TheRedPlague.PrefabFiles.UpgradeModules;

namespace TheRedPlague.Patches;

[HarmonyPatch(typeof(SubRoot))]
public static class SubRootPatches
{
    [HarmonyPatch(nameof(SubRoot.Start))]
    [HarmonyPostfix]
    public static void StartPostfix(SubRoot __instance)
    {
        if (__instance.isCyclops && __instance.gameObject.name.ToLower().Contains("cyclops"))
        {
            __instance.gameObject.AddComponent<CyclopsClogEngines>().sub = __instance;
        }
        
        UpdatePlagueCyclopsCore(__instance, true);
    }
    
    [HarmonyPatch(nameof(SubRoot.UpdateSubModules))]
    [HarmonyPrefix]
    public static void UpdateSubModulesPrefix(SubRoot __instance)
    {
        if (__instance.subModulesDirty)
        {
            UpdatePlagueCyclopsCore(__instance, false);
        }
    }

    private static void UpdatePlagueCyclopsCore(SubRoot sub, bool forInstantConversion)
    {
        if (sub.upgradeConsole == null || sub.upgradeConsole.modules == null) return;
        var hasPlagueCyclopsCore = false;
        var modules = sub.upgradeConsole.modules;
        for (var i = 0; i < 6; i++)
        {
            var slot = SubRoot.slotNames[i];
            var techTypeInSlot = modules.GetTechTypeInSlot(slot);
            if (techTypeInSlot != PlagueCyclopsCore.Info.TechType) continue;
            hasPlagueCyclopsCore = true;
            break;
        }

        if (hasPlagueCyclopsCore)
        {
            PlagueCyclopsBehavior.ConvertPlagueCyclops(sub.gameObject, forInstantConversion);
        }
    }
}