using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MonkeySayMonkeyGet.Mono.StarPlatinum;

namespace MonkeySayMonkeyGet.VoiceCommands;

public class Die : VoiceCommandBase
{
    public override bool AllowPartialInput => false;

    public override Priority Priority => Priority.Medium;

    protected override bool IsValid(SpeechInput input)
    {
        return !PhraseManager.ContainsPhrase(input, PhraseManager.You) && PhraseManager.ContainsPhrase(input, PhraseManager.Die);
    }

    protected override void Perform(SpeechInput input)
    {
        if (StarPlatinum.StarPlatinumActivated)
        {
            var instance = StarPlatinum.Instance;
            var task = instance.AssignTask<KillSmall>();
            if (task)
            {
                task.targetObject = Player.main.gameObject;
                return;
            }
        }
        Player.main.liveMixin.TakeDamage(1000f);
    }

    public override float MinimumDelay => 10f;
}