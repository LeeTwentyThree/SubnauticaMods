using System.Collections;
using ECCLibrary;
using ECCLibrary.Data;
using Nautilus.Assets;
using UnityEngine;

namespace DeExtinction.Prefabs.Creatures;

public class RibbonRayPrefab : CreatureAsset
{
    public RibbonRayPrefab(PrefabInfo prefabInfo) : base(prefabInfo)
    {
    }

    protected override CreatureTemplate CreateTemplate()
    {
        var template = new CreatureTemplate(Plugin.AssetBundle.LoadAsset<GameObject>("RibbonRay_Prefab"), BehaviourType.MediumFish, EcoTargetType.MediumFish, 40f)
        {
            SwimRandomData = new SwimRandomData(0.2f, 3, new Vector3(15, 3, 15), 3),
            AvoidObstaclesData = new AvoidObstaclesData(0.6f, 3f, false, 5f, 6f),
            Mass = 4f,
            CellLevel = LargeWorldEntity.CellLevel.Near,
            BioReactorCharge = 400f,
            SizeDistribution = new AnimationCurve(new Keyframe(0, 0.8f), new Keyframe(1, 1)),
            LocomotionData = new LocomotionData(10, 2, 3, 0.3f),
            AnimateByVelocityData = new AnimateByVelocityData(6f),
            StayAtLeashData = new StayAtLeashData(0.4f, 3f, 25f)
        };
        template.SetWaterParkCreatureData(new WaterParkCreatureDataStruct(0.1f, 0.7f, 0.9f, 1f, true, true, PrefabInfo.ClassID));
        CreatureTemplateUtils.SetPreyEssentials(template, 4f,
            new PickupableFishData(TechType.Peeper, "WorldModel", "ViewModel"), new EdibleData(12, -6, false));
        return template;
    }

    protected override IEnumerator ModifyPrefab(GameObject prefab, CreatureComponents components)
    {
        components.Creature.activity = DeExtinctionUtils.StandardActivityCurve;
        
        var trailManagerBuilder1 = new TrailManagerBuilder(components, prefab.transform.SearchChild("LTail1Spade"), 1f);
        trailManagerBuilder1.SetTrailArrayToAllChildren();
        trailManagerBuilder1.Apply();
        
        var trailManagerBuilder2 = new TrailManagerBuilder(components, prefab.transform.SearchChild("RTail1Spade"), 1f);
        trailManagerBuilder2.SetTrailArrayToAllChildren();
        trailManagerBuilder2.Apply();

        yield break;
    }
}