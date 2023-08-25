using UnityEngine;

namespace MonkeySayMonkeyGet.VoiceCommands;

public class Stuck : VoiceCommandBase
{
    public override bool AllowPartialInput => true;

    public override Priority Priority => Priority.Low;

    protected override bool IsValid(SpeechInput input)
    {
        return PhraseManager.ContainsPhrase(input, PhraseManager.Stuck) && Player.main.IsSwimming();
    }

    protected override void Perform(SpeechInput input)
    {
        if (Player.main.IsSwimming())
        {
            Player.main.gameObject.AddComponent<Mono.FreezePlayer>();
        }
    }

    public override float MinimumDelay => 4f;
}