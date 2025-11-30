using System.Collections;
using Nautilus.Assets;
using Nautilus.Utility;
using UnityEngine;

namespace KallieʼsPropPack.Prefabs.Lab;

public abstract class LabPropBase
{
    private PrefabInfo Info { get; }
    private string ModelName { get; }
    private LargeWorldEntity.CellLevel CellLevel { get; }

    public LabPropBase(string id, string modelName, LargeWorldEntity.CellLevel cellLevel)
    {
        Info = PrefabInfo.WithTechType("KalliesLab/" + id);
        ModelName = modelName;
        CellLevel = cellLevel;
    }

    public void Register()
    {
        var prefab = new CustomPrefab(Info);
        prefab.SetGameObject(CreatePrefab);
        prefab.Register();
    }

    private IEnumerator CreatePrefab(IOut<GameObject> result)
    {
        var obj = Object.Instantiate(Plugin.Bundle.LoadAsset<GameObject>(ModelName));
        obj.SetActive(false);
        PrefabUtils.AddBasicComponents(obj, Info.ClassID, Info.TechType, LargeWorldEntity.CellLevel.Near);
        ApplyMaterials(obj);
        foreach (var collider in obj.GetComponentsInChildren<Collider>(true))
        {
            collider.gameObject.EnsureComponent<VFXSurface>().surfaceType = VFXSurfaceTypes.metal;
        }

        obj.AddComponent<ConstructionObstacle>();
        yield return null;
        result.Set(obj);
    }

    protected abstract void ApplyMaterials(GameObject obj);
}