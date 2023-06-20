namespace CreatureMorphs.Patches;

[HarmonyPatch(typeof(GameModeConsoleCommands))]
internal static class GameModeConsoleCommandsPatches
{
    [HarmonyPatch(nameof(GameModeConsoleCommands.OnGameModeChanged))]
    [HarmonyPrefix]
    public static bool OnGameModeChangedPrefix(GameModeOption gameMode)
    {
        return false;
    }
}