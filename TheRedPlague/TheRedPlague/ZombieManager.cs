using Nautilus.Handlers;
using TheRedPlague.Mono;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace TheRedPlague;

public static class ZombieManager
{
    public static bool IsZombie(GameObject creature)
    {
        return creature.GetComponent<Zombified>() != null;
    }
    
    public static void AddZombieAI(GameObject creature)
    {
        creature.AddComponent<Zombified>();
        
        var techType = CraftData.GetTechType(creature);

        var creatureComponent = creature.GetComponent<Creature>();

        if (creatureComponent == null)
        {
            Debug.LogError($"No creature component on infected object {creature.name}!");
        }
        
        bool aggressiveToSharks = false;
        foreach (var aggressiveWhenSeeTarget in creature.GetComponents<AggressiveWhenSeeTarget>())
        {
            if (aggressiveWhenSeeTarget.targetType == EcoTargetType.Shark)
                aggressiveToSharks = true;
        }

        if (!aggressiveToSharks)
        {
            MakeCreatureAggressiveToSharks(creatureComponent);
        }

        if (creature.GetComponent<AttackLastTarget>() == null)
        {
            AddAttackLastTarget(creatureComponent);
        }

        creatureComponent.Scared = new CreatureTrait(0, 100000f);
        creatureComponent.Aggression = new CreatureTrait(1, 0.01f);
        
        creatureComponent.ScanCreatureActions();
    }

    private static void MakeCreatureAggressiveToSharks(Creature creature)
    {
        var aggressiveComponent = creature.gameObject.AddComponent<AggressiveWhenSeeTarget>();
        // What the fuck is this and why does every creature do this
        aggressiveComponent.maxRangeMultiplier = new AnimationCurve(new Keyframe(0, 1), new Keyframe(0.5f, 0.5f), new Keyframe(1, 1));
        aggressiveComponent.distanceAggressionMultiplier = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 0));
        aggressiveComponent.creature = creature;
        aggressiveComponent.targetType = EcoTargetType.Shark;
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
        var attackLastTarget = creature.gameObject.AddComponent<AttackLastTarget>();
        var swimRandom = creature.GetComponent<SwimRandom>();
        attackLastTarget.swimVelocity = swimRandom != null ? swimRandom.swimVelocity * 2.5f : 10f;
        attackLastTarget.aggressionThreshold = 0.75f;
        attackLastTarget.swimInterval = 0.5f;
        attackLastTarget.minAttackDuration = 3f;
        attackLastTarget.maxAttackDuration = 10f;
        attackLastTarget.pauseInterval = 10f;
        attackLastTarget.rememberTargetTime = 5f;
        attackLastTarget.evaluatePriority = 1.1f;
        attackLastTarget.lastTarget = creature.gameObject.GetComponent<LastTarget>();
    }

    private static void AddBasicMeleeAttack(Creature creature)
    {
        
    }
}