﻿using System.Collections;
using System.Linq;
using ECCLibrary;
using ECCLibrary.Data;
using Nautilus.Assets;
using Nautilus.Handlers;
using Nautilus.Utility;
using Nautilus.Utility.MaterialModifiers;
using TheRedPlague.Mono;
using TheRedPlague.Mono.CreatureBehaviour.Drifter;
using UnityEngine;

namespace TheRedPlague.PrefabFiles.Creatures;

public class DrifterPrefab : CreatureAsset
{
    public const float BaseVelocity = 10;

    private bool _flyOnly;

    public DrifterPrefab(PrefabInfo prefabInfo, bool flyOnly) : base(prefabInfo)
    {
        _flyOnly = flyOnly;
    }

    protected override CreatureTemplate CreateTemplate()
    {
        var model = Plugin.AssetBundle.LoadAsset<GameObject>("DrifterPrefab");
        var template = new CreatureTemplate(model, BehaviourType.Leviathan, EcoTargetType.None, 5000)
        {
            LocomotionData = new LocomotionData(5f, 0.1f, 1f, 1f, true, true),
            SwimRandomData = new SwimRandomData(0.1f, BaseVelocity, new Vector3(50, 3, 50), 5f, 0.8f),
            Mass = 3000,
            RespawnData = new RespawnData(false),
            CellLevel = LargeWorldEntity.CellLevel.VeryFar,
            CanBeInfected = false
        };
        return template;
    }

    protected override IEnumerator ModifyPrefab(GameObject prefab, CreatureComponents components)
    {
        var wf = components.WorldForces;
        wf.aboveWaterGravity = 0;
        wf.aboveWaterDrag = 1;
        wf.underwaterDrag = 1;
        
        var eyeTracking = prefab.AddComponent<DrifterEyeTracking>();
        var eyeArmatureParent = prefab.transform.Find("DrifterAnimated/DrifterArmature/EyeArmature");
        eyeTracking.transforms = new[]
        {
            eyeArmatureParent.Find("Bone 1"),
            eyeArmatureParent.Find("Bone.001 1"),
            eyeArmatureParent.Find("Bone.002"),
            eyeArmatureParent.Find("Bone.003 1")
        };

        var loopingEmitter = prefab.AddComponent<FMOD_CustomLoopingEmitter>();
        loopingEmitter.SetAsset(AudioUtils.GetFmodAsset("DrifterIdle"));
        loopingEmitter.followParent = true;
        loopingEmitter.playOnAwake = true;

        var gasopodTask = CraftData.GetPrefabForTechTypeAsync(TechType.Gasopod);
        yield return gasopodTask;
        var gasopodGas = gasopodTask.GetResult().GetComponent<GasoPod>().gasFXprefab;
        var mistPrefab = Object.Instantiate(gasopodGas, prefab.transform);
        mistPrefab.SetActive(false);
        mistPrefab.AddComponent<DrifterMistInstance>();
        foreach (var renderer in mistPrefab.GetComponentsInChildren<Renderer>(true))
        {
            renderer.material.color = new Color(0.5f, 0.1f, 0.1f, 0.6f);
            renderer.material.SetColor("_ColorStrengthAtNight", Color.gray);
        }

        foreach (var ps in mistPrefab.GetComponentsInChildren<ParticleSystem>(true))
        {
            var main = ps.main;
            main.startSizeMultiplier *= 15f;
            main.startLifetimeMultiplier *= 10f;
            var sizeOverLifetime = ps.sizeOverLifetime;
            sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f,
                new AnimationCurve(new(0, 0.3f), new(1, 1)));
        }

        foreach (var trail in mistPrefab.GetComponentsInChildren<Trail_v2>(true))
        {
            trail.gameObject.SetActive(false);
        }

        var destroyAfterSeconds = mistPrefab.GetComponent<VFXDestroyAfterSeconds>();
        destroyAfterSeconds.lifeTime = 20f;
        
        mistPrefab.transform.Find("xGasopodSmoke/xSmkMesh").gameObject.SetActive(false);

        var spawnMist = prefab.AddComponent<DrifterSprayMist>();
        spawnMist.mistPrefab = mistPrefab;
        spawnMist.rb = components.Rigidbody;

        prefab.EnsureComponent<InfectionTarget>().invalidTarget = true;

        if (!_flyOnly)
            prefab.EnsureComponent<DrifterHoverAboveTerrain>();

        var infectedMixin = prefab.EnsureComponent<InfectedMixin>();
        infectedMixin.renderers = prefab.GetComponentsInChildren<Renderer>(true).Where(r => r is not ParticleSystemRenderer).ToArray();
    }

    protected override void ApplyMaterials(GameObject prefab)
    {
        MaterialUtils.ApplySNShaders(prefab, 5.5f, 1.3f, 2f, new DrifterMaterialModifier());
    }

    private class DrifterMaterialModifier : MaterialModifier
    {
        public override void EditMaterial(Material material, Renderer renderer, int materialIndex, MaterialUtils.MaterialType materialType)
        {
            
        }

        public override bool BlockShaderConversion(Material material, Renderer renderer, MaterialUtils.MaterialType materialType)
        {
            return renderer is ParticleSystemRenderer;
        }
    }
}