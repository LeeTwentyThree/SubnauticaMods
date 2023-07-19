using HarmonyLib;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BlueprintSearchBar;

[HarmonyPatch]
public static class Patches
{
    private static TMP_FontAsset font;

    [HarmonyPatch(typeof(uGUI_BlueprintsTab), nameof(uGUI_BlueprintsTab.Awake))]
    [HarmonyPostfix]
    public static void BlueprintsTabAwakePatch(uGUI_BlueprintsTab __instance)
    {
        var contentParent = __instance.transform.Find("Content").GetComponent<RectTransform>();
        var outlineImage = contentParent.Find("ButtonPinsClear/Outline").gameObject;

        font = contentParent.Find("ButtonPinsClear/Text").GetComponent<TextMeshProUGUI>().font;

        var outline = GameObject.Instantiate(outlineImage);

        var outlineRect = outline.GetComponent<RectTransform>();
        outlineRect.SetParent(contentParent);
        outlineRect.anchorMin = new Vector2(0, 0);
        outlineRect.anchorMax = new Vector2(0, 0);
        outlineRect.pivot = new Vector2(0, 0.5f);
        outlineRect.anchoredPosition = new Vector2(125, 800);
        outlineRect.sizeDelta = new Vector2(360, 43);
        outlineRect.localScale = Vector3.one;
        outlineRect.localPosition = new Vector3(-472.5f, 310f, 0f);

        var inputField = outline.AddComponent<TMP_InputField>();
        var normalText = CreateTextChild(outline, "Text");
        inputField.textComponent = normalText;
        normalText.fontStyle = FontStyles.UpperCase;
        var placeholderText = CreateTextChild(outline, "Placeholder");
        placeholderText.fontStyle = FontStyles.Italic | FontStyles.UpperCase | FontStyles.Bold;
        placeholderText.text = "Search for blueprints...";
        placeholderText.color = new Color(0.3f, 0.867f, 1f);
        inputField.placeholder = placeholderText;
        outline.GetComponent<Image>().raycastTarget = true;
        inputField.targetGraphic = placeholderText;

        inputField.enabled = false;
        inputField.enabled = true;

        outline.AddComponent<SearchBarBehaviour>();
    }

    private static TextMeshProUGUI CreateTextChild(GameObject outline, string name)
    {
        GameObject textObj = new GameObject(name);
        textObj.transform.SetParent(outline.GetComponent<RectTransform>());
        var transform = textObj.EnsureComponent<RectTransform>();
        transform.pivot = new Vector2(0.5f, 1f);
        transform.anchorMin = new Vector2(0.9f, 1);
        transform.anchorMax = new Vector2(1, 1);
        transform.localScale = Vector3.one;
        transform.localPosition = new Vector3(185, 10, 0);
        transform.sizeDelta = new Vector2(295, 50);

        var text = textObj.AddComponent<TextMeshProUGUI>();
        text.font = font;
        text.fontSize = 20f;

        return text;
    }

    [HarmonyPatch(typeof(uGUI_BlueprintsTab), nameof(uGUI_BlueprintsTab.UpdateOrder), new System.Type[0])]
    [HarmonyPostfix]
    public static void BlueprintsTabUpdateOrderPatch(uGUI_BlueprintsTab __instance)
    {
        var main = SearchBarBehaviour.main;
        main?.Refresh();
    }
}