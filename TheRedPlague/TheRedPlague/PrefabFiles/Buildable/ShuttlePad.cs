using System.Collections;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Crafting;
using Nautilus.Utility;
using UnityEngine;

namespace TheRedPlague.PrefabFiles.Buildable;

public static class ShuttlePad
{
    public static PrefabInfo Info { get; } = PrefabInfo.WithTechType("ShuttlePad", false);

    public static void Register()
    {
        var prefab = new CustomPrefab(Info);
        prefab.SetGameObject(CreatePrefab);
        prefab.SetPdaGroupCategory(TechGroup.ExteriorModules, TechCategory.ExteriorModule);
        prefab.SetRecipe(new RecipeData(new CraftData.Ingredient(
                TechType.TitaniumIngot),
            new CraftData.Ingredient(TechType.ComputerChip),
            new CraftData.Ingredient(TechType.Lubricant)));
        prefab.Register();
    }

    private static IEnumerator CreatePrefab(IOut<GameObject> obj)
    {
        var pad = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("ShuttlePad_Prefab"));
        pad.SetActive(false);
        PrefabUtils.AddBasicComponents(pad, Info.ClassID, Info.TechType, LargeWorldEntity.CellLevel.Global);
        MaterialUtils.ApplySNShaders(pad, 5, 1, 3);
        var constructable = PrefabUtils.AddConstructable(pad, Info.TechType,
            ConstructableFlags.Outside | ConstructableFlags.Rotatable | ConstructableFlags.AllowedOnConstructable
            | ConstructableFlags.Ground, pad.transform.Find("LandingpadWholey").gameObject);
        constructable.forceUpright = true;
        constructable.placeDefaultDistance = 10;
        constructable.placeMinDistance = 2;
        constructable.placeMaxDistance = 20;
        obj.Set(pad);
        yield break;
    }
}