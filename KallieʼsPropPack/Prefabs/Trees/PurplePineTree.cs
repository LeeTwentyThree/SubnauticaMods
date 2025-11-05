using System.Collections;
using System.Collections.Generic;
using Nautilus.Assets;
using Nautilus.Utility;
using UnityEngine;
using UWE;

namespace KallieʼsPropPack.Prefabs.Trees;

public static class PurplePineTree
{
    public static PrefabInfo Info { get; } = PrefabInfo.WithTechType("Kallies_PurplePineTree");

    public static void Register()
    {
        var prefab = new CustomPrefab(Info);
        prefab.SetGameObject(BuildGameObject);
        prefab.Register();
    }

    private static IEnumerator BuildGameObject(IOut<GameObject> result)
    {
        var prefab = new GameObject("PurplePineTree");
        prefab.SetActive(false);

        var entities = GetEntities();

        Dictionary<string, GameObject> loadedPrefabs = new();
        foreach (var entity in entities)
        {
            if (!loadedPrefabs.TryGetValue(entity.classId, out var entityPrefab))
            {
                var task = PrefabDatabase.GetPrefabAsync(entity.classId);
                yield return task;
                task.TryGetPrefab(out entityPrefab);
                loadedPrefabs.Add(entity.classId, entityPrefab);
            }

            if (entityPrefab == null)
            {
                Plugin.Logger.LogWarning("Purple pine tree entity not found");
            }
            
            var child = Object.Instantiate(entityPrefab, prefab.transform);
            Object.DestroyImmediate(child.GetComponent<PrefabIdentifier>());
            Object.DestroyImmediate(child.GetComponent<LargeWorldEntity>());
            Object.DestroyImmediate(child.GetComponent<BreakableResource>());
            Object.DestroyImmediate(child.GetComponent<SkyApplier>());
            child.transform.localPosition = entity.position;
            child.transform.localRotation = entity.rotation;
            child.transform.localScale = entity.scale;
        }
        
        PrefabUtils.AddBasicComponents(prefab, Info.ClassID, Info.TechType, LargeWorldEntity.CellLevel.Medium);

        result.Set(prefab);
    }

    private static List<(string classId, Vector3 position, Quaternion rotation, Vector3 scale)> GetEntities() => new()
    {
        // Trunk
        ("2aea1607-0519-44a3-b6c8-ccdf32370fa0", Vector3.zero,
            new Quaternion(0f, 0f, 0f, 1f),
            new Vector3(6.10670328f, 4.380586f, 5.23899269f)),

        // Pinecones
        ("d8838f12-2e24-40c9-a7c5-24fb9c08e934", new Vector3(0.1359f, 10.1587f, -0.1861f),
            new Quaternion(0.127017453f, 0f, 0.151244819f, 0.9803018f),
            new Vector3(1.66679811f, 1.66679811f, 1.66679811f)),

        ("d8838f12-2e24-40c9-a7c5-24fb9c08e934", new Vector3(-0.19666f, 8.65622f, 0.33495f),
            new Quaternion(0.08928278f, 0.0903438851f, 0.100033507f, 0.9868434f),
            new Vector3(1.40389979f, 1.40389979f, 1.40389979f)),

        ("d8838f12-2e24-40c9-a7c5-24fb9c08e934", new Vector3(-0.0325628f, 9.13748f, -0.56835f),
            new Quaternion(-0.182665139f, -0.7901372f, 0.102007687f, 0.5761174f),
            new Vector3(1.64807153f, 1.64807153f, 1.64807153f)),

        ("d8838f12-2e24-40c9-a7c5-24fb9c08e934", new Vector3(0.4571031f, 8.601074f, 0.29805f),
            new Quaternion(-0.238714129f, -0.963268638f, 0.0547960773f, -0.110120565f),
            new Vector3(1.49846721f, 1.49846721f, 1.49846721f)),

        ("d8838f12-2e24-40c9-a7c5-24fb9c08e934", new Vector3(0.1365f, 11.05205f, -0.06775f),
            new Quaternion(0.127017453f, 0f, 0.151244819f, 0.9803018f),
            new Vector3(0.7463167f, 0.7463167f, 0.7463167f)),

        ("d8838f12-2e24-40c9-a7c5-24fb9c08e934", new Vector3(-0.6426f, 9.08032f, -0.37325f),
            new Quaternion(-0.107939847f, 0.265024036f, -0.231739178f, -0.9297356f),
            new Vector3(1.27330089f, 1.27330089f, 1.27330089f)),

        ("d8838f12-2e24-40c9-a7c5-24fb9c08e934", new Vector3(0.15358f, 8.403498f, -0.76837f),
            new Quaternion(0.0223515891f, 0.364455819f, -0.0001541654f, 0.9309524f),
            new Vector3(1.42534566f, 1.42534566f, 1.42534566f)),

        ("d8838f12-2e24-40c9-a7c5-24fb9c08e934", new Vector3(-0.62738f, 8.561464f, -0.204406f),
            new Quaternion(-0.06035002f, -0.08027332f, 0.390931338f, 0.9149245f),
            new Vector3(1.38202965f, 1.38202965f, 1.38202965f)),

        ("d8838f12-2e24-40c9-a7c5-24fb9c08e934", new Vector3(-0.4929472f, 7.694581f, -0.204406f),
            new Quaternion(-0.0741399f, -0.06774368f, 0.2151941f, 0.971393645f),
            new Vector3(1.87182045f, 1.87182045f, 1.87182045f)),

        ("d8838f12-2e24-40c9-a7c5-24fb9c08e934", new Vector3(0.7282f, 7.403518f, -0.627899f),
            new Quaternion(-0.0856225044f, -0.154291525f, -0.0253833979f, 0.9839811f),
            new Vector3(1.87182045f, 1.87182033f, 1.87182045f)),

        ("d8838f12-2e24-40c9-a7c5-24fb9c08e934", new Vector3(0.09466f, 7.264516f, 0.770203f),
            new Quaternion(-0.080126375f, -0.0605449937f, 0.123797655f, 0.9872124f),
            new Vector3(1.87182045f, 1.87182033f, 1.87182045f)),

        ("d8838f12-2e24-40c9-a7c5-24fb9c08e934", new Vector3(0.962768f, 7.264516f, 0.119812f),
            new Quaternion(-0.141612634f, -0.69201833f, 0.0411335044f, 0.7066573f),
            new Vector3(1.87182045f, 1.87182033f, 1.87182045f)),

        ("d8838f12-2e24-40c9-a7c5-24fb9c08e934", new Vector3(-0.3560148f, 9.750416f, -0.873016f),
            new Quaternion(0.127017453f, 0f, 0.151244819f, 0.9803018f),
            new Vector3(1.242807f, 1.242807f, 1.242807f)),

        ("d8838f12-2e24-40c9-a7c5-24fb9c08e934", new Vector3(0.5256456f, 9.567348f, -0.315918f),
            new Quaternion(0.0223515891f, 0.364455819f, -0.0001541654f, 0.9309524f),
            new Vector3(1.42534578f, 1.42534578f, 1.42534566f)),

        ("d8838f12-2e24-40c9-a7c5-24fb9c08e934", new Vector3(0.70384f, 8.370066f, -0.7137f),
            new Quaternion(-0.211372644f, -0.248536557f, 0.0266760681f, -0.944901943f),
            new Vector3(1.49846733f, 1.49846721f, 1.49846733f)),

        ("d8838f12-2e24-40c9-a7c5-24fb9c08e934", new Vector3(0.0357126f, 9.424683f, -0.098419f),
            new Quaternion(0.1298263f, 0.6237174f, 0.0335149243f, 0.770064f),
            new Vector3(1.40389991f, 1.40389979f, 1.40389991f)),

        ("d8838f12-2e24-40c9-a7c5-24fb9c08e934", new Vector3(-0.12463f, 6.857836f, -0.446167f),
            new Quaternion(-0.0204540659f, 0.253884017f, -0.008679405f, 0.9669795f),
            new Vector3(1f, 1f, 1f)),

        ("d8838f12-2e24-40c9-a7c5-24fb9c08e934", new Vector3(0.5375f, 6.857836f, 0.31235f),
            new Quaternion(-0.0204540659f, 0.253884017f, -0.008679405f, 0.9669795f),
            new Vector3(1f, 1f, 1f)),

        ("d8838f12-2e24-40c9-a7c5-24fb9c08e934", new Vector3(-0.4090558f, 7.060282f, -0.09715f),
            new Quaternion(-0.08597036f, -0.05191368f, 0.0208050422f, 0.9947267f),
            new Vector3(1.87182045f, 1.87182057f, 1.87182057f)),

        ("d8838f12-2e24-40c9-a7c5-24fb9c08e934", new Vector3(-0.9117863f, 7.060282f, -0.243713f),
            new Quaternion(-0.049614422f, 0.630749464f, 0.07322679f, 0.7709289f),
            new Vector3(1.87182045f, 1.87182081f, 1.87182057f)),

        ("d8838f12-2e24-40c9-a7c5-24fb9c08e934", new Vector3(1.02577f, 9.292601f, 0.323609f),
            new Quaternion(0.127017453f, 0f, 0.151244819f, 0.9803018f),
            new Vector3(0.746316731f, 0.7463167f, 0.7463167f)),

        ("d8838f12-2e24-40c9-a7c5-24fb9c08e934", new Vector3(-0.33377f, 8.116974f, -0.86835f),
            new Quaternion(-0.0878836438f, -0.390643269f, -0.0100108655f, 0.9162828f),
            new Vector3(1.4505347f, 1.45053494f, 1.45053482f))
    };
}