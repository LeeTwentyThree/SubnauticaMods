namespace ResourceBonanza;

[HarmonyPatch]
internal static class Patches
{
    [HarmonyPatch(typeof(BreakableResource), nameof(BreakableResource.SpawnResourceFromPrefab), new Type[] { typeof(AssetReferenceGameObject) })]
    [HarmonyPrefix]
    public static bool BreakIntoResourcesPatch(BreakableResource __instance, AssetReferenceGameObject breakPrefab)
    {
        Plugin.log.LogInfo("BreakIntoResourcesPatch");
        UWE.CoroutineHost.StartCoroutine(Randomizer.outcropRandomizer.SpawnOutcropItems(__instance, breakPrefab));
        return false;
    }
}