using System;
using UnityEngine;
using UnityEngine.UI;

namespace ModStructureHelperPlugin.UI;

public class DisableButtonIfNoStructureExists : MonoBehaviour
{
    [SerializeField] private Button button;

    private void OnValidate()
    {
        if (button == null) button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        StructureInstance.OnStructureInstanceUpdated += OnStructureInstanceUpdated;
        button.interactable = StructureInstance.Main != null;
    }

    private void OnDisable()
    {
        StructureInstance.OnStructureInstanceUpdated -= OnStructureInstanceUpdated;
    }

    private void OnStructureInstanceUpdated(StructureInstance newInstance)
    {
        button.interactable = newInstance != null;
    }
}