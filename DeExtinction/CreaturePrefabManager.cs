using DeExtinction.Prefabs.Creatures;
using ECCLibrary;
using ECCLibrary.Data;
using Nautilus.Assets;
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

    private static AssetBundle Bundle => Plugin.AssetBundle;

    private static Sprite LoadIcon(string name) => Bundle.LoadAsset<Sprite>(name);

    public static void RegisterCreatures()
    {
        Filtorb = new FiltorbPrefab(PrefabInfo.WithTechType("Filtorb", "Filtorb", "Small, filter feeding organism.", unlockAtStart: false)
            .WithIcon(LoadIcon("Filtorb_Item")));
        Filtorb.Register();
        
        Axetail = new AxetailPrefab(PrefabInfo.WithTechType("Axetail", "Axetail", "Small, edible prey fish.", unlockAtStart: false)
            .WithIcon(LoadIcon("Axetail_Item")));
        Axetail.Register();
        
        JellySpinner = new JellySpinnerPrefab(PrefabInfo.WithTechType("JellySpinner", "Jelly spinner", "Small organism.", unlockAtStart: false)
            .WithIcon(LoadIcon("JellySpinner_Item")));
        JellySpinner.Register();
        
        RibbonRay = new RibbonRayPrefab(PrefabInfo.WithTechType("RibbonRay", "Ribbon ray", "Small, edible prey fish.", unlockAtStart: false)
            .WithIcon(LoadIcon("RibbonRay_Item"))
            .WithSizeInInventory(new Vector2int(2, 1)));
        RibbonRay.Register();
        
        TriangleFish = new TriangleFishPrefab(PrefabInfo.WithTechType("TriangleFish", "Trianglefish", "Small, edible prey fish.", unlockAtStart: false)
            .WithIcon(LoadIcon("Trianglefish_Item")));
        TriangleFish.Register();
        
        RubyClownPincher = new ClownPincherPrefab(PrefabInfo.WithTechType("RubyClownPincher", "Ruby clown pincher", "Small, edible prey fish.", unlockAtStart: false)
                .WithIcon(LoadIcon("RCP_Item")),
            Bundle.LoadAsset<GameObject>("RCP_Prefab"));
        RubyClownPincher.Register();

        SapphireClownPincher = new ClownPincherPrefab(PrefabInfo.WithTechType("SapphireClownPincher", "Sapphire clown pincher", "Small, edible prey fish.", unlockAtStart: false)
            .WithIcon(LoadIcon("SCP_Item")),
            Bundle.LoadAsset<GameObject>("SCP_Prefab"));
        SapphireClownPincher.Register();
        
        EmeraldClownPincher = new ClownPincherPrefab(PrefabInfo.WithTechType("EmeraldClownPincher", "Emerald clown pincher", "Small, edible prey fish.", unlockAtStart: false)
                .WithIcon(LoadIcon("ECP_Item")),
            Bundle.LoadAsset<GameObject>("ECP_Prefab"));
        EmeraldClownPincher.Register();

        AmberClownPincher = new ClownPincherPrefab(PrefabInfo.WithTechType("AmberClownPincher", "Amber clown pincher", "Small, edible prey fish.", unlockAtStart: false)
                .WithIcon(LoadIcon("ACP_Item")),
            Bundle.LoadAsset<GameObject>("ACP_Prefab"));
        AmberClownPincher.Register();

        CitrineClownPincher = new ClownPincherPrefab(PrefabInfo.WithTechType("CitrineClownPincher", "Citrine clown pincher", "Small, edible prey fish.", unlockAtStart: false)
                .WithIcon(LoadIcon("CCP_Item")),
            Bundle.LoadAsset<GameObject>("CCP_Prefab"));
        CitrineClownPincher.Register();
        
        /*
         *             rubyClownPincher = new ClownPincherRuby("RubyClownPincher", "Ruby clown pincher", "Small, edible prey fish.", assetBundle.LoadAsset<GameObject>("RCP_Prefab"), assetBundle.LoadAsset<Texture2D>("RCP_Item"));
            rubyClownPincher.Patch();

            sapphireClownPincher = new ClownPincherSapphire("SapphireClownPincher", "Sapphire clown pincher", "Small, edible prey fish.", assetBundle.LoadAsset<GameObject>("SCP_Prefab"), assetBundle.LoadAsset<Texture2D>("SCP_Item"));
            sapphireClownPincher.Patch();

            emeraldClownPincher = new ClownPincherEmerald("EmeraldClownPincher", "Emerald clown pincher", "Small, edible prey fish.", assetBundle.LoadAsset<GameObject>("ECP_Prefab"), assetBundle.LoadAsset<Texture2D>("ECP_Item"));
            emeraldClownPincher.Patch();

            amberClownPincher = new ClownPincherAmber("AmberClownPincher", "Amber clown pincher", "Small, edible prey fish.", assetBundle.LoadAsset<GameObject>("ACP_Prefab"), assetBundle.LoadAsset<Texture2D>("ACP_Item"));
            amberClownPincher.Patch();

            citrineClownPincher = new ClownPincherCitrine("CitrineClownPincher", "Citrine clown pincher", "Small, edible prey fish.", assetBundle.LoadAsset<GameObject>("CCP_Prefab"), assetBundle.LoadAsset<Texture2D>("CCP_Item"));
            citrineClownPincher.Patch();
         */
        
        StellarThalassacean = new ThalassaceanPrefab(PrefabInfo.WithTechType("StellarThalassacean", "Stellar thalassacean", "Large filter feeder, raised in containment.", unlockAtStart: false)
            .WithIcon(LoadIcon("Stellar_Item"))
            .WithSizeInInventory(new Vector2int(4, 4)),
            Plugin.AssetBundle.LoadAsset<GameObject>("StellarThalassaceanPrefab"));
        StellarThalassacean.Register();
        
        JasperThalassacean = new ThalassaceanPrefab(PrefabInfo.WithTechType("JasperThalassacean", "Jasper thalassacean", "Large filter feeder, raised in containment.", unlockAtStart: false)
                .WithIcon(LoadIcon("Jasper_Item"))
                .WithSizeInInventory(new Vector2int(4, 4)),
            Plugin.AssetBundle.LoadAsset<GameObject>("JasperThalassaceanPrefab"));
        JasperThalassacean.Register();

        GrandGlider = new GrandGliderPrefab(PrefabInfo.WithTechType("GrandGlider", "Grand glider", "Medium sized prey animal, raised in containment.", unlockAtStart: false)
            .WithIcon(LoadIcon("GrandGlider_Item"))
            .WithSizeInInventory(new Vector2int(3, 3)));
        GrandGlider.Register();

        Twisteel = new TwisteelPrefab(PrefabInfo.WithTechType("Twisteel", "Twisteel",
            "Thin eel-like organism, raised in containment.", unlockAtStart: false)
            .WithIcon(LoadIcon("Twisteel_Item"))
            .WithSizeInInventory(new Vector2int(3, 3)));
        Twisteel.Register();
        
        GulperPrefab = new GulperLeviathanPrefab(PrefabInfo.WithTechType("GulperLeviathan", "Gulper leviathan", "Leviathan-class predator with a huge mouth."));
        GulperPrefab.Register();
    }

    public static void RegisterFood()
    {
        CookedCreatureHandler.RegisterAllCreatureFood(
            Filtorb,
            "Cooked filtorb",
            "Juicy.",
            LoadIcon("Filtorb_Cooked"),
            "Cured filtorb",
            "Chalky. Dehydrating, but keeps well.",
            LoadIcon("Filtorb_Cured"),
            new EdibleData(5, 20, true),
            new VFXFabricatingData("CraftModel", 0, 0.3f)
        );
        
        CookedCreatureHandler.RegisterAllCreatureFood(
            Axetail,
            "Cooked axetail",
            "A sharp taste. Somewhat hydrating.",
            LoadIcon("Axetail_Cooked"),
            "Cured axetail",
            "Eat around the pointy bits. Dehydrating, but keeps well.",
            LoadIcon("Axetail_Cured"),
            new EdibleData(20, 13, true),
            new VFXFabricatingData("CraftModel", 0, 0.3f)
        );

        CookedCreatureHandler.RegisterAllCreatureFood(
            JellySpinner,
            "Cooked jelly spinner",
            "Pops in your mouth.",
            LoadIcon("JellySpinner_Cooked"),
            "Cured jelly spinner",
            "Like eating bubble wrap. Dehydrating, but keeps well.",
            LoadIcon("JellySpinner_Cured"),
            new EdibleData(9, 2, true),
            new VFXFabricatingData("CraftModel", 0, 0.3f)
        );

    }
}
