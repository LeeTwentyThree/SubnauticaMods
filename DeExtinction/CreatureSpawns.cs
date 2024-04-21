using System.Collections.Generic;
using ECCLibrary;
using Nautilus.Assets;
using Nautilus.Handlers;

namespace DeExtinction;

public static partial class CreatureSpawns
{
    public static partial void RegisterLootDistribution();
    public static partial void ModifyBaseGameSpawns();
    public static partial void RegisterCoordinatedSpawns();

    private static void RegisterFishSpawns(CreatureAsset creature, List<LootDistributionData.BiomeData> biomeData)
    {
        LootDistributionHandler.AddLootDistributionData(creature.PrefabInfo.ClassID, creature.PrefabInfo.PrefabFileName, biomeData);
    }
    
    private static void RegisterEggSpawns(PrefabInfo eggInfo, List<LootDistributionData.BiomeData> biomeData)
    {
        LootDistributionHandler.AddLootDistributionData(eggInfo.ClassID, eggInfo.PrefabFileName, biomeData);
    }
}