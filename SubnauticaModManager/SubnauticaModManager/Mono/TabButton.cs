namespace SubnauticaModManager.Mono;

internal class TabButton : MonoBehaviour
{
    public Tab.Type tabType;

    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        var spriteState = button.spriteState;
        spriteState.disabledSprite = spriteState.selectedSprite;
        button.spriteState = spriteState;
        button.onClick = new Button.ButtonClickedEvent();
        button.onClick.AddListener(() => OnClick());
    }

    private void OnClick()
    {
        var menu = ModManagerMenu.main;
        if (menu == null) return;
        menu.tabManager.SetTabActive(tabType);
        SoundUtils.PlaySound(UISound.MainMenuButton);
    }

    private void Update()
    {
        var menu = ModManagerMenu.main;
        if (menu == null) return;
        button.interactable = menu.tabManager.ActiveTab.type != tabType;
    }
}