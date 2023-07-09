using UnityEngine;

namespace LiveMinimap;

internal class MinimapEnabler : MonoBehaviour
{
    private Equipment equipment;

    private void Start()
    {
        equipment = Inventory.main.equipment;
        equipment.onEquip += OnChange;
        equipment.onUnequip += OnChange;
    }

    private void OnChange(string slot, InventoryItem item)
    {
        if (equipment == null)
            return;

        SetMinimapEnabled(equipment.GetCount(Items.Equipment.MinimapChipPrefab.Info.TechType) > 0);
    }

    public void SetMinimapEnabled(bool enabled)
    {
        MinimapController.Main.gameObject.SetActive(enabled);
    }
}