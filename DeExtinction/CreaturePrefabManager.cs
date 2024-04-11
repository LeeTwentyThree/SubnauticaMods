﻿using DeExtinction.Prefabs.Creatures;
using ECCLibrary;
using ECCLibrary.Data;
using Nautilus.Assets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Handlers;
using UnityEngine;

namespace DeExtinction;

internal static class CreaturePrefabManager
{
    public static FiltorbPrefab Filtorb { get; private set; }
    public static AxetailPrefab Axetail { get; private set; }
    public static JellySpinnerPrefab JellySpinner { get; private set; }
    public static RibbonRayPrefab RibbonRay { get; private set; }
    public static TriangleFishPrefab TriangleFish { get; private set; }
    public static ClownPincherPrefab RubyClownPincher { get; private set; }
    public static ClownPincherPrefab SapphireClownPincher { get; private set; }
    public static ClownPincherPrefab EmeraldClownPincher { get; private set; }
    public static ClownPincherPrefab AmberClownPincher { get; private set; }
    public static ClownPincherPrefab CitrineClownPincher { get; private set; }
    public static ThalassaceanPrefab StellarThalassacean { get; private set; }
    public static ThalassaceanPrefab JasperThalassacean { get; private set; }
    public static GrandGliderPrefab GrandGlider { get; private set; }
    public static TwisteelPrefab Twisteel { get; private set; }
    public static GulperLeviathanPrefab GulperPrefab { get; private set; }
    public static GulperLeviathanBabyPrefab GulperBabyPrefab { get; private set; }
    
    public static PrefabInfo GrandGliderEgg { get; private set; }
    public static PrefabInfo StellarThalassaceanEgg { get; private set; }
    public static PrefabInfo JasperThalassaceanEgg { get; private set; }
    public static PrefabInfo TwisteelEgg { get; private set; }
    public static PrefabInfo GulperLeviathanEgg { get; private set; }

    private static AssetBundle Bundle => Plugin.AssetBundle;

    private static Sprite LoadIcon(string name) => Bundle.LoadAsset<Sprite>(name);

    public static void RegisterCreatures()
    {
        Filtorb = new FiltorbPrefab(PrefabInfo.WithTechType("Filtorb")
            .WithIcon(LoadIcon("Filtorb_Item")));
        Filtorb.Register();
        
        Axetail = new AxetailPrefab(PrefabInfo.WithTechType("Axetail")
            .WithIcon(LoadIcon("Axetail_Item")));
        Axetail.Register();
        
        JellySpinner = new JellySpinnerPrefab(PrefabInfo.WithTechType("JellySpinner")
            .WithIcon(LoadIcon("JellySpinner_Item")));
        JellySpinner.Register();
        
        RibbonRay = new RibbonRayPrefab(PrefabInfo.WithTechType("RibbonRay")
            .WithIcon(LoadIcon("RibbonRay_Item"))
            .WithSizeInInventory(new Vector2int(2, 1)));
        RibbonRay.Register();
        
        TriangleFish = new TriangleFishPrefab(PrefabInfo.WithTechType("TriangleFish")
            .WithIcon(LoadIcon("Trianglefish_Item")));
        TriangleFish.Register();
        
        RubyClownPincher = new ClownPincherPrefab(PrefabInfo.WithTechType("RubyClownPincher")
                .WithIcon(LoadIcon("RCP_Item")),
            Bundle.LoadAsset<GameObject>("RCP_Prefab"),
            "RCP_Ency",
            "RCP_Popup");
        RubyClownPincher.Register();

        SapphireClownPincher = new ClownPincherPrefab(PrefabInfo.WithTechType("SapphireClownPincher")
            .WithIcon(LoadIcon("SCP_Item")),
            Bundle.LoadAsset<GameObject>("SCP_Prefab"),
            "SCP_Ency",
            "SCP_Popup");
        SapphireClownPincher.Register();
        
        EmeraldClownPincher = new ClownPincherPrefab(PrefabInfo.WithTechType("EmeraldClownPincher")
                .WithIcon(LoadIcon("ECP_Item")),
            Bundle.LoadAsset<GameObject>("ECP_Prefab"),
            "ECP_Ency",
            "ECP_Popup");
        EmeraldClownPincher.Register();

        AmberClownPincher = new ClownPincherPrefab(PrefabInfo.WithTechType("AmberClownPincher")
                .WithIcon(LoadIcon("ACP_Item")),
            Bundle.LoadAsset<GameObject>("ACP_Prefab"),
            "ACP_Ency",
            "ACP_Popup");
        AmberClownPincher.Register();

        CitrineClownPincher = new ClownPincherPrefab(PrefabInfo.WithTechType("CitrineClownPincher")
                .WithIcon(LoadIcon("CCP_Item")),
            Bundle.LoadAsset<GameObject>("CCP_Prefab"),
            "CCP_Ency",
            "CCP_Popup"
            );
        CitrineClownPincher.Register();
        
        StellarThalassacean = new ThalassaceanPrefab(PrefabInfo.WithTechType("StellarThalassacean")
            .WithIcon(LoadIcon("Stellar_Item"))
            .WithSizeInInventory(new Vector2int(4, 4)),
            Plugin.AssetBundle.LoadAsset<GameObject>("StellarThalassaceanPrefab"),
            "Stellar_Ency",
            "Stellar_Popup");
        StellarThalassaceanEgg = CreateEggPrefab("StellarThalassaceanEgg", "StellarThalassaceanEggPrefab",
            "StellarThalassaceanEgg_Icon", new Vector2int(3, 3), StellarThalassacean.PrefabInfo.TechType, 2);
        StellarThalassacean.EggInfo = StellarThalassaceanEgg;
        StellarThalassacean.Register();
        
        JasperThalassacean = new ThalassaceanPrefab(PrefabInfo.WithTechType("JasperThalassacean")
                .WithIcon(LoadIcon("Jasper_Item"))
                .WithSizeInInventory(new Vector2int(4, 4)),
            Plugin.AssetBundle.LoadAsset<GameObject>("JasperThalassaceanPrefab"),
            "Jasper_Ency",
            "Jasper_Popup");
        JasperThalassaceanEgg = CreateEggPrefab("JasperThalassaceanEgg", "JasperThalassaceanEggPrefab",
            "JasperThalassaceanEgg_Icon", new Vector2int(3, 3), JasperThalassacean.PrefabInfo.TechType, 2);
        JasperThalassacean.EggInfo = StellarThalassaceanEgg;
        JasperThalassacean.Register();

        GrandGlider = new GrandGliderPrefab(PrefabInfo.WithTechType("GrandGlider")
            .WithIcon(LoadIcon("GrandGlider_Item"))
            .WithSizeInInventory(new Vector2int(3, 3)));
        GrandGliderEgg = CreateEggPrefab("GrandGliderEgg", "GGEggPrefab",
            "GGEgg_Item", new Vector2int(2, 2), GrandGlider.PrefabInfo.TechType, 1);
        GrandGlider.EggInfo = GrandGliderEgg;
        GrandGlider.Register();

        Twisteel = new TwisteelPrefab(PrefabInfo.WithTechType("Twisteel")
            .WithIcon(LoadIcon("Twisteel_Item"))
            .WithSizeInInventory(new Vector2int(3, 3)));
        TwisteelEgg = CreateEggPrefab("TwisteelEgg", "TwisteelEgg_Prefab",
            "TwisteelEgg_Item", new Vector2int(2, 2), Twisteel.PrefabInfo.TechType, 1.5f);
        Twisteel.EggInfo = TwisteelEgg;
        Twisteel.Register();
        
        GulperPrefab = new GulperLeviathanPrefab(PrefabInfo.WithTechType("GulperLeviathan"));
        GulperPrefab.Register();
        
        // PLACEHOLDER BABY GULPER; PLEASE REDO LATER!
        GulperBabyPrefab = new GulperLeviathanBabyPrefab(PrefabInfo.WithTechType("GulperLeviathanBaby")
            .WithIcon(LoadIcon("GulperBaby_Sprite"))
            .WithSizeInInventory(new Vector2int(4, 4)));
        GulperLeviathanEgg = CreateEggPrefab("GulperEgg", "GulperEgg_Prefab",
            "GulperEgg_Sprite", new Vector2int(3, 3), GulperBabyPrefab.PrefabInfo.TechType, 2);
        GulperBabyPrefab.EggInfo = GulperLeviathanEgg;
        GulperBabyPrefab.Register();
    }

    public static void RegisterFood()
    {
        CookedCreatureHandler.RegisterAllCreatureFood(
            Filtorb,
            null,
            null,
            LoadIcon("Filtorb_Cooked"),
            null,
            null,
            LoadIcon("Filtorb_Cured"),
            new EdibleData(5, 20, true),
            new VFXFabricatingData("CraftModel", 0, 0.3f)
        );
        
        CookedCreatureHandler.RegisterAllCreatureFood(
            Axetail,
            null,
            null,
            LoadIcon("Axetail_Cooked"),
            null,
            null,
            LoadIcon("Axetail_Cured"),
            new EdibleData(20, 13, true),
            new VFXFabricatingData("CraftModel", 0, 0.3f)
        );

        CookedCreatureHandler.RegisterAllCreatureFood(
            JellySpinner,
            null,
            null,
            LoadIcon("JellySpinner_Cooked"),
            null,
            null,
            LoadIcon("JellySpinner_Cured"),
            new EdibleData(9, 2, true),
            new VFXFabricatingData("CraftModel", 0, 0.3f)
        );
        
        CookedCreatureHandler.RegisterAllCreatureFood(
            RibbonRay,
            null,
            null,
            LoadIcon("RibbonRay_Cooked"),
            null,
            null,
            LoadIcon("RibbonRay_Cured"),
            new EdibleData(36, 7, true),
            new VFXFabricatingData("CraftModel", 0, 0.3f)
        );
        
        CookedCreatureHandler.RegisterAllCreatureFood(
            TriangleFish,
            null,
            null,
            LoadIcon("Trianglefish_Cooked"),
            null,
            null,
            LoadIcon("Trianglefish_Cured"),
            new EdibleData(22, 3, true),
            new VFXFabricatingData("CraftModel", 0, 0.3f)
        );
        
        CookedCreatureHandler.RegisterAllCreatureFood(
            RubyClownPincher,
            null,
            null,
            LoadIcon("RCP_Cooked"),
            null,
            null,
            LoadIcon("RCP_Cured"),
            new EdibleData(30, 9, true),
            new VFXFabricatingData("CraftModel", 0, 0.3f)
        );
        
        CookedCreatureHandler.RegisterAllCreatureFood(
            SapphireClownPincher,
            null,
            null,
            LoadIcon("SCP_Cooked"),
            null,
            null,
            LoadIcon("SCP_Cured"),
            new EdibleData(30, 9, true),
            new VFXFabricatingData("CraftModel", 0, 0.3f)
        );
        
        CookedCreatureHandler.RegisterAllCreatureFood(
            EmeraldClownPincher,
            null,
            null,
            LoadIcon("ECP_Cooked"),
            null,
            null,
            LoadIcon("ECP_Cured"),
            new EdibleData(30, 9, true),
            new VFXFabricatingData("CraftModel", 0, 0.3f)
        );
        
        CookedCreatureHandler.RegisterAllCreatureFood(
            AmberClownPincher,
            null,
            null,
            LoadIcon("ACP_Cooked"),
            null,
            null,
            LoadIcon("ACP_Cured"),
            new EdibleData(30, 9, true),
            new VFXFabricatingData("CraftModel", 0, 0.3f)
        );
        
        CookedCreatureHandler.RegisterAllCreatureFood(
            CitrineClownPincher,
            null,
            null,
            LoadIcon("CCP_Cooked"),
            null,
            null,
            LoadIcon("CCP_Cured"),
            new EdibleData(30, 9, true),
            new VFXFabricatingData("CraftModel", 0, 0.3f)
        );
    }
    
    private static PrefabInfo CreateEggPrefab(string classId, string eggModelName, string eggIconName, Vector2int size, TechType hatchingCreature, float hatchingTime)
    {
        var info = PrefabInfo.WithTechType(classId)
            .WithIcon(Plugin.AssetBundle.LoadAsset<Sprite>(eggIconName))
            .WithSizeInInventory(size);
        var eggPrefab = new CustomPrefab(info);
        var eggTemplate = new EggTemplate(info, Plugin.AssetBundle.LoadAsset<GameObject>(eggModelName))
            .WithHatchingCreature(hatchingCreature)
            .WithCellLevel(LargeWorldEntity.CellLevel.Medium)
            .WithHatchingTime(hatchingTime)
            .SetUndiscoveredTechType();
        eggPrefab.SetGameObject(eggTemplate);
        eggPrefab.Register();
        return info;
    }
}
