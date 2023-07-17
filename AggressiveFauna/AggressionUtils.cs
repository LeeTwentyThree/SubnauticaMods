namespace AggressiveFauna;

internal static class AggressionUtils
{
    public static bool PlayerCanBeTargeted(Player player)
    {
        if (GameModeUtils.IsInvisible()) return false;
        if (player.justSpawned) return false;
        if (!AggressionSettings.CanSeeInsideBases && Player.main.IsInsideWalkable()) return false;
        return true;
    }
}
