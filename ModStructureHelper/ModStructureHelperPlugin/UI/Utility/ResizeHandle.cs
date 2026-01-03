using UnityEngine;
using UnityEngine.EventSystems;

namespace ModStructureHelperPlugin.UI.Utility;

public class ResizeHandle : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private ResizableElement resizableElement;
    [SerializeField] private HandleSide side;

    private void OnValidate()
    {
        if (resizableElement == null)
        {
            resizableElement = GetComponentInParent<ResizableElement>();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        resizableElement.OnPointerDown(eventData, side);
    }

    public void OnDrag(PointerEventData eventData)
    {
        resizableElement.OnDrag(eventData, side);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        resizableElement.OnPointerEnter(side);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        resizableElement.OnPointerExit(side);
    }

    private void OnDisable()
    {
        resizableElement.OnPointerExit(side);
    }
}