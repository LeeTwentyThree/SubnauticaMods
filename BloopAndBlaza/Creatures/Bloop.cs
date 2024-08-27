using System.Collections;
using System.Collections.Generic;
using BloopAndBlaza.Mono;
using ECCLibrary;
using ECCLibrary.Data;
using ECCLibrary.Mono;
using Nautilus.Assets;
using Nautilus.Utility;
using UnityEngine;

namespace BloopAndBlaza.Creatures;

public class Bloop : CreatureAsset
{
    private readonly GameObject _model;
    private readonly bool _deep;
    private readonly bool _grandDeep;
    
    public Bloop(PrefabInfo prefabInfo, GameObject model, bool deep, bool grandDeep = false) : base(prefabInfo)
    {
        _model = model;
        _deep = deep;
        _grandDeep = grandDeep;
    }

    protected override CreatureTemplate CreateTemplate()
    {
        var template = new CreatureTemplate(_model,
            BehaviourType.Leviathan, EcoTargetType.Leviathan, _deep ? 10000 : 5000);
        template.CellLevel = LargeWorldEntity.CellLevel.VeryFar;
        template.SwimRandomData = new SwimRandomData(0.1f,
            _deep ? 15f : 10f, new Vector3(40, 10, 40), _deep ? 10 : 3,
             _deep ? 1.2f : 0.8f);
        template.AnimateByVelocityData = new AnimateByVelocityData(3);
        template.AggressiveWhenSeeTargetList = new List<AggressiveWhenSeeTargetData>()
        {
            new(EcoTargetType.Shark, 1.5f, _deep ? 200 : 70, 2)
        };
        template.AggressiveToPilotingVehicleData = new AggressiveToPilotingVehicleData(17f, 0.4f);
        template.AttackLastTargetData = new AttackLastTargetData(0.3f, _deep ? 25f : 13f, 0.5f, 13f, 40f);
        template.Mass = 1500;
        template.StayAtLeashData = new StayAtLeashData(0.2f, 10f, 60f);
        template.BehaviourLODData = new BehaviourLODData(50, 300, 5000);
        template.LocomotionData = new LocomotionData(10f,
            _grandDeep ? 0.01f : (_deep ? 0.03f : 0.05f), _deep ? 1f : 3f, 0.9f);
        template.CanBeInfected = false;
        template.RespawnData = new RespawnData(false);
        return template;
    }

    protected override IEnumerator ModifyPrefab(GameObject prefab, CreatureComponents components)
    {
        var seamothTask = CraftData.GetPrefabForTechTypeAsync(TechType.Seamoth);
        yield return seamothTask;
        var seamoth = seamothTask.GetResult().GetComponent<SeaMoth>();
        var vortexVfxPrefab = seamoth.torpedoTypes[0].prefab.GetComponent<SeamothTorpedo>().explosionPrefab
            .GetComponent<AddressablesPrefabSpawn>().prefab;
        var handle = vortexVfxPrefab.InstantiateAsync();
        var request = AddressablesPrefabSpawnRequest.Get(handle);
        var complete = false;
        GameObject vortexVfx = null;
        request.RegisterCallback(spawn =>
        {
            vortexVfx = Object.Instantiate(spawn.GetResultAndRelease());
            vortexVfx.SetActive(false);
            Object.DontDestroyOnLoad(vortexVfx);
            vortexVfx.AddComponent<SceneCleanerPreserve>();
            complete = true;
        });
        yield return new WaitUntil(() => complete);
        Object.DestroyImmediate(vortexVfx.GetComponent<VFXDestroyAfterSeconds>());

        prefab.AddComponent<Mono.SwimAmbience>();

        var trailBuilder = new TrailManagerBuilder(components, prefab.SearchChild("Spine2").transform,
            _deep ? 1f : 4f)
        {
            Trails = new[]
            {
                prefab.SearchChild("Spine3").transform,
                prefab.SearchChild("Spine4").transform,
                prefab.SearchChild("Spine5").transform,
                prefab.SearchChild("Spine6").transform,
                prefab.SearchChild("Tail1").transform
            },
            AllowDisableOnScreen = false
        };
        
        trailBuilder.Apply();

        var vortexAttack = prefab.AddComponent<BloopVortexAttack>();
        var bloopVortexEmitter = prefab.AddComponent<FMOD_CustomEmitter>();
        bloopVortexEmitter.followParent = true;
        bloopVortexEmitter.SetAsset(AudioUtils.GetFmodAsset("BloopVortexAttack"));
        vortexAttack.vortexAttackEmitter = bloopVortexEmitter;
        vortexAttack.vortexVfx = vortexVfx;

        var mouth = prefab.SearchChild("Mouth");
        var meleeAttack = prefab.AddComponent<BloopMeleeAttack>();
        meleeAttack.mouth = mouth;
        meleeAttack.canBeFed = false;
        meleeAttack.biteInterval = 3f;
        meleeAttack.biteDamage = 75f;
        meleeAttack.eatHungerDecrement = 0.05f;
        meleeAttack.eatHappyIncrement = 0.1f;
        meleeAttack.biteAggressionDecrement = 0.02f;
        meleeAttack.biteAggressionThreshold = 0.1f;
        meleeAttack.lastTarget = components.LastTarget;
        meleeAttack.creature = components.Creature;
        meleeAttack.liveMixin = components.LiveMixin;
        meleeAttack.animator = components.Animator;
        var meleeAttackEmitter = prefab.AddComponent<FMOD_CustomEmitter>();
        meleeAttackEmitter.followParent = true;
        meleeAttackEmitter.SetAsset(AudioUtils.GetFmodAsset("BlazaBite"));
        meleeAttack.emitter = meleeAttackEmitter;

        mouth.AddComponent<OnTouch>();

        var attackCyclops = prefab.AddComponent<AttackCyclops>();
        attackCyclops.swimVelocity = 15f;
        attackCyclops.aggressiveToNoise = new CreatureTrait(0f, 0.01f);
        attackCyclops.evaluatePriority = 0.6f;
        attackCyclops.maxDistToLeash = 70f;
        attackCyclops.attackAggressionThreshold = 0.4f;

        if (_deep)
        {
            var deepBloopVoiceEmitter = prefab.AddComponent<FMOD_CustomLoopingEmitter>();
            deepBloopVoiceEmitter.SetAsset(AudioUtils.GetFmodAsset("DeepBloopAmbience"));
            deepBloopVoiceEmitter.playOnAwake = true;
            deepBloopVoiceEmitter.followParent = true;
        }
        else
        {
            var bloopVoiceEmitter = prefab.AddComponent<FMOD_CustomEmitter>();
            bloopVoiceEmitter.followParent = true;
            var bloopVoice = prefab.AddComponent<CreatureVoice>();
            bloopVoice.emitter = bloopVoiceEmitter;
            bloopVoice.closeIdleSound = AudioUtils.GetFmodAsset("BloopIdle");
            bloopVoice.minInterval = 20f;
            bloopVoice.maxInterval = 40f;
        }

        if (_grandDeep)
        {
            prefab.transform.Find("Pivot").localScale = Vector3.one * 3;
            var capsule = prefab.GetComponent<CapsuleCollider>();
            capsule.radius *= 3;
            capsule.height *= 3;
            capsule.center *= 3;
        }
    }

    protected override void PostRegister()
    {
        if (_grandDeep)
        {
            CreatureDataUtils.AddCreaturePDAEncyclopediaEntry(this, "Lifeforms/Fauna/Leviathans", null, null, 7.7777f,
                null,
                null);
        }
        else if (_deep)
        {
            CreatureDataUtils.AddCreaturePDAEncyclopediaEntry(this, "Lifeforms/Fauna/Leviathans", null, null, 6f,
                null,
                Plugin.AssetBundle.LoadAsset<Sprite>("DeepBloop_Popup"));
        }
        else
        {
            CreatureDataUtils.AddCreaturePDAEncyclopediaEntry(this, "Lifeforms/Fauna/Leviathans", null, null, 6f,
                Plugin.AssetBundle.LoadAsset<Texture2D>("Fatfish_Ency"),
                Plugin.AssetBundle.LoadAsset<Sprite>("Fatfish_Popup"));
        }
    }

    protected override void ApplyMaterials(GameObject prefab)
    {
        MaterialUtils.ApplySNShaders(prefab, 12f, 5f, _deep ? 20f : 2f);
    }
}