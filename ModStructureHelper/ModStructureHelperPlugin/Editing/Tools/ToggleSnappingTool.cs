using TMPro;
using UnityEngine;

namespace ModStructureHelperPlugin.Editing.Tools;

public class ToggleSnappingTool : ToolBase
{
    [SerializeField] private GameObject snappingWindow;
    [SerializeField] private TMP_InputField positionSnapping;
    [SerializeField] private TMP_InputField angleSnapping;
    
    public override ToolType Type => ToolType.Snapping;

    public override bool MultitaskTool => true;

    protected override void OnToolEnabled()
    {
        snappingWindow.SetActive(true);
        OnUpdateSnapping();
    }

    protected override void OnToolDisabled()
    {
        snappingWindow.SetActive(false);
        manager.handle.positionSnap = Vector3.zero;
        manager.handle.rotationSnap = 0;
    }

    public void OnUpdateSnapping()
    {
        if (float.TryParse(positionSnapping.text, out var positionSnap)) manager.handle.positionSnap = Vector3.one * positionSnap;
        if (float.TryParse(angleSnapping.text, out var rotationSnap)) manager.handle.rotationSnap = rotationSnap;
    }
}