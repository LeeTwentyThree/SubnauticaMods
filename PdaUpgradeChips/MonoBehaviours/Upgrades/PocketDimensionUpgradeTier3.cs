using UnityEngine;

namespace PdaUpgradeChips.MonoBehaviours.Upgrades;

public class PocketDimensionUpgradeTier3 : PocketDimensionUpgrade
{
    protected override TechType DimensionTechType => Plugin.PocketDimensionTier3TechType;
    protected override int DimensionTier => 3;
    protected override Vector3 WorldLocation => new(-2500, -500, 200);
}