using ModStructureHelperPlugin.EntityHandling;
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
