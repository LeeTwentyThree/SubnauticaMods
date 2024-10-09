using Nautilus.Assets;
using Nautilus.Utility;
using TheRedPlague.Mono;
using UnityEngine;

namespace TheRedPlague.PrefabFiles;

public class SpawnWithHivemind
{
    private PrefabInfo Info { get; }
    private string SpawnClassId { get; }
    private LargeWorldEntity.CellLevel CellLevel { get; }
    
    public SpawnWithHivemind(PrefabInfo info, string spawnClassId, LargeWorldEntity.CellLevel cellLevel)
    {
        Info = info;
        SpawnClassId = spawnClassId;
        CellLevel = cellLevel;
    }

    public void Register()
    {
        var prefab = new CustomPrefab(Info);
        prefab.SetGameObject(GetPrefab);
        prefab.Register();
    }

    private GameObject GetPrefab()
    {
        var obj = new GameObject(Info.ClassID);
        obj.SetActive(false);
        PrefabUtils.AddBasicComponents(obj, Info.ClassID, Info.TechType, CellLevel);
        obj.AddComponent<SpawnOnceHiveMindIsReleased>().spawnClassId = SpawnClassId;
        return obj;
    }
}