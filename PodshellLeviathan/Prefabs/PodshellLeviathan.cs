using ECCLibrary;
using ECCLibrary.Data;
using Nautilus.Assets;
using Nautilus.Utility;
using System.Collections;
using PodshellLeviathan.Mono;
using UnityEngine;

namespace PodshellLeviathan.Prefabs;

internal class PodshellLeviathanPrefab : CreatureAsset
{
    private const float kSwimSpeedPriority = 0.1f;
    private const float kAvoidTerrainPriority = 0.8f;
    private const float kStayAtLeashPriority = 0.1f;

    private const float kStandardSwimVelocity = 3f;

    public PodshellLeviathanPrefab(PrefabInfo prefabInfo) : base(prefabInfo)
    {
    }

    protected override CreatureTemplate CreateTemplate()
    {
        var template = new CreatureTemplate(
            Plugin.Assets.LoadAsset<GameObject>("PodshellLeviathanPrefab"),
            BehaviourType.Leviathan,
            EcoTargetType.Leviathan,
            6000);

        CreatureTemplateUtils.SetCreatureDataEssentials(template, LargeWorldEntity.CellLevel.VeryFar, 3000f, 0, new BehaviourLODData(150, 250, 300), 10000);
        template.SwimRandomData = new SwimRandomData(kSwimSpeedPriority, kStandardSwimVelocity, new Vector3(100, 4, 100), 5f, 1f, true);
        template.AvoidTerrainData = new AvoidTerrainData(kAvoidTerrainPriority, kStandardSwimVelocity, 30f, 30f);
        template.StayAtLeashData = new StayAtLeashData(kStayAtLeashPriority, kStandardSwimVelocity, 140f);
        template.CanBeInfected = false;
        template.SizeDistribution = new AnimationCurve(new Keyframe(0f, 0.7f), new Keyframe(1f, 1f));
        template.LocomotionData = new LocomotionData(10f, 0.06f, 0.1f, 0.5f);
        template.LiveMixinData.broadcastKillOnDeath = true;

        return template;
    }

    protected override IEnumerator ModifyPrefab(GameObject prefab, CreatureComponents components)
    {
        var tailRoot = prefab.transform.Find("turtle_rigged/DO_NOT_TOUCH/root/cog/tail1").GetChild(0);
        var tailTrailManager = new TrailManagerBuilder(components, tailRoot, 10, 3);
        tailTrailManager.SetTrailArrayToAllChildren();
        tailTrailManager.Apply();

        var voice = prefab.AddComponent<PodshellVoice>();
        var emitter = prefab.AddComponent<FMOD_CustomEmitter>();
        emitter.followParent = true;
        voice.emitter = emitter;
        voice.liveMixin = components.LiveMixin;
        
        yield break;
    }

    protected override void ApplyMaterials(GameObject prefab)
    {
        MaterialUtils.ApplySNShaders(prefab, 6, 1, 1, new PodshellMaterialModifier());
    }
}