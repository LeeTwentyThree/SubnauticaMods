using ECCLibrary;
using System.Collections.Generic;
using ECCLibrary.Data;
using System.Collections;
using Nautilus.Assets;
using Nautilus.Utility;
using TheRedPlague.Mono.PlagueGarg.GargBehavior.GargEssentials;
using TheRedPlague.PrefabFiles.GargTeaser.GargPrefabData;
using UnityEngine;

namespace TheRedPlague.PrefabFiles.GargTeaser;

public class GargantuanBase : CreatureAsset
{
    private GameObject _model;

    public GargantuanBase(PrefabInfo info, GameObject model) : base(info)
    {
        _model = model;
    }

    // Properties that vary across the different types of gargs
    protected virtual float SlowSwimVelocity => 10;
    protected virtual float FastSwimVelocity => 25;
    protected virtual GargRoarSettings RoarSettings { get; } = new GargRoarSettings(true, 11, 18, false, GargantuanRoar.RoarMode.Automatic, "RPGargAdult", 150, false);
    protected virtual GargSmallVehicleAggressionSettings SmallVehicleAggressionSettings => null;
    protected virtual bool UseEyeTracking => false;
    protected virtual string SwimSoundEventPath => null;
    protected virtual float CyclopsStalkVelocity => FastSwimVelocity;
    protected virtual GargBiteSettings BiteSettings { get; } = new GargBiteSettings(true, 1500, true, false, GargGrabFishMode.CantGrabFish);
    protected virtual TrailManagerSettings SpineTrailManagerSettings { get; } = new TrailManagerSettings(0.075f, 40f, new AnimationCurve(new Keyframe(0, 0.7f), new Keyframe(0.7f, 0.7f), new Keyframe(1, 0.05f)));
    protected virtual TrailManagerSettings JawTrailManagerSettings { get; } = new TrailManagerSettings(6, -1, new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1)));
    protected virtual TrailManagerSettings StabilizerFinTrailManagerSettings { get; } = new TrailManagerSettings(30, -1, new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1)));
    protected virtual TrailManagerSettings TentacleTrailManagerSettings { get; } = new TrailManagerSettings(6, -1, new AnimationCurve(new Keyframe(0, 0.25f), new Keyframe(1, 0.26f)));
    protected virtual float GrabAttackDamage => 10000f;
    protected virtual GargCollisionsMode CollisionsMode => GargCollisionsMode.None;
    protected virtual bool CanStalkCyclops => false;
    protected virtual bool AggressiveTowardsCyclops => true;
    // Seamoth has 300 health. Vehicle attack lasts 4 seconds.
    protected virtual float VehicleDamagePerSecond => 49f;
    // Cyclops has 1500 health. Cyclops attack lasts 11 seconds.
    protected virtual float CyclopsDamagePerSecond => 130f;
    
    // Component references
    protected TrailManager spineTrailManager;

    // Constants
    private const string kAttachBoneName = "AttachBone";

    protected override CreatureTemplate CreateTemplate()
    {
        var template = new CreatureTemplate(_model, BehaviourType.Leviathan, EcoTargetType.Leviathan, float.MaxValue)
        {
            CellLevel = LargeWorldEntity.CellLevel.VeryFar,
            SwimRandomData = new SwimRandomData(0.1f, SlowSwimVelocity, new Vector3(120, 30, 120), 3, 1f, true),
            StayAtLeashData = new StayAtLeashData(0.2f, SlowSwimVelocity, 120f),
            LocomotionData = new LocomotionData(27, 0.3f, 1f, 1f),
            SwimBehaviourData = new SwimBehaviourData(0.3f),
            EyeFOV = -1,
            ScannerRoomScannable = true,
            BehaviourLODData = new BehaviourLODData(75, 1000, 2000),
            AttackLastTargetData = new AttackLastTargetData(0.4f, 24, 0.5f, 30, 17, 20, 30, true),
            Mass = 10000,
            AcidImmune = true,
            CanBeInfected = false,
            AvoidObstaclesData = new AvoidObstaclesData(1, FastSwimVelocity, false, 30, 30, 3, 0.5f, avoidanceIterations: 15),
            SurfaceType = VFXSurfaceTypes.metal,
            TraitsData = new CreatureTraitsData(0.07f, 0.04f, 0.4f),
            AnimateByVelocityData = new AnimateByVelocityData(15)
        };

        return template;
    }

    protected virtual void ApplyAggression(GameObject prefab, CreatureComponents components)
    {
        CreaturePrefabUtils.AddAggressiveWhenSeeTarget(prefab, new AggressiveWhenSeeTargetData(EcoTargetType.Shark, 2, 80, 2, true, false, 0.2f, 0), components.LastTarget, components.Creature);
        CreaturePrefabUtils.AddAggressiveWhenSeeTarget(prefab, new AggressiveWhenSeeTargetData(EcoTargetType.Whale, 2.3f, 60, 2, true, false, 0.23f, 0), components.LastTarget, components.Creature);
        CreaturePrefabUtils.AddAggressiveWhenSeeTarget(prefab, new AggressiveWhenSeeTargetData(EcoTargetType.Leviathan, 5f, 250, 6, true, false, 0.3f, 0), components.LastTarget, components.Creature);
    }

    protected override void ApplyMaterials(GameObject prefab)
    {
        MaterialUtils.ApplySNShaders(prefab, 5, 1, 2);
    }

    protected override IEnumerator ModifyPrefab(GameObject prefab, CreatureComponents components)
    {
        ApplyAggression(prefab, components);

        // advanced collision and trail manager stuff, i wrote it and still don't even know what it does
        var stopPlacingColliders = false;

        var spines = new List<Transform>();

        GameObject currentSpine = prefab.SearchChild("Spine");
        while (currentSpine != null)
        {
            currentSpine = currentSpine.SearchChild("Spine", ECCStringComparison.StartsWith);
            if (currentSpine == null) continue;

            if (currentSpine.name.Contains("57")) // dont add colliders after you've gone to the 57th spine. kinda arbitrary.
            {
                stopPlacingColliders = true;
            }
            if (CollisionsMode != GargCollisionsMode.None && stopPlacingColliders == false && currentSpine.name != "Spine") // dont add collider to first spine (the one named "Spine")
            {
                if (CollisionsMode == GargCollisionsMode.Trigger)
                {
                    currentSpine.layer = LayerID.Useable;
                    var newCapsule = currentSpine.AddComponent<CapsuleCollider>();
                    newCapsule.height = 0.85f;
                    newCapsule.direction = 1;
                    newCapsule.isTrigger = true;
                    var firstSpine = currentSpine.name.Contains("001");
                    newCapsule.radius = firstSpine ? 0.14f : 0.2f; //first segment of the garg is a lot thinner than the rest, until it gradually tapers off about halfway
                }

                currentSpine.AddComponent<GargantuanBone>().creature = components.Creature;
            }
            spines.Add(currentSpine.transform);
            if (currentSpine.name.Contains("60"))
            {
                break;
            }
        }

        spines.Add(prefab.SearchChild("Tail").transform);
        spines.Add(prefab.SearchChild("Tail1").transform);
        spines.Add(prefab.SearchChild("Tail2").transform);
        spines.Add(prefab.SearchChild("Tail3").transform);
        spines.Add(prefab.SearchChild("Tail4").transform);
        spines.Add(prefab.SearchChild("Tail5").transform);
        spines.Add(prefab.SearchChild("Tail6").transform);

        var fakeRoot = new GameObject("Fake Trail Manager Root").transform;
        fakeRoot.SetParent(prefab.transform);
        fakeRoot.localEulerAngles = new Vector3(-90, 90, 180);
        
        var spineTrailManagerBuilder = new TrailManagerBuilder(components, prefab.SearchChild("Spine").transform, SpineTrailManagerSettings.SnapSpeed, SpineTrailManagerSettings.MaxOffset);
        spineTrailManagerBuilder.Trails = spines.ToArray();
        spineTrailManagerBuilder.SetAllMultiplierAnimationCurves(SpineTrailManagerSettings.Multiplier);
        spineTrailManagerBuilder.CreatureRoot = fakeRoot;

        spineTrailManager = spineTrailManagerBuilder.Apply();

        var tentacleNames = new string[] {"BLT", "BRT", "TLT", "TRT", "MLT", "MRT"};
        foreach (var name in tentacleNames)
        {
            ApplyTrailManager(prefab, prefab.SearchChild(name), components, TentacleTrailManagerSettings, prefab.transform);
        }
        
        var jawTentacleNames = new string[] {"LLA", "LRA", "SLA", "SRA", "LJT", "RJT"};
        foreach (var name in jawTentacleNames)
        {
            ApplyTrailManager(prefab, prefab.SearchChild(name), components, JawTrailManagerSettings, prefab.transform);
        }
        
        var stabilizerFins = new string[]
        {
            "FGFLF", "FGFRF", "FGMLF", "FGMRF", "FGBLF", "FGBRF",
            "FMGFLF", "FMGFRF", "FMGMLF", "FMGMRF", "FMGBLF", "FMGBRF",
            "BMGFLF", "BMGFRF", "BMGMLF", "BMGMRF", "BMGBLF", "BMGBRF",
            "BGFLF", "BGFRF", "BGMLF", "BGMRF", "BGBLF", "BGBRF"
        };
        
        foreach (var name in stabilizerFins)
        {
            ApplyTrailManager(prefab, prefab.SearchChild($"{name}.001"), components, StabilizerFinTrailManagerSettings, prefab.transform);
        }
        
        var atkLast = prefab.GetComponent<AttackLastTarget>();
        if (atkLast)
        {
            atkLast.resetAggressionOnTime = false;
            atkLast.swimInterval = 0.2f;
        }

        var gargantuanBehaviour = prefab.EnsureComponent<GargantuanBehaviour>();

        if (CollisionsMode == GargCollisionsMode.Solid)
        {
            prefab.EnsureComponent<GargAdultPhysics>().gargantuanBehaviour = gargantuanBehaviour;
        }

        GargantuanGrab gargantuanGrab = prefab.EnsureComponent<GargantuanGrab>();
        gargantuanGrab.attachBoneName = kAttachBoneName;
        gargantuanGrab.vehicleDamagePerSecond = VehicleDamagePerSecond;
        gargantuanGrab.cyclopsDamagePerSecond = CyclopsDamagePerSecond;
        gargantuanGrab.grabFishMode = BiteSettings.GrabFishMode;
        gargantuanGrab.grabAttackDamage = GrabAttackDamage;

        var gargantuanGrabEmitter = new GameObject("GrabSound").AddComponent<FMOD_CustomEmitter>();
        gargantuanGrabEmitter.transform.SetParent(prefab.transform, false);
        gargantuanGrabEmitter.transform.localPosition = Vector3.zero;
        gargantuanGrabEmitter.followParent = true;
        gargantuanGrab.emitter = gargantuanGrabEmitter;

        GameObject mouth = prefab.SearchChild("Mouth");
        GargantuanMouthAttack mouthAttack = prefab.AddComponent<GargantuanMouthAttack>();
        mouthAttack.mouth = mouth;
        mouthAttack.canBeFed = false;
        mouthAttack.biteInterval = 2f;
        mouthAttack.lastTarget = components.LastTarget;
        mouthAttack.creature = components.Creature;
        mouthAttack.liveMixin = components.LiveMixin;
        mouthAttack.animator = components.Creature.GetAnimator();
        mouthAttack.canAttackPlayer = BiteSettings.CanAttackPlayer;
        mouthAttack.biteDamage = BiteSettings.Damage;
        mouthAttack.instantlyKillsPlayer = BiteSettings.InstantlyKillsPlayer;
        mouthAttack.attachBoneName = kAttachBoneName;
        mouthAttack.canPerformCyclopsCinematic = BiteSettings.CanGrabCyclops;
        mouthAttack.grabFishMode = BiteSettings.GrabFishMode;

        var gargantuanMouthEmitter = new GameObject("BiteSounds").AddComponent<FMOD_CustomEmitter>();
        gargantuanMouthEmitter.transform.SetParent(prefab.transform, false);
        gargantuanMouthEmitter.transform.localPosition = Vector3.zero;
        gargantuanMouthEmitter.followParent = true;
        mouthAttack.emitter = gargantuanGrabEmitter;

        if (AggressiveTowardsCyclops)
        {
            var actionAtkCyclops = prefab.AddComponent<AttackCyclops>();
            actionAtkCyclops.swimVelocity = 25f;
            actionAtkCyclops.aggressiveToNoise = new CreatureTrait(0f, 0.02f);
            actionAtkCyclops.evaluatePriority = 0.5f;
            actionAtkCyclops.priorityMultiplier = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));
            actionAtkCyclops.maxDistToLeash = 110f;
            actionAtkCyclops.attackAggressionThreshold = 0.65f;
            actionAtkCyclops.aggressPerSecond = 5f;
            actionAtkCyclops.lastTarget = components.LastTarget;
        }

        if (CanStalkCyclops)
        {
            var cyclopsAggression = prefab.AddComponent<GargAdultCyclopsAggression>();
            cyclopsAggression.chargeSpeed = CyclopsStalkVelocity;
        }
        
        if (RoarSettings != null)
        {
            GargantuanRoar roar = prefab.AddComponent<GargantuanRoar>();
            roar.gargantuanBehaviour = gargantuanBehaviour;

            roar.delayMin = RoarSettings.MinRoarDelay;
            roar.delayMax = RoarSettings.MaxRoarDelay;
            roar.screenShake = RoarSettings.TriggerScreenShake;
            roar.closeRoarThreshold = RoarSettings.CloseRoarDistanceThreshold;
            roar.roarDoesDamage = RoarSettings.DealsDamage;
            roar.producesChromaticAberration = RoarSettings.ProducesChromaticAberration;
            roar.roarMode = RoarSettings.RoarMode;

            if (RoarSettings.RoarMode.HasFlag(GargantuanRoar.RoarMode.CloseOnly))
            {
                roar.closeEmitter = prefab.AddComponent<FMOD_CustomEmitter>();
                roar.closeEmitter.SetAsset(AudioUtils.GetFmodAsset($"{RoarSettings.RoarSoundEventPrefix}RoarClose"));
                roar.closeEmitter.followParent = true;
                roar.closeEmitter.playOnAwake = false;
            }

            if (RoarSettings.RoarMode.HasFlag(GargantuanRoar.RoarMode.FarOnly))
            {
                roar.farEmitter = prefab.AddComponent<FMOD_CustomEmitter>();
                roar.farEmitter.SetAsset(AudioUtils.GetFmodAsset($"{RoarSettings.RoarSoundEventPrefix}RoarFar"));
                roar.farEmitter.followParent = true;
                roar.farEmitter.playOnAwake = false;
            }
        }

        if (!string.IsNullOrEmpty(SwimSoundEventPath))
        {
            var swimAmbience = prefab.AddComponent<GargantuanSwimAmbience>();
            swimAmbience.eventPath = SwimSoundEventPath;
        }

        if (UseEyeTracking)
        {
            var eyes = new GargEyeTracker[]
            {
                prefab.SearchChild("BLE").AddComponent<GargEyeTracker>(),
                prefab.SearchChild("BRE").AddComponent<GargEyeTracker>(),
                prefab.SearchChild("FLE").AddComponent<GargEyeTracker>(),
                prefab.SearchChild("FRE").AddComponent<GargEyeTracker>(),
                prefab.SearchChild("MLE").AddComponent<GargEyeTracker>(),
                prefab.SearchChild("MRE").AddComponent<GargEyeTracker>()
            };

            foreach (var eye in eyes)
            {
                eye.garg = gargantuanBehaviour;
            }
        }

        if (SmallVehicleAggressionSettings != null)
        {
            var attackSmallVehicles = prefab.AddComponent<AggressiveToPilotingVehicle>();
            attackSmallVehicles.lastTarget = components.LastTarget;
            attackSmallVehicles.creature = components.Creature;
            attackSmallVehicles.range = SmallVehicleAggressionSettings.MaxRange;
            attackSmallVehicles.aggressionPerSecond = SmallVehicleAggressionSettings.AggressionPerSecond;
        }

        prefab.SearchChild("BLE").AddComponent<GargEyeFixer>();
        prefab.SearchChild("BRE").AddComponent<GargEyeFixer>();
        prefab.SearchChild("FLE").AddComponent<GargEyeFixer>();
        prefab.SearchChild("FRE").AddComponent<GargEyeFixer>();
        prefab.SearchChild("MLE").AddComponent<GargEyeFixer>();
        prefab.SearchChild("MRE").AddComponent<GargEyeFixer>();

        prefab.AddComponent<VFXSchoolFishRepulsor>();

        yield break;
    }

    private TrailManager ApplyTrailManager(GameObject prefab, GameObject boneRoot, CreatureComponents components, TrailManagerSettings settings, Transform creatureRoot)
    {
        var builder = new TrailManagerBuilder(components, boneRoot.transform, settings.SnapSpeed, settings.MaxOffset);
        builder.CreatureRoot = creatureRoot;
        builder.SetTrailArrayToAllChildren();
        builder.SetAllMultiplierAnimationCurves(settings.Multiplier);
        var trailManager = builder.Apply();
        return trailManager;
    }

    private List<Transform> GetAllChildBones(GameObject prefab, string boneRootName)
    {
        var result = new List<Transform>();
        var currentBone = prefab.SearchChild(boneRootName)?.transform;
        while (currentBone != null && currentBone.transform.childCount > 0)
        {
            currentBone = currentBone.transform.GetChild(0);
            if (currentBone)
                result.Add(currentBone);
        }

        return result;
    }
}

public enum GargGrabFishMode
{
    CantGrabFish,            // None
    LeviathansOnlyNoSwallow,  // Juvenile garg
    LeviathansOnlyAndSwallow, // Adult garg
    PickupableOnlyAndSwalllow // Baby garg
}

public enum GargCollisionsMode
{
    None,
    Trigger,
    Solid
}