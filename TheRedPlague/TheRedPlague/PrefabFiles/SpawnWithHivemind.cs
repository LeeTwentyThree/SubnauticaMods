using Nautilus.Assets;
using Nautilus.Utility;
using UnityEngine;

namespace TheRedPlague.PrefabFiles;

public class SpawnWithHivemind
{
    private PrefabInfo Info { get; }
    private string SpawnClassId { get; }
    private LargeWorldEntity.CellLevel CellLevel { get; }
    
    public SpawnWithHivemind(string classId, LargeWorldEntity.CellLevel cellLevel)
    {
        Info = PrefabInfo.WithTechType(classId + "HMSpawn");
        SpawnClassId = classId;
        CellLevel = cellLevel;
    }

    public void Register()
    {
        var prefab = new CustomPrefab(Info);
        prefab.Register();
    }

    private GameObject GetPrefab()
    {
        var obj = new GameObject(Info.ClassID);
        obj.SetActive(false);
        PrefabUtils.AddBasicComponents();
        return obj;
    }
}