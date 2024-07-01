using System;
using ModStructureHelperPlugin.CableGeneration;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace ModStructureHelperPlugin.Tools;

public class CableGeneratorTool : ToolBase
{
    [SerializeField] private GameObject cableGeneratorMenu;
    [SerializeField] private TMP_InputField cableScaleInputField;
    [SerializeField] private TMP_InputField cableSpacingInputField;
    [SerializeField] private CableBuilder cableBuilder;

    private string _startPointPrefab;
    private string _endPointPrefab;

    private readonly string[] _midPointPrefabs = new string[]
    {
        "69cd7462-7cd2-456c-bfff-50903c391737",
        "94933bb3-0587-4e8d-a38d-b7ec4c859b1a",
        "31f84eba-d435-438c-a58e-f3f7bae8bfbd"
    };

    public override ToolType Type => ToolType.CableGenerator;

    public override bool MultitaskTool => true;

    protected override void OnToolEnabled()
    {
        cableGeneratorMenu.SetActive(true);
    }

    protected override void OnToolDisabled()
    {
        cableGeneratorMenu.SetActive(false);
    }

    private void Awake()
    {
        OnUpdateCableScale();
        OnUpdateCableSpacing();
    }

    public void OnUpdateCableScale()
    {
        if (float.TryParse(cableScaleInputField.text, out var scale))
        {
            cableBuilder.Scale = scale;
        }
    }
    
    public void OnUpdateCableSpacing()
    {
        if (float.TryParse(cableSpacingInputField.text, out var spacing))
        {
            cableBuilder.Spacing = Mathf.Max(spacing, 0.1f);
        }
    }
    
    public void OnButtonGenerateNewCable() => cableBuilder.GenerateNewCable(_startPointPrefab, _midPointPrefabs, _endPointPrefab);
    public void OnButtonAddControlPoint() => cableBuilder.AddControlPoint();
    public void OnButtonRemoveControlPoint() => cableBuilder.RemoveControlPoint();
    public void OnButtonSaveCable() => cableBuilder.SaveCable();
    public void OnButtonDeleteCable() => cableBuilder.DeleteCable();
    
    public void SetCablePrefab(CableLocation location, string classId)
    {
        switch (location)
        {
            case CableLocation.Start:
                _startPointPrefab = classId;
                break;
            case CableLocation.End:
                _endPointPrefab = classId;
                break;
            case CableLocation.Middle:
            default:
                throw new ArgumentOutOfRangeException(nameof(location), location, null);
        }
    }
}