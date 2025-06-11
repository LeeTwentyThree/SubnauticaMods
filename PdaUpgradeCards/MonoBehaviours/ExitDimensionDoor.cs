using PdaUpgradeCards.MonoBehaviours.Upgrades;

namespace PdaUpgradeCards.MonoBehaviours;

public class ExitDimensionDoor : HandTarget, IHandTarget
{
    public void OnHandHover(GUIHand hand)
    {
        HandReticle.main.SetText(HandReticle.TextType.Hand, "ExitPocketDimensionDoor", true, GameInput.Button.LeftHand);
        HandReticle.main.SetIcon(HandReticle.IconType.Hand);
    }

    public void OnHandClick(GUIHand hand)
    {
        PocketDimensionUpgrade.TeleportPlayerOut();
    }
}