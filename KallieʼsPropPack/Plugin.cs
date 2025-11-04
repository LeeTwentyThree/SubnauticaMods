using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using KallieʼsPropPack.Prefabs.Plants;
using KallieʼsPropPack.Prefabs.Trees;
using Nautilus.Handlers;

namespace KallieʼsPropPack;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus")]
public class Plugin : BaseUnityPlugin
{
    public new static ManualLogSource Logger { get; private set; }

    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();

    private void Awake()
    {
        Logger = base.Logger;

        WaitScreenHandler.RegisterEarlyLoadTask(PluginInfo.PLUGIN_NAME, InitializePrefabs);

        Harmony.CreateAndPatchAll(Assembly, $"{PluginInfo.PLUGIN_GUID}");
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }

    private void InitializePrefabs(WaitScreenHandler.WaitScreenTask task)
    {
        // Register purple pine tree
        PurplePineTree.Register();

        // Register kelp roots
        (string classId, string originalClassId)[] kelpRootPrefabs =
        {
            ("Coral_reef_cave_root_small_01_kelp", "4bfe1877-1b83-4d5d-9470-3bb2d5f389cc"),
            ("Coral_reef_cave_root_03_kelp_Light", "5beba896-bccf-4993-8bcb-1cdabb68e706"),
            ("Coral_reef_cave_root_small_02_kelp", "a0c5b949-22a4-4899-9c51-64ccce6956bc"),
            ("Coral_reef_cave_root_small_03_kelp", "a0cbac2e-f86d-4ab0-a090-8115f5196f7c"),
            ("Coral_reef_cave_root_01_kelp", "abe572e9-126b-43eb-bf5c-4edf9ec365c1"),
            ("Coral_reef_cave_root_02_kelp_Light", "b0cae640-b155-4bac-9ed5-29ba64a1ee9f"),
            ("Coral_reef_cave_root_small_05_kelp", "cd004d89-f798-40d0-bf65-ee4c1c48700c"),
            ("Coral_reef_cave_root_small_04_kelp", "d8efe522-5355-48b8-b4fb-4d077bbc621d"),
            ("Coral_reef_cave_root_01_kelp_Light", "da7341c3-e6a3-4cd3-ad57-49a4dc732ac9"),
            ("Coral_reef_cave_root_04_kelp_Light", "db79ee0b-65e9-4ea1-8b8b-948bbae128f7"),
            ("Coral_reef_cave_root_03_kelp", "e3fd373d-6ecc-497a-b396-816f3cb5f9b6"),
            ("Coral_reef_cave_root_04_kelp", "e40daa31-8eb8-463a-b91a-d3aedb631744"),
            ("Coral_reef_cave_root_02_kelp", "f0a54d9a-7717-473f-8450-5ff24824ed7e")
        };

        foreach (var kelpRootData in kelpRootPrefabs)
        {
            var kelpRootPrefab = new KelpRoot(kelpRootData.classId, kelpRootData.originalClassId);
            kelpRootPrefab.Register();
        }
    }
}