using System.Collections;
using Nautilus.Assets;
using Nautilus.Utility;
using TheRedPlague.Mono;
using UnityEngine;

namespace TheRedPlague.PrefabFiles;

public class FleshDecorationPrefab
{
    public FleshDecorationPrefab(PrefabInfo info, string modelName, bool infected)
    {
        Info = info;
        ModelName = modelName;
        Infected = infected;
    }

    public PrefabInfo Info { get; }
    public string ModelName { get; }
    public bool Infected { get; }

    public void Register()
    {
        var prefab = new CustomPrefab(Info);
        prefab.SetGameObject(GetCorpsePrefab);
        prefab.Register();
    }

    private IEnumerator GetCorpsePrefab(IOut<GameObject> prefab)
    {
        var go = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>(ModelName));
        go.SetActive(false);
        PrefabUtils.AddBasicComponents(go, Info.ClassID, Info.TechType, LargeWorldEntity.CellLevel.Medium);
        MaterialUtils.ApplySNShaders(go);
        if (Infected)
        {
            var infect = go.AddComponent<InfectAnything>();
            infect.infectionHeightStrength = 0.05f;
        }

        yield return null;
        prefab.Set(go);
    }
}