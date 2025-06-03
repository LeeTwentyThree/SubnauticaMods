using PdaUpgradeCards.MonoBehaviours;
using UnityEngine;

namespace PdaUpgradeCards.Data;

public static class PdaElements
{
    public static CustomPdaButton OpenMenuButton { get; private set; }

    public static void RegisterAll()
    {
        OpenMenuButton = new CustomPdaButton(
            "OpenUpgradesMenuButton",
            rect => { rect.anchoredPosition = new Vector2(500, 240); },
            new CustomPdaButton.ButtonIcons("ViewUpgradeChipsButtonIcon", "ViewUpgradeChipsButtonSelectedIcon",
                "ViewUpgradeChipsButtonPressedIcon"),
            PDATab.Inventory,
            () => PdaUpgradesManager.Main.DisplayMenu(),
            true);
    }
}