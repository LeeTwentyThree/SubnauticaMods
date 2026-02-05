using System;
using System.Collections;
using Nautilus.Assets;
using Nautilus.Handlers;
using Nautilus.Utility;
using UnityEngine;
using UWE;
using Object = UnityEngine.Object;

namespace KallieʼsPropPack.Prefabs.Precursor;

public class StrippedPrecursorProp
{
    private PrefabInfo Info { get; set; }
    
    private readonly string _cloneClassId;
    private readonly string _pathToModel;
    private readonly CollisionsMode _collisionsMode;
    private readonly LargeWorldEntity.CellLevel _cellLevel;
    private readonly bool _zUp;
    
    public Action<GameObject> ModifyPrefab { private get; init; }

    public StrippedPrecursorProp(string classId, string cloneClassId, string pathToModel,
        CollisionsMode collisionsMode, bool zUp = true, LargeWorldEntity.CellLevel cellLevel = LargeWorldEntity.CellLevel.Near)
    {
        var actualClassId = "Kallies_" + classId;
        Info = PrefabInfo.WithTechType(actualClassId).WithFileName("KallieʼsPropPack/Precursor/" + actualClassId);
        _cloneClassId = cloneClassId;
        _pathToModel = pathToModel;
        _collisionsMode = collisionsMode;
        _zUp = zUp;
        _cellLevel = cellLevel;
    }

    public enum CollisionsMode
    {
        None,
        Mesh,
        ConvexMesh,
        BoundingBox
    }

    public void Register()
    {
        var prefab = new CustomPrefab(Info);
        prefab.SetGameObject(CreatePrefab);
        prefab.Register();
    }
    
    private IEnumerator CreatePrefab(IOut<GameObject> result)
    {
        var request = PrefabDatabase.GetPrefabAsync(_cloneClassId);
        yield return request;
        request.TryGetPrefab(out var clonePrefab);
        var prefab = new GameObject(Info.ClassID);
        prefab.SetActive(false);
        var obj = Object.Instantiate(clonePrefab.transform.Find(_pathToModel).gameObject, prefab.transform);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        Object.DestroyImmediate(obj.GetComponent<TechTag>());
        PrefabUtils.AddBasicComponents(prefab, Info.ClassID, Info.TechType, _cellLevel);

        if (_collisionsMode != CollisionsMode.None)
        {
            foreach (var renderer in obj.GetComponentsInChildren<Renderer>())
            {
                if (_collisionsMode == CollisionsMode.BoundingBox)
                {
                    var boxCollider = renderer.gameObject.AddComponent<BoxCollider>();
                    var bounds = renderer.bounds;
                    boxCollider.size = bounds.size;
                    boxCollider.center = bounds.center;
                    continue;
                }
                
                var meshCollider = renderer.gameObject.AddComponent<MeshCollider>();
                if (renderer is MeshRenderer)
                    meshCollider.sharedMesh = renderer.gameObject.GetComponent<MeshFilter>()?.mesh;
                else if (renderer is SkinnedMeshRenderer skinned)
                    meshCollider.sharedMesh = skinned.sharedMesh;
                meshCollider.convex = _collisionsMode == CollisionsMode.ConvexMesh;
            }
        }
        
        obj.transform.localEulerAngles = new Vector3(_zUp ? -90 : 0, 0, 0);
        
        ModifyPrefab?.Invoke(prefab);
        
        result.Set(prefab);
    }
}