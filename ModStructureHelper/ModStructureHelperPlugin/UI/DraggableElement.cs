using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ModStructureHelperPlugin.UI;

public class DraggableElement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform rectTransform;
    private bool _draggin;
    private Vector3 _previousMousePosition;

    private void OnValidate()
    {
        if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _draggin = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _draggin = false;
    }

    private void OnEnable()
    {
        _previousMousePosition = Input.mousePosition;
    }

    private void OnDisable()
    {
        _draggin = false;
    }

    private void Update()
    {
        var mouseDelta = Input.mousePosition - _previousMousePosition;
        _previousMousePosition = Input.mousePosition;
        if (!_draggin) return;
        var position = transform.position;
        position = new Vector3(
            Mathf.Clamp(position.x + mouseDelta.x, 0, Screen.width),
            Mathf.Clamp(position.y + mouseDelta.y, 0, Screen.height),
            position.z);
        transform.position = position;
    }
}