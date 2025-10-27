using HarmonyLib;
using ModStructureHelperPlugin.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ModStructureHelperPlugin.Patches;

[HarmonyPatch(typeof(IngameMenu))]
public static class IngameMenuPatches
{
    [HarmonyPatch(nameof(IngameMenu.Awake))]
    [HarmonyPostfix]
    public static void AwakePostfix(IngameMenu __instance)
    {
        var buttonLayout = __instance.transform.Find("Main/ButtonLayout");
        var structuresButton = Object.Instantiate(buttonLayout.transform.Find("ButtonBack"), buttonLayout.GetComponent<RectTransform>());
        structuresButton.transform.SetSiblingIndex(structuresButton.parent.Find("ButtonSave").GetSiblingIndex() + 1);
        structuresButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Edit Structures ({GameInput.FormatButton(StructureHelperInput.ToggleStructureHelperKeyBind)})";
        structuresButton.name = "ButtonEditStructures";
        var button = structuresButton.GetComponent<Button>();
        button.onClick = new Button.ButtonClickedEvent();
        button.onClick.AddListener(StructureHelperUIBuilder.ConstructAndActivateUI);
    }
}