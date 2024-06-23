using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ModStructureHelperPlugin.UI;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Main { get; private set; }
    [SerializeField] private RectTransform tooltipTransform;
    [SerializeField] private TextMeshProUGUI textComponent;

    private readonly List<TooltipTarget> _targets = new();

    private bool _tooltipActive;

    private void Awake()
    {
        Main = this;
        UpdateTooltipActive();
    }

    public void AddTarget(TooltipTarget target)
    {
        if (_targets.Contains(target)) return;
        _targets.Add(target);
        UpdateTooltipActive();
        textComponent.text = _targets[^1].GetTooltipText();
    }
    
    public void RemoveTarget(TooltipTarget target)
    {
        _targets.Remove(target);
        UpdateTooltipActive();
    }

    private void UpdateTooltipActive()
    {
        _tooltipActive = _targets.Count > 0;
        tooltipTransform.gameObject.SetActive(_tooltipActive);
    }
    
    private void Update()
    {
        if (!_tooltipActive)
        {
            return;
        }

        tooltipTransform.localPosition = Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2);

        var selected = _targets[^1];
        if (selected.updateToolTipEachFrame)
        {
            textComponent.text = selected.GetTooltipText();
        }
    }
}