using System.Collections;
using HarmonyLib;
using Nautilus.Utility;
using UnityEngine;

namespace EpicImprovedTextures.Patches;

[HarmonyPatch(typeof(uGUI_MainMenu))]
public static class MainMenuPatch
{
    [HarmonyPatch(nameof(uGUI_MainMenu.Start))]
    [HarmonyPostfix]
    public static void MainMenuStartPostfix()
    {
        var textureUpdaterObject = new GameObject("TextureUpdater");
        textureUpdaterObject.AddComponent<PeriodicallyUpdateRenderers>();

        var catPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        catPlane.transform.position = new Vector3(0, 6, 35);
        catPlane.transform.eulerAngles = new Vector3(90, 180, 0);
        catPlane.transform.localScale = Vector3.one * 2;
        // unnecessarily rename the object
        catPlane.name = "main cat plane lol";
        TextureUtils.ConvertRenderer(catPlane.GetComponent<Renderer>(), TextureDatabase.GetInstance());

        var catPlane2 = GameObject.CreatePrimitive(PrimitiveType.Plane);
        catPlane2.transform.position = new Vector3(24, 6, 36);
        catPlane2.transform.eulerAngles = new Vector3(90, 180, 0);
        catPlane2.transform.localScale = Vector3.one * 3;
        // unnecessarily rename the object
        catPlane2.name = "second cat plane lol";
        TextureUtils.ConvertRenderer(catPlane2.GetComponent<Renderer>(), TextureDatabase.GetInstance());

        for (var i = 0; i < 100; i++)
        {
            SpawnIrrelevantCatPlane();
        }

        UWE.CoroutineHost.StartCoroutine(AddSusText());
    }

    private static void SpawnIrrelevantCatPlane()
    {
        var catPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        catPlane.transform.position = new Vector3(Random.Range(-100, 100), Random.Range(3, 8), Random.Range(36, 300));
        catPlane.transform.eulerAngles = new Vector3(90, 180, 0);
        catPlane.transform.localScale = Vector3.one * Random.Range(0.5f, 3);
        // unnecessarily rename the object
        catPlane.name = "irrelevant random cat plane";
        TextureUtils.ConvertRenderer(catPlane.GetComponent<Renderer>(), TextureDatabase.GetInstance());
    }

    private static IEnumerator AddSusText()
    {
        yield return new WaitForSecondsRealtime(5);
        var menuLogo = Object.FindObjectOfType<MenuLogo>();
        if (menuLogo == null) yield break;
        var logo = Object.Instantiate(menuLogo.transform.GetChild(0).gameObject);
        logo.transform.localPosition = new Vector3(11, 0, 39);
        logo.transform.localEulerAngles = new Vector3(278, 180, 0);
    }
}