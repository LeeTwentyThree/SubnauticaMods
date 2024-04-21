using System.Collections.Generic;
using ECCLibrary;
using Nautilus.Handlers;

namespace DeExtinction;

public static partial class CreatureSpawns
{
    public static partial void Register();
    public static partial void ModifyBaseGameSpawns();

    private static void RegisterFishSpawns(CreatureAsset creature, List<LootDistributionData.BiomeData> biomeData)
    {
        LootDistributionHandler.AddLootDistributionData(creature.PrefabInfo.ClassID, creature.PrefabInfo.PrefabFileName, biomeData);
    }
}