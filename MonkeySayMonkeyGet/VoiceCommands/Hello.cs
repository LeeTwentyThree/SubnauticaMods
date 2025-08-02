using UnityEngine;

namespace MonkeySayMonkeyGet.VoiceCommands;

public class Hello : VoiceCommandBase
{
    public override bool AllowPartialInput => false;

    public override Priority Priority => Priority.None;

    public override float MinimumDelay => 2f;

    protected override bool IsValid(SpeechInput input)
    {
        if (Plugin.ModConfig.DisableShrek)
        {
            return false;
        }
        return PhraseManager.ContainsPhrase(input, PhraseManager.Hello);
    }

    protected override void Perform(SpeechInput input)
    {
        Utils.PlaySoundEffect("OhHelloThere", 2f);
        var shrekHud = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("ShrekHUD"));
        Object.Destroy(shrekHud, 1f);
    }
}