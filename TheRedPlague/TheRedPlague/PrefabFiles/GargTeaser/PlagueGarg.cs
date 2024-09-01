using System.Collections;
using ECCLibrary;
using ECCLibrary.Data;
using Nautilus.Assets;
using TheRedPlague.Mono;
using TheRedPlague.Mono.PlagueGarg;
using TheRedPlague.Mono.PlagueGarg.GargBehavior.GargEssentials;
using TheRedPlague.PrefabFiles.GargTeaser.GargPrefabData;
using UnityEngine;

namespace TheRedPlague.PrefabFiles.GargTeaser;

public class PlagueGarg : GargantuanBase
{
    private static readonly int _zWrite = Shader.PropertyToID("_ZWrite");
    private static readonly int _fresnel = Shader.PropertyToID("_Fresnel");
    private static readonly int _shininess = Shader.PropertyToID("_Shininess");
    private static readonly int _specInt = Shader.PropertyToID("_SpecInt");
    private static readonly int _emissionLm = Shader.PropertyToID("_EmissionLM");
    private static readonly int _emissionLmNight = Shader.PropertyToID("_EmissionLMNight");
    private static readonly int _glowStrength = Shader.PropertyToID("_GlowStrength");
    private static readonly int _glowStrengthNight = Shader.PropertyToID("_GlowStrengthNight");
    private static readonly int InfectionHeightStrength = Shader.PropertyToID("_InfectionHeightStrength");
    private static readonly int InfectionScale = Shader.PropertyToID("_InfectionScale");
    private static readonly int InfectionOffset = Shader.PropertyToID("_InfectionOffset");

    public PlagueGarg(PrefabInfo info, GameObject model) : base(info, model)
    {
    }
    protected override float SlowSwimVelocity => 30f;
    protected override float FastSwimVelocity => 50f;
    protected override GargBiteSettings BiteSettings { get; } = new GargBiteSettings(true, 5000, true, true, GargGrabFishMode.LeviathansOnlyAndSwallow);
    protected override float VehicleDamagePerSecond => 80;
    protected override TrailManagerSettings TentacleTrailManagerSettings { get; } = new TrailManagerSettings(2, -1, new AnimationCurve(new Keyframe(0, 0.25f), new Keyframe(1, 0.26f)));
    protected override TrailManagerSettings SpineTrailManagerSettings { get; } = new TrailManagerSettings(0.075f, 40f, new AnimationCurve(new Keyframe(0, 0.7f), new Keyframe(0.7f, 0.7f), new Keyframe(1, 0.05f)));
    protected override TrailManagerSettings JawTrailManagerSettings => new TrailManagerSettings(10f, -1, new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1)));
    protected override float CyclopsDamagePerSecond => 127;
    protected override float CyclopsStalkVelocity => 26f;
    protected override bool CanStalkCyclops => true;
    protected override GargCollisionsMode CollisionsMode => GargCollisionsMode.Solid;
    protected override bool UseEyeTracking => true;
    protected override GargRoarSettings RoarSettings => new GargRoarSettings(true, 40, 80, true, GargantuanRoar.RoarMode.Automatic, "PlagueGarg", 350, true);
    protected override string SwimSoundEventPath => null;
    protected override GargSmallVehicleAggressionSettings SmallVehicleAggressionSettings => new GargSmallVehicleAggressionSettings(300, 0.08f);

    protected override CreatureTemplate CreateTemplate()
    {
        var template = base.CreateTemplate();
        template.EyeFOV = -1;
        template.StayAtLeashData = new StayAtLeashData(0.2f, SlowSwimVelocity, 300);
        template.AttackLastTargetData = new AttackLastTargetData(0.4f, FastSwimVelocity, 0.35f, 30f, 17f, 25f, 30f);
        template.AnimateByVelocityData = new AnimateByVelocityData(FastSwimVelocity + 10);
        template.SwimRandomData = new SwimRandomData(0.1f, SlowSwimVelocity, new Vector3(250, 125, 250), 4f, 1f, true);
        template.BehaviourLODData = new BehaviourLODData(20000, 40000, 100000);
        template.SwimBehaviourData = new SwimBehaviourData(0.1f);
        template.LocomotionData = new LocomotionData(45, 0.1f, 0.6f, 1f);
        template.AvoidObstaclesData = null;
        template.CellLevel = LargeWorldEntity.CellLevel.Global;
        template.CanBeInfected = true;
        return template;
    }

    protected override IEnumerator ModifyPrefab(GameObject prefab, CreatureComponents components)
    {
        yield return base.ModifyPrefab(prefab, components);
        
        Renderer gutsRenderer = prefab.SearchChild("Gargantuan.001").GetComponent<SkinnedMeshRenderer>();
        Renderer eyesRenderer = prefab.SearchChild("Gargantuan.002").GetComponent<SkinnedMeshRenderer>();
        Renderer skeletonRenderer = prefab.SearchChild("Gargantuan.003").GetComponent<SkinnedMeshRenderer>();
        Renderer tongueRenderer = prefab.SearchChild("Gargantuan.007").GetComponent<SkinnedMeshRenderer>();
        
        UpdateGargGutsMaterial(gutsRenderer.materials[0]);
        UpdateGargEyeMaterial(eyesRenderer.materials[0]);
        UpdateGargSolidMaterial(skeletonRenderer.materials[0]);
        UpdateGargGutsMaterial(skeletonRenderer.materials[1]);
        UpdateGargSolidMaterial(skeletonRenderer.materials[2]);
        UpdateGargSolidMaterial(tongueRenderer.materials[0]);
        
        var swimAmbience = prefab.AddComponent<GargantuanSwimAmbience>();
        swimAmbience.delay = 10;
        swimAmbience.eventPath = "PlagueGargSnarl";
        
        ApplyAdvancedObstacleAvoidance(prefab, components, 1f, false, 30f);
        
        components.Rigidbody.angularDrag = 1;
        
        components.WorldForces.waterDepth = -30f;
        components.WorldForces.aboveWaterGravity = 25f;
        
        components.SkyApplier.dynamic = false;

        prefab.AddComponent<InfectOnStart>();

        prefab.AddComponent<PlagueGargDestroyCables>().evaluatePriority = 100;

        // prefab.AddComponent<DisableAuroraRadiationOnStart>();
    }
    
    private void ApplyAdvancedObstacleAvoidance(GameObject prefab, CreatureComponents components, float evaluatePriority, bool terrainOnly, float avoidDistance)
    {
        GargAvoidObstacles avoidObstacles = prefab.AddComponent<GargAvoidObstacles>();
        avoidObstacles.avoidTerrainOnly = terrainOnly;
        avoidObstacles.avoidanceDistance = avoidDistance;
        avoidObstacles.scanDistance = avoidDistance;
        avoidObstacles.priorityMultiplier = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));
        avoidObstacles.evaluatePriority = evaluatePriority;
        avoidObstacles.swimVelocity = SlowSwimVelocity;
        avoidObstacles.lastTarget = components.LastTarget;
        avoidObstacles.avoidanceDistance = 100f;
        avoidObstacles.avoidanceIterations = 25;
        avoidObstacles.scanDistance = 35;
        avoidObstacles.scanInterval = 0.2f;
        avoidObstacles.scanDistance = 100f;
        avoidObstacles.scanRadius = 100f;
    }

    public static void UpdateGargSolidMaterial(Material material)
    {
        material.SetFloat(_fresnel, 0.6f);
        material.SetFloat(_shininess, 8f);
        material.SetFloat(_specInt, 15);
        material.SetFloat(_emissionLm, 0.1f);
        material.SetFloat(_emissionLmNight, 0.1f);
        material.SetColor(ShaderPropertyID._SpecColor, material.color);
        material.SetFloat(ShaderPropertyID._InfectionAmount, 4);
        UpdateInfectionMaterial(material);
    }

    public static void UpdateGargEyeMaterial(Material material)
    {
        material.SetFloat(_specInt, 15f);
        material.SetFloat(_glowStrength, 0.70f);
        material.SetFloat(_glowStrengthNight, 0.70f);
        UpdateInfectionMaterial(material);
    }

    public static void UpdateGargSkeletonMaterial(Material material)
    {
        material.SetFloat(_fresnel, 1);
        material.SetFloat(_specInt, 50);
        material.SetFloat(_glowStrength, 2.5f);
        material.SetFloat(_glowStrengthNight, 6f);
        UpdateInfectionMaterial(material);
    }

    public static void UpdateGargGutsMaterial(Material material)
    {
        material.EnableKeyword("MARMO_ALPHA_CLIP");
        material.SetFloat(_fresnel, 1f);
        material.SetFloat(_specInt, 50);
        material.SetFloat(_glowStrength, 10f);
        material.SetFloat(_glowStrengthNight, 10f);
        UpdateInfectionMaterial(material);
    }

    private static void UpdateInfectionMaterial(Material material)
    {
        material.SetFloat(InfectionHeightStrength, 0.1f);
        material.SetVector(InfectionScale, Vector4.one);
        material.SetVector(InfectionOffset, Vector4.zero);
    }

    protected override void ApplyAggression(GameObject prefab, CreatureComponents components)
    {
        CreaturePrefabUtils.AddAggressiveWhenSeeTarget(prefab, new AggressiveWhenSeeTargetData(EcoTargetType.Shark, 0.5f, 250,
            6, true, false, 0.2f, 0), components.LastTarget, components.Creature);
        CreaturePrefabUtils.AddAggressiveWhenSeeTarget(prefab, new AggressiveWhenSeeTargetData(EcoTargetType.Whale, 2.3f, 100,
            2, true, false, 0.23f, 0), components.LastTarget, components.Creature);
        CreaturePrefabUtils.AddAggressiveWhenSeeTarget(prefab, new AggressiveWhenSeeTargetData(EcoTargetType.Leviathan, 3, 350,
            7, true, false, 0.3f, 0), components.LastTarget, components.Creature);
    }
}