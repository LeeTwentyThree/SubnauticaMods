using System;
using TMPro;
using UnityEngine;

namespace ModStructureHelperPlugin.Tools;

public class CableGeneratorTool : ToolBase
{
    [SerializeField] private GameObject cableGeneratorMenu;
    [SerializeField] private TMP_InputField cableScaleInputField;

    private float _cableScale;
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
    }

    public void OnUpdateCableScale()
    {
        float.TryParse(cableScaleInputField.text, out _cableScale);
    }

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