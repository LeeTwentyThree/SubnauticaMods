using System;
using ModStructureHelperPlugin.UI.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ModStructureHelperPlugin.Editing.Tools;

public class ToggleSnappingTool : ToolBase
{
    [SerializeField] private GameObject snappingWindow;
    [SerializeField] private TMP_InputField positionSnapping;
    [SerializeField] private TMP_InputField angleSnapping;
    [SerializeField] private Toggle useGlobalGridToggle;
    [SerializeField] private Vector3InputField gridCenterField;
    [SerializeField] private Vector3InputField gridRotationField;
    [SerializeField] private Button createGridCenterButton;
    [SerializeField] private Button teleportToGridCenterButton;
    [SerializeField] private Button removeGlobalGridButton;
    
    public override ToolType Type => ToolType.Snapping;

    public override bool MultitaskTool => true;

    private bool _windowOpen;
    private bool _snapBindHeld;

    private void Start()
    {
        SetDefaultValues();
    }

    private void SetDefaultValues()
    {
        positionSnapping.text = "1.0";
        angleSnapping.text = "45";
        useGlobalGridToggle.isOn = false;
        gridCenterField.SetValue(Vector3.zero);
        gridRotationField.SetValue(Vector3.zero);
        teleportToGridCenterButton.interactable = false;
        removeGlobalGridButton.interactable = false;
    }
    
    protected override string GetBindString()
    {
        var binding = GameInput.FormatButton(StructureHelperInput.HoldToSnap);
        return base.GetBindString() + $" (or hold {binding})";
    }

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
            manager.snappingManager.SetPositionSnapping(positionSnap);
        if (float.TryParse(angleSnapping.text, out var rotationSnap))
            manager.snappingManager.SetRotationSnapping(rotationSnap);
        OnUpdateGlobalGridChanged(useGlobalGridToggle.isOn);
        OnUpdateGridPosition();
        OnUpdateGridRotation();
    }

    public void OnUpdateGridPosition()
    {
        manager.snappingManager.SetGlobalGridCenter(gridCenterField.Value);
    }
    
    public void OnUpdateGridRotation()
    {
        manager.snappingManager.SetGlobalGridRotation(gridRotationField.Value);
    }

    public void OnUpdateGlobalGridChanged(bool uselessBoolean)
    {
        manager.snappingManager.SetUseGlobalGrid(useGlobalGridToggle.isOn);
    }

    private void EnableSnapping()
    {
        OnUpdateSnapping();
        manager.snappingManager.SnappingEnabled = true;
    }

    private void DisableSnapping()
    {
        manager.snappingManager.SnappingEnabled = false;
    }

    private void Update()
    {
        _snapBindHeld = GameInput.GetButtonHeld(StructureHelperInput.HoldToSnap);
        if (!manager.snappingManager.SnappingEnabled && _snapBindHeld)
        {
            EnableSnapping();
        }
        else if (manager.snappingManager.SnappingEnabled && !_windowOpen && !_snapBindHeld)
        {
            DisableSnapping();
        }
    }
}