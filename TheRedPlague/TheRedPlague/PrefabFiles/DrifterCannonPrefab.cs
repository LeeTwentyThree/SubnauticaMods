using System.Collections;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Utility;
using TheRedPlague.Mono.Tools;
using UnityEngine;

namespace TheRedPlague.PrefabFiles;

public static class DrifterCannonPrefab
{
    public static PrefabInfo Info { get; } = PrefabInfo.WithTechType("DrifterCannon")
        .WithSizeInInventory(new Vector2int(2, 2))
        .WithIcon(Plugin.AssetBundle.LoadAsset<Sprite>("DrifterCannonIcon"));

    public static void Register()
    {
        var prefab = new CustomPrefab(Info);
        prefab.SetGameObject(GetPrefabAsync);
        prefab.SetEquipment(EquipmentType.Hand);
        prefab.Register();
    }

    private static IEnumerator GetPrefabAsync(IOut<GameObject> prefab)
    {
        var obj = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("DrifterCannonPrefab"));
        obj.SetActive(false);
        var rb = obj.EnsureComponent<Rigidbody>();
        rb.mass = 50;
        rb.useGravity = false;
        var wf = obj.EnsureComponent<WorldForces>();
        wf.useRigidbody = rb;
        wf.aboveWaterDrag = 0.15f;
        wf.underwaterDrag = 0.3f;
        PrefabUtils.AddBasicComponents(obj, Info.ClassID, Info.TechType, LargeWorldEntity.CellLevel.Near);
        MaterialUtils.ApplySNShaders(obj, 6f, 2f, 0.5f);
        var fpModel = obj.AddComponent<FPModel>();
        fpModel.propModel = obj.transform.Find("DrifterCannonAnimated").gameObject;
        fpModel.viewModel = obj.transform.Find("DrifterCannonAnimated_ViewModel").gameObject;
        var tool = obj.AddComponent<DrifterCannonTool>();
        tool.pickupable = obj.EnsureComponent<Pickupable>();
        tool.mainCollider = obj.GetComponent<Collider>();
        tool.animator = fpModel.viewModel.GetComponent<Animator>();
        tool.hasAnimations = true;
        tool.socket = PlayerTool.Socket.RightHand;
        tool.ikAimLeftArm = false;
        tool.ikAimRightArm = true;

        var projectile = obj.transform.Find("DrifterProjectilePrefab").gameObject;
        var projectileWf = projectile.AddComponent<WorldForces>();
        projectileWf.useRigidbody = projectile.GetComponent<Rigidbody>();

        tool.projectilePrefab = projectile;
        tool.projectileSpawnPoint = fpModel.viewModel.transform.Find("ProjectileSpawnPoint");
        tool.glowRenderer = fpModel.viewModel.transform.Find("DrifterCannon").gameObject.GetComponent<Renderer>();
        
        prefab.Set(obj);
        yield break;
    }
}