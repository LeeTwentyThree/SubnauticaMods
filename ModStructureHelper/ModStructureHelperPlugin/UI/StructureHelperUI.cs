using ModStructureHelperPlugin.Editing.Managers;
using ModStructureHelperPlugin.Editing.Tools;
using UnityEngine;
using ModStructureHelperPlugin.UI.Menus;

namespace ModStructureHelperPlugin.UI;

public class StructureHelperUI : MonoBehaviour
{
    public static StructureHelperUI main;

    [SerializeField] private uGUI_InputGroup inputGroup;
    
    public StructureMainMenu structureMainMenu;
    public StructureCreationMenu structureCreationMenu;
    public StructureLoadingMenu structureLoadingMenu;
    public StructureEditingMenu structureEditingMenu;
    public TooltipManager tooltipManager;
    public EditingScreenChecker editingScreenChecker;

    public ToolManager toolManager;
    public CursorOverrideManager cursorManager;

    public PromptHandler promptHandler;

    public static bool IsActive => main != null && main.isActiveAndEnabled;

    public bool IsFocused => inputGroup.focused;
    public bool IsCursorHoveringOverExternalWindows => editingScreenChecker.IsCursorHoveredOverExternalWindows();

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        SetMenuActive(MenuType.Main);
    }

    private void Update()
    {
        if (GameInput.GetButtonDown(GameInput.Button.UIMenu))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        inputGroup.enabled = true;
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

    public void SetInputGroupOverride(bool active)
    {
        inputGroup.enabled = active;
        if (active) inputGroup.Select();
    }
}

public enum MenuType
{
    Main,
    StructureCreation,
    Loading,
    Editing
}