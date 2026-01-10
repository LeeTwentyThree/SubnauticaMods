using System.Collections;
using System.IO;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Nautilus.Assets;
using Nautilus.Handlers;
using PodshellLeviathan.Prefabs;
using System.Reflection;
using ECCLibrary;
using Nautilus.Assets.PrefabTemplates;
using UnityEngine;

namespace PodshellLeviathan;
[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus", "1.0.0.47")]
[BepInDependency("com.lee23.ecclibrary", "2.2.0")]
[BepInDependency("com.lee23.kalliesproppack", "1.2.3")]
public class Plugin : BaseUnityPlugin
{
    public new static ManualLogSource Logger { get; private set; }

    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();

    internal static AssetBundle Assets { get; private set;}

    internal static PodshellLeviathanPrefab PodshellLeviathan { get; private set; }
    
    internal static PodshellLeviathanJuvenilePrefab PodshellLeviathanJuvenile { get; private set; }
    internal static PodshellLeviathanBabyPrefab PodshellLeviathanBaby { get; private set; }

    private bool _assetsLoaded;


    private void Awake()
    {
        // set project-scoped logger instance
        Logger = base.Logger;

        LanguageHandler.RegisterLocalizationFolder();
        WaitScreenHandler.RegisterEarlyAsyncLoadTask(PluginInfo.PLUGIN_NAME, LoadModAsync);
        
        // register harmony patches, if there are any
        Harmony.CreateAndPatchAll(Assembly, $"{PluginInfo.PLUGIN_GUID}");
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }

    private IEnumerator LoadModAsync(WaitScreenHandler.WaitScreenTask task)
    {
        if (_assetsLoaded)
            yield break;

        _assetsLoaded = true;
        
        var assetBundleTask = AssetBundle.LoadFromFileAsync(GetAssetBundlePath("podshellleviathan"));
        yield return assetBundleTask;
        Assets = assetBundleTask.assetBundle;
        
        InitializePrefabs();
        ModAudio.RegisterAudio();
        StructureLoading.RegisterStructures(StructureLoading.GetStructuresFolderPath(Assembly));
    }
    
    private static string GetAssetBundlePath(string assetBundleFileName)
    {
        return Path.Combine(Path.GetDirectoryName(Assembly.Location), "Assets", assetBundleFileName);
    }

    private void InitializePrefabs()
    {
        PodshellLeviathan = new PodshellLeviathanPrefab(
            PrefabInfo.WithTechType("PodshellLeviathan", null, null));
        PodshellLeviathan.Register();
        CreatureDataUtils.AddCreaturePDAEncyclopediaEntry(PodshellLeviathan, "Lifeforms/Fauna/Leviathans",
            null, null, 8,
            Assets.LoadAsset<Texture2D>("PodshellEntryImage"),
            Assets.LoadAsset<Sprite>("PodshellPopup"));
        
        PodshellLeviathanJuvenile = new PodshellLeviathanJuvenilePrefab(
            PrefabInfo.WithTechType("PodshellLeviathanJuvenile", null, null));
        PodshellLeviathanJuvenile.Register();
        
        PodshellLeviathanBaby = new PodshellLeviathanBabyPrefab(
            PrefabInfo.WithTechType("PodshellLeviathanBaby", null, null)
                .WithSizeInInventory(new Vector2int(4, 3))
                .WithIcon(Assets.LoadAsset<Sprite>("PodshellBabyIcon")));
        PodshellLeviathanBaby.Register();

        var podshellEggPrefab = new CustomPrefab(PrefabInfo.WithTechType("PodshellLeviathanEgg")
            .WithSizeInInventory(new Vector2int(3, 3))
            .WithIcon(Assets.LoadAsset<Sprite>("PodshellEggIcon")));
        var eggTemplate = new EggTemplate(podshellEggPrefab.Info,
                Assets.LoadAsset<GameObject>("PodshellEgg"))
            .WithMass(75)
            .WithCellLevel(LargeWorldEntity.CellLevel.Medium)
            .WithHatchingCreature(PodshellLeviathanBaby.PrefabInfo.TechType)
            .WithHatchingTime(1.5f)
            .WithMaxHealth(300)
            .SetUndiscoveredTechType();
        eggTemplate.ModifyPrefab = egg =>
        {
            var material = egg.GetComponentInChildren<Renderer>().material;
            material.color = new Color(0.5f, 0.5f, 0.5f);
            material.SetFloat("_SpecInt", 10);
            material.SetFloat("_Shininess", 6.2f);
            material.SetFloat("_Fresnel", 0);
        };
        podshellEggPrefab.SetGameObject(eggTemplate);
        podshellEggPrefab.Register();
        
        PodshellNestRock.Register();
    }
}