using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Crafting;
using Nautilus.Handlers;
using Nautilus.Utility;
using PdaUpgradeChips.MonoBehaviours.Upgrades;
using UnityEngine;

namespace PdaUpgradeChips.Prefabs;

public class UpgradeChipPrefab<T> where T : UpgradeChipBase
{
    public PrefabInfo Info { get; }
    
    public bool InModificationStation { private get; init; }

    private readonly RecipeData _recipe;

    public UpgradeChipPrefab(PrefabInfo info, RecipeData recipe)
    {
        Info = info;
        _recipe = recipe;
    }

    public void Register()
    {
        var prefab = new CustomPrefab(Info);
        prefab.SetGameObject(GetPrefab);
        prefab.SetEquipment(PdaUpgradesAPI.PdaUpgradeEquipmentType);
        prefab.SetRecipe(_recipe)
            .WithCraftingTime(3)
            .WithFabricatorType(InModificationStation ? CraftTree.Type.Workbench : CraftTree.Type.Fabricator)
            .WithStepsToFabricatorTab(InModificationStation ? null : CraftTreeHandler.Paths.FabricatorEquipment);
        prefab.SetPdaGroupCategory(PdaUpgradesAPI.PdaUpgradesTechGroup, PdaUpgradesAPI.PdaUpgradesTechCategory);
        prefab.Register();
        PdaUpgradesAPI.RegisterUpgradeChip(Info.TechType, typeof(T));
    }

    private GameObject GetPrefab()
    {
        var obj = Object.Instantiate(Plugin.Bundle.LoadAsset<GameObject>("PDAUpgradeChipPrefab"));
        obj.SetActive(false);
        PrefabUtils.AddBasicComponents(obj, Info.ClassID, Info.TechType, LargeWorldEntity.CellLevel.Near);
        obj.AddComponent<Pickupable>();
        MaterialUtils.ApplySNShaders(obj, 7);
        PrefabUtils.AddVFXFabricating(obj, "PDAUpgradeChip", -0.02f, 0.01f,
            new Vector3(0, InModificationStation ? 0.01f : -0.01f, 0), 4f, new Vector3(-90, 0, 0));
        PrefabUtils.AddWorldForces(obj, 1.2f);
        return obj;
    }
}