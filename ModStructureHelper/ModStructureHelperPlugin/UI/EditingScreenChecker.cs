using UnityEngine;
using UnityEngine.EventSystems;

namespace ModStructureHelperPlugin.UI;

// Checks whether the cursor is hovering over any windows or not
public class EditingScreenChecker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool _hoveringOverThis;
    
    public bool IsCursorHoveredOverExternalWindows()
    {
        return !_hoveringOverThis;
    }

    private void OnEnable()
    {
        _hoveringOverThis = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _hoveringOverThis = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _hoveringOverThis = false;
    }
}