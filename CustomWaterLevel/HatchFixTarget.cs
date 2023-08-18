using UnityEngine;

namespace CustomWaterLevel;

internal class HatchFixTarget : HandTarget, IHandTarget
{
    public bool enter;

    public void OnHandClick(GUIHand hand)
    {
        Player.main.SetPosition(GetInteractPosition());
    }

    private Vector3 GetInteractPosition()
    {
        var parent = transform.parent;
        if (enter)
        {
            return parent.position + (parent.forward * -2f);
        }
        return parent.position + (parent.forward * 2f);
    }

    public void OnHandHover(GUIHand hand)
    {
        HandReticle.main.SetText(HandReticle.TextType.Use, "Use", false, GameInput.Button.LeftHand);
    }

    private bool AboveWater()
    {
        if (transform.position.y > Plugin.WaterLevel)
        {
            return true;
        }
        return false;
    }

    private void Update()
    {
        if (!AboveWater())
        {
            gameObject.SetActive(false);
        }
    }
}
