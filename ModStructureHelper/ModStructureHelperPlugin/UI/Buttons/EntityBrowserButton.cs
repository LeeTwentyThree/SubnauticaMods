using ModStructureHelperPlugin;
using ModStructureHelperPlugin.EntityHandling;
using ModStructureHelperPlugin.Tools;
using ModStructureHelperPlugin.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntityBrowserButton : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI text;
    public RectTransform rectTransform;

    private EntityBrowserEntryBase browserEntry;

    public void OnInteract()
    {
        browserEntry?.OnInteract();
    }

    public void OnBrushButtonPressed()
    {
        if (browserEntry is not EntityBrowserEntity entityEntry)
        {
            Plugin.Logger.LogError($"Cannot use a non-entity entry as a brush!");
            return;
        }

        var paintTool = StructureHelperUI.main.toolManager.GetTool(ToolType.PaintBrush) as PaintTool;
        paintTool.SetCurrentBrushEntity(entityEntry.EntityData.ClassId);
    }

    public void SetBrowserEntry(EntityBrowserEntryBase browserEntry)
    {
        this.browserEntry = browserEntry;
        if (browserEntry != null)
        {
            if (image) image.sprite = browserEntry.Sprite;
            text.text = browserEntry.Name;
        }
    }

    public EntityBrowserEntryBase GetBrowserEntry() => browserEntry;
}
