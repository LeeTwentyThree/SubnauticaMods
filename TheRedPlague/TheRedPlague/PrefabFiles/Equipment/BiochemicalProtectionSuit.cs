using System;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Crafting;
using Nautilus.Handlers;
using TheRedPlague.Patches;
using UnityEngine;

namespace TheRedPlague.PrefabFiles.Equipment;

public static class BiochemicalProtectionSuit
{
    public static PrefabInfo Info { get; } = PrefabInfo.WithTechType("BiochemicalProtectionModule")
        .WithIcon(Plugin.AssetBundle.LoadAsset<Sprite>("BiochemicalProtectionModuleIcon"));

    public static void Register()
    {
        var prefab = new CustomPrefab(Info);
        var template = new CloneTemplate(Info, TechType.MapRoomUpgradeScanRange);
        template.ModifyPrefab += obj =>
        {
            // i hate this fucking class name
            var eatable = obj.AddComponent<Eatable>();
            eatable.foodValue = 0;
            eatable.waterValue = 0;
            eatable.decomposes = false;
            eatable.despawnDelay = EatablePatches.MagicBiochemicalProtectionModuleDespawnDelayAmount;

            obj.GetComponentInChildren<VFXFabricating>().posOffset = default;
        };
        prefab.SetPdaGroupCategory(TechGroup.Personal, TechCategory.Equipment);
        prefab.SetRecipe(new RecipeData(
                new CraftData.Ingredient(TechType.ComputerChip), new CraftData.Ingredient(TechType.Silicone, 2)))
            .WithFabricatorType(CraftTree.Type.Fabricator)
            .WithCraftingTime(5)
            .WithStepsToFabricatorTab(CraftTreeHandler.Paths.FabricatorEquipment);
        prefab.SetGameObject(template);
        prefab.Register();
        KnownTechHandler.SetAnalysisTechEntry(Info.TechType,
            Array.Empty<TechType>(), KnownTechHandler.DefaultUnlockData.BasicUnlockSound,
            Plugin.AssetBundle.LoadAsset<Sprite>("BiochemicalProtectionModulePopup"));
    }
}