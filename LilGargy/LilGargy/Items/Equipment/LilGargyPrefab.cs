using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Crafting;
using Nautilus.Utility;
using UnityEngine;

namespace LilGargy.Items.Equipment;

public static class LilGargyPrefab
{
    public static PrefabInfo Info { get; } = PrefabInfo
        .WithTechType("LilGargy", true)
        .WithIcon(Plugin.Bundle.LoadAsset<Sprite>("LilGargyIcon"));

    public static void Register()
    {
        var customPrefab = new CustomPrefab(Info);
        
        customPrefab.SetGameObject(GetGameObject);
        customPrefab.SetRecipe(new RecipeData(new Ingredient(TechType.FiberMesh, 1), new Ingredient(TechType.CreepvineSeedCluster, 1)))
            .WithFabricatorType(CraftTree.Type.Fabricator)
            .WithStepsToFabricatorTab("Machines")
            .WithCraftingTime(10);
        customPrefab.SetEquipment(EquipmentType.Hand);
        customPrefab.SetPdaGroupCategory(TechGroup.Machines, TechCategory.Machines);
        customPrefab.Register();
    }

    private static GameObject GetGameObject()
    {
        var prefab = Object.Instantiate(Plugin.Bundle.LoadAsset<GameObject>("LilGargyPrefab"));
        prefab.SetActive(false);
        PrefabUtils.AddBasicComponents(prefab, Info.ClassID, Info.TechType, LargeWorldEntity.CellLevel.Near);
        MaterialUtils.ApplySNShaders(prefab, 5f, 5f, 10);
        var rb = prefab.AddComponent<Rigidbody>();
        rb.useGravity = false;
        var wf = prefab.AddComponent<WorldForces>();
        wf.useRigidbody = rb;
        wf.underwaterGravity = -1;
        prefab.AddComponent<Pickupable>();
        var placeTool = prefab.AddComponent<PlaceTool>();
        placeTool.allowedOutside = true;
        placeTool.rotationEnabled = true;
        placeTool.hasAnimations = false;
        var fpModel = prefab.AddComponent<FPModel>();
        fpModel.propModel = prefab.transform.Find("worldmodel").gameObject;
        fpModel.viewModel = prefab.transform.Find("fpmodel").gameObject;
        var bounds = prefab.AddComponent<ConstructableBounds>();
        bounds.bounds = new OrientedBounds(new Vector3(0, 0.2f, 0), Quaternion.identity, new Vector3(0.2f, 0.1f, 0.2f));
        PrefabUtils.AddVFXFabricating(prefab, "worldmodel", -0.01f, 0.3f, default, 0.2f);
        return prefab;
    }
}