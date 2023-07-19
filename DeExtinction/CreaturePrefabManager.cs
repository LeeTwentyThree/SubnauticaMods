using DeExtinction.Prefabs.Creatures;
using ECCLibrary.Assets;
using ECCLibrary.Data;
using Nautilus.Assets;
using Nautilus.Handlers;
using UnityEngine;

namespace DeExtinction;

internal static class CreaturePrefabManager
{
    public static FiltorbPrefab Filtorb { get; private set; }

    private static AssetBundle Bundle => Plugin.AssetBundle;

    private static Sprite LoadIcon(string name) => Bundle.LoadAsset<Sprite>(name);

    public static void RegisterCreatures()
    {
        Filtorb = new FiltorbPrefab(PrefabInfo.WithTechType("Filtorb", "Filtorb", "Small, filter feeding organism.").WithIcon(LoadIcon("Filtorb_Item")));
        Filtorb.Register();
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
    }
}
