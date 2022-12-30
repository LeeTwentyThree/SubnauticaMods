namespace SubnauticaModManager.Mono;

internal class ApplyChangesButton : MonoBehaviour
{
    private void Start()
    {
        var button = GetComponent<Button>();
        button.onClick = new Button.ButtonClickedEvent();
        button.onClick.AddListener(() => OnClick() );
    }

    private void OnClick()
    {
        if (ModManagerMenu.main == null) return;
        ModManagerMenu.main.prompt.Ask(
            "Do you want to apply these changes?",
            new PromptChoice("Yes", () => Yes()),
            new PromptChoice("No")
            );
    }

    private void Yes()
    {
        ModManagerMenu.main.MarkAllChangesSaved();
        SoundUtils.PlaySound(UISound.Select);
    }
}
