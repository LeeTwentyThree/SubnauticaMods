using UnityEngine;

namespace MonkeySayMonkeyGet.VoiceCommands;

public class Vomit : VoiceCommandBase
{
    public override bool AllowPartialInput => false;

    public override Priority Priority => Priority.Low;

    protected override bool IsValid(SpeechInput input)
    {
        return PhraseManager.ContainsPhrase(input, PhraseManager.Gross);
    }

    protected override void Perform(SpeechInput input)
    {
        var asset = Player.main.IsUnderwater() ? belowWater : aboveWater;
        global::Utils.PlayFMODAsset(asset, MainCameraControl.main.transform.position);
    }

    private static FMODAsset aboveWater = Utils.GetFMODAsset("event:/player/Puke");
    private static FMODAsset belowWater = Utils.GetFMODAsset("event:/player/Puke_underwater");

    public override float MinimumDelay => 3f;
}