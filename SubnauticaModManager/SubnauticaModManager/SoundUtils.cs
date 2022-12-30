using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubnauticaModManager;

internal class SoundUtils
{
    private static Dictionary<UISound, FMODAsset> uiSounds = new Dictionary<UISound, FMODAsset>()
    {
        { UISound.Tweak, Get("event:/interface/option_tweek") },
        { UISound.Select, Get("event:/interface/select") },
        { UISound.MainMenuButton, Get("event:/sub/seamoth/select") },
        { UISound.Type, Get("event:/interface/text_type") },
        { UISound.BatteryDie, Get("event:/tools/battery_die") }
    };

    private static FMODAsset Get(string eventPath) => Helpers.GetFmodAsset(eventPath);

    public static void PlaySound(UISound sound)
    {
        Utils.PlayFMODAsset(uiSounds[sound], Camera.current.transform.position);
    }
}
public enum UISound
{
    Tweak,
    Select,
    MainMenuButton,
    Type,
    BatteryDie
}