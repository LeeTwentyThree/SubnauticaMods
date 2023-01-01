namespace SubnauticaModManager;

using Mono;

internal static class MenuCreator
{
    public static ModManagerMenu MenuInstance { get; private set; }

    public static bool MenuExists => MenuInstance != null;

    public static void ShowMenu()
    {
        if (MenuExists) return;
        MenuInstance = InstantiateMenu();
        SetModMenuShownState(true);
    }

    private static ModManagerMenu InstantiateMenu()
    {
        // setup gameobject
        var menuObject = Object.Instantiate(Plugin.assetBundle.LoadAsset<GameObject>("ModManagerCanvas"));
        var menuComponent = menuObject.AddComponent<ModManagerMenu>();

        // fix existing components
        Helpers.FixUIObjects(menuObject);
        Object.Destroy(menuObject.GetComponent<GraphicRaycaster>());
        menuObject.EnsureComponent<uGUI_GraphicRaycaster>();
        Object.Destroy(menuObject.GetComponent<CanvasScaler>());
        menuObject.EnsureComponent<uGUI_CanvasScaler>();

        // add essential components
        menuComponent.mainHeader = menuObject.SearchChild("MainHeader").AddComponent<MainHeader>();
        menuComponent.closeButton = menuObject.SearchChild("CloseButton").AddComponent<CloseButton>();
        menuComponent.applyChangesButton = menuObject.SearchChild("ApplyChangesButton").AddComponent<ApplyChangesButton>();
        menuComponent.quitGameButton = menuObject.SearchChild("QuitGameButton").AddComponent<QuitGameButton>();
        menuComponent.restartRequiredText = menuObject.SearchChild("RestartRequiredText").AddComponent<RestartRequiredText>();
        menuComponent.footer = menuObject.SearchChild("Footer").AddComponent<Footer>();
        menuComponent.prompt = menuObject.SearchChild("Prompt").AddComponent<PromptMenu>();
        menuComponent.loadingPrompt = menuObject.SearchChild("LoadingPrompt").AddComponent<LoadingPrompt>();

        menuObject.SearchChild("TestArrangement").AddComponent<TestArrangementButton>();

        // tabs
        menuComponent.tabManager = menuObject.SearchChild("TabRoot").AddComponent<TabManager>();
        menuObject.SearchChild("InstallTab").AddComponent<Tab>().type = Tab.Type.Install;
        menuObject.SearchChild("ManageTab").AddComponent<TabModManagement>().type = Tab.Type.Manage;
        menuObject.SearchChild("DownloadTab").AddComponent<TabDownloadMods>().type = Tab.Type.Download;

        // tab buttons
        menuObject.SearchChild("InstallTabButton").AddComponent<TabButton>().tabType = Tab.Type.Install;
        menuObject.SearchChild("ManageTabButton").AddComponent<TabButton>().tabType = Tab.Type.Manage;
        menuObject.SearchChild("DownloadTabButton").AddComponent<TabButton>().tabType = Tab.Type.Download;

        return menuComponent;
    }

    public static void HideMenu()
    {
        if (!MenuExists) return;
        MenuInstance.Hide();
        SetModMenuShownState(false);
        SoundUtils.PlaySound(UISound.Select);
    }

    private static void SetModMenuShownState(bool shown)
    {
        SetMainMenuShown(!shown);
    }

    private static void SetMainMenuShown(bool shown)
    {
        var mainMenu = uGUI_MainMenu.main ?? Object.FindObjectOfType<uGUI_MainMenu>();
        if (mainMenu == null) return;
        mainMenu.gameObject.SetActive(shown);
    }
}