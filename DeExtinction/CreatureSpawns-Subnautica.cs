using System.Collections.Generic;
using Nautilus.Handlers;

#if SUBNAUTICA
namespace DeExtinction;

public static partial class CreatureSpawns
{
    public static partial void Register()
    {
        RegisterFishSpawns(CreaturePrefabManager.AmberClownPincher,
            new List<LootDistributionData.BiomeData>()
            {
                new()
                {
                    biome = BiomeType.MushroomForest_Grass,
                    probability = 0.8f,
                    count = 4
                },
                new()
                {
                    biome = BiomeType.MushroomForest_Sand,
                    probability = 0.8f,
                    count = 4
                },
                new()
                {
                    biome = BiomeType.MushroomForest_GiantTreeExterior,
                    probability = 0.8f,
                    count = 4
                },
                new()
                {
                    biome = BiomeType.MushroomForest_GiantTreeExterior,
                    probability = 0.4f,
                    count = 4
                },
                new()
                {
                    biome = BiomeType.UnderwaterIslands_OpenShallow_CreatureOnly,
                    probability = 0.15f,
                    count = 4
                },
                new()
                {
                    biome = BiomeType.UnderwaterIslands_IslandTop,
                    probability = 0.15f,
                    count = 4
                },
                new()
                {
                    biome = BiomeType.SparseReef_OpenDeep_CreatureOnly,
                    probability = 0.1f,
                    count = 4
                },
                new()
                {
                    biome = BiomeType.SparseReef_OpenShallow_CreatureOnly,
                    probability = 0.05f,
                    count = 4
                },
                new()
                {
                    biome = BiomeType.SparseReef_Spike,
                    probability = 0.2f,
                    count = 4
                },
                new()
                {
                    biome = BiomeType.SparseReef_Coral,
                    probability = 0.2f,
                    count = 4
                }
            });

        RegisterFishSpawns(CreaturePrefabManager.CitrineClownPincher,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.SparseReef_OpenDeep_CreatureOnly,
                    probability = 0.1f,
                    count = 4
                },
                new()
                {
                    biome = BiomeType.SparseReef_OpenShallow_CreatureOnly,
                    probability = 0.05f,
                    count = 4
                },
                new()
                {
                    biome = BiomeType.SparseReef_Spike,
                    probability = 0.2f,
                    count = 4
                },
                new()
                {
                    biome = BiomeType.SparseReef_Coral,
                    probability = 0.2f,
                    count = 4
                },
                new()
                {
                    biome = BiomeType.Dunes_OpenShallow_CreatureOnly,
                    count = 4,
                    probability = 0.1f
                },
                new()
                {
                    biome = BiomeType.Dunes_OpenDeep_CreatureOnly,
                    count = 4,
                    probability = 0.2f
                },
                new()
                {
                    biome = BiomeType.Mountains_OpenShallow_CreatureOnly,
                    count = 4,
                    probability = 0.1f
                },
                new()
                {
                    biome = BiomeType.Mountains_OpenDeep_CreatureOnly,
                    count = 4,
                    probability = 0.2f
                }
            });

        RegisterFishSpawns(CreaturePrefabManager.EmeraldClownPincher,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.SparseReef_OpenDeep_CreatureOnly,
                    probability = 0.1f,
                    count = 4
                },
                new()
                {
                    biome = BiomeType.SparseReef_OpenShallow_CreatureOnly,
                    probability = 0.05f,
                    count = 4
                },
                new()
                {
                    biome = BiomeType.SparseReef_Spike,
                    probability = 0.2f,
                    count = 4
                },
                new()
                {
                    biome = BiomeType.SparseReef_Coral,
                    probability = 0.2f,
                    count = 4
                },
                new()
                {
                    biome = BiomeType.Kelp_DenseVine,
                    probability = 0.5f,
                    count = 4
                },
                new()
                {
                    biome = BiomeType.Kelp_GrassDense,
                    probability = 0.6f,
                    count = 4
                },
                new()
                {
                    biome = BiomeType.Kelp_GrassSparse,
                    probability = 0.5f,
                    count = 4
                }
            });

        RegisterFishSpawns(CreaturePrefabManager.RubyClownPincher,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.InactiveLavaZone_Chamber_Open_CreatureOnly,
                    probability = 0.4f,
                    count = 4
                },
                new()
                {
                    biome = BiomeType.InactiveLavaZone_LavaPit_Open_CreatureOnly,
                    probability = 0.4f,
                    count = 4
                },
                new()
                {
                    biome = BiomeType.ActiveLavaZone_Chamber_Open_CreatureOnly,
                    probability = 0.4f,
                    count = 4
                }
            });

        RegisterFishSpawns(CreaturePrefabManager.SapphireClownPincher,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.GrandReef_Grass,
                    count = 4,
                    probability = 0.2f
                },
                new()
                {
                    biome = BiomeType.GrandReef_Ground,
                    count = 4,
                    probability = 0.2f
                },
                new()
                {
                    biome = BiomeType.GrandReef_OpenDeep_CreatureOnly,
                    count = 4,
                    probability = 0.2f
                },
                new()
                {
                    biome = BiomeType.LostRiverCorridor_Open_CreatureOnly,
                    count = 6,
                    probability = 0.12f
                },
                new()
                {
                    biome = BiomeType.Dunes_Grass,
                    count = 4,
                    probability = 0.2f
                },
                new()
                {
                    biome = BiomeType.Dunes_SandDune,
                    count = 4,
                    probability = 0.2f
                },
                new()
                {
                    biome = BiomeType.Dunes_OpenDeep_CreatureOnly,
                    count = 4,
                    probability = 0.2f
                },
                new()
                {
                    biome = BiomeType.CragField_OpenDeep_CreatureOnly,
                    count = 4,
                    probability = 0.06f
                },
                new()
                {
                    biome = BiomeType.CragField_Ground,
                    count = 4,
                    probability = 0.1f
                },
                new()
                {
                    biome = BiomeType.CragField_Rock,
                    count = 4,
                    probability = 0.1f
                },
                new()
                {
                    biome = BiomeType.CragField_Sand,
                    count = 4,
                    probability = 0.1f
                }
            });

        RegisterFishSpawns(CreaturePrefabManager.Axetail,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.GrandReef_OpenDeep_CreatureOnly,
                    probability = 0.4f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.KooshZone_OpenDeep_CreatureOnly,
                    probability = 0.4f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.KooshZone_Grass,
                    probability = 0.4f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.SeaTreaderPath_OpenDeep_CreatureOnly,
                    probability = 0.4f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.SeaTreaderPath_Path,
                    probability = 0.4f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.GrassyPlateaus_OpenShallow_CreatureOnly,
                    probability = 0.5f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.GrassyPlateaus_Grass,
                    probability = 0.5f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.JellyshroomCaves_CaveFloor,
                    count = 2,
                    probability = 1f
                }
            });

        RegisterFishSpawns(CreaturePrefabManager.Filtorb,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.Mountains_OpenDeep_CreatureOnly,
                    count = 3,
                    probability = 0.2f
                },
                new()
                {
                    biome = BiomeType.Mountains_OpenShallow_CreatureOnly,
                    count = 3,
                    probability = 0.2f
                },
                new()
                {
                    biome = BiomeType.Dunes_OpenDeep_CreatureOnly,
                    count = 3,
                    probability = 0.2f
                },
                new()
                {
                    biome = BiomeType.Dunes_OpenShallow_CreatureOnly,
                    count = 3,
                    probability = 0.2f
                },
                new()
                {
                    biome = BiomeType.GrandReef_OpenDeep_CreatureOnly,
                    count = 3,
                    probability = 0.2f
                },
                new()
                {
                    biome = BiomeType.GrandReef_OpenShallow_CreatureOnly,
                    count = 3,
                    probability = 0.2f
                },
                new()
                {
                    biome = BiomeType.DeepGrandReef_Ceiling,
                    count = 3,
                    probability = 0.8f
                },
                new()
                {
                    biome = BiomeType.DeepGrandReef_Ground,
                    count = 3,
                    probability = 0.8f
                },
                new()
                {
                    biome = BiomeType.DeepGrandReef_BlueCoral,
                    count = 3,
                    probability = 0.3f
                },
                new()
                {
                    biome = BiomeType.SafeShallows_CaveWall,
                    count = 3,
                    probability = 0.8f
                },
                new()
                {
                    biome = BiomeType.GrassyPlateaus_OpenShallow_CreatureOnly,
                    count = 5,
                    probability = 0.1f
                }
            });

        RegisterFishSpawns(CreaturePrefabManager.FloralFiltorb,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.Kelp_CaveFloor,
                    count = 5,
                    probability = 0.9f
                },
                new()
                {
                    biome = BiomeType.Kelp_Sand,
                    count = 3,
                    probability = 0.9f
                },
                new()
                {
                    biome = BiomeType.Kelp_DenseVine,
                    count = 3,
                    probability = 0.1f
                },
                new()
                {
                    biome = BiomeType.GhostTree_Open_CreatureOnly,
                    count = 3,
                    probability = 0.1f
                }
            });

        RegisterFishSpawns(CreaturePrefabManager.GrandGlider,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.GrandReef_OpenDeep_CreatureOnly,
                    count = 12,
                    probability = 0.015f
                },
                new()
                {
                    biome = BiomeType.GrandReef_OpenShallow_CreatureOnly,
                    count = 12,
                    probability = 0.015f
                },
                new()
                {
                    biome = BiomeType.Mountains_OpenDeep_CreatureOnly,
                    count = 12,
                    probability = 0.01f
                },
                new()
                {
                    biome = BiomeType.Mountains_OpenShallow_CreatureOnly,
                    count = 12,
                    probability = 0.01f
                },
                new()
                {
                    biome = BiomeType.CragField_OpenDeep_CreatureOnly,
                    count = 12,
                    probability = 0.02f
                },
                new()
                {
                    biome = BiomeType.CragField_OpenShallow_CreatureOnly,
                    count = 12,
                    probability = 0.02f
                },
                new()
                {
                    biome = BiomeType.Mesas_Open,
                    count = 12,
                    probability = 0.04f
                }
            });

        RegisterFishSpawns(CreaturePrefabManager.JasperThalassacean,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.LostRiverCorridor_Open_CreatureOnly,
                    count = 1,
                    probability = 0.04f
                },
                new()
                {
                    biome = BiomeType.LostRiverJunction_Open_CreatureOnly,
                    count = 1,
                    probability = 0.04f
                },
                new()
                {
                    biome = BiomeType.BonesField_Skeleton_Open_CreatureOnly,
                    count = 1,
                    probability = 0.06f
                },
                new()
                {
                    biome = BiomeType.BonesField_LakePit_Open_CreatureOnly,
                    count = 1,
                    probability = 0.04f
                },
                new()
                {
                    biome = BiomeType.GhostTree_Open_Big_CreatureOnly,
                    count = 1,
                    probability = 0.04f
                },
                new()
                {
                    biome = BiomeType.Canyon_Open_CreatureOnly,
                    count = 1,
                    probability = 0.04f
                },
                new()
                {
                    biome = BiomeType.BonesField_Corridor_CreatureOnly,
                    count = 1,
                    probability = 0.04f
                }
            });

        RegisterFishSpawns(CreaturePrefabManager.JellySpinner,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.BloodKelp_Grass,
                    probability = 0.2f,
                    count = 2
                },
                new()
                {
                    biome = BiomeType.BloodKelp_OpenDeep_CreatureOnly,
                    probability = 0.075f,
                    count = 2
                },
                new()
                {
                    biome = BiomeType.BloodKelp_TrenchWall,
                    probability = 0.3f,
                    count = 2
                },
                new()
                {
                    biome = BiomeType.BloodKelp_CaveFloor,
                    probability = 0.6f,
                    count = 2
                },
                new()
                {
                    biome = BiomeType.UnderwaterIslands_IslandTop,
                    probability = 1.4f,
                    count = 6
                },
                new()
                {
                    biome = BiomeType.UnderwaterIslands_ValleyFloor,
                    probability = 0.05f,
                    count = 5
                },
                new()
                {
                    biome = BiomeType.UnderwaterIslands_OpenShallow_CreatureOnly,
                    probability = 0.06f,
                    count = 6
                },
                new()
                {
                    biome = BiomeType.UnderwaterIslands_OpenDeep_CreatureOnly,
                    probability = 0.06f,
                    count = 6
                },
                new()
                {
                    biome = BiomeType.LostRiverCorridor_Open_CreatureOnly,
                    probability = 0.12f,
                    count = 6
                },
                new()
                {
                    biome = BiomeType.LostRiverJunction_Open_CreatureOnly,
                    probability = 0.12f,
                    count = 6
                },
                new()
                {
                    biome = BiomeType.BonesField_Corridor_CreatureOnly,
                    probability = 0.12f,
                    count = 6
                },
                new()
                {
                    biome = BiomeType.BonesField_Open_Creature,
                    probability = 0.12f,
                    count = 6
                },
                new()
                {
                    biome = BiomeType.BonesField_Corridor_CreatureOnly,
                    probability = 0.12f,
                    count = 6
                }
            });

        RegisterFishSpawns(CreaturePrefabManager.RibbonRay,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.MushroomForest_GiantTreeExterior,
                    probability = 2f,
                    count = 2
                },
                new()
                {
                    biome = BiomeType.MushroomForest_GiantTreeExteriorBase,
                    probability = 0.9f,
                    count = 2
                },
                new()
                {
                    biome = BiomeType.MushroomForest_CaveSand,
                    probability = 1f,
                    count = 2
                },
                new()
                {
                    biome = BiomeType.MushroomForest_Grass,
                    probability = 1f,
                    count = 2
                },
                new()
                {
                    biome = BiomeType.MushroomForest_MushroomTreeBase,
                    probability = 1f,
                    count = 2
                },
                new()
                {
                    biome = BiomeType.MushroomForest_Sand,
                    probability = 1f,
                    count = 2
                },
                new()
                {
                    biome = BiomeType.SparseReef_Coral,
                    count = 3,
                    probability = 0.2f
                },
                new()
                {
                    biome = BiomeType.SafeShallows_ShellTunnelHuge,
                    count = 2,
                    probability = 3f
                }
            });

        RegisterFishSpawns(CreaturePrefabManager.StellarThalassacean,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.GrandReef_OpenShallow_CreatureOnly,
                    count = 1,
                    probability = 0.02f
                },
                new()
                {
                    biome = BiomeType.GrandReef_OpenDeep_CreatureOnly,
                    count = 1,
                    probability = 0.02f
                },
                new()
                {
                    biome = BiomeType.KooshZone_OpenShallow_CreatureOnly,
                    count = 1,
                    probability = 0.1f
                },
                new()
                {
                    biome = BiomeType.KooshZone_OpenDeep_CreatureOnly,
                    count = 1,
                    probability = 0.05f
                },
                new()
                {
                    biome = BiomeType.Dunes_OpenShallow_CreatureOnly,
                    count = 1,
                    probability = 0.04f
                },
                new()
                {
                    biome = BiomeType.Dunes_OpenDeep_CreatureOnly,
                    count = 1,
                    probability = 0.02f
                }
            });

        RegisterFishSpawns(CreaturePrefabManager.TriangleFish,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.SafeShallows_Grass,
                    count = 2,
                    probability = 0.24f
                },
                new()
                {
                    biome = BiomeType.SafeShallows_SandFlat,
                    count = 2,
                    probability = 0.23f
                },
                new()
                {
                    biome = BiomeType.SparseReef_Coral,
                    count = 3,
                    probability = 0.2f
                },
                new()
                {
                    biome = BiomeType.SparseReef_OpenDeep_CreatureOnly,
                    count = 3,
                    probability = 0.2f
                },
                new()
                {
                    biome = BiomeType.KooshZone_Sand,
                    count = 3,
                    probability = 1.5f
                },
                new()
                {
                    biome = BiomeType.KooshZone_Grass,
                    count = 3,
                    probability = 2f
                },
                new()
                {
                    biome = BiomeType.CrashZone_Sand,
                    count = 3,
                    probability = 0.4f
                }
            });

        RegisterFishSpawns(CreaturePrefabManager.Twisteel,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.UnderwaterIslands_OpenShallow_CreatureOnly,
                    probability = 0.006f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.UnderwaterIslands_OpenDeep_CreatureOnly,
                    probability = 0.006f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.UnderwaterIslands_IslandSides,
                    probability = 0.1f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.UnderwaterIslands_ValleyFloor,
                    probability = 0.1f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.UnderwaterIslands_ValleyLedge,
                    probability = 0.04f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.BloodKelp_UniqueCreatures,
                    probability = 0.04f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.BloodKelp_TrenchWall,
                    probability = 0.1f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.BloodKelp_WreckCreatures,
                    probability = 0.2f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.KooshZone_Grass,
                    probability = 0.1f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.BloodKelp_Wall,
                    probability = 0.02f,
                    count = 1
                }
            });
        
                RegisterFishSpawns(CreaturePrefabManager.Pyrambassis,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.UnderwaterIslands_ValleyFloor,
                    probability = 0.01f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.DeepGrandReef_Ground,
                    probability = 0.01f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.BloodKelp_TrenchWall,
                    probability = 0.1f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.BloodKelp_CaveFloor,
                    probability = 0.05f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.KooshZone_Grass,
                    probability = 0.08f,
                    count = 1
                }
            });
    }

    public static partial void ModifyBaseGameSpawns()
    {
        var boneSharkClassId = CraftData.GetClassIdForTechType(TechType.BoneShark);
        LootDistributionHandler.EditLootDistributionData(boneSharkClassId, BiomeType.UnderwaterIslands_OpenDeep_CreatureOnly, 0f, 0);
        LootDistributionHandler.EditLootDistributionData(boneSharkClassId, BiomeType.UnderwaterIslands_ValleyFloor, 0f, 0);
        LootDistributionHandler.EditLootDistributionData(boneSharkClassId, BiomeType.UnderwaterIslands_IslandSides, 0f, 0);
        LootDistributionHandler.EditLootDistributionData(boneSharkClassId, BiomeType.UnderwaterIslands_ValleyLedge, 0f, 0);
        var bloomPlanktonClassId = CraftData.GetClassIdForTechType(TechType.Bloom);
        LootDistributionHandler.EditLootDistributionData(bloomPlanktonClassId, BiomeType.TreeCove_Ground, 0.05f, 1);
        LootDistributionHandler.EditLootDistributionData(bloomPlanktonClassId, BiomeType.TreeCove_Wall, 0.05f, 1);
        LootDistributionHandler.EditLootDistributionData(bloomPlanktonClassId, BiomeType.TreeCove_Ceiling, 0.05f, 1);
        LootDistributionHandler.EditLootDistributionData(bloomPlanktonClassId, BiomeType.TreeCove_TreeOpen_CreatureOnly, 0.25f, 1);
        LootDistributionHandler.EditLootDistributionData(bloomPlanktonClassId, BiomeType.Kelp_CaveFloor, 0.3f, 1);
        LootDistributionHandler.EditLootDistributionData(bloomPlanktonClassId, BiomeType.GrassyPlateaus_CaveCeiling, 0.4f, 1);
        LootDistributionHandler.EditLootDistributionData(bloomPlanktonClassId, BiomeType.DeepGrandReef_Ceiling, 0.25f, 1);
    }
}
#endif