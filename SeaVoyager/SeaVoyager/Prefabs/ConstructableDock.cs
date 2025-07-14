using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Crafting;
using Nautilus.Utility;
using SeaVoyager.Mono;
using UnityEngine;

namespace SeaVoyager.Prefabs;

public static class ConstructableDock
{
    public static PrefabInfo Info { get; } = PrefabInfo.WithTechType("SuspendedDock")
        .WithIcon(Plugin.assetBundle.LoadAsset<Sprite>("SuspendedDockIcon"));

    public static void Register()
    {
        var prefab = new CustomPrefab(Info);
        prefab.SetGameObject(GetPrefab);
        prefab.SetRecipe(new RecipeData(new CraftData.Ingredient(TechType.Titanium, 2),
            new CraftData.Ingredient(TechType.CopperWire, 2), new CraftData.Ingredient(TechType.Silicone, 4)));
        prefab.SetPdaGroupCategoryAfter(TechGroup.ExteriorModules, TechCategory.ExteriorModule, TechType.Spotlight);
        prefab.Register();
    }

    private static GameObject GetPrefab()
    {
        var obj = Object.Instantiate(Plugin.assetBundle.LoadAsset<GameObject>("BuildableDock"));
        obj.SetActive(false);
        PrefabUtils.AddBasicComponents(obj, Info.ClassID, Info.TechType, LargeWorldEntity.CellLevel.Global);
        MaterialUtils.ApplySNShaders(obj, 6);
        
        var dock = obj.AddComponent<SuspendedDock>();
        dock.Initialize();

        var constructable = PrefabUtils.AddConstructable(obj, Info.TechType,
            ConstructableFlags.Outside | ConstructableFlags.AllowedOnConstructable |
            ConstructableFlags.Ground, obj.transform.Find("DockArmBase").gameObject);
        constructable.allowedUnderwater = false;
        constructable.placeMinDistance = 1.3f;
        constructable.placeMaxDistance = 14;
        constructable.placeDefaultDistance = 8;

        obj.transform.Find("CableTrigger").localScale = Vector3.one * 4;
        
        return obj;
    }
}