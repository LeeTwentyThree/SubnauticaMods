using HarmonyLib;
using PdaUpgradeChips.MonoBehaviours.UI;
using UnityEngine;
using UnityEngine.UI;

namespace PdaUpgradeChips.Patches;

[HarmonyPatch(typeof(uGUI))]
public static class CreateLeviathanDetectorUIPatch
{
    [HarmonyPatch(nameof(uGUI.Awake))]
    [HarmonyPostfix]
    public static void CreateIcon(uGUI __instance)
    {
        var go = new GameObject("LeviathanDetectorUI");
        go.SetActive(false);
        var rect = go.AddComponent<RectTransform>();
        rect.SetParent(__instance.transform.Find("ScreenCanvas"));
        rect.localPosition = Vector2.zero;
        rect.localEulerAngles = Vector3.zero;
        rect.localScale = Vector3.one;
        rect.anchoredPosition = Vector2.zero;
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.zero;
        rect.offsetMin = new Vector2(20, 20);
        rect.offsetMax = new Vector2(120, 120);
        var image = go.AddComponent<Image>();
        image.raycastTarget = false;
        image.sprite = Plugin.Bundle.LoadAsset<Sprite>("UpgradeIcon_LeviathanDetector");
        var component = go.AddComponent<LeviathanDetectorUI>();
        LeviathanDetectorUI.main = component;
    }
}