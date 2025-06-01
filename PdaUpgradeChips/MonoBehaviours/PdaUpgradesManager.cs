using System.Collections;
using UnityEngine;

namespace PdaUpgradeChips.MonoBehaviours;

public class PdaUpgradesManager : MonoBehaviour
{
    public static PdaUpgradesManager Main { get; private set; }

    public Transform storageParent;

    private Equipment _equipment;
    private bool _opening;

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
        ErrorMessage.AddMessage("equipped " + item.techType + " to " + slot);
    }

    private void OnUnequip(string slot, InventoryItem item)
    {
        ErrorMessage.AddMessage("unequipped " + item.techType + " from " + slot);
    }
}