using UnityEngine;

namespace PdaUpgradeChips.MonoBehaviours;

public class PdaUpgradesManager : MonoBehaviour
{
    public static PdaUpgradesManager Main { get; private set; }

    public Transform storageParent;

    private Equipment _equipment;

    private void Awake()
    {
        Main = this;

        _equipment = new Equipment(gameObject, storageParent);
        _equipment.SetLabel("PdaUpgradesStorageLabel");
        _equipment.onEquip += OnEquip;
        _equipment.onUnequip += OnUnequip;
        UnlockDefaultModuleSlots();
    }

    private void UnlockDefaultModuleSlots()
    {
        _equipment.AddSlots(PdaUpgradesAPI.GetUpgradeEquipmentSlotNames());
    }

    public void DisplayMenu()
    {
        Inventory.main.SetUsedStorage(_equipment);
        Player.main.GetPDA().Open(PDATab.Inventory);
    }

    private void OnEquip(string slot, InventoryItem item)
    {
        ErrorMessage.AddMessage("equipped " + item.techType + " to " + slot);
    }

    private void OnUnequip(string slot, InventoryItem item)
    {
        ErrorMessage.AddMessage("unequipped " + item.techType + " from " + slot);
    }
}