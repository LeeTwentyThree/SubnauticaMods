using UnityEngine;
using UnityEngine.UI;

namespace ModStructureHelperPlugin.UI;

public class DisableButtonIfNoStructureExists : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private GameObject objectToDisable;
    [SerializeField] private Mode mode;
    [SerializeField] private bool inverted;

    private void OnValidate()
    {
        if (button == null && mode == Mode.DisableButton) button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        StructureInstance.OnStructureInstanceChanged += OnStructureInstanceChanged;
        OnStructureInstanceChanged(StructureInstance.Main);
    }

    private void OnDisable()
    {
        StructureInstance.OnStructureInstanceChanged -= OnStructureInstanceChanged;
    }

    private void OnStructureInstanceChanged(StructureInstance newInstance)
    {
        var shouldEnable = newInstance != null;
        if (inverted) shouldEnable = !shouldEnable;
        if (mode == Mode.DisableButton)
            button.interactable = shouldEnable;
        else objectToDisable.SetActive(shouldEnable);
    }

    private enum Mode
    {
        DisableButton,
        DisableObject
    }
}