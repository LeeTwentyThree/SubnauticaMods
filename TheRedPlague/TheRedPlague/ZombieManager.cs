using System.Linq;
using Nautilus.Handlers;
using Nautilus.Utility;
using TheRedPlague.Mono;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace TheRedPlague;

public static class ZombieManager
{
    private static FMODAsset _biteSound = AudioUtils.GetFmodAsset("event:/creature/blood_kelp_biter/bite");
    
    public static bool IsZombie(GameObject creature)
    {
        return creature.GetComponent<Zombified>() != null;
    }

    public static void Zombify(GameObject creature)
    {
        if (IsZombie(creature)) return;
        
        AddZombieBehaviour(creature);
        var infectedMixin = creature.GetComponent<InfectedMixin>();
        if (infectedMixin)
        {
            infectedMixin.SetInfectedAmount(4);
        }
    }
    
    public static void AddZombieBehaviour(GameObject creature)
    {
        creature.AddComponent<Zombified>();
        
        var techType = CraftData.GetTechType(creature);

        var creatureComponent = creature.GetComponent<Creature>();

        if (creatureComponent == null)
        {
            // Debug.LogWarning($"No creature component on infected object {creature.name}!");
        }
        
        bool aggressiveToSharks = false;
        bool aggressiveToWhale = false;
        foreach (var aggressiveWhenSeeTarget in creature.GetComponents<AggressiveWhenSeeTarget>())
        {
            if (aggressiveWhenSeeTarget.targetType == EcoTargetType.Shark)
                aggressiveToSharks = true;
            if (aggressiveWhenSeeTarget.targetType == EcoTargetType.Whale)
                aggressiveToWhale = true;
        }

        if (!aggressiveToSharks)
        {
            MakeCreatureAggressiveToEcoTargetType(creatureComponent, EcoTargetType.Shark);
        }
        if (!aggressiveToWhale)
        {
            MakeCreatureAggressiveToEcoTargetType(creatureComponent, EcoTargetType.Whale);
        }

        if (creature.GetComponent<AttackLastTarget>() == null)
        {
            AddAttackLastTarget(creatureComponent);
        }

        if (creature.GetComponent<MeleeAttack>() == null)
        {
            AddMeleeAttack(creatureComponent);
        }

        // creature.GetComponent<LiveMixin>().health = float.MaxValue;

        creatureComponent.Scared = new CreatureTrait(0, 100000f);
        creatureComponent.Aggression = new CreatureTrait(1, 0.01f);
        
        creatureComponent.ScanCreatureActions();

        var pickupable = creature.GetComponent<Pickupable>();
        if (pickupable)
        {
            pickupable.isPickupable = false;
        }
        
        AmalgamationManager.AmalgamateCreature(creature);
    }

    private static void AddMeleeAttack(Creature creature)
    {
        if (creature.GetAnimator() == null)
        {
            // Plugin.Logger.LogWarning($"Creature '{creature.gameObject.name}' has no Animator! Skipping MeleeAttack instantiation.");
            return;
        }
        var meleeAttack = creature.gameObject.AddComponent<MeleeAttack>();
        meleeAttack.biteAggressionThreshold = 0.1f;
        meleeAttack.biteInterval = 2;
        meleeAttack.biteDamage = creature.liveMixin.maxHealth >= 200 ? 14 : 2;
        meleeAttack.biteAggressionDecrement = 0.2f;
        meleeAttack.lastTarget = creature.GetComponent<LastTarget>();
        meleeAttack.creature = creature;
        meleeAttack.liveMixin = creature.liveMixin;
        meleeAttack.animator = creature.GetAnimator();
        meleeAttack.canBiteVehicle = true;
        meleeAttack.canBiteCyclops = true;
        var biteEmitter = creature.gameObject.AddComponent<FMOD_StudioEventEmitter>();
        biteEmitter.asset = _biteSound;
        biteEmitter.path = _biteSound.path;
        meleeAttack.attackSound = biteEmitter;
        
        var triggerObj = new GameObject("AttackTrigger");
        meleeAttack.mouth = triggerObj;
        var collider = triggerObj.AddComponent<SphereCollider>();
        collider.isTrigger = true;
        triggerObj.AddComponent<VFXSurface>().surfaceType = VFXSurfaceTypes.organic;
        var onTouch = triggerObj.AddComponent<OnTouch>();
        onTouch.onTouch = new OnTouch.OnTouchEvent();
        onTouch.onTouch.AddListener(meleeAttack.OnTouch);

        triggerObj.transform.parent = creature.transform;
        triggerObj.transform.localPosition = Vector3.forward * 0.5f;
    }

    private static void MakeCreatureAggressiveToEcoTargetType(Creature creature, EcoTargetType type)
    {
        var aggressiveComponent = creature.gameObject.AddComponent<AggressiveWhenSeeTarget>();
        // What the fuck is this and why does every creature do this
        aggressiveComponent.maxRangeMultiplier = new AnimationCurve(new Keyframe(0, 1), new Keyframe(0.5f, 0.5f), new Keyframe(1, 1));
        aggressiveComponent.distanceAggressionMultiplier = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 0));
        aggressiveComponent.creature = creature;
        aggressiveComponent.targetType = type;
        aggressiveComponent.maxRangeScalar = 100;
        aggressiveComponent.maxSearchRings = 2;
        aggressiveComponent.aggressionPerSecond = 1;
        aggressiveComponent.ignoreSameKind = false;
        aggressiveComponent.hungerThreshold = 0;
        
        var lastTarget = creature.gameObject.EnsureComponent<LastTarget>();
        aggressiveComponent.lastTarget = lastTarget;
    }

    private static void AddAttackLastTarget(Creature creature)
    {
        if (creature.GetAnimator() == null)
        {
            // Plugin.Logger.LogWarning($"Creature '{creature.gameObject.name}' has no Animator! Skipping AttackLastTarget instantiation.");
            return;
        }
        var attackLastTarget = creature.gameObject.AddComponent<AttackLastTarget>();
        var swimRandom = creature.GetComponent<SwimRandom>();
        attackLastTarget.swimVelocity = swimRandom != null ? swimRandom.swimVelocity * 2f : 10f;
        attackLastTarget.aggressionThreshold = 0.75f;
        attackLastTarget.swimInterval = 0.5f;
        attackLastTarget.minAttackDuration = 3f;
        attackLastTarget.maxAttackDuration = 10f;
        attackLastTarget.pauseInterval = 15f;
        attackLastTarget.rememberTargetTime = 5f;
        attackLastTarget.evaluatePriority = 1.1f;
        attackLastTarget.lastTarget = creature.gameObject.GetComponent<LastTarget>();
    }

    public static void InfectSeaEmperor(GameObject seaEmperor)
    {
        var renderers = seaEmperor.GetComponentsInChildren<Renderer>().Where(r => !(r is ParticleSystemRenderer) && !(r is TrailRenderer));
        foreach (var r in renderers)
        {
            var materials = r.materials;
            foreach (var m in materials)
            {
                m.shader = MaterialUtils.Shaders.MarmosetUBER;
                m.color = new Color(3, 3, 3);
                m.EnableKeyword(InfectedMixin.uwe_infection);
                m.SetFloat(ShaderPropertyID._InfectionAmount, 1);
                m.SetFloat("InfectionHeightStrength", -3.9f);
                m.SetVector("_InfectionScale", new Vector4(2, 2, 2, 0));
                m.SetVector("_InfectionOffset", new Vector4(0.285f, 0, 0.142f, 0));
                m.SetColor("_GlowColor", new Color(3, 0, 0));
                m.SetTexture(ShaderPropertyID._InfectionAlbedomap, Plugin.ZombieInfectionTexture);
            }
        }
    }
    
    public static float GetInfectionStrengthAtPosition(Vector3 position)
    {
        var depthWeight = Mathf.InverseLerp(0, -1500, position.y);
        var distanceWeight = Mathf.Sqrt(position.x * position.x + position.z * position.z) / 2400;
        return Mathf.Clamp01(depthWeight + distanceWeight);
    }
}