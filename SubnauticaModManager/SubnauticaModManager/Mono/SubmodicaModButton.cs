namespace SubnauticaModManager.Mono;

internal class SubmodicaModButton : MonoBehaviour
{
    private void Start()
    {
        var button = GetComponent<Button>();
        button.onClick = new Button.ButtonClickedEvent();
        button.onClick.AddListener(() => OnClick());
    }

    private void OnClick()
    {
        var menu = ModManagerMenu.main;
        if (menu == null) return;
        if (menu.UnappliedChanges)
        {
            menu.prompt.Ask(
                "You have unsaved changes. Are you sure you wish to continue?",
                new PromptChoice("Yes", true, () => MenuCreator.HideMenu()),
                new PromptChoice("No")
            );
            return;
        }
        MenuCreator.HideMenu();
    }
}
