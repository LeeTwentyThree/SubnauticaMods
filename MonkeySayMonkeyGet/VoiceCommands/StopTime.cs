using MonkeySayMonkeyGet.Mono;
using System.Collections;
using UnityEngine;
using UWE;

namespace MonkeySayMonkeyGet.VoiceCommands;

public class StopTime : VoiceCommandBase
{
    public override bool AllowPartialInput => true;

    public override Priority Priority => Priority.High;

    protected override bool IsValid(SpeechInput input)
    {
        if (StarPlatinum.StarPlatinumActivated && !Utils.timeStopActive && input.ContainsPhrase(PhraseManager.TheWorld))
        {
            return true;
        }
        return PhraseManager.ContainsPhrase(input, PhraseManager.Stop);
    }

    protected override void Perform(SpeechInput input)
    {
        UWE.CoroutineHost.StartCoroutine(Utils.ZaWarudo());
    }

    public override float MinimumDelay => 3f;
}