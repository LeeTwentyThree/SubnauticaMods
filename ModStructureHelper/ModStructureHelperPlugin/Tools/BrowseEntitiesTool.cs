using UnityEngine;

namespace ModStructureHelperPlugin.Tools;

public class BrowseEntitiesTool : ToolBase
{
    [SerializeField] private GameObject entityBrowser;

    public override ToolType Type => ToolType.BrowseEntities;

    public override bool MultitaskTool => true;

    protected override void OnToolEnabled()
    {
        entityBrowser.SetActive(true);
    }

    protected override void OnToolDisabled()
    {
        entityBrowser.SetActive(false);
    }
}