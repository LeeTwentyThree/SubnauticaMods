using System.Collections;
using KallieʼsPropPack.MaterialModifiers;
using KallieʼsPropPack.MonoBehaviours;
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
        
        // emergency fix
        if (Info.ClassID.Equals("Kallies_SingleCell_Blob_Flat"))
        {
            prefab.AddComponent<DestroyIfIdMatches>().ids = new[]
            {
                "2ee22d3b-9c2c-4b20-81be-735b2c2251e5",
                "50143ef9-c0a2-4dee-bc80-0066d60c03e3"
            };
        }
        
        yield return null;
        result.Set(prefab);
    }
}