using System;
using System.Collections.Generic;
using Nautilus.Handlers;

namespace PdaUpgradeChips;

public static class PdaUpgradesAPI
{
    public static EquipmentType PdaUpgradeEquipmentType { get; } = EnumHandler.AddEntry<EquipmentType>("PdaUpgrade");

    public static int UpgradeSlotsCount => 8;
    
    public const string PdaUpgradesEquipmentSlotPrefix = "PdaUpgradeChip";

    private static readonly Dictionary<TechType, Type> ChipData = new();

    internal static void Register()
    {
        foreach (var upgrade in GetUpgradeEquipmentSlotNames())
        {
            Equipment.slotMapping.Add(upgrade, PdaUpgradeEquipmentType);
        }
    }
    
    public static IEnumerable<string> GetUpgradeEquipmentSlotNames()
    {
        for (var i = 1; i <= UpgradeSlotsCount; i++)
        {
            yield return $"{PdaUpgradesEquipmentSlotPrefix}{i}";
        }
    }
    
    public static void RegisterUpgradeChip(TechType techType, Type behaviourType)
    {
        ChipData.Add(techType, behaviourType);
    }

    public static bool GetUpgradeChipBehaviourType(TechType techType, out Type behaviourType)
    {
        return ChipData.TryGetValue(techType, out behaviourType);
    }
}