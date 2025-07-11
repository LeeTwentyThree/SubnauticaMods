﻿using System;
using System.Collections;
using System.Collections.Generic;
using Nautilus.Handlers;
using Nautilus.Json;
using Nautilus.Json.Attributes;
using PdaUpgradeChips.MonoBehaviours.Upgrades;
using UnityEngine;

namespace PdaUpgradeChips.MonoBehaviours;

public class PdaUpgradesManager : MonoBehaviour, IProtoEventListener, IProtoTreeEventListener
{
    public static PdaUpgradesManager Main { get; private set; }

    public Transform storageParent;

    private Equipment _equipment;
    private bool _opening;

    private Dictionary<Type, UpgradeChipBase> _upgradeChipBehaviors = new();
    
    private static SaveData _saveData;
    
    private void Awake()
    {
        Main = this;

        _equipment = new Equipment(gameObject, storageParent);
        _equipment.SetLabel("PdaUpgradesStorageLabel");
        _equipment.onEquip += OnEquip;
        _equipment.onUnequip += OnUnequip;
        UnlockDefaultModuleSlots();
    }

    public static void RegisterSaveData()
    {
        _saveData = SaveDataHandler.RegisterSaveDataCache<SaveData>();
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
            if (isActiveAndEnabled)
                StartCoroutine(AddChipCoroutine(upgradeChipType));
            else
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

    private IEnumerator AddChipCoroutine(Type chipType)
    {
        // Wait at least a frame to prevent issues when quickly swapping in the same frame (required with this setup)
        yield return null;
        AddChipBehaviour(chipType);
    }

    private void AddChipBehaviour(Type chipType)
    {
        if (_upgradeChipBehaviors.ContainsKey(chipType))
        {
            Plugin.Logger.LogWarning("Can't add duplicate upgrade chip modules!");
            return;
        }
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

    public void OnProtoSerialize(ProtobufSerializer serializer)
    {
        _saveData.savedSlots = _equipment.SaveEquipment();
        _saveData.Save();
    }

    public void OnProtoDeserialize(ProtobufSerializer serializer)
    {
        
    }

    public void OnProtoSerializeObjectTree(ProtobufSerializer serializer)
    {
        
    }

    public void OnProtoDeserializeObjectTree(ProtobufSerializer serializer)
    {
        var savedSlots = _saveData.savedSlots;
        if (savedSlots == null)
            return;
        StorageHelper.TransferEquipment(storageParent.gameObject, savedSlots, _equipment);
    }
    
    [FileName("PdaUpgradesStorage")]
    private class SaveData : SaveDataCache
    {
        public Dictionary<string, string> savedSlots;
    }
}