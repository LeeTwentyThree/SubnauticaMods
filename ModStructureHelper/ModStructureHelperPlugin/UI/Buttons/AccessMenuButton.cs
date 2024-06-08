using UnityEngine;

namespace ModStructureHelperPlugin.UI.Buttons;

public class AccessMenuButton : MonoBehaviour
{
    [SerializeField] private StructureHelperUI structureHelperUI;
    [SerializeField] private MenuType menuType;
    
    private void OnValidate()
    {
        if (structureHelperUI == null) structureHelperUI = GetComponentInParent<StructureHelperUI>();
    }

    public void OnClick() => structureHelperUI.SetMenuActive(menuType);
}