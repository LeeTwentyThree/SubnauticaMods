using System;
using System.Collections;
using System.Collections.Generic;
using PdaUpgradeCards.MonoBehaviours.Upgrades;
using UnityEngine;

namespace PdaUpgradeCards.MonoBehaviours;

public class PdaUpgradesManager : MonoBehaviour
{
    public static PdaUpgradesManager Main { get; private set; }

    public Transform storageParent;

    private Equipment _equipment;
    private bool _opening;

    private Dictionary<Type, UpgradeChipBase> _upgradeChipBehaviors = new();
    
    private bool _initialized;

    private void Awake()
    {
        Main = this;

        _equipment = new Equipment(gameObject, storageParent);
        _equipment.SetLabel("PdaUpgradesStorageLabel");
        _equipment.onEquip += OnEquip;
        _equipment.onUnequip += OnUnequip;
        _equipment.isAllowedToAdd = IsAllowedToAdd;
        UnlockDefaultModuleSlots();
    }

    private void Start()
    {
        var equipment = _equipment.GetEquipment();
        while (equipment.MoveNext())
        {
            var item = equipment.Current;
            if (item.Value != null)
            {
                OnEquip(item.Key, item.Value);
            }
        }
        _initialized = true;
    }

    private bool IsAllowedToAdd(Pickupable pickupable, bool verbose)
    {
        if (!_initialized)
            return false;
        
        var techType = pickupable.GetTechType();
        return _equipment.GetCount(techType) < 1;
    }

    private void UnlockDefaultModuleSlots()
    {
        _equipment.AddSlots(PdaUpgradesAPI.GetUpgradeEquipmentSlotNames());
    }

    public void DisplayMenu()
    {
        if (_opening)
            return;
        StartCoroutine(OpenMenuCoroutine());
    }

    private IEnumerator OpenMenuCoroutine()
    {
        _opening = true;
        var pda = Player.main.GetPDA();
        pda.Close();
        yield return new WaitForSeconds(0.5f);
        Inventory.main.SetUsedStorage(_equipment);
        pda.Open(PDATab.Inventory);
        _opening = false;
    }

    private void OnEquip(string slot, InventoryItem item)
    {
        if (PdaUpgradesAPI.TryGetUpgradeChipBehaviourType(item.techType, out var upgradeChipType))
        {
            AddChipBehaviour(upgradeChipType);
        }
        else
        {
            Plugin.Logger.LogError("Failed to find upgrade chip behaviour type for TechType " + item.techType);
        }
    }

    private void OnUnequip(string slot, InventoryItem item)
    {
        if (PdaUpgradesAPI.TryGetUpgradeChipBehaviourType(item.techType, out var upgradeChipType))
        {
            RemoveChipBehaviour(upgradeChipType);
        }
        else
        {
            Plugin.Logger.LogError("Failed to find upgrade chip behaviour type for TechType " + item.techType);
        }
    }

    private void AddChipBehaviour(Type chipType)
    {
        var chipObject = new GameObject(chipType.Name);
        chipObject.transform.parent = transform;
        var chipComponent = chipObject.AddComponent(chipType);
        _upgradeChipBehaviors.Add(chipType, chipComponent as UpgradeChipBase);
    }

    private void RemoveChipBehaviour(Type chipType)
    {
        if (_upgradeChipBehaviors.TryGetValue(chipType, out var chipComponent))
        {
            if (chipComponent != null)
            {
                Destroy(chipComponent.gameObject);
            }
            _upgradeChipBehaviors.Remove(chipType);
        }
        else
        {
            Plugin.Logger.LogWarning("Failed to find chip behaviour to remove!");
        }
    }
}