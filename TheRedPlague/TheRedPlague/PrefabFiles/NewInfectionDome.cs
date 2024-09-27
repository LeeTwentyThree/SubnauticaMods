using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using UnityEngine;
using System.Collections;
using Nautilus.Utility;
using TheRedPlague.Mono;
using TheRedPlague.Mono.VFX;

namespace TheRedPlague.PrefabFiles;

public static class NewInfectionDome
{
    public static PrefabInfo Info { get; } = PrefabInfo.WithTechType("NewInfectionDome");

    private static GameObject _shieldPrefab;
    private static bool _cyclopsLoaded;
    
    public static void Register()
    {
        var infectionDome = new CustomPrefab(Info);
        infectionDome.SetGameObject(GetPrefab);
        infectionDome.Register();
        infectionDome.RemoveFromCache();
    }
    
    private static IEnumerator GetPrefab(IOut<GameObject> prefab)
    {
        var obj = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("NewInfectionDome"));
        obj.SetActive(false);
        MaterialUtils.ApplySNShaders(obj);
        PrefabUtils.AddBasicComponents(obj, Info.ClassID, Info.TechType,
            LargeWorldEntity.CellLevel.Global);

        _cyclopsLoaded = false;
        
        yield return new WaitUntil(() => LightmappedPrefabs.main);

        LightmappedPrefabs.main.RequestScenePrefab("Cyclops", OnCyclopsReferenceLoaded);

        yield return new WaitUntil(() => _cyclopsLoaded);

        var shield = Object.Instantiate(_shieldPrefab, obj.transform);
        shield.SetActive(true);
        shield.transform.localPosition = Vector3.zero;
        shield.transform.localEulerAngles = Vector3.right * 90;
        shield.transform.localScale = new Vector3(0.744f, 0.744f, 0.677f);
        
        var shieldRenderer = shield.GetComponent<Renderer>();
        var shieldMaterial = shieldRenderer.material;
        shieldMaterial.SetFloat(ShaderPropertyID._Intensity, 1);
        shieldMaterial.SetVector("_WobbleParams", new Vector4(2, 0.7f, 8, 0));
        
        var domeController = obj.AddComponent<InfectionDomeController>();
        domeController.domeRenderer = shieldRenderer;

        var constructionVfx = obj.AddComponent<DomeConstructionVfx>();
        constructionVfx.domeShieldRenderer = shieldRenderer;
        
        var fabricatorTask = CraftData.GetPrefabForTechTypeAsync(TechType.Fabricator);
        yield return fabricatorTask;
        constructionVfx.emissiveTex = fabricatorTask.GetResult().GetComponent<Fabricator>().ghost._EmissiveTex;

        prefab.Set(obj);
    }

    private static void OnCyclopsReferenceLoaded(GameObject obj)
    {
        _shieldPrefab = obj.transform.Find("FX/x_Cyclops_GlassShield").gameObject;
        _cyclopsLoaded = true;
    }
}