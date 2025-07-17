using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Handlers;
using Nautilus.Utility;
using UnityEngine;
using UWE;

namespace SeaVoyager.Prefabs.Fragments;

public class SeaVoyagerFragment
{
    public static TechType SeaVoyagerFragmentTechType { get; } = EnumHandler.AddEntry<TechType>("SeaVoyagerFragment");

    public PrefabInfo Info { get; private set; }

    private readonly GameObject _model;

    private readonly float _mass;

    public SeaVoyagerFragment(string classId, string modelName, float mass)
    {
        Info = new PrefabInfo(classId, classId, SeaVoyagerFragmentTechType);
        _model = Plugin.assetBundle.LoadAsset<GameObject>(modelName);
        _mass = mass;
    }

    public PrefabInfo Register()
    {
        var customPrefab = new CustomPrefab(Info);

        var entityInfo = new WorldEntityInfo()
        {
            classId = Info.ClassID,
            cellLevel = LargeWorldEntity.CellLevel.Medium,
            slotType = EntitySlot.Type.Medium,
            techType = Info.TechType,
            localScale = Vector3.one
        };

        PrefabUtils.AddBasicComponents(_model, Info.ClassID, Info.TechType, LargeWorldEntity.CellLevel.Medium);
        MaterialUtils.ApplySNShaders(_model);
        var rb = _model.AddComponent<Rigidbody>();
        rb.mass = _mass;
        rb.useGravity = false;
        rb.isKinematic = true;
        var wf = _model.AddComponent<WorldForces>();
        wf.useRigidbody = rb;
        customPrefab.SetGameObject(_model);

        customPrefab.SetSpawns(entityInfo, BiomesToSpawnIn);

        customPrefab.Register();

        return Info;
    }

    private static LootDistributionData.BiomeData[] BiomesToSpawnIn => new LootDistributionData.BiomeData[]
    {
        new LootDistributionData.BiomeData()
        {
            biome = BiomeType.Kelp_Sand,
            probability = 0.05f,
            count = 1
        },
        new LootDistributionData.BiomeData()
        {
            biome = BiomeType.Kelp_GrassSparse,
            probability = 0.05f,
            count = 1
        }
    };
}
