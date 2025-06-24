using UnityEngine;

namespace PdaUpgradeChips.MonoBehaviours.Upgrades;

public class PocketDimensionUpgradeTier2 : PocketDimensionUpgrade
{
    protected override TechType DimensionTechType => Plugin.PocketDimensionTier2TechType;
    protected override int DimensionTier => 2;
    protected override Vector3 WorldLocation => new(-2500, -500, 120);
}