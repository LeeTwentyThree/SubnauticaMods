using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Utility;
using PdaUpgradeCards.MonoBehaviours;
using UnityEngine;

namespace PdaUpgradeCards.Prefabs;

public static class PdaUpgradesContainerPrefab
{
    public static PrefabInfo Info { get; } = PrefabInfo.WithTechType("PdaUpgradesContainer");

    public static void Register()
    {
        var prefab = new CustomPrefab(Info);
        prefab.SetGameObject(GetGameObject);
        prefab.SetSpawns(new SpawnLocation(new Vector3(1, 2, 3)));
        prefab.Register();
    }

    private static GameObject GetGameObject()
    {
        var obj = new GameObject(Info.ClassID);
        obj.SetActive(false);
        PrefabUtils.AddBasicComponents(obj, Info.ClassID, Info.TechType, LargeWorldEntity.CellLevel.Global);
        
        var upgradeManager = obj.AddComponent<PdaUpgradesManager>();
        
        var equipmentParent = new GameObject("EquipmentParent");
        equipmentParent.transform.parent = obj.transform;
        equipmentParent.AddComponent<ChildObjectIdentifier>().ClassId = "PdaUpgradesContainer";
        
        upgradeManager.storageParent = equipmentParent.transform;
        
        return obj;
    }
}