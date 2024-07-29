using UnityEngine;

using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Utility;
using Nautilus.Utility.MaterialModifiers;

namespace TheRedPlague.PrefabFiles.UpgradeModules;

public static class PlagueCyclopsCore
{
    public static PrefabInfo Info { get; } = PrefabInfo.WithTechType("PlagueCyclopsCore")
        .WithIcon(Plugin.AssetBundle.LoadAsset<Sprite>("PlagueCyclopsCoreIcon"))
        .WithSizeInInventory(new Vector2int(2, 2));

    public static void Register()
    {
        var prefab = new CustomPrefab(Info);
        prefab.SetGameObject(GetGameObject);
        prefab.SetEquipment(EquipmentType.CyclopsModule);
        prefab.Register();
    }

    private static GameObject GetGameObject()
    {
        var obj = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("PlagueCyclopsCore_Prefab"));
        obj.SetActive(false);
        PrefabUtils.AddBasicComponents(obj, Info.ClassID, Info.TechType, LargeWorldEntity.CellLevel.Near);
        var rb = obj.AddComponent<Rigidbody>();
        rb.mass = 8;
        rb.isKinematic = true;
        rb.useGravity = false;
        var wf = obj.AddComponent<WorldForces>();
        wf.useRigidbody = rb;
        MaterialUtils.ApplySNShaders(obj, 6, 1, 1, new Modifier());
        obj.AddComponent<Pickupable>();
        return obj;
    }

    private class Modifier : MaterialModifier
    {
        public override void EditMaterial(Material material, Renderer renderer, int materialIndex, MaterialUtils.MaterialType materialType)
        {
            if (renderer.name.ToLower().Contains("container"))
            {
                material.SetFloat("_SpecInt", 3);
                material.SetColor(ShaderPropertyID._SpecColor, new Color(1, 0.5f, 0.5f));
                material.SetFloat("_Fresnel", 0.3f);
            }
        }
    }
}