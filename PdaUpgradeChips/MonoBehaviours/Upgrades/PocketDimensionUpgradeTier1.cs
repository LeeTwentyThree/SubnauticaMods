using UnityEngine;

namespace PdaUpgradeChips.MonoBehaviours.Upgrades;

public class PocketDimensionUpgradeTier1 : PocketDimensionUpgrade
{
    protected override TechType DimensionTechType => Plugin.PocketDimensionTier1TechType;
    protected override int DimensionTier => 1;
    protected override Vector3 WorldLocation => new(-2500, -500, 60);
}