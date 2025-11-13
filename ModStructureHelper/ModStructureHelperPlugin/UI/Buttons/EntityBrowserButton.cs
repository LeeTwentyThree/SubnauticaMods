using ModStructureHelperPlugin.Editing.Tools;
using ModStructureHelperPlugin.EntityHandling;
using ModStructureHelperPlugin.EntityHandling.Icons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ModStructureHelperPlugin.UI.Buttons;

public class EntityBrowserButton : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI text;
    public RectTransform rectTransform;
    public TooltipTarget tooltipTarget;

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

    public void SetBrowserEntry(EntityBrowserEntryBase entry)
    {
        browserEntry = entry;
        if (entry != null)
        {
            SetIcon(entry.Icon);
            text.text = entry.Name;
        }
    }

    public void SetIcon(EntityIcon icon)
    {
        if (image == null) return;
        if (icon != null)
        {
            image.sprite = icon.Sprite;
            image.color = icon.ColorMultiplier;
        }
        else
        {
            Plugin.Logger.LogWarning("Failed to find icon for entity: " + browserEntry.Name);
        }
    }

    public EntityBrowserEntryBase GetBrowserEntry() => browserEntry;
}