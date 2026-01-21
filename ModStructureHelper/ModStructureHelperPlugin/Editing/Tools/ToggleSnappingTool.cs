using System;
using ModStructureHelperPlugin.Editing.Managers;
using ModStructureHelperPlugin.Mono;
using ModStructureHelperPlugin.StructureHandling;
using ModStructureHelperPlugin.UI.Utility;
using ModStructureHelperPlugin.Utility;
using Nautilus.Utility;
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
    [SerializeField] private Button teleportToGridCenterButton;
    
    public override ToolType Type => ToolType.Snapping;

    public override bool MultitaskTool => true;

    private bool _windowOpen;
    private bool _snapBindHeld;

    private GameObject _snapGridPreview;
    
    private void Start()
    {
        CreateSnapGridPreview();
        SetDefaultValues();
        
        if (StructureInstance.Main != null)
        {
            LoadValuesFromStructureMetadata(StructureInstance.Main);
        }

        StructureInstance.OnStructureInstanceChanged += OnStructureInstanceChanged;
    }

    private void OnDestroy()
    {
        StructureInstance.OnStructureInstanceChanged -= OnStructureInstanceChanged;
        Destroy(_snapGridPreview);
    }

    private void SetDefaultValues()
    {
        ErrorMessage.AddMessage("setting defaults");
        positionSnapping.text = "1.0";
        angleSnapping.text = "45";
        useGlobalGridToggle.isOn = false;
        
        gridCenterField.SetValue(Vector3.zero);
        gridRotationField.SetValue(Vector3.zero);

        UpdateInteractability(false);
        
        OnUpdateSnapping();
    }

    private void LoadValuesFromStructureMetadata(StructureInstance instance)
    {
        ErrorMessage.AddMessage("loading");
        if (instance.TryGetMetadata<float>(StructureMetadataKeys.GridSnappingDistanceFloat, out var snappingDistance))
        {
            positionSnapping.text = snappingDistance.ToString("0.#");
        }
        if (instance.TryGetMetadata<float>(StructureMetadataKeys.GridRotationSnappingFloat, out var rotationSnapping))
        {
            angleSnapping.text = rotationSnapping.ToString("#");
        }
        if (instance.TryGetMetadata<Vector3>(StructureMetadataKeys.GridCenterVector, out var gridCenter))
        {
            gridCenterField.SetValue(gridCenter);
            ErrorMessage.AddMessage("loaded center");
        }
        if (instance.TryGetMetadata<Vector3>(StructureMetadataKeys.GridRotationVector, out var gridRotation))
        {
            gridRotationField.SetValue(gridRotation);
        }

        bool hasGlobalGrid =
            instance.TryGetMetadata<bool>(StructureMetadataKeys.GridCenterExistsBool, out var gridCenterExists) &&
            gridCenterExists;
        
        UpdateInteractability(hasGlobalGrid);
        
        OnUpdateSnapping();
    }

    private void UpdateInteractability(bool hasGlobalGrid)
    {
        gridCenterField.SetInteractable(hasGlobalGrid);
        gridRotationField.SetInteractable(hasGlobalGrid);
        teleportToGridCenterButton.interactable = hasGlobalGrid;
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
    
    protected override string GetBindString()
    {
        var binding = GameInput.FormatButton(StructureHelperInput.HoldToSnap);
        return base.GetBindString() + $" (or hold {binding})";
    }

    public void OnUpdateSnapping()
    {
        if (float.TryParse(positionSnapping.text, out var positionSnap))
        {
            manager.snappingManager.SetPositionSnapping(positionSnap);
            if (StructureInstance.Main != null)
                StructureInstance.Main.SaveMetadata(StructureMetadataKeys.GridSnappingDistanceFloat, positionSnap);
        }

        if (float.TryParse(angleSnapping.text, out var rotationSnap))
        {
            manager.snappingManager.SetRotationSnapping(rotationSnap);
            if (StructureInstance.Main != null)
                StructureInstance.Main.SaveMetadata(StructureMetadataKeys.GridRotationSnappingFloat, rotationSnap);
        }
        OnUpdateGlobalGridChanged(useGlobalGridToggle.isOn);
        OnUpdateGridPosition();
        OnUpdateGridRotation();
    }

    public void OnUpdateGridPosition()
    {
        manager.snappingManager.SetGlobalGridCenter(gridCenterField.Value);
        if (StructureInstance.Main != null)
            StructureInstance.Main.SaveMetadata(StructureMetadataKeys.GridCenterVector, gridCenterField.Value);
        UpdateSnappingGridPreview();
    }
    
    public void OnUpdateGridRotation()
    {
        manager.snappingManager.SetGlobalGridRotation(gridRotationField.Value);
        if (StructureInstance.Main != null)
            StructureInstance.Main.SaveMetadata(StructureMetadataKeys.GridRotationVector, gridRotationField.Value);
        UpdateSnappingGridPreview();
    }

    public void OnUpdateGlobalGridChanged(bool uselessBoolean)
    {
        bool useGlobalSnapping = useGlobalGridToggle.isOn;
        if (useGlobalSnapping && StructureInstance.Main != null &&
            !StructureInstance.Main.TryGetMetadata<bool>(StructureMetadataKeys.GridCenterExistsBool, out _))
        {
            var gridCenterUnrounded = MainCamera.camera.transform.position +
                             MainCamera.camera.transform.forward * 5;
            var gridCenterRounded = new Vector3(Mathf.RoundToInt(gridCenterUnrounded.x),
                Mathf.RoundToInt(gridCenterUnrounded.y), Mathf.RoundToInt(gridCenterUnrounded.z));
            gridCenterField.SetValue(gridCenterRounded);
            _snapGridPreview.transform.position = gridCenterRounded;
            StructureInstance.Main.SaveMetadata(StructureMetadataKeys.GridCenterExistsBool, true);
        }
        manager.snappingManager.SetUseGlobalGrid(useGlobalSnapping);
        UpdateSnappingGridPreview();
        UpdateInteractability(useGlobalSnapping);
    }
    
    public void TeleportToGlobalGridCenter()
    {
        Player.main.SetPosition(gridCenterField.Value);
    }
    
    private void UpdateSnappingGridPreview()
    {
        _snapGridPreview.SetActive(manager.snappingManager.UseGlobalGrid);
        _snapGridPreview.transform.position = gridCenterField.Value;
        _snapGridPreview.transform.eulerAngles = gridRotationField.Value;
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
    
    private void OnStructureInstanceChanged(StructureInstance instance)
    {
        if (instance == null)
            SetDefaultValues();
        else
            LoadValuesFromStructureMetadata(instance);
    }

    private void CreateSnapGridPreview()
    {
        var obj = Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("SnapGridPrefab"));
        MaterialUtils.ApplySNShaders(obj, 6);
        obj.SetActive(false);
        obj.AddComponent<TransformableObject>();
        obj.AddComponent<TransformableGridPlane>();
        _snapGridPreview = obj;
    }
}