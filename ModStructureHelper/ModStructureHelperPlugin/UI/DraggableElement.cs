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
        var mouseDelta = (Input.mousePosition - _previousMousePosition) * Time.deltaTime;
        _previousMousePosition = Input.mousePosition;
        if (!_draggin) return;
        var position = transform.position;
        position = new Vector3(
            Mathf.Clamp(position.x + mouseDelta.x, -Screen.width / 2, Screen.width / 2),
            Mathf.Clamp(position.y + mouseDelta.y, -Screen.height / 2, Screen.height / 2),
            position.z);
        transform.position = position;
    }
}