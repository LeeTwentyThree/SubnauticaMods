using ModStructureHelperPlugin.StructureHandling;
using ModStructureHelperPlugin.UI;
using UnityEngine;

namespace ModStructureHelperPlugin.Mono;

public class InputHandler : MonoBehaviour
{
    private void Update()
    {
        if (GameInput.GetButtonDown(StructureHelperInput.ToggleStructureHelperKeyBind))
        {
            StructureHelperUI.SetUIEnabled(!StructureHelperUI.IsActive);
        }

        if (!StructureHelperUI.main || !StructureHelperUI.main.isActiveAndEnabled) return;
        
        if (GameInput.GetButtonDown(StructureHelperInput.SaveHotkeyModifier) && GameInput.GetButtonDown(StructureHelperInput.SaveKeyBind))
        {
            StructureInstance.TrySave();
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            StructureHelperUI.main.SetInputGroupOverride(false);
        }
        if (Input.GetMouseButtonUp(1))
        {
            StructureHelperUI.main.SetInputGroupOverride(true);
        }
    }
}