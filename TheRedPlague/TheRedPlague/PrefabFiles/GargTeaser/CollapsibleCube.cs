using System.Collections;
using Nautilus.Assets;
using Nautilus.Assets.PrefabTemplates;
using UnityEngine;
using UWE;

namespace TheRedPlague.PrefabFiles.GargTeaser;

public class CollapsibleCube
{
    public PrefabInfo Info { get; }

    private readonly string _referenceClassId;

    public CollapsibleCube(string classId, string referenceClassId)
    {
        Info = PrefabInfo.WithTechType(classId);
        _referenceClassId = referenceClassId;
    }

    public void Register()
    {
        var prefab = new CustomPrefab(Info);
        prefab.SetGameObject(new CloneTemplate(Info, _referenceClassId)
        {
            ModifyPrefabAsync = ModifyPrefab
        });
        prefab.Register();
    }

    private IEnumerator ModifyPrefab(GameObject prefab)
    {
        Object.DestroyImmediate(prefab.GetComponentInChildren<VolumeCullManager>());
        prefab.GetComponent<LargeWorldEntity>().cellLevel = LargeWorldEntity.CellLevel.VeryFar;
        // alien thermal plant
        var request = PrefabDatabase.GetPrefabAsync("59eefa74-2dd8-4522-83bd-c498831eb2aa");
        yield return request;
        if (!request.TryGetPrefab(out var alienThermalPlantPrefab)) yield break;
        var gen = UWE.Utils.InstantiateDeactivated(alienThermalPlantPrefab);
        gen.transform.parent = prefab.transform;
        gen.transform.localPosition = new Vector3(2, 11, 1);
        gen.transform.localRotation = Quaternion.identity;
        Object.DestroyImmediate(gen.GetComponent<PrefabIdentifier>());
        Object.DestroyImmediate(gen.GetComponent<LargeWorldEntity>());
        Object.DestroyImmediate(gen.GetComponent<TechTag>());
        Object.DestroyImmediate(gen.GetComponentInChildren<VolumeCullManager>());
        gen.SetActive(true);
    }
}