using UnityEngine;
using ModStructureHelperPlugin.UI.Menus;
using UnityEngine.Serialization;

namespace ModStructureHelperPlugin.UI;

public class StructureHelperUI : MonoBehaviour
{
    public static StructureHelperUI main;

    [SerializeField] private uGUI_InputGroup inputGroup;
    
    public StructureMainMenu structureMainMenu;
    public StructureCreationMenu structureCreationMenu;
    public StructureLoadingMenu structureLoadingMenu;
    public StructureEditingMenu structureEditingMenu;

    public PromptHandler promptHandler;

    public static bool IsActive => main != null && main.isActiveAndEnabled;

    private void Awake()
    {
        main = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        inputGroup.Select();
    }

    public void SetMenuActive(MenuType type)
    {
        foreach (var menuType in System.Enum.GetValues(typeof(MenuType)))
        {
            var menu = GetMenu((MenuType)menuType);
            menu.gameObject.SetActive((MenuType)menuType == type);
        }
    }

    public static void SetUIEnabled(bool enabled)
    {
        if (enabled)
        {
            StructureHelperUIBuilder.ConstructAndActivateUI();
        }
        else if (main != null)
        {
            main.gameObject.SetActive(false);
        }
    }

    public StructureHelperMenuBase GetMenu(MenuType type)
    {
        switch (type)
        {
            case MenuType.Main:
                return structureMainMenu;
            case MenuType.StructureCreation:
                return structureCreationMenu;
            case MenuType.Loading:
                return structureLoadingMenu;
            case MenuType.Editing:
                return structureEditingMenu;
        }

        Plugin.Logger.LogError($"Menu not found by type '{type}'!");
        return null;
    }
}

public enum MenuType
{
    Main,
    StructureCreation,
    Loading,
    Editing
}