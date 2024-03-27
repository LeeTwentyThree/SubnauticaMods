using System.Collections;
using DeExtinction.MaterialModifiers;
using ECCLibrary;
using ECCLibrary.Data;
using Nautilus.Assets;
using Nautilus.Utility;
using UnityEngine;

namespace DeExtinction.Prefabs.Creatures;

public class GrandGliderPrefab : CreatureAsset
{
    public GrandGliderPrefab(PrefabInfo prefabInfo) : base(prefabInfo)
    {
    }

    protected override CreatureTemplate CreateTemplate()
    {
        var template = new CreatureTemplate(Plugin.AssetBundle.LoadAsset<GameObject>("GrandGliderPrefab"), BehaviourType.MediumFish, EcoTargetType.MediumFish, 90f)
        {
            SwimRandomData = new SwimRandomData(0.2f, 4f, new Vector3(30, 5, 30)),
            AvoidObstaclesData = new AvoidObstaclesData(0.6f, 4f, false, 5f, 6f),
            Mass = 20,
            CellLevel = LargeWorldEntity.CellLevel.Medium,
            BioReactorCharge = 830,
            SizeDistribution = new AnimationCurve(new Keyframe(0, 0.25f), new Keyframe(1, 1)),
            LocomotionData = new LocomotionData(10, 0.5f, 3, 0.3f),
            AnimateByVelocityData = new AnimateByVelocityData(8f),
            SwimInSchoolData = new SwimInSchoolData(0.8f, 6f, 2f, 1f, 1f, 0f),
            FleeWhenScaredData = new FleeWhenScaredData(0.4f, 6f),
            ScareableData = new ScareableData(maxRangeScalar: 5)
        };
        template.SetWaterParkCreatureData(new WaterParkCreatureDataStruct(0.025f, 0.15f, 0.5f, 1f, true, true, PrefabInfo.ClassID));
        
        return template;
    }

    protected override IEnumerator ModifyPrefab(GameObject prefab, CreatureComponents components)
    {
        var trailManagerBuilder = new TrailManagerBuilder(components, prefab.transform.Find("Grand Glider/Grand Glider Armature/Root/Spine1"), 0.5f);
        trailManagerBuilder.SetTrailArrayToChildrenWithKeywords("Spine");
        trailManagerBuilder.Apply();
        
        yield break;
    }

    protected override void ApplyMaterials(GameObject prefab)
    {
        MaterialUtils.ApplySNShaders(prefab, 4f, 1f, 1f, new GrandGliderModifier());
    }
}