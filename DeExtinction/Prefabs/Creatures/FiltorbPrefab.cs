using DeExtinction.Mono;
using ECCLibrary;
using ECCLibrary.Data;
using Nautilus.Assets;
using System.Collections;
using UnityEngine;

namespace DeExtinction.Prefabs.Creatures;

internal class FiltorbPrefab : CreatureAsset
{
    private readonly GameObject _prefabModel;

    public FiltorbPrefab(PrefabInfo prefabInfo, GameObject prefabModel) : base(prefabInfo)
    {
        CreatureDataUtils.AddCreaturePDAEncyclopediaEntry(this, CommonDatabankPaths.SmallHerbivores, null, null, 3,
            Plugin.AssetBundle.LoadAsset<Texture2D>($"{prefabInfo.ClassID}_Ency"),
            Plugin.AssetBundle.LoadAsset<Sprite>($"{prefabInfo.ClassID}_Popup"));
        _prefabModel = prefabModel;
    }

    protected override CreatureTemplate CreateTemplate()
    {
        var template = new CreatureTemplate(_prefabModel, BehaviourType.SmallFish, EcoTargetType.SmallFish, 130f)
        {
            SwimRandomData = null,
            Mass = 5f,
            CellLevel = LargeWorldEntity.CellLevel.Near,
            PickupableFishData = new PickupableFishData(TechType.Peeper, "WorldModel", "ViewModel"),
            BioReactorCharge = 300f,
            EdibleData = new EdibleData(2, 0, false),
            SizeDistribution = new AnimationCurve(new []{new Keyframe(0, 0.8f), new Keyframe(1, 1)})
        };
        template.SetWaterParkCreatureData(new WaterParkCreatureDataStruct(0.01f, 0.8f, 0.8f, 1.1f, true, true, ClassID));
        
        return template;
    }

    protected override IEnumerator ModifyPrefab(GameObject prefab, CreatureComponents components)
    {
        components.WorldForces.underwaterDrag = 2;
        var hide = prefab.AddComponent<FiltorbHide>();
        hide.actionLength = 0.5f;
        hide.maxReactDistance = 14f;
        hide.evaluatePriority = 0.2f;
        prefab.AddComponent<FiltorbSwimBehaviour>().evaluatePriority = 0.1f;

        components.FleeOnDamage.damageThreshold = 100;
        components.FleeOnDamage.swimVelocity = 0;
        
#if BELOWZERO
        var viewModelTransform = prefab.GetComponent<FPModel>().viewModel.transform;
        viewModelTransform.localPosition = new Vector3(-0.05f, 0, -0.03f);
#endif

        yield break;
    }
}
