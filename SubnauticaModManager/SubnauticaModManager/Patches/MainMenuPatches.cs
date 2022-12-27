namespace SubnauticaModManager.Patches;

[HarmonyPatch(typeof(uGUI_MainMenu))]
internal class MainMenuPatches
{
    [HarmonyPatch(nameof(uGUI_MainMenu.Awake))]
    [HarmonyPostfix]
    private static void AwakePatch(uGUI_MainMenu __instance)
    {
        var playButton = __instance.gameObject.GetComponentInChildren<MainMenuPrimaryOptionsMenu>().transform.Find("MenuButtons/ButtonPlay").gameObject;
        var modManagerButton = Object.Instantiate(playButton, playButton.transform.parent);
        var text = modManagerButton.GetComponentInChildren<TextMeshProUGUI>();
        text.text = "Mod Manager";
        Object.Destroy(text.gameObject.GetComponent<TranslationLiveUpdate>());
    }
}
