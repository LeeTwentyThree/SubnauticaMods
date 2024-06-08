using UnityEngine;
using UnityEngine.UI;
using ModStructureHelperPlugin.UI.Menus;

namespace ModStructureHelperPlugin.UI;

public static class StructureHelperUIBuilder
{
    public static void ConstructAndActivateUI()
    {
        if (StructureHelperUI.main != null)
        {
            StructureHelperUI.main.gameObject.SetActive(true);
            return;
        }
        IngameMenu.main.Close();
        ConstructUI();
    }
    
    private static void ConstructUI()
    {
        var obj = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("StructureHelperCanvas"));
    }
}