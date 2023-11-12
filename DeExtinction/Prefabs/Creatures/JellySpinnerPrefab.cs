using DeExtinction.Mono;
using ECCLibrary;
using ECCLibrary.Data;
using Nautilus.Assets;
using System.Collections;
using UnityEngine;

namespace DeExtinction.Prefabs.Creatures;

internal class JellySpinnerPrefab : CreatureAsset
{
    public JellySpinnerPrefab(PrefabInfo prefabInfo) : base(prefabInfo)
    {
    }

    protected override CreatureTemplate CreateTemplate()
    {
        var template = new CreatureTemplate(Plugin.AssetBundle.LoadAsset<GameObject>("JellySpinner_Prefab"),
            BehaviourType.SmallFish, EcoTargetType.SmallFish, 5)
        {
            SwimRandomData = new SwimRandomData(0.1f, 2f, new Vector3(10, 10, 10), 6f, 1f),
            AvoidObstaclesData = new AvoidObstaclesData(0.3f, 2f, false, 3f, 4f),
            StayAtLeashData = new StayAtLeashData(0.2f, 2f, 32),
            LocomotionData = new LocomotionData(4f, 0.4f),
            Mass = 0.31987f,
            CellLevel = LargeWorldEntity.CellLevel.Near,
            BioReactorCharge = 300f,
            AnimateByVelocityData = new AnimateByVelocityData(4),
            AcidImmune = true,
            SizeDistribution = new AnimationCurve(new []{new Keyframe(0, 0.2f), new Keyframe(1, 1)}),
            ItemSoundsType = ItemSoundsType.Floater
        };
        template.SetWaterParkCreatureData(new WaterParkCreatureDataStruct(0.01f, 0.7f, 1f, 0.3f, true, true, "JellySpinner"));
        CreatureTemplateUtils.SetPreyEssentials(template, 4f,
            new PickupableFishData(TechType.Peeper, "WorldModel", "ViewModel"), new EdibleData(2, -2, false));
        return template;
    }

    protected override IEnumerator ModifyPrefab(GameObject prefab, CreatureComponents components)
    {
        yield break;
    }
}