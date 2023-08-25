using UnityEngine;

namespace MonkeySayMonkeyGet.VoiceCommands;

public class PlaySound : VoiceCommandBase
{
    public override bool AllowPartialInput => true;

    public override Priority Priority => Priority.Low;

    protected override bool IsValid(SpeechInput input)
    {
        return PhraseManager.ContainsPhrase(input, PhraseManager.Sound);
    }

    protected override void Perform(SpeechInput input)
    {
        var asset = fmodAsset[Random.Range(0, fmodAsset.Length)];
        global::Utils.PlayFMODAsset(asset, MainCameraControl.main.transform.position);
    }

    private readonly FMODAsset[] fmodAsset = new FMODAsset[]
    {
        Utils.GetFMODAsset("event:/creature/boneshark/roar"),
        Utils.GetFMODAsset("event:/creature/crabsnake/attack_cine"),
        Utils.GetFMODAsset("event:/creature/crash/die"),
        Utils.GetFMODAsset("event:/creature/gasopod/death"),
        Utils.GetFMODAsset("event:/creature/lavalizard/spit"),
        Utils.GetFMODAsset("event:/creature/magistrate/say_hello"),
        Utils.GetFMODAsset("event:/creature/mesmer/mesmerize_start"),
        Utils.GetFMODAsset("event:/creature/seadragon/attack_mech_cin"),
        Utils.GetFMODAsset("event:/creature/tred/shit_from_ass"),
        Utils.GetFMODAsset("event:/creature/warper/swipe"),
        Utils.GetFMODAsset("event:/player/gunterminal_access_denied"),
    };
}