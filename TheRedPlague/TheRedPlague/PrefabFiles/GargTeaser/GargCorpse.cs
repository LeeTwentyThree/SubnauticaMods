using ECCLibrary;
using Nautilus.Assets;
using Nautilus.Utility;
using TheRedPlague.Mono;
using TheRedPlague.Mono.PlagueGarg;
using TheRedPlague.Mono.VFX;
using UnityEngine;

namespace TheRedPlague.PrefabFiles.GargTeaser;

public static class GargCorpse
{
    public static PrefabInfo Info { get; } = PrefabInfo.WithTechType("GargCorpse");

    public static void Register()
    {
        var prefab = new CustomPrefab(Info);
        prefab.SetGameObject(GetGameObject);
        prefab.Register();
    }

    private static GameObject GetGameObject()
    {
        var obj = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("GargCorpse_Prefab"));
        obj.SetActive(false);
        MaterialUtils.ApplySNShaders(obj, 6, 100, 3);
        PrefabUtils.AddBasicComponents(obj, Info.ClassID, Info.TechType, LargeWorldEntity.CellLevel.Global);
        
        // outer skin
        Renderer mainRenderer = obj.transform.Find("Gargantuan_AdultV3").SearchChild("Gargantuan.004").GetComponent<SkinnedMeshRenderer>();
        Renderer eyeRenderer = obj.transform.Find("Gargantuan_AdultV3").SearchChild("Gargantuan.002").GetComponent<SkinnedMeshRenderer>();

        AdultGargantuan.UpdateGargTransparentMaterial(mainRenderer.materials[0]);
        AdultGargantuan.UpdateGargTransparentMaterial(mainRenderer.materials[1]);
        AdultGargantuan.UpdateGargTransparentMaterial(mainRenderer.materials[2]);

        AdultGargantuan.UpdateGargEyeMaterial(eyeRenderer.materials[0]);
        AdultGargantuan.UpdateGargSolidMaterial(eyeRenderer.materials[1]);
        
        // inner skin
        Renderer plagueGargGutsRenderer = obj.transform.Find("Gargantuan_AdultRedPlague").SearchChild("Gargantuan.001").GetComponent<SkinnedMeshRenderer>();
        Renderer plagueGargEyesRenderer = obj.transform.Find("Gargantuan_AdultRedPlague").SearchChild("Gargantuan.002").GetComponent<SkinnedMeshRenderer>();
        Renderer plagueGargSkeletonRenderer = obj.transform.Find("Gargantuan_AdultRedPlague").SearchChild("Gargantuan.003").GetComponent<SkinnedMeshRenderer>();
        Renderer plagueGargTongueRenderer = obj.transform.Find("Gargantuan_AdultRedPlague").SearchChild("Gargantuan.007").GetComponent<SkinnedMeshRenderer>();
        
        PlagueGarg.UpdateGargGutsMaterial(plagueGargGutsRenderer.materials[0]);
        PlagueGarg.UpdateGargEyeMaterial(plagueGargEyesRenderer.materials[0]);
        PlagueGarg.UpdateGargSolidMaterial(plagueGargSkeletonRenderer.materials[0]);
        PlagueGarg.UpdateGargGutsMaterial(plagueGargSkeletonRenderer.materials[1]);
        PlagueGarg.UpdateGargSolidMaterial(plagueGargSkeletonRenderer.materials[2]);
        PlagueGarg.UpdateGargSolidMaterial(plagueGargTongueRenderer.materials[0]);

        obj.AddComponent<GargCorpseBehavior>();

        var floatUp = obj.AddComponent<ConstantMotionWhileUnderWater>();
        floatUp.maxYLevel = 8;
        floatUp.motionPerSecond = Vector3.up * 0.5f;

        obj.AddComponent<InfectAnything>().infectionAmount = 1;
        
        return obj;
    }
}