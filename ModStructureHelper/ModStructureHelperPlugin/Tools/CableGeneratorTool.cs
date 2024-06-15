using UnityEngine;

namespace ModStructureHelperPlugin.Tools;

public class CableGeneratorTool : ToolBase
{
    [SerializeField] private GameObject cableGeneratorMenu;

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
}