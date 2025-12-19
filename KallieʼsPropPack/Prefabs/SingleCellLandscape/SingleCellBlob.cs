using System.Collections;
using KallieʼsPropPack.MaterialModifiers;
using Nautilus.Assets;
using Nautilus.Utility;
using Nautilus.Utility.MaterialModifiers;
using UnityEngine;

namespace KallieʼsPropPack.Prefabs.SingleCellLandscape;

public class SingleCellBlob
{
    private PrefabInfo Info { get; }
    private readonly string _prefabName;

    public SingleCellBlob(string classId, string prefabName)
    {
        Info = PrefabInfo.WithTechType(classId);
        _prefabName = prefabName;
    }

    public void Register()
    {
        var prefab = new CustomPrefab(Info);
        prefab.SetGameObject(CreatePrefab);
        prefab.Register();
    }

    private IEnumerator CreatePrefab(IOut<GameObject> result)
    {
        var prefab = Object.Instantiate(Plugin.Bundle.LoadAsset<GameObject>(_prefabName));
        PrefabUtils.AddBasicComponents(prefab, Info.ClassID, Info.TechType, LargeWorldEntity.CellLevel.Far);
        MaterialUtils.ApplySNShaders(prefab, 5f, 0.2f, 1, new WavingEffectModifier(1)
            {
                Scale = new Vector4(0.4f, 0.4f, 0.4f, 0.1f),
                Frequency = new Vector4(0.3f, 0.3f, 0.3f, 0.7f),
                Speed = new Vector2(0.5f, 0.68f)
            },
            new FresnelModifier(0.6f),
            new FloatPropertyModifier("_IBLreductionAtNight", 0));
        
        prefab.AddComponent<ConstructionObstacle>();
        
        var collider = prefab.GetComponentInChildren<Collider>();
        if (collider != null)
        {
            collider.gameObject.layer = LayerID.TerrainCollider;
            collider.gameObject.AddComponent<VFXSurface>().surfaceType = VFXSurfaceTypes.vegetation;
        }
        
        yield return null;
        result.Set(prefab);
    }
}