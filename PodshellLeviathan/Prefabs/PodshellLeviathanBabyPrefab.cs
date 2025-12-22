using System.Collections;
using ECCLibrary.Data;
using Nautilus.Assets;
using PodshellLeviathan.Mono;
using UnityEngine;

namespace PodshellLeviathan.Prefabs;

public class PodshellLeviathanBabyPrefab : PodshellLeviathanPrefab
{
    public PodshellLeviathanBabyPrefab(PrefabInfo prefabInfo) : base(prefabInfo)
    {
    }

    protected override float StandardSwimVelocity => 1.8f;
    protected override string ModelName => "PodshellLeviathanBabyPrefab";
    protected override float MaxHealth => 500;
    protected override float Mass => 200;
    protected override bool UseScreenShake => false;

    protected override CreatureTemplate CreateTemplate()
    {
        var template = base.CreateTemplate();
        template.BioReactorCharge = 650;
        template.SetWaterParkCreatureData(new WaterParkCreatureDataStruct(0.18f, 0.3f, 0.9f, 2, false, false,
            new CustomGameObjectReference(PrefabInfo.ClassID)));
        template.AvoidObstaclesData =
            new AvoidObstaclesData(AvoidTerrainPriority, StandardSwimVelocity, false, 7, 8);
        return template;
    }

    protected override IEnumerator ModifyPrefab(GameObject prefab, CreatureComponents components)
    {
        yield return base.ModifyPrefab(prefab, components);
        var head = prefab.transform.Find("turtle_rigged/DO_NOT_TOUCH/root/cog/neck1");
        prefab.AddComponent<PodshellBabyHeadScaler>().headTransform = head;
    }
}