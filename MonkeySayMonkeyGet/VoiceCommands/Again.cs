using UnityEngine;
using MonkeySayMonkeyGet.Mono;

namespace MonkeySayMonkeyGet.VoiceCommands;

public class Again : VoiceCommandBase
{
    public override bool AllowPartialInput => false;

    public override Priority Priority => Priority.Low;

    public override float MinimumDelay => 2f;

    protected override bool IsValid(SpeechInput input)
    {
        return PhraseManager.ContainsPhrase(input, PhraseManager.Repeat);
    }

    protected override void Perform(SpeechInput input)
    {
        var listener = VoiceListener.main;
        if (listener.lastPerformed != this)
        {
            listener.lastPerformed.FeedInput(listener.lastFedInput);
        }
    }
}