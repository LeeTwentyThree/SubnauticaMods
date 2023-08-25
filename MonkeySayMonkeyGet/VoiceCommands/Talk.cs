using UnityEngine;

namespace MonkeySayMonkeyGet.VoiceCommands;

public class Talk : VoiceCommandBase
{
    public override bool AllowPartialInput => false;

    public override Priority Priority => Priority.Low;

    protected override bool IsValid(SpeechInput input)
    {
        return PhraseManager.ContainsPhrase(input, PhraseManager.Talk);
    }

    protected override void Perform(SpeechInput input)
    {
        var asset = fmodAsset[Random.Range(0, fmodAsset.Length)];
        global::Utils.PlayFMODAsset(asset, MainCameraControl.main.transform.position);
    }

    private readonly FMODAsset[] fmodAsset = new FMODAsset[]
    {
        Utils.GetFMODAsset("event:/player/story/Goal_BaseWindow"),
        Utils.GetFMODAsset("event:/player/oxygen_25"),
        Utils.GetFMODAsset("event:/player/scan_planet"),
        Utils.GetFMODAsset("event:/player/scan_aurora"),
        Utils.GetFMODAsset("event:/player/story/Goal_Bleach"),
        Utils.GetFMODAsset("event:/player/story/Goal_Diamond"),
        Utils.GetFMODAsset("event:/player/story/Goal_FarmingTray"),
        Utils.GetFMODAsset("event:/player/story/Goal_LocationAuroraHallway"),
        Utils.GetFMODAsset("event:/player/story/Goal_LocationAuroraFurtherIn"),
        Utils.GetFMODAsset("event:/player/story/Goal_BiomeDeepGrandReef"),
        Utils.GetFMODAsset("event:/player/story/OvereatingWarning"),
    };

    public override float MinimumDelay => 8f;
}