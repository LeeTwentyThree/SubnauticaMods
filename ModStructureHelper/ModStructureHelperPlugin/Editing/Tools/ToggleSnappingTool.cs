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

    private bool _windowOpen;
    private bool _snapBindHeld;

    private bool _snappingEnabled;

    protected override void OnToolEnabled()
    {
        snappingWindow.SetActive(true);
        _windowOpen = true;
        EnableSnapping();
    }

    protected override void OnToolDisabled()
    {
        snappingWindow.SetActive(false);
        _windowOpen = false;
        DisableSnapping();
    }

    public void OnUpdateSnapping()
    {
        if (float.TryParse(positionSnapping.text, out var positionSnap))
            manager.handle.positionSnap = Vector3.one * positionSnap;
        if (float.TryParse(angleSnapping.text, out var rotationSnap))
            manager.handle.rotationSnap = rotationSnap;
    }

    private void EnableSnapping()
    {
        OnUpdateSnapping();
        _snappingEnabled = true;
    }

    private void DisableSnapping()
    {
        manager.handle.positionSnap = Vector3.zero;
        manager.handle.rotationSnap = 0;
        _snappingEnabled = false;
    }

    private void Update()
    {
        _snapBindHeld = GameInput.GetButtonHeld(StructureHelperInput.HoldToSnap);
        if (!_snappingEnabled && _snapBindHeld)
        {
            EnableSnapping();
        }
        else if (_snappingEnabled && !_windowOpen && !_snapBindHeld)
        {
            DisableSnapping();
        }
    }
}