using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Crafting;
using Nautilus.Utility;
using UnityEngine;
using Ingredient = CraftData.Ingredient;

namespace YoutoozGargStatue.Items.Equipment;

public static class GargStatuePrefab
{
    public static PrefabInfo Info { get; } = PrefabInfo
        .WithTechType("YoutoozGargStatue", true)
        .WithIcon(Plugin.Bundle.LoadAsset<Sprite>("YoutoozGargIcon"));

    public static void Register()
    {
        var customPrefab = new CustomPrefab(Info);
        
        customPrefab.SetGameObject(GetGameObject);
        customPrefab.SetRecipe(new RecipeData(new Ingredient(TechType.Silicone), new Ingredient(TechType.CreepvineSeedCluster)))
            .WithFabricatorType(CraftTree.Type.Fabricator)
            .WithStepsToFabricatorTab("Machines")
            .WithCraftingTime(13);
        customPrefab.SetEquipment(EquipmentType.Hand);
        customPrefab.SetPdaGroupCategory(TechGroup.Machines, TechCategory.Machines);
        customPrefab.Register();
    }

    private static GameObject GetGameObject()
    {
        var prefab = Object.Instantiate(Plugin.Bundle.LoadAsset<GameObject>("YoutoozGargPrefab"));
        prefab.SetActive(false);
        PrefabUtils.AddBasicComponents(prefab, Info.ClassID, Info.TechType, LargeWorldEntity.CellLevel.Near);
        MaterialUtils.ApplySNShaders(prefab, 5.9f, 2f);
        PrefabUtils.AddWorldForces(prefab, 14f, 0.9f, 1, true);
        prefab.AddComponent<Pickupable>();
        var placeTool = prefab.AddComponent<PlaceTool>();
        placeTool.allowedOutside = true;
        placeTool.rotationEnabled = true;
        placeTool.hasAnimations = false;
        placeTool.mainCollider = prefab.GetComponent<Collider>();
        var fpModel = prefab.AddComponent<FPModel>();
        fpModel.propModel = prefab.transform.Find("worldmodel").gameObject;
        fpModel.viewModel = prefab.transform.Find("fpmodel").gameObject;
        var bounds = prefab.AddComponent<ConstructableBounds>();
        bounds.bounds = new OrientedBounds(new Vector3(0, 0.2f, 0), Quaternion.identity, new Vector3(0.2f, 0.1f, 0.2f));
        PrefabUtils.AddVFXFabricating(prefab, "worldmodel", -0.03f, 0.5f, default, 0.5f);
        return prefab;
    }
}