using System.Collections;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Crafting;
using Nautilus.Utility;
using TheRedPlague.Mono;
using UnityEngine;

namespace TheRedPlague.PrefabFiles;

public static class BoneArmor
{
    public static PrefabInfo BoneArmorInfo { get; } = PrefabInfo.WithTechType("PlagueArmor", "Plague armor", "A rare mutation of the plague that allows a host to survive otherwise fatal contact with the disease. The side effects are unknown.");

    public static void Register()
    {
        var prefab = new CustomPrefab(BoneArmorInfo);
        prefab.SetGameObject(GetPrefab);
        prefab.SetEquipment(EquipmentType.Body);
        prefab.SetRecipe(new RecipeData(new CraftData.Ingredient(TechType.ReinforcedDiveSuit, 1),
                new CraftData.Ingredient(ModPrefabs.WarperHeart.TechType, 1)))
            .WithCraftingTime(5)
            .WithFabricatorType(CraftTree.Type.Fabricator)
            .WithStepsToFabricatorTab("Personal", "Equipment");
        prefab.Register();
    }
    
    private static IEnumerator GetPrefab(IOut<GameObject> prefab)
    {
        var obj = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("BoneArmor_Prefab"));
        obj.SetActive(false);
        
        var material = new Material(MaterialUtils.IonCubeMaterial);
        material.SetColor(ShaderPropertyID._Color, Color.black);
        material.SetColor(ShaderPropertyID._SpecColor, Color.black);
        material.SetColor(ShaderPropertyID._SpecColor, Color.black);
        material.SetColor(ShaderPropertyID._GlowColor, Color.red);
        material.SetFloat(ShaderPropertyID._GlowStrength, 2.2f);
        material.SetFloat(ShaderPropertyID._GlowStrengthNight, 2.2f);
        material.SetColor("_DetailsColor", Color.red);
        material.SetColor("_SquaresColor", new Color(3, 2, 1));
        material.SetFloat("_SquaresTile", 78);
        material.SetFloat("_SquaresSpeed", 8.8f);
        material.SetVector("_NoiseSpeed", new Vector4(0.5f, 0.3f, 0f));
        material.SetVector("_FakeSSSParams", new Vector4(0.2f, 1f, 1f));
        material.SetVector("_FakeSSSSpeed", new Vector4(0.5f, 0.5f, 1.37f));

        var renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers)
        {
            renderer.material = material;
        }
        
        PrefabUtils.AddBasicComponents(obj, BoneArmorInfo.ClassID, BoneArmorInfo.TechType,
            LargeWorldEntity.CellLevel.Near);
        var rb = obj.EnsureComponent<Rigidbody>();
        rb.mass = 100;
        rb.useGravity = false;
        var wf = obj.EnsureComponent<WorldForces>();
        obj.AddComponent<Pickupable>();
        
        prefab.Set(obj);
        yield break;
    }
}