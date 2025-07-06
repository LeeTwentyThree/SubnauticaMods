using Nautilus.FMod;
using Nautilus.Utility;

namespace SeaVoyager;

public static class AudioRegistry
{
    public static void RegisterAudio()
    {
        var soundBuilder = new FModSoundBuilder(new AssetBundleSoundSource(Plugin.assetBundle));
        soundBuilder.CreateNewEvent("SuspendedDockMove", AudioUtils.BusPaths.SFX)
            .SetSound("SuspendedDockMove")
            .SetMode3D(1f, 14f)
            .Register();
        soundBuilder.CreateNewEvent("SeaVoyagerEnter", AudioUtils.BusPaths.SFX)
            .SetSound("SeaVoyagerEnter")
            .SetMode3D(2f, 14f)
            .Register();
        soundBuilder.CreateNewEvent("SeaVoyagerExit", AudioUtils.BusPaths.SFX)
            .SetSound("SeaVoyagerExit")
            .SetMode3D(2f, 14f)
            .Register();
        soundBuilder.CreateNewEvent("SvSlidingDoorOpen", AudioUtils.BusPaths.SFX)
            .SetSound("SlidingDoorOpen")
            .SetMode3D(1f, 14f)
            .Register();
        soundBuilder.CreateNewEvent("SvSlidingDoorClose", AudioUtils.BusPaths.SFX)
            .SetSound("SlidingDoorClose")
            .SetMode3D(1f, 14f)
            .Register();
        soundBuilder.CreateNewEvent("SvButtonPress", AudioUtils.BusPaths.SFX)
            .SetSound("ButtonPress")
            .SetMode2D()
            .Register();

        var voiceFileNames = new[]
        {
            "ahead_flank", "ahead_slow", "ahead_standard", "approaching_shallow_water", "engine_powering_down",
            "engine_powering_up", "first_use", "reserve_power_empty", "seavoyager_main", "sonar_map_activated",
            "updating_regional_map", "vehicle_attached", "vehicle_docked_successfully", "vehicle_released",
            "welcome_aboard_captain"
        };

        foreach (var voice in voiceFileNames)
        {
            soundBuilder.CreateNewEvent("seavoyager_" + voice, AudioUtils.BusPaths.CyclopsVoice)
                .SetSound(voice)
                .SetMode2D()
                .Register();
        }
    }
}