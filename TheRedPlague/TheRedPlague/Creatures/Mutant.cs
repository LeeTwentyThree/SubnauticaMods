using System.Collections;
using System.Collections.Generic;
using ECCLibrary;
using ECCLibrary.Data;
using Nautilus.Assets;
using TheRedPlague.Mono;
using UnityEngine;

namespace TheRedPlague.Creatures;

public class Mutant : CreatureAsset
{
    private GameObject _model;
    private bool _heavilyMutated;
    
    public Mutant(PrefabInfo prefabInfo, string prefabName, bool heavilyMutated) : base(prefabInfo)
    {
        _model = Plugin.AssetBundle.LoadAsset<GameObject>(prefabName);
        _heavilyMutated = heavilyMutated;
    }

    protected override CreatureTemplate CreateTemplate()
    {
        var template = new CreatureTemplate(_model, BehaviourType.Shark, EcoTargetType.Shark, 1000);
        CreatureTemplateUtils.SetCreatureDataEssentials(template, LargeWorldEntity.CellLevel.Medium, 500, -0.5f);
        CreatureTemplateUtils.SetCreatureMotionEssentials(template,
            new SwimRandomData(0.3f, _heavilyMutated ? 10f : 5f, new Vector3(20f, 10f, 20f), 4f), new StayAtLeashData(0.4f, _heavilyMutated ? 10f : 5f, 50f));
        template.LocomotionData = new LocomotionData(5f);
        template.AggressiveWhenSeeTargetList = new List<AggressiveWhenSeeTargetData>()
            {new AggressiveWhenSeeTargetData(EcoTargetType.Shark, 1, 40, 2, false)};
        template.AttackLastTargetData = new AttackLastTargetData(0.5f, _heavilyMutated ? 20f : 12f, 0.5f, 7f);
        return template;
    }

    protected override IEnumerator ModifyPrefab(GameObject prefab, CreatureComponents components)
    {
        prefab.AddComponent<InfectOnStart>();
        if (_heavilyMutated)
        {
            prefab.AddComponent<DisableRigidbodyWhileOffScreen>();
        }
        yield break;
    }
}