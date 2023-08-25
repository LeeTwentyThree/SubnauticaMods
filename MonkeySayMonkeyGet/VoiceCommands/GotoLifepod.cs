using UnityEngine;

namespace MonkeySayMonkeyGet.VoiceCommands;

public class GotoLifepod : VoiceCommandBase
{
    public override bool AllowPartialInput => false;

    public override Priority Priority => Priority.Low;

    protected override bool IsValid(SpeechInput input)
    {
        return PhraseManager.ContainsPhrase(input, PhraseManager.Lifepod);
    }

    protected override void Perform(SpeechInput input)
    {
        Player.main.lastEscapePod?.RespawnPlayer();
        Player.main.currentEscapePod = Player.main.lastEscapePod;
        Player.main.SetPrecursorOutOfWater(false);
    }
}