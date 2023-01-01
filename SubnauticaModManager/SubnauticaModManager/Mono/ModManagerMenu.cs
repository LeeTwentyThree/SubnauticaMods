namespace SubnauticaModManager.Mono;

internal class ModManagerMenu : MonoBehaviour
{
    public static ModManagerMenu main;

    public MainHeader mainHeader;
    public CloseButton closeButton;
    public ApplyChangesButton applyChangesButton;
    public QuitGameButton quitGameButton;
    public RestartRequiredText restartRequiredText;
    public Footer footer;
    public TabManager tabManager;
    public PromptMenu prompt;
    public LoadingPrompt loadingPrompt;

    public bool UnappliedChanges { get; private set; }

    public void NotifyUnsavedChanges() => UnappliedChanges = true;

    public void MarkAllChangesSaved() => UnappliedChanges = false;

    private void Awake()
    {
        main = this;
    }

    public void Hide()
    {
        Destroy(gameObject);
    }
}
