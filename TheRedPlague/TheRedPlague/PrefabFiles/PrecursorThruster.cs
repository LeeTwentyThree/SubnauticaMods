using System.Collections;
using Nautilus.Assets;
using Nautilus.Utility;
using UnityEngine;
using UWE;

namespace TheRedPlague.PrefabFiles;

public static class PrecursorThruster
{
    public static PrefabInfo Info { get; } = PrefabInfo.WithTechType("PrecursorThruster");

    public static void Register()
    {
        var customPrefab = new CustomPrefab(Info);
        customPrefab.SetGameObject(GetPrefab);
        customPrefab.Register();
    }

    private static IEnumerator GetPrefab(IOut<GameObject> prefab)
    {
        var request = PrefabDatabase.GetPrefabAsync("e8143977-448e-4202-b780-83485fa5f31a");
        yield return request;
        if (!request.TryGetPrefab(out var antechamber))
        {
            Plugin.Logger.LogError("Failed to load antechamber prefab!");
        }

        var obj = UWE.Utils.InstantiateDeactivated(antechamber.transform
            .Find("Precursor_Prison_Interior_Antechamber/anim_PrecPillar_root/anim_precPillarUpper_root")
            .gameObject);
        
        PrefabUtils.AddBasicComponents(obj, Info.ClassID, Info.TechType, LargeWorldEntity.CellLevel.VeryFar);

        var vfx = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("PrecursorThrusterVFX"),
            obj.transform);

        vfx.transform.localPosition = Vector3.zero;
        vfx.transform.localRotation = Quaternion.identity;

        obj.AddComponent<ConstructionObstacle>();
        
        prefab.Set(obj);
    }
}