using System.Collections;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Crafting;
using Nautilus.Utility;
using TheRedPlague.MaterialModifiers;
using TheRedPlague.Mono.Tools;
using TMPro;
using UnityEngine;

namespace TheRedPlague.PrefabFiles;

public static class AirStrikeDevice
{
    public static PrefabInfo Info { get; } = PrefabInfo.WithTechType("AirStrikeDevice", true)
        .WithIcon(Plugin.AssetBundle.LoadAsset<Sprite>("AirStrikeDeviceIcon"));

    public static void Register()
    {
        var prefab = new CustomPrefab(Info);
        prefab.SetGameObject(GetPrefab);
        prefab.SetEquipment(EquipmentType.Hand);
        prefab.SetRecipe(new RecipeData(new CraftData.Ingredient(TechType.Titanium, 2),
                new CraftData.Ingredient(TechType.WiringKit)))
            .WithCraftingTime(4)
            .WithFabricatorType(AdminFabricator.AdminCraftTree);
        prefab.Register();
    }

    private static IEnumerator GetPrefab(IOut<GameObject> prefab)
    {
        var obj = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("AirStrikeDevicePrefab"));
        obj.SetActive(false);
        var rb = obj.EnsureComponent<Rigidbody>();
        rb.mass = 3;
        rb.useGravity = false;
        var wf = obj.EnsureComponent<WorldForces>();
        wf.useRigidbody = rb;
        PrefabUtils.AddBasicComponents(obj, Info.ClassID, Info.TechType, LargeWorldEntity.CellLevel.Near);
        MaterialUtils.ApplySNShaders(obj, 7f, 1, 1f, new AirStrikeDeviceModifier());
        var fpModel = obj.AddComponent<FPModel>();
        fpModel.propModel = obj.transform.Find("WorldModel").gameObject;
        fpModel.viewModel = obj.transform.Find("FPModel").gameObject;
        var tool = obj.AddComponent<AirStrikeTool>();
        tool.pickupable = obj.EnsureComponent<Pickupable>();
        tool.mainCollider = obj.GetComponent<Collider>();
        tool.hasAnimations = true;
        tool.socket = PlayerTool.Socket.RightHand;
        tool.ikAimLeftArm = true;
        tool.ikAimRightArm = true;
        obj.GetComponentInChildren<TextMeshProUGUI>().font = FontUtils.Aller_W_Bd;
        prefab.Set(obj);
        PrefabUtils.AddVFXFabricating(obj, "WorldModel", 0, 0.2f, Vector3.zero, 1, new Vector3(270, 0, 0));
        yield break;
    }
}