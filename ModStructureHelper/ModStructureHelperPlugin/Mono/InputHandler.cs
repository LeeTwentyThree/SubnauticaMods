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
        if (StructureHelperUI.main && StructureHelperUI.main.isActiveAndEnabled && Input.GetKeyDown(KeyCode.LeftControl) && Input.GetKeyDown(Plugin.ModConfig.SaveKeyBind))
        {
            StructureInstance.TrySave();
        }
    }
}