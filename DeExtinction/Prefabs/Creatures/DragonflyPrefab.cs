using System.Collections;
using DeExtinction.MaterialModifiers;
using DeExtinction.Mono;
using ECCLibrary;
using ECCLibrary.Data;
using Nautilus.Assets;
using Nautilus.Extensions;
using Nautilus.Utility;
using UnityEngine;

namespace DeExtinction.Prefabs.Creatures;

public class DragonflyPrefab : CreatureAsset
{
    private const float FlyVelocity = 4;
    
    public DragonflyPrefab(PrefabInfo prefabInfo) : base(prefabInfo)
    {
        CreatureDataUtils.AddCreaturePDAEncyclopediaEntry(this, "Lifeforms/Fauna/Carnivores", null, null, 4,
            Plugin.AssetBundle.LoadAsset<Texture2D>("Dragonfly_Ency"),
            Plugin.AssetBundle.LoadAsset<Sprite>("Dragonfly_Popup"));
    }

    protected override CreatureTemplate CreateTemplate()
    {
        var template = new CreatureTemplate(Plugin.AssetBundle.LoadAsset<GameObject>("Dragonfly_Prefab"), BehaviourType.MediumFish, EcoTargetType.None, 300)
        {
            SwimRandomData = new SwimRandomData(0.2f, FlyVelocity, new Vector3(16,1, 16), 5, 0.5f),
            AvoidObstaclesData = new AvoidObstaclesData(0.45f, FlyVelocity, true, 5f, 6f),
            Mass = 87f,
            CellLevel = LargeWorldEntity.CellLevel.VeryFar,
            BioReactorCharge = 600f,
            SizeDistribution = new AnimationCurve(new Keyframe(0, 0.9f), new Keyframe(1, 1)),
            LocomotionData = new LocomotionData(1, 0.18f, 3, 0f, false, true),
            StayAtLeashData = new StayAtLeashData(0.5f, FlyVelocity, 35f),
            BehaviourLODData = new BehaviourLODData(50, 200, 400),
            AnimateByVelocityData = new AnimateByVelocityData(FlyVelocity, 30, 5),
            FleeOnDamageData = null,
            TraitsData = new CreatureTraitsData(0.01f)
        };

        template.SetCreatureComponentType<BirdBehaviour>();
        
        return template;
    }

    protected override IEnumerator ModifyPrefab(GameObject prefab, CreatureComponents components)
    {
        ((BirdBehaviour) components.Creature).worldForces = components.WorldForces;
        components.Creature.initialHunger = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 0.3f));
        
        var wf = components.WorldForces;
        wf.underwaterGravity = -1;
        wf.aboveWaterGravity = 0;
        wf.aboveWaterDrag = 0;
        wf.underwaterDrag = 2;
        var flyAboveMinHeight = prefab.AddComponent<FlyAboveMinHeight>();
        flyAboveMinHeight.minHeight = 14;
        flyAboveMinHeight.flyVelocity = FlyVelocity;
        flyAboveMinHeight.evaluatePriority = 0.4f;

#if SUBNAUTICA
        var birdsFlapping = prefab.AddComponent<BirdsFlapping>();
#elif BELOWZERO
        var birdsFlapping = prefab.AddComponent<BirdFlap>();
#endif
        birdsFlapping.flyVelocity = FlyVelocity;
        birdsFlapping.flappingDuration = 3;
        birdsFlapping.flyInterval = 1;
        birdsFlapping.flappingInterval = 9;
        birdsFlapping.flappingDuration = 1.5f;
        birdsFlapping.animator = components.Animator;
        birdsFlapping.flyUp = 0.08f;

        var drowning = prefab.AddComponent<SlowDrowning>();
        drowning.damage = 20;
        drowning.animator = components.Animator;

        prefab.AddComponent<BirdHuntBehaviour>().evaluatePriority = 0.6f;

        var tailRoot = prefab.SearchChild("Tail");
        var fakeRoot = prefab.transform.Find("FakeTrailManagerRoot");
        var trailManagerBuilder = new TrailManagerBuilder(components, tailRoot.transform, 10);
        trailManagerBuilder.SetTrailArrayToChildrenWithKeywords("Tail");
        var trailManager = trailManagerBuilder.Apply();
        trailManager.rootSegment = fakeRoot;

        prefab.AddComponent<RandomizeHungerOnStart>();

        var birdAttack = prefab.transform.Find("FishAttackTrigger").gameObject.AddComponent<BirdGrabFish>();
        birdAttack.fishParent = prefab.transform.Find("FishParent");
        
        yield break;
    }

    protected override void ApplyMaterials(GameObject prefab)
    {
        MaterialUtils.ApplySNShaders(prefab, 6, 2f, 0.25f, new FresnelModifier(0), new DragonflyModifier());
    }
}