using DeExtinction.Mono;
using ECCLibrary;
using ECCLibrary.Data;
using Nautilus.Assets;
using System.Collections;
using UnityEngine;

namespace DeExtinction.Prefabs.Creatures;

internal class AxetailPrefab : CreatureAsset
{
    public AxetailPrefab(PrefabInfo prefabInfo) : base(prefabInfo)
    {
    }

    protected override CreatureTemplate CreateTemplate()
    {
        var template = new CreatureTemplate(Plugin.AssetBundle.LoadAsset<GameObject>("Axetail_Prefab"),
            BehaviourType.SmallFish, EcoTargetType.SmallFish, 30)
        {
            SwimRandomData = new SwimRandomData(0.1f, 2.5f, new Vector3(20, 5, 20), 5f),
            AvoidObstaclesData = new AvoidObstaclesData(0.3f, 2.5f, false, 5f, 6f),
            StayAtLeashData = new StayAtLeashData(0.2f, 2.5f, 12),
            LocomotionData = new LocomotionData(10, 0.4f),
            Mass = 30,
            CellLevel = LargeWorldEntity.CellLevel.Near,
            BioReactorCharge = 300f,
            AnimateByVelocityData = new AnimateByVelocityData(6),
            SizeDistribution = new AnimationCurve(new []{new Keyframe(0, 0.5f), new Keyframe(1, 1)})
        };
        template.SetWaterParkCreatureData(new WaterParkCreatureDataStruct(0.08f, 0.8f, 1f, 1f, true, true, ClassID));
        CreatureTemplateUtils.SetPreyEssentials(template, 4f,
            new PickupableFishData(TechType.GarryFish, "Axetail", "AxetailViewModel"), new EdibleData(12, -7, false));
        return template;
    }

    protected override IEnumerator ModifyPrefab(GameObject prefab, CreatureComponents components)
    {
        yield break;
    }
}