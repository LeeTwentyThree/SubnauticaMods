using System.Linq;
using HarmonyLib;
using TheRedPlague.Mono;
using TheRedPlague.PrefabFiles;
using UnityEngine;

namespace TheRedPlague.Patches;

[HarmonyPatch(typeof(Player))]
public static class PlayerPatcher
{
    [HarmonyPatch(nameof(Player.Start))]
    [HarmonyPostfix]
    public static void StartPostfix(Player __instance)
    {
        MainCamera.camera.farClipPlane = 99999;
        __instance.gameObject.EnsureComponent<RandomFishSpawner>();
        __instance.gameObject.EnsureComponent<JumpScares>();
        __instance.gameObject.AddComponent<EnzymeRainController>();
        MainCamera.camera.gameObject.AddComponent<PlagueScreenFXController>();
    }
    
    [HarmonyPatch(nameof(Player.EquipmentChanged))]
    [HarmonyPostfix]
    public static void EquipmentChangedPostfix(Player __instance)
    {
        var equipment = Inventory.main.equipment;
        __instance.gameObject.EnsureComponent<PlagueArmorBehavior>()
            .SetArmorActive(equipment.GetTechTypeInSlot("Body") == BoneArmor.Info.TechType);
    }
}