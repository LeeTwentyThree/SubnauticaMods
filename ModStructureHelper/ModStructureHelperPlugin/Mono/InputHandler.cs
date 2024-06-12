using ModStructureHelperPlugin.UI;
using UnityEngine;

namespace ModStructureHelperPlugin.Mono;

public class InputHandler : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(Plugin.ModConfig.ToggleStructureHelperKeyBind))
        {
            StructureHelperUI.SetUIEnabled(!StructureHelperUI.IsActive);
        }

        if (!StructureHelperUI.main || !StructureHelperUI.main.isActiveAndEnabled) return;
        
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(Plugin.ModConfig.SaveKeyBind))
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