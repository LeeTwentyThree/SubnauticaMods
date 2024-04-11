using System.Collections;
using DeExtinction.Mono;
using ECCLibrary;
using ECCLibrary.Data;
using Nautilus.Assets;
using UnityEngine;

namespace DeExtinction.Prefabs.Creatures;

public class TriangleFishPrefab : CreatureAsset
{
    public TriangleFishPrefab(PrefabInfo prefabInfo) : base(prefabInfo)
    {
        CreatureDataUtils.AddCreaturePDAEncyclopediaEntry(this, "Lifeforms/Fauna/SmallHerbivores", null, null, 2,
            Plugin.AssetBundle.LoadAsset<Texture2D>("Trianglefish_Ency"),
            Plugin.AssetBundle.LoadAsset<Sprite>("Trianglefish_Popup"));
    }

    protected override CreatureTemplate CreateTemplate()
    {
        var template = new CreatureTemplate(Plugin.AssetBundle.LoadAsset<GameObject>("Trianglefish_Prefab"), BehaviourType.SmallFish, EcoTargetType.SmallFish, 30f)
        {
            SwimRandomData = new SwimRandomData(0.2f, 4f, new Vector3(7,1, 7), 1f),
            AvoidObstaclesData = new AvoidObstaclesData(0.6f, 4f, false, 4f, 5f),
            Mass = 4f,
            CellLevel = LargeWorldEntity.CellLevel.Near,
            BioReactorCharge = 300f,
            EdibleData = new EdibleData(7, -5, false),
            SizeDistribution = new AnimationCurve(new Keyframe(0, 0.8f), new Keyframe(1, 1)),
            LocomotionData = new LocomotionData(10, 2, 3, 0.3f),
            StayAtLeashData = new StayAtLeashData(0.3f, 4f, 16f)
        };
        template.SetWaterParkCreatureData(new WaterParkCreatureDataStruct(0.1f, 0.7f, 0.9f, 1f, true, true, PrefabInfo.ClassID));
        CreatureTemplateUtils.SetPreyEssentials(template, 6f,
            new PickupableFishData(TechType.GarryFish, "WorldModel", "ViewModel"), new EdibleData(7, -5, false));

        return template;
    }

    protected override IEnumerator ModifyPrefab(GameObject prefab, CreatureComponents components)
    {
        components.Creature.activity = DeExtinctionUtils.StandardActivityCurve;
        
        yield break;
    }
}