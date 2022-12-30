namespace SubnauticaModManager.Mono;

internal class QuitGameButton : MonoBehaviour
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
                new PromptChoice("Yes", true, () => QuitGame()),
                new PromptChoice("No")
            );
            return;
        }
        menu.prompt.Ask(
                "Quit to desktop?",
                new PromptChoice("Yes", false, () => QuitGame()),
                new PromptChoice("No")
            );
    }

    private void QuitGame()
    {
        SoundUtils.PlaySound(UISound.BatteryDie);
        Application.Quit();
    }
}