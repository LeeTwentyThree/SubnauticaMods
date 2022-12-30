namespace SubnauticaModManager.Mono;

internal class TabManager : MonoBehaviour
{
    private Tab[] tabs;

    public Tab ActiveTab { get; private set; }

    private void Start()
    {
        tabs = gameObject.GetComponentsInChildren<Tab>(true);

        SetTabActive(Tab.Type.Manage);
    }

    public Tab GetTab(Tab.Type type)
    {
        foreach (var tab in tabs)
            if (tab.type == type)
                return tab;
        return null;
    }

    public void SetTabActive(Tab.Type type)
    {
        ActiveTab = GetTab(type);
        foreach (Tab tab in tabs)
        {
            tab.gameObject.SetActive(tab.type == type);
        }
    }
}