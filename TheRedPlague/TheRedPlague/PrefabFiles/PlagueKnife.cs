using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Crafting;
using Nautilus.Handlers;
using Nautilus.Utility;
using TheRedPlague.Mono.Tools;
using UnityEngine;

namespace TheRedPlague.PrefabFiles;

public static class PlagueKnife
{
    public static PrefabInfo Info { get; } = PrefabInfo.WithTechType("PlagueKnife")
        .WithIcon(Plugin.AssetBundle.LoadAsset<Sprite>("PlagueKnifeIcon"));

    public static void Register()
    {
        var plagueKnife = new CustomPrefab(Info);
        var plagueKnifeTemplate = new CloneTemplate(Info, TechType.Knife);
        plagueKnifeTemplate.ModifyPrefab += ModifyPrefab;
        plagueKnife.SetGameObject(plagueKnifeTemplate);
        plagueKnife.SetEquipment(EquipmentType.Hand);
        plagueKnife.SetRecipe(new RecipeData(new CraftData.Ingredient(TechType.Knife, 1),
                new CraftData.Ingredient(ModPrefabs.WarperHeart.TechType, 3)))
            .WithCraftingTime(5)
            .WithFabricatorType(CraftTree.Type.Workbench)
            .WithStepsToFabricatorTab("PlagueEquipment");
        plagueKnife.SetPdaGroupCategory(TechGroup.Personal, TechCategory.Tools);
        plagueKnife.Register();
        KnownTechHandler.SetAnalysisTechEntry(ModPrefabs.AmalgamatedBone.TechType, new[] {Info.TechType},
            KnownTechHandler.DefaultUnlockData.BlueprintUnlockSound,
            Plugin.AssetBundle.LoadAsset<Sprite>("PlagueKnifePopup"));
    }

    private static void ModifyPrefab(GameObject prefab)
    {
        var renderer = prefab.GetComponentInChildren<Renderer>();
        var material = new Material(MaterialUtils.IonCubeMaterial);
        material.SetColor(ShaderPropertyID._Color, Color.black);
        material.SetColor(ShaderPropertyID._SpecColor, Color.black);
        material.SetColor(ShaderPropertyID._SpecColor, Color.black);
        material.SetColor(ShaderPropertyID._GlowColor, Color.red);
        material.SetFloat(ShaderPropertyID._GlowStrength, 2.2f);
        material.SetFloat(ShaderPropertyID._GlowStrengthNight, 2.2f);
        material.SetColor("_DetailsColor", Color.red);
        material.SetColor("_SquaresColor", new Color(3, 2, 1));
        material.SetFloat("_SquaresTile", 5.6f);
        material.SetFloat("_SquaresSpeed", 8.8f);
        material.SetVector("_NoiseSpeed", new Vector4(0.5f, 0.3f, 0f));
        material.SetVector("_FakeSSSParams", new Vector4(0.2f, 1f, 1f));
        material.SetVector("_FakeSSSSpeed", new Vector4(0.5f, 0.5f, 1.37f));
        renderer.material = material;

        var oldKnifeComponent = prefab.GetComponent<Knife>();

        var newKnifeComponent = prefab.AddComponent<PlagueKnifeTool>();
        newKnifeComponent.attackSound = AudioUtils.GetFmodAsset("event:/creature/warper/swipe");
        newKnifeComponent.underwaterMissSound = AudioUtils.GetFmodAsset("event:/creature/warper/swipe");
        newKnifeComponent.surfaceMissSound = oldKnifeComponent.surfaceMissSound;
        newKnifeComponent.damageType = oldKnifeComponent.damageType;
        newKnifeComponent.damage = 50;
        newKnifeComponent.attackDist = 4;
        newKnifeComponent.vfxEventType = VFXEventTypes.knife;
        newKnifeComponent.mainCollider = oldKnifeComponent.mainCollider;
        newKnifeComponent.drawSound = oldKnifeComponent.drawSound;
        newKnifeComponent.firstUseSound = oldKnifeComponent.firstUseSound;
        newKnifeComponent.hitBleederSound = oldKnifeComponent.hitBleederSound;
        newKnifeComponent.bleederDamage = 50;
        newKnifeComponent.socket = oldKnifeComponent.socket;
        newKnifeComponent.ikAimRightArm = true;
        newKnifeComponent.drawTime = 0;
        newKnifeComponent.holsterTime = 0.1f;
        newKnifeComponent.pickupable = oldKnifeComponent.pickupable;
        newKnifeComponent.hasFirstUseAnimation = true;
        newKnifeComponent.hasBashAnimation = true;
        Object.DestroyImmediate(oldKnifeComponent);
    }
}