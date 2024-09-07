using System.Collections;
using System.Collections.Generic;
using ECCLibrary;
using ECCLibrary.Data;
using Nautilus.Assets;
using Nautilus.Handlers;
using TheRedPlague.Mono;
using UnityEngine;

namespace TheRedPlague.PrefabFiles.Creatures;

public class Mutant : CreatureAsset
{
    private readonly string _prefabName;
    private readonly bool _heavilyMutated;

    private const float NormalVariantVelocity = 3f;
    private const float HeavilyMutatedVelocity = 14f;

    public Mutant(PrefabInfo prefabInfo, string prefabName, bool heavilyMutated) : base(prefabInfo)
    {
        _prefabName = prefabName;
        _heavilyMutated = heavilyMutated;
    }

    protected override void PostRegister()
    {
        if (_heavilyMutated)
        {
            LootDistributionHandler.AddLootDistributionData(PrefabInfo.ClassID,
                new LootDistributionData.BiomeData
                {
                    biome = BiomeType.ActiveLavaZone_Chamber_Open_CreatureOnly,
                    probability = 0.2f,
                    count = 1
                },
                /*
                new LootDistributionData.BiomeData
                {
                    biome = BiomeType.Dunes_OpenShallow_CreatureOnly,
                    probability = 0.1f,
                    count = 1
                },
                new LootDistributionData.BiomeData
                {
                    biome = BiomeType.Dunes_OpenDeep_CreatureOnly,
                    probability = 0.1f,
                    count = 1
                },
                */
                new LootDistributionData.BiomeData
                {
                    biome = BiomeType.ActiveLavaZone_Falls_Open_CreatureOnly,
                    probability = 0.2f,
                    count = 1
                }
            );
            return;
        }
        
        LootDistributionHandler.AddLootDistributionData(PrefabInfo.ClassID,
            new LootDistributionData.BiomeData
            {
                biome = BiomeType.KooshZone_OpenDeep_CreatureOnly,
                probability = 0.02f,
                count = 1
            },
            new LootDistributionData.BiomeData
            {
                biome = BiomeType.KooshZone_OpenShallow_CreatureOnly,
                probability = 0.02f,
                count = 1
            },
            new LootDistributionData.BiomeData
            {
                biome = BiomeType.UnderwaterIslands_OpenShallow_CreatureOnly,
                probability = 0.02f,
                count = 1
            },
            new LootDistributionData.BiomeData
            {
                biome = BiomeType.UnderwaterIslands_OpenDeep_CreatureOnly,
                probability = 0.02f,
                count = 1
            },
            new LootDistributionData.BiomeData
            {
                biome = BiomeType.MushroomForest_Grass,
                probability = 0.02f,
                count = 1
            },
            new LootDistributionData.BiomeData
            {
                biome = BiomeType.CragField_OpenShallow_CreatureOnly,
                probability = 0.02f,
                count = 1
            },
            new LootDistributionData.BiomeData
            {
                biome = BiomeType.CragField_OpenDeep_CreatureOnly,
                probability = 0.02f,
                count = 1
            },
            new LootDistributionData.BiomeData
            {
                biome = BiomeType.GrandReef_OpenShallow_CreatureOnly,
                probability = 0.02f,
                count = 1
            },
            new LootDistributionData.BiomeData
            {
                biome = BiomeType.GrandReef_OpenDeep_CreatureOnly,
                probability = 0.02f,
                count = 1
            },
            new LootDistributionData.BiomeData
            {
                biome = BiomeType.Dunes_OpenShallow_CreatureOnly,
                probability = 0.05f,
                count = 1
            },
            new LootDistributionData.BiomeData
            {
                biome = BiomeType.Dunes_OpenDeep_CreatureOnly,
                probability = 0.05f,
                count = 1
            },
            new LootDistributionData.BiomeData
            {
                biome = BiomeType.LostRiverCorridor_Open_CreatureOnly,
                probability = 0.02f,
                count = 1
            },
            new LootDistributionData.BiomeData
            {
                biome = BiomeType.LostRiverJunction_Open_CreatureOnly,
                probability = 0.02f,
                count = 1
            },
            new LootDistributionData.BiomeData
            {
                biome = BiomeType.BonesField_Open_Creature,
                probability = 0.02f,
                count = 1
            },
            new LootDistributionData.BiomeData
            {
                biome = BiomeType.InactiveLavaZone_Corridor_Open_CreatureOnly,
                probability = 0.05f,
                count = 1
            },
            new LootDistributionData.BiomeData
            {
                biome = BiomeType.InactiveLavaZone_Chamber_Open_CreatureOnly,
                probability = 0.05f,
                count = 1
            }
        );
    }

    protected override CreatureTemplate CreateTemplate()
    {
        var template = new CreatureTemplate(Plugin.AssetBundle.LoadAsset<GameObject>(_prefabName), BehaviourType.Shark,
            EcoTargetType.Shark, float.PositiveInfinity);
        CreatureTemplateUtils.SetCreatureDataEssentials(template, LargeWorldEntity.CellLevel.Medium, 500, -0.5f);
        CreatureTemplateUtils.SetCreatureMotionEssentials(template,
            new SwimRandomData(0.3f, _heavilyMutated ? HeavilyMutatedVelocity : NormalVariantVelocity,
                new Vector3(20f, 6f, 20f), 3f),
            new StayAtLeashData(0.4f, _heavilyMutated ? HeavilyMutatedVelocity : NormalVariantVelocity, 50f));
        template.LocomotionData = new LocomotionData(5f, _heavilyMutated ? 3 : 0.6f);
        template.AggressiveWhenSeeTargetList = new List<AggressiveWhenSeeTargetData>()
            {new(EcoTargetType.Shark, 1, 40, 2)};
        template.AttackLastTargetData =
            new AttackLastTargetData(0.5f, _heavilyMutated ? 20f : NormalVariantVelocity * 2f, 0.5f, 7f, _heavilyMutated ? 5 : 10);
        return template;
    }

    protected override IEnumerator ModifyPrefab(GameObject prefab, CreatureComponents components)
    {
        prefab.AddComponent<InfectOnStart>();
        if (_heavilyMutated)
        {
            prefab.AddComponent<DisableRigidbodyWhileOnScreen>();
        }

        var attackTrigger = prefab.transform.Find("AttackTrigger").gameObject.AddComponent<MutantAttackTrigger>();
        attackTrigger.prefabFileName = _prefabName;
        attackTrigger.heavilyMutated = _heavilyMutated;
        attackTrigger.damage = _heavilyMutated ? 24 : 14;
        yield break;
    }
}