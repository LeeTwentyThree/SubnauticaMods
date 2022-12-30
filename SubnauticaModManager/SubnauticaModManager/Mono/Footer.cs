namespace SubnauticaModManager.Mono;

internal class Footer : MonoBehaviour
{
    private void Update()
    {
        var menu = ModManagerMenu.main;
        if (menu == null) return;

        var mustQuit = FileManagement.RestartRequired;

        menu.closeButton.gameObject.SetActive(!mustQuit);
        menu.quitGameButton.gameObject.SetActive(mustQuit);
        menu.restartRequiredText.gameObject.SetActive(mustQuit);
        menu.applyChangesButton.gameObject.SetActive(menu.UnappliedChanges);
    }
}
