using ModStructureHelperPlugin.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ModStructureHelperPlugin.UI;

public class OverlayIconInstance : MonoBehaviour
{
    public OverlayIconManager manager;
    
    public Image iconImage;
    public TextMeshProUGUI labelText;

    private IOverlayIconData _data;

    public void OnCreate(IOverlayIconData iconData)
    {
        _data = iconData;
        iconData.OnCreation(this);
        UpdateData(true);
    }

    private void UpdateData(bool firstTime)
    {
        SetPositionAndScale(SNCameraRoot.main.mainCam.WorldToScreenPoint(_data.Position));
        labelText.text = _data.Label;
        iconImage.sprite = _data.Icon;
    }

    private void SetPositionAndScale(Vector3 screenPoint)
    {
        var x = screenPoint.x - (float)Screen.width / 2;
        var y = screenPoint.y - (float)Screen.height / 2;
        transform.localPosition = new Vector3(x, y, 0f);
        transform.localScale = Vector3.one * _data.Scale;
    }

    private void Update()
    {
        UpdateData(false);
    }
}