using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ModStructureHelperPlugin.Tools;

public class CableGeneratorTool : ToolBase
{
    [SerializeField] private GameObject cableGeneratorMenu;
    [SerializeField] private uGUI_InputField cableScaleInputField;
    [SerializeField] private Toggle cableEndPointsToggleStart;
    [SerializeField] private Toggle cableEndPointsToggleEnd;

    private float _cableScale;
    private bool _useEndPointForStart;
    private bool _useEndPointForEnd;

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
        OnUpdateCableEndPoints();
    }

    public void OnUpdateCableScale()
    {
        float.TryParse(cableScaleInputField.text, out _cableScale);
    }
    
    public void OnUpdateCableEndPoints()
    {
        _useEndPointForStart = cableEndPointsToggleStart.isOn;
        _useEndPointForEnd = cableEndPointsToggleEnd.isOn;
    }
}