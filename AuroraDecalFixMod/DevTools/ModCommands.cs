using System;
using System.Collections;
using System.IO;
using BepInEx.Logging;
using UWE;

namespace AuroraDecalFixMod.DevTools;

public static class ModCommands
{
    private static readonly (string,Func<String, bool>)[] PrefabSearchRules =
    {
        ("WorldEntities/Doodads/Debris/Aurora", _ => true),
        ("WorldEntities/Doodads/Debris/Wrecks", str => str.Contains("aurora"))
    };
    
    public static void GenerateEssentialDataCommand()
    {
        CoroutineHost.StartCoroutine(GenerateAndSaveDataCoroutine());
    }

    private static IEnumerator GenerateAndSaveDataCoroutine()
    {
        Plugin.Logger.Log(LogLevel.Info, "Generating essential data!");
        string filePath = Path.Combine(Path.GetDirectoryName(Plugin.Assembly.Location), "ShadowsDisableData.json");
        yield return PrefabDecalScraper.ScrapeGameForPrefabs(PrefabSearchRules, filePath);
        Plugin.Logger.Log(LogLevel.Info, string.Format("Essential data saved at path '{0}'!", filePath));
    }
}