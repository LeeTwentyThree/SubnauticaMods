using HarmonyLib;
using Nautilus.Utility;
using UnityEngine;

namespace TheRedPlague.Patches;

[HarmonyPatch(typeof(uGUI_MainMenu))]
public class MainMenuPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(uGUI_MainMenu.Start))]
    public static void StartPostfix()
    {
        var logo = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("RedPlagueLogoPrefab"));
        logo.AddComponent<SkyApplier>().renderers = logo.GetComponentsInChildren<Renderer>();
        logo.transform.position = new Vector3(-25.5f, 1, 40);
        logo.transform.eulerAngles = Vector3.up * 180;
        MaterialUtils.ApplySNShaders(logo);
    }
}