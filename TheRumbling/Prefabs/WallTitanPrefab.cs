using Nautilus.Assets;
using Nautilus.Utility;
using System.Collections;
using TheRumbling.MaterialModifiers;
using TheRumbling.Mono;
using UnityEngine;

namespace TheRumbling.Prefabs;

internal static class WallTitanPrefab
{
    public static PrefabInfo Info { get; } = PrefabInfo.WithTechType("WallTitan", "WALL TITAN", "ONE OF THE TITANS THAT FORMED THE THREE WALLS.");

    private static GameObject _smokeVfxOriginal;

    public static void Register()
    {
        var customPrefab = new CustomPrefab(Info);

        customPrefab.SetGameObject(GetGameObject);
        customPrefab.Register();
    }

    private static IEnumerator GetGameObject(IOut<GameObject> result)
    {
        var titan = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("WallTitan"));
        titan.gameObject.SetActive(false);
        PrefabUtils.AddBasicComponents(titan, Info.ClassID, Info.TechType, LargeWorldEntity.CellLevel.Global);
        MaterialUtils.ApplySNShaders(titan, 4, 1, 1, new TitanMaterialModifier(), new SteamMaterialModifier());
        var heightmapEntity = titan.AddComponent<HeightmapEntity>();
        heightmapEntity.SetEssentials(Balance.DefaultTitanWalkSpeed, -9.8f, 9.8f, -80, Balance.DefaultTitanSwimYMin, Balance.DefaultTitanSwimYMax, -20);
        var titanComponent = titan.AddComponent<WallTitan>();
        titanComponent.heightmapEntity = heightmapEntity;

        var audioRoot = titan.transform.Find("AudioRoot");
        var emitter = audioRoot.gameObject.AddComponent<FMOD_CustomEmitter>();
        emitter.SetAsset(RumblingAudio.TitanAmbienceSound);
        emitter.followParent = true;
        emitter.playOnAwake = true;
        emitter.gameObject.AddComponent<RepeatSoundEvery36Seconds>();

        titanComponent.heatEmitter = titan.transform.Find("HeatEmitter");

        var hurtBox = titan.transform.Find("HurtBox").gameObject;
        hurtBox.AddComponent<HurtBox>();
        hurtBox.GetComponent<Collider>().isTrigger = true;
        hurtBox.AddComponent<Rigidbody>().isKinematic = true;

        // VFX
        var vfxParent = titan.transform.GetChild(3);
        vfxParent.GetChild(0).gameObject.SetActive(false);

        if (_smokeVfxOriginal == null)
        {
            _smokeVfxOriginal = CrashedShipExploder.main.transform
                .Find("unexplodedFX/Ship_Exterior_CrashedFX(Clone)/xFireHuge1").gameObject;
        }

        var smokeVfx = Object.Instantiate(_smokeVfxOriginal, vfxParent, false);
        smokeVfx.transform.localPosition = Vector3.zero;
        smokeVfx.transform.localScale = Vector3.one * 0.1f;
        smokeVfx.GetComponent<Renderer>().enabled = false;
        smokeVfx.transform.GetChild(0).gameObject.SetActive(false);
        smokeVfx.transform.GetChild(2).gameObject.SetActive(false);
        smokeVfx.transform.GetChild(4).gameObject.SetActive(false);
        
        smokeVfx.transform.GetChild(1).gameObject.AddComponent<RandomDisablement>().appearProbability = 0.2f; // smoke
        smokeVfx.transform.GetChild(3).gameObject.AddComponent<RandomDisablement>().appearProbability = 0.3f; // sparks
        //var sparksLight = smokeVfx.transform.GetChild(3).gameObject.AddComponent<Light>();
        //sparksLight.color = new Color(1f, 0.2f, 0f);
        //sparksLight.range = 200;
        smokeVfx.transform.GetChild(3).gameObject.AddComponent<ParticlesInProximityOnly>(); // sparks
        
        foreach (var particleSystem in smokeVfx.GetComponentsInChildren<ParticleSystem>(true))
        {
            var psMain = particleSystem.main;
            psMain.scalingMode = ParticleSystemScalingMode.Hierarchy;
        }
        
        result.Set(titan);
        yield break;
    }
}