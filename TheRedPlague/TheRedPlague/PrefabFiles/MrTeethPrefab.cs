using System.Collections;
using ECCLibrary;
using ECCLibrary.Data;
using Nautilus.Assets;
using TheRedPlague.Mono;
using TheRedPlague.Mono.CreatureBehaviour.MrTeeth;
using UnityEngine;

namespace TheRedPlague.PrefabFiles;

public class MrTeethPrefab : CreatureAsset
{
    public MrTeethPrefab(PrefabInfo prefabInfo) : base(prefabInfo)
    {
    }

    protected override CreatureTemplate CreateTemplate()
    {
        var template = new CreatureTemplate(Plugin.AssetBundle.LoadAsset<GameObject>("MrTeethPrefab"),
            BehaviourType.Shark, EcoTargetType.Shark, 5000)
        {
            SwimRandomData = null,
            LocomotionData = new LocomotionData(40f),
            Mass = 1000,
            AvoidObstaclesData = new AvoidObstaclesData(0.4f, 6f, false, 5f, 6f, scanInterval: 0.2f)
        };
        return template;
    }

    protected override IEnumerator ModifyPrefab(GameObject prefab, CreatureComponents components)
    {
        var hunt = prefab.AddComponent<MrTeethHuntBehaviour>();
        hunt.evaluatePriority = 0.3f;

        var bury = prefab.AddComponent<MrTeethBuryBehaviour>();
        bury.evaluatePriority = 0.8f;

        var attackTrigger = prefab.transform.Find("AttackTrigger").gameObject.AddComponent<MrTeethAttackTrigger>();
        attackTrigger.animator = components.Animator;
        attackTrigger.rootObject = prefab;

        prefab.EnsureComponent<InfectionTarget>().invalidTarget = true;

        foreach (var rb in prefab.GetComponentsInChildren<Rigidbody>())
        {
            if (rb.gameObject != prefab) rb.interpolation = RigidbodyInterpolation.Interpolate;
        }
        
        yield break;
    }
}