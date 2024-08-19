using System.Collections;
using ECCLibrary;
using ECCLibrary.Data;
using Nautilus.Assets;
using Nautilus.Handlers;
using Nautilus.Utility;
using TheRedPlague.Mono.CreatureBehaviour.Drifter;
using UnityEngine;

namespace TheRedPlague.PrefabFiles;

public class DrifterPrefab : CreatureAsset
{
    private const float BaseVelocity = 10;

    public DrifterPrefab(PrefabInfo prefabInfo) : base(prefabInfo)
    {
    }

    protected override CreatureTemplate CreateTemplate()
    {
        var model = Plugin.AssetBundle.LoadAsset<GameObject>("DrifterPrefab");
        var template = new CreatureTemplate(model, BehaviourType.Leviathan, EcoTargetType.None, 5000)
        {
            LocomotionData = new LocomotionData(5f, 0.1f, 1f, 1f, true, true),
            SwimRandomData = new SwimRandomData(0.1f, BaseVelocity, new Vector3(50, 3, 50), 5f, 0.8f),
            Mass = 3000,
            RespawnData = new RespawnData(false),
            CellLevel = LargeWorldEntity.CellLevel.VeryFar
        };
        return template;
    }

    protected override void PostRegister()
    {
        var randomGenerator = new System.Random(51034581);
        for (int i = 0; i < 40; i++)
        {
            var angle = (float) randomGenerator.NextDouble() * Mathf.PI * 2f;
            var distance = Mathf.Pow((float) randomGenerator.NextDouble(), 1/2f) * 1500f;
            var height = 20 + (float) randomGenerator.NextDouble() * 30;
            CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(PrefabInfo.ClassID,
                new Vector3(Mathf.Cos(angle) * distance, height, Mathf.Sin(angle) * distance)));
        }
    }

    protected override IEnumerator ModifyPrefab(GameObject prefab, CreatureComponents components)
    {
        var wf = components.WorldForces;
        wf.aboveWaterGravity = 0;
        wf.aboveWaterDrag = 1;
        wf.underwaterDrag = 1;
        
        var eyeTracking = prefab.AddComponent<DrifterEyeTracking>();
        var eyeArmatureParent = prefab.transform.Find("DrifterAnimated/DrifterArmature/EyeArmature");
        eyeTracking.transforms = new[]
        {
            eyeArmatureParent.Find("Bone 1"),
            eyeArmatureParent.Find("Bone.001 1"),
            eyeArmatureParent.Find("Bone.002"),
            eyeArmatureParent.Find("Bone.003 1")
        };

        var loopingEmitter = prefab.AddComponent<FMOD_CustomLoopingEmitter>();
        loopingEmitter.SetAsset(AudioUtils.GetFmodAsset("DrifterIdle"));
        loopingEmitter.followParent = true;
        loopingEmitter.playOnAwake = true;

        yield break;
    }
}