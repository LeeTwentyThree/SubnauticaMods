using ModStructureHelperPlugin.Editing.Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ModStructureHelperPlugin.UI.Utility;

public class ResizableElement : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Vector2 minimumSize;
    
    private Vector2 _startMousePosition;
    private Vector2 _startSize;
    private Vector2 _startPosition;
    
    private void OnValidate()
    {
        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
        }
    }

    public void OnPointerDown(PointerEventData eventData, HandleSide side)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out _startMousePosition
        );

        _startSize = rectTransform.sizeDelta;
        _startPosition = rectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData, HandleSide side)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out var currentMousePosition
        );

        var delta = currentMousePosition - _startMousePosition;

        var size = _startSize;
        var pos = _startPosition;

        if (side.HasFlag(HandleSide.Right))
        {
            size.x += delta.x;
        }
        if (side.HasFlag(HandleSide.Left))
        {
            size.x -= delta.x;
        }

        if (side.HasFlag(HandleSide.Top))
        {
            size.y += delta.y;
        }
        if (side.HasFlag(HandleSide.Bottom))
        {
            size.y -= delta.y;
        }

        size.x = Mathf.Max(size.x, minimumSize.x);
        size.y = Mathf.Max(size.y, minimumSize.y);
        
        var sizeDelta = size - _startSize;
        if (side.HasFlag(HandleSide.Bottom))
        {
            sizeDelta = new Vector2(sizeDelta.x, -sizeDelta.y);
        }
        if (side.HasFlag(HandleSide.Left))
        {
            sizeDelta = new Vector2(-sizeDelta.x, sizeDelta.y);
        }
        pos += sizeDelta * 0.5f;

        rectTransform.sizeDelta = size;
        rectTransform.anchoredPosition = pos;
    }
    
    public void OnPointerEnter(HandleSide side)
    {
        CursorOverrideManager.CustomCursor cursor;
        switch (side)
        {
            case HandleSide.Left:
            case HandleSide.Right:
                cursor = CursorOverrideManager.CustomCursor.Horizontal;
                break;
            case HandleSide.Top:
            case HandleSide.Bottom:
                cursor = CursorOverrideManager.CustomCursor.Vertical;
                break;
            case HandleSide.TopLeft:
            case HandleSide.BottomRight:
                cursor = CursorOverrideManager.CustomCursor.DownwardDiagonal;
                break;
            case HandleSide.BottomLeft:
            case HandleSide.TopRight:
                cursor = CursorOverrideManager.CustomCursor.UpwardDiagonal;
                break;
            default:
                cursor = CursorOverrideManager.CustomCursor.None;
                break;
        }
        StructureHelperUI.main.cursorManager.SetCursor(cursor);
    }

    public void OnPointerExit(HandleSide side)
    {
        StructureHelperUI.main.cursorManager.SetCursor(CursorOverrideManager.CustomCursor.None);
    }
}