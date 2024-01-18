using System.Collections;
using DeExtinction.Mono;
using ECCLibrary;
using ECCLibrary.Data;
using Nautilus.Assets;
using UnityEngine;

namespace DeExtinction.Prefabs.Creatures;

/* IMPLEMENT
 * THE
 * DAMN
 * EGG
 * PLEASE
 * LEE23
 * AND
 * THE
 * SOUND EFFECTS
*/
public class ThalassaceanPrefab : CreatureAsset
{
    private GameObject _prefabModel;
    
    public ThalassaceanPrefab(PrefabInfo prefabInfo, GameObject prefabModel) : base(prefabInfo)
    {
        _prefabModel = prefabModel;
    }

    protected override CreatureTemplate CreateTemplate()
    {
        var template = new CreatureTemplate(_prefabModel, BehaviourType.Whale, EcoTargetType.Whale, 600)
        {
            SwimRandomData = new SwimRandomData(0.2f, 1.2f, new Vector3(30, 0, 30), 5f, 1f),
            AvoidObstaclesData = new AvoidObstaclesData(0.7f, 1.3f, false, 8f, 11f),
            Mass = 150,
            CellLevel = LargeWorldEntity.CellLevel.Medium,
            BioReactorCharge = 1600,
            SizeDistribution = new AnimationCurve(new Keyframe(0, 0.6f), new Keyframe(1, 1)),
            LocomotionData = new LocomotionData(12f, 0.15f, 3),
            AnimateByVelocityData = new AnimateByVelocityData(6f),
            FleeOnDamageData = new FleeOnDamageData(0.5f, 6f, 10),
        };
        template.SetWaterParkCreatureData(new WaterParkCreatureDataStruct(0.02f, 0.09f, 0.2f, 1.25f, true, true, PrefabInfo.ClassID));
        
        return template;
    }

    protected override IEnumerator ModifyPrefab(GameObject prefab, CreatureComponents components)
    {
        components.Creature.activity = DeExtinctionUtils.StandardActivityCurve;
        
        /*
        var trailManagerBuilder = new TrailManagerBuilder(components, prefab.transform.SearchChild("root"), 0.2f)
            {
                Trails = new [] { prefab.SearchChild("spine1").transform, prefab.SearchChild("spine2").transform, prefab.SearchChild("spine3").transform, prefab.SearchChild("spine4").transform }
            };
        trailManagerBuilder.Apply();
        */
        
        var fleeFromPredators = prefab.AddComponent<FleeFromPredators>();
        fleeFromPredators.actionLength = 6f;
        fleeFromPredators.swimVelocity = 15f;
        fleeFromPredators.maxReactDistance = 35f;
        fleeFromPredators.evaluatePriority = 0.9f;
        fleeFromPredators.fearedTargetType = EcoTargetType.Leviathan;
        
        yield break;
    }
}