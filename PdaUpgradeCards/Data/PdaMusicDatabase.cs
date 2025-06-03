using System.Collections.Generic;

namespace PdaUpgradeCards.Data;

public static class PdaMusicDatabase
{
    public static IReadOnlyList<PdaMusic> MusicList { get; } = new[]
    {
        new PdaMusic("CyclopsMusic", "event:/env/music/firefighting_music"),
        new PdaMusic("DeathMusic", "event:/env/music/death_music")
    };
}