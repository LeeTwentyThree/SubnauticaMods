using System.Collections;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Crafting;
using Nautilus.Handlers;
using Nautilus.Utility;
using UnityEngine;

namespace TheRedPlague.PrefabFiles;

public static class BoneArmor
{
    public static PrefabInfo Info { get; } = PrefabInfo.WithTechType("PlagueArmor")
        .WithIcon(Plugin.AssetBundle.LoadAsset<Sprite>("BoneArmorIcon"))
        .WithSizeInInventory(new Vector2int(2, 2));

    public static void Register()
    {
        var prefab = new CustomPrefab(Info);
        prefab.SetGameObject(GetPrefab);
        prefab.SetEquipment(EquipmentType.Body);
        prefab.SetRecipe(new RecipeData(new CraftData.Ingredient(TechType.ReinforcedDiveSuit, 1),
                new CraftData.Ingredient(ModPrefabs.WarperHeart.TechType, 1),
                new CraftData.Ingredient(ModPrefabs.AmalgamatedBone.TechType, 8)))
            .WithCraftingTime(5)
            .WithFabricatorType(CraftTree.Type.Workbench)
            .WithStepsToFabricatorTab("PlagueEquipment");
        prefab.SetPdaGroupCategory(TechGroup.Personal, TechCategory.Equipment);
        prefab.Register();
        
        KnownTechHandler.SetAnalysisTechEntry(Info.TechType, System.Array.Empty<TechType>(),
            KnownTechHandler.DefaultUnlockData.BlueprintUnlockSound,
            Plugin.AssetBundle.LoadAsset<Sprite>("BoneArmorPopup"));
    }

    public static Material GetMaterial()
    {
        var material = new Material(MaterialUtils.IonCubeMaterial);
        material.SetColor(ShaderPropertyID._Color, Color.black);
        material.SetColor(ShaderPropertyID._SpecColor, Color.black);
        material.SetColor(ShaderPropertyID._SpecColor, Color.black);
        material.SetColor(ShaderPropertyID._GlowColor, Color.red);
        material.SetFloat(ShaderPropertyID._GlowStrength, 2.2f);
        material.SetFloat(ShaderPropertyID._GlowStrengthNight, 2.2f);
        material.SetColor("_DetailsColor", Color.red);
        material.SetColor("_SquaresColor", new Color(3, 2, 1));
        material.SetFloat("_SquaresTile", 100);
        material.SetFloat("_SquaresSpeed", 8.8f);
        material.SetVector("_NoiseSpeed", new Vector4(0.5f, 0.3f, 0f));
        material.SetVector("_FakeSSSParams", new Vector4(0.2f, 1f, 1f));
        material.SetVector("_FakeSSSSpeed", new Vector4(0.5f, 0.5f, 1.37f));

        return material;
    }

    private static IEnumerator GetPrefab(IOut<GameObject> prefab)
    {
        var obj = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("BoneArmor_Prefab"));
        obj.SetActive(false);

        var material = GetMaterial();

        var renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers)
        {
            renderer.material = material;
        }

        PrefabUtils.AddBasicComponents(obj, Info.ClassID, Info.TechType,
            LargeWorldEntity.CellLevel.Near);
        var rb = obj.EnsureComponent<Rigidbody>();
        rb.mass = 100;
        rb.useGravity = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        var wf = obj.EnsureComponent<WorldForces>();
        wf.useRigidbody = rb;
        obj.AddComponent<Pickupable>();

        prefab.Set(obj);
        yield break;
    }
}