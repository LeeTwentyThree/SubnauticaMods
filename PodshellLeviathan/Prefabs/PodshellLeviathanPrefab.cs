using ECCLibrary;
using ECCLibrary.Data;
using Nautilus.Assets;
using Nautilus.Utility;
using System.Collections;
using System.Linq;
using Nautilus.Utility.ModMessages;
using PodshellLeviathan.Mono;
using UnityEngine;

namespace PodshellLeviathan.Prefabs;

public class PodshellLeviathanPrefab : CreatureAsset
{
    protected const float SwimSpeedPriority = 0.1f;
    protected const float AvoidTerrainPriority = 0.8f;
    protected const float StayAtLeashPriority = 0.1f;

    public PodshellLeviathanPrefab(PrefabInfo prefabInfo) : base(prefabInfo)
    {
    }
    
    protected virtual float StandardSwimVelocity => 3;
    protected virtual string ModelName => "PodshellLeviathanPrefab";
    protected virtual float MaxHealth => 6000;
    protected virtual float Mass => 3000f;
    protected virtual bool UseScreenShake => true;

    protected override CreatureTemplate CreateTemplate()
    {
        var template = new CreatureTemplate(
            () => Plugin.Assets.LoadAsset<GameObject>(ModelName),
            BehaviourType.Leviathan,
            EcoTargetType.Leviathan,
            MaxHealth);

        CreatureTemplateUtils.SetCreatureDataEssentials(template, LargeWorldEntity.CellLevel.VeryFar, Mass, 0, new BehaviourLODData(150, 250, 300), 10000);
        template.SwimRandomData = new SwimRandomData(SwimSpeedPriority, StandardSwimVelocity, new Vector3(100, 4, 100), 5f, 1f, true);
        template.AvoidObstaclesData =
            new AvoidObstaclesData(AvoidTerrainPriority, StandardSwimVelocity, true, 30, 30);
        template.StayAtLeashData = new StayAtLeashData(StayAtLeashPriority, StandardSwimVelocity, 140f);
        template.CanBeInfected = false;
        template.SizeDistribution = new AnimationCurve(new Keyframe(0f, 0.7f), new Keyframe(1f, 1f));
        template.LocomotionData = new LocomotionData(10f, 0.01f, 0.8f, 0.5f, true);
        template.AnimateByVelocityData = new AnimateByVelocityData(StandardSwimVelocity + 1f);
        template.LiveMixinData.broadcastKillOnDeath = true;
        template.SwimBehaviourData = new SwimBehaviourData(0.3f);

        template.SetCreatureComponentType<PodshellLeviathanBehavior>();

        return template;
    }

    protected override IEnumerator ModifyPrefab(GameObject prefab, CreatureComponents components)
    {
        var tailRoot = prefab.transform.Find("turtle_rigged/DO_NOT_TOUCH/root/cog/tail1").GetChild(0);
        var tailTrailManager = new TrailManagerBuilder(components, tailRoot, 10, 3);
        tailTrailManager.SetTrailArrayToAllChildren();
        tailTrailManager.Apply();

        var voice = prefab.AddComponent<PodshellVoice>();
        voice.useScreenShake = UseScreenShake;
        var emitter = prefab.AddComponent<FMOD_CustomEmitter>();
        emitter.followParent = true;
        voice.emitter = emitter;

        var randomActions = prefab.AddComponent<PodshellRandomAnimations>();
        
        var behavior = (PodshellLeviathanBehavior) components.Creature;
        
        behavior.voice = voice;
        behavior.randomAnimations = randomActions;

        var infectedMixin = prefab.AddComponent<InfectedMixin>();
        infectedMixin.renderers = prefab.GetComponentsInChildren<Renderer>(true)
            .Where(r => !r.gameObject.name.StartsWith("polySurface")).ToArray();

        components.Rigidbody.angularDrag = 0.5f;
        
        yield break;
    }

    /*
    protected override void PostRegister()
    {
        base.PostRegister();
        ModMessageSystem.Send("TheRedPlague", "SetTechTypeImmune", PrefabInfo.TechType);
    }
    */

    protected override void ApplyMaterials(GameObject prefab)
    {
        MaterialUtils.ApplySNShaders(prefab, 6, 1, 1, new PodshellMaterialModifier());
    }
}