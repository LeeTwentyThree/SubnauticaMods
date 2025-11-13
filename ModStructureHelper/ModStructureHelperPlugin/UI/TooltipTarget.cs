using UnityEngine;
using UnityEngine.EventSystems;

namespace ModStructureHelperPlugin.UI;

public class TooltipTarget : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string defaultTooltipText;
    public bool updateToolTipEachFrame;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isActiveAndEnabled)
            TooltipManager.Main.AddTarget(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Main.RemoveTarget(this);
    }

    protected virtual void OnDisable()
    {
       TooltipManager.Main.RemoveTarget(this);
    }

    public virtual string GetTooltipText()
    {
        return defaultTooltipText;
    }
}